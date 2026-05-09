namespace Appoo.Services;

public class UnrecognizableImageService : IImageRecognitionService
{
    public Task<string> RecognizeAsync(string imagePath)
        => Task.FromResult("无法识别该景点");
}