public interface IOpenAIImageService
{
    Task<string> GenerateImageAsync(string prompt);

    Task<string> UploadImageAsync(byte[] imageBytes, string? fileName = "upload.png");
    

    Task<string> UploadImageAsync(byte[] imageBytes);

}