using System.Net.Http.Headers;
using System.Net.Http.Json;
using GoogleGson;
using Newtonsoft.Json.Linq;   // 需安装 Newtonsoft.Json NuGet 包

namespace Appoo.Services;

public class RecognizableImageService : IImageRecognitionService
{
    private const string Endpoint = "https://你的服务名称.cognitiveservices.azure.com/";
    private const string SubscriptionKey = "你的订阅密钥";

    public async Task<string> RecognizeAsync(string imagePath)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

            // Azure 视觉 API 分析图片描述和地标
            string requestUrl = $"{Endpoint}vision/v3.2/analyze?visualFeatures=Categories,Description&details=Landmarks";

            using var content = new ByteArrayContent(File.ReadAllBytes(imagePath));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = await client.PostAsync(requestUrl, content);
            response.EnsureSuccessStatusCode();

            var json = JObject.Parse(await response.Content.ReadAsStringAsync());

            // 尝试提取地标名称
            var categories = json["categories"];
            if (categories != null)
            {
                foreach (var cat in categories)
                {
                    string name = cat["name"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(name) && name.Contains("landmark"))
                    {
                        // 从 detail 中获取更具体的地标名
                        var detail = cat["detail"];
                        if (detail?["landmarks"] is JArry landmarks && landmarks.Count > 0)
                        {
                            return landmarks[0]["name"]?.ToString() ?? "某著名景点";
                        }
                    }
                }
            }
            // 无地标则尝试返回描述信息
            var description = json["description"]?["captions"]?[0]?["text"]?.ToString();
            return description ?? "无法识别该景点";
        }
        catch
        {
            return "识别服务暂时不可用";
        }
    }
}