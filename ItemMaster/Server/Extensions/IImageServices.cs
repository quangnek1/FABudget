namespace ItemMaster.Server.Extensions
{
    public interface IImageServices
    {
        Task<string> GenerateImage(string name, string date, string division);
    }
}
