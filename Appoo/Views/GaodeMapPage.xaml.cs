namespace Appoo.Views;

public partial class GaodeMapPage : ContentPage
{
    public GaodeMapPage()
    {
        InitializeComponent();
        LoadMapWithBaseUrl(); // 在这里调用加载方法
    }

    private void LoadMapWithBaseUrl()
    {
        // 1. 读取 amap.html 文件内容
        var htmlContent = "";
        using (var stream = FileSystem.OpenAppPackageFileAsync("wwwroot/amap.html").GetAwaiter().GetResult())
        {
            using (var reader = new StreamReader(stream))
            {
                htmlContent = reader.ReadToEnd();
            }
        }

        // 2. 设置 BaseUrl，这是解决问题的关键！
        // 这告诉 WebView：把 "wwwroot" 这个目录当作网站根目录
        var baseUrl = "wwwroot";

        // 3. 使用 HtmlWebViewSource 加载
        var htmlSource = new HtmlWebViewSource
        {
            Html = htmlContent,
            BaseUrl = baseUrl
        };

        mapView.Source = htmlSource;
    }
}