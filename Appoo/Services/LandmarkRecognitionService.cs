using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Appoo.Services
{
    public class LandmarkRecognitionService : IImageRecognitionService
    {
        private readonly string _apiKey = "dZJnXzH9vJ4oouKYoSxBh9JO";  
        private readonly string _secretKey = "mqPzRBEzjrqAHc3Z1wh1BfvdJJyzDRH8"; 
        private readonly HttpClient _httpClient;

        public LandmarkRecognitionService()
        {
            _httpClient = new HttpClient();
        }

        private async Task<string> GetAccessToken()
        {
            string url = $"https://aip.baidubce.com/oauth/2.0/token" +
                         $"?grant_type=client_credentials" +
                         $"&client_id={_apiKey}" +
                         $"&client_secret={_secretKey}";

            var response = await _httpClient.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(json);
            return obj["access_token"]?.ToString() ?? string.Empty;
        }

        public async Task<string> RecognizeAsync(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                return "[Error] Image data is empty.";

            string token = await GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return "[Error] Failed to get access token.";

            // 将图片字节转换为 Base64 字符串
            string base64Image = Convert.ToBase64String(imageBytes);

            string requestUrl = $"https://aip.baidubce.com/rest/2.0/image-classify/v1/landmark" +
                                $"?access_token={token}";

            // 使用 FormUrlEncodedContent 发送 x-www-form-urlencoded 数据
            var postData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("image", base64Image),
                new KeyValuePair<string, string>("top_num", "1")   // 只取最可能的地标
            });

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(requestUrl, postData);
            }
            catch (Exception ex)
            {
                return $"[Error] Network error: {ex.Message}";
            }

            string resultJson = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"[Baidu] Response: {resultJson}");

            JObject resultObj = JObject.Parse(resultJson);
            if (resultObj["error_code"] != null)
            {
                return $"[Error] {resultObj["error_code"]}: {resultObj["error_msg"]}";
            }

            // 提取 landmark 字段
            return resultObj["result"]?["landmark"]?.ToString() ?? "[Error] No landmark found.";
        }
    }
}