using Android.App;
using Android.Content.PM;
using Android.OS;
using NUnit.Runner;
using NUnit.Runner.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace SafeApp.Tests.Android
{
    [Activity(
      Name = "SafeApp.Tests.Android.MainActivity",
      Label = "SafeApp.Tests.Android",
      Icon = "@drawable/icon",
      Theme = "@android:style/Theme.Holo.Light",
      MainLauncher = true,
      ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    // ReSharper disable once UnusedMember.Global
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);

            // This will load all tests within the current project
            var nunit = new App
            {
                Options = new TestOptions
                {
                    // If True, the tests will run automatically when the app starts
                    // otherwise you must run them manually.
                    AutoRun = true,

                    // If True, the application will terminate automatically after running the tests.
                    // TerminateAfterExecution = true,

                    // Information about the tcp listener host and port.
                    // For now, send result as XML to the listening server.
                    TcpWriterParameters = new TcpWriterInfo("10.0.2.2", 10500)

                    // Creates a NUnit Xml result file on the host file system using PCLStorage library.
                    // CreateXmlResultFile = true,

                    // Choose a different path for the xml result file
                    // ResultFilePath = Path.Combine(Environment.ExternalStorageDirectory.Path, Environment.DirectoryDownloads, "Nunit", "Results.xml")
                }
            };

            // If you want to add tests in another assembly
            // nunit.AddTestAssembly(typeof(MyTests).Assembly);

            // Available options for testing

            LoadApplication(nunit);
        }
    }
}
