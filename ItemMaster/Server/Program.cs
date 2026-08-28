using System;
using Common.Logging;
using Hangfire;
using ItemMaster.Server.Data;
using ItemMaster.Server.Data.Entities;
using ItemMaster.Server.Extensions;
using ItemMaster.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Serilogger.Configure);
Log.Information(messageTemplate: "Starting Proect...");

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "ItemMaster.Server",
		Description = "An ASP.NET Core API app for MudBlazor",
		Contact = new OpenApiContact
		{
			Name = "AIH",
			Email = "hanoi-eng82@asahi-intecc.com",
			Url = new Uri("https://172.16.33.123/"),
		},
		License = new OpenApiLicense
		{
			Name = "Use under MIT",
			Url = new Uri("https://opensource.org/licenses/MIT"),
		}
	});

	c.EnableAnnotations();
	c.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
	c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

	#region 
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,

		Description = " JWT Authorization header using the Bearer scheme. \r\n\r\n Put **_ONLY_** your JWT Bearer token on textbox below!"
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference
								{
									Type = ReferenceType.SecurityScheme,
									Id = "Bearer"
								}
							},
							new string[] {}
					}
				});
	#endregion
});

builder.Services.AddDbContext<ApplicationDbContext>
	(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<UserDbContext>
	(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));
builder.Services.AddSingleton<PurchaseContext>();

// Cấu hình Hangfire
builder.Services.AddHangfire(config =>
	config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

// Add services to the container.
//builder.Services.AddIdentity<User, IdentityRole>(options =>
//{
//}).AddEntityFrameworkStores<ApplicationDbContext>()
//            .AddDefaultTokenProviders();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
}).AddEntityFrameworkStores<UserDbContext>()
			.AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 5;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
});

builder.Services.AddAuthentication(
	options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	}).AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
				builder.Configuration.GetSection("TokenSettings:SymmetricKey").Value)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();


builder.Services.AddTransient<DbInitializer>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ISendmailServices, SendmailServices>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IImageServices, ImageServices>();
builder.Services.AddScoped<IImageServices, ImageServices>();
builder.Services.AddScoped<IFABudgetServices, FABudgetServices>();
builder.Services.AddScoped<IPURCHASE_SYSTEM, PURCHASE_SYSTEM>();
//builder.Services.AddScoped<IAutoRun, AutoRun>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddHostedService<TimedHostedService>();
//builder.Services.AddHostedService<WeeklyEmailService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	try
	{
		var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		db.Database.Migrate();
	}
	catch (Exception ex)
	{
		Log.Fatal(ex, "Database migration failed");
		throw;
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "ItemMaster.Server v1");
	});
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();

}
using (var scope = app.Services.CreateScope())
{
	try
	{
		var db = scope.ServiceProvider.GetService<IPURCHASE_SYSTEM>();
		db.DownloadAllData().Wait();

		var dbInitializer = scope.ServiceProvider.GetService<DbInitializer>();
		dbInitializer.Seed().Wait();
	}
	catch (Exception ex)
	{
		Console.WriteLine(ex.ToString());
	}
}

// app.UseHttpsRedirection();
// Dashboard để xem job
app.UseHangfireDashboard("/hangfire");

// Lấy timezone (Windows + Linux đều chạy)
TimeZoneInfo tz;
try
{
	tz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows
}
catch (TimeZoneNotFoundException)
{
	tz = TimeZoneInfo.FindSystemTimeZoneById("Asia/Bangkok"); // Linux
}

// Đăng ký job (chạy mỗi ngày lúc 8:00 sáng)
RecurringJob.AddOrUpdate<HangfireJobs>(
	"send-daily-report",
	job => job.SendDailyEmail(),
	Cron.Daily(8, 0),
	new RecurringJobOptions { TimeZone = tz }
);

RecurringJob.AddOrUpdate<HangfireJobs>(
	"update-monthly-data",
	job => job.UpdateMonthlyData(),
	"0 8 1 * *", // 8:00 sáng ngày 1 mỗi tháng
	new RecurringJobOptions { TimeZone = tz }
);


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();