using Android.Webkit;
using Android.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Content.PM;

namespace Appoo.Platforms.Android;

public class MyWebChromeClient : WebChromeClient
{
    private readonly Activity _activity;

    public MyWebChromeClient(Activity activity)
    {
        _activity = activity ?? throw new ArgumentNullException(nameof(activity));
    }

    public override void OnGeolocationPermissionsShowPrompt(string origin, GeolocationPermissions.ICallback callback)
    {
        const string fine = "android.permission.ACCESS_FINE_LOCATION";
        const string coarse = "android.permission.ACCESS_COARSE_LOCATION";
        if (ContextCompat.CheckSelfPermission(_activity, fine) == Permission.Granted ||
            ContextCompat.CheckSelfPermission(_activity, coarse) == Permission.Granted)
        {
            callback.Invoke(origin, true, false);
        }
        else
        {
            callback.Invoke(origin, false, false);
        }
    }
}