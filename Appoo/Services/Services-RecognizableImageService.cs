using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Appoo.Services;

public class RecognizableImageService : IImageRecognitionService
{
    private const string Endpoint = "https://你的Azure终结点.cognitiveservices.azure.com/";
    private const string SubscriptionKey = "你的订阅密钥";

    public async Task<string> RecognizeAsync(string imagePath)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
            string requestUrl = $"{Endpoint}vision/v3.2/analyze?visualFeatures=Categories,Description&details=Landmarks";
            using var content = new ByteArrayContent(File.ReadAllBytes(imagePath));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            var response = await client.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var categories = json["categories"];
            if (categories != null)
                foreach (var cat in categories)
                    if (cat["detail"]?["landmarks"] is JArray landmarks && landmarks.Count > 0)
                        return landmarks[0]["name"]?.ToString() ?? "某著名景点";
            return json["description"]?["captions"]?[0]?["text"]?.ToString() ?? "无法识别";
        }
        catch { return "识别服务暂时不可用"; }
    }
}