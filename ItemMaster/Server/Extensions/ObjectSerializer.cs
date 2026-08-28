using Newtonsoft.Json;

public static class ObjectSerializer
{
	public static string SerializeObject(object obj)
	{
		return JsonConvert.SerializeObject(obj, Formatting.Indented);
	}
}