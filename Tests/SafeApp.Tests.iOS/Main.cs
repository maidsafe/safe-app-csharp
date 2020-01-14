using UIKit;

#pragma warning disable SA1300 // Element should begin with upper-case letter
namespace SafeApp.Tests.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
#pragma warning restore SA1300 // Element should begin with upper-case letter
