using System.Threading.Tasks;

namespace Appoo.Services
{
    public class UnrecognizableImageService : IImageRecognitionService
    {
        public Task<string> RecognizeAsync(byte[] imageBytes)
        {
            // 始终返回“无法识别”，无需处理图片数据
            return Task.FromResult("Sorry, unable to identify this spot.");
        }
    }
}