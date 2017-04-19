using HockeyApp.Android;

public class CustomCrashListener: CrashManagerListener
{
    public override bool ShouldAutoUploadCrashes()
    {
        return true;
    }

    public override bool IncludeDeviceIdentifier()
    {
        return true;
    }

    public override bool IncludeDeviceData()
    {
        return true;
    }
}