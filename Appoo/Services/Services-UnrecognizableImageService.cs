namespace Appoo.Services;

public class UnrecognizableImageService : IImageRecognitionService
{
    public Task<string> RecognizeAsync(string imagePath)
    {
        // 模拟无法识别的情况
        return Task.FromResult("抱歉，无法识别该景点");
    }
}