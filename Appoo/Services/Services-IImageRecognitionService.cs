// Services/IImageRecognitionService.cs
using System.Threading.Tasks;

namespace Appoo.Services
{
    public interface IImageRecognitionService
    {
        Task<string> RecognizeAsync(byte[] imageBytes);
    }
}