namespace Appoo.Services;

public interface IImageRecognitionService
{
    // 返回识别的景点名称，若失败则返回 null 或 "无法识别"
    Task<string> RecognizeAsync(string imagePath);
}