namespace Appoo.Services;

public interface IImageRecognitionService
{
    Task<string> RecognizeAsync(string imagePath);
}