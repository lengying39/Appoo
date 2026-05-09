namespace Appoo.Services;

public class UnrecognizableImageService : IImageRecognitionService
{
    public Task<string> RecognizeAsync(string imagePath)
        => Task.FromResult("Sorry, unable to identify this spot.");
}