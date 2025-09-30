public interface IOpenAIImageService
{
    Task<string> GenerateImageAsync(string prompt);
}