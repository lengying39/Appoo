namespace Appoo.Services;

public class UnrecognizableImageService : IImageRecognitionService
{
    public Task<string> RecognizeAsync(string imagePath)
    {
        // 永远返回无法识别，模拟离线或失败场景
        return Task.FromResult("抱歉，无法识别该景点");
    }
}