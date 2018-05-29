
var TAG = "6be5558";
var ANDROID_DIR_NAME = "SafeApp.AppBindings.Android";
var IOS_DIR_NAME = "SafeApp.AppBindings.iOS";
var DESKTOP_DIR_NAME = "SafeApp.AppBindings.Desktop";

var ANDROID_X86  = "android-x86";
var ANDROID_ARMEABI_V7A  = "android-armeabiv7a";
var ANDROID_ARCHITECTURES  = new string[]{ANDROID_X86, ANDROID_ARMEABI_V7A};
var IOS_ARCHITECTURES  = new string[]{"ios"};
var DESKTOP_ARCHITECTURES  = new string[]{"linux-x64", "osx-x64", "win-x64"};
var All_ARCHITECTURES = new string[][]{ANDROID_ARCHITECTURES, IOS_ARCHITECTURES, DESKTOP_ARCHITECTURES};

var Native_DIR = Directory(string.Concat(EnvironmentVariable("TEMP"), "/nativelibs"));
enum Enviornment {Android,Ios,Desktop}

Task ("CleanTask")
    .Does (() => {
        CleanDirectories (Native_DIR);
        Information ("Finished cleaning");
    })
.ReportError(exception =>
{  
    Information(exception.Message);
});

Task("DownloadTask")
    .Does(() =>
{
    foreach (var item in Enum.GetValues(typeof(Enviornment)))
    {
        string[] targets = null;
        Information(string.Format("\n {0} ",item));
        switch (item)
        {   
            case Enviornment.Android:
                targets = ANDROID_ARCHITECTURES;
                break;
            case Enviornment.Ios:
                targets = IOS_ARCHITECTURES;
                break;
            case Enviornment.Desktop:
                targets = DESKTOP_ARCHITECTURES;
                break;
        }

        foreach (var target in targets)
        {   
            var targetdirectory =  string.Format("{0}/{1}/{2}", Native_DIR.Path,item, target);
            var mockzipurl  = string.Format("https://s3.eu-west-2.amazonaws.com/safe-client-libs/safe_app-mock-{0}-{1}.zip", TAG, target);
            var notmockzipurl  = string.Format("https://s3.eu-west-2.amazonaws.com/safe-client-libs/safe_app-mock-{0}-{1}.zip", TAG, target);
            var mockzipsavepath = string.Format("{0}/{1}/{2}/safe_app-mock-{3}-{4}.zip", Native_DIR.Path,item, target, TAG,target);
            var notmockzipsavepath = string.Format("{0}/{1}/{2}/safe_app-{3}-{4}.zip", Native_DIR.Path,item, target, TAG,target);

            if(!DirectoryExists(targetdirectory))
                CreateDirectory(targetdirectory);

            if(!FileExists(mockzipsavepath) && !FileExists(notmockzipsavepath))
            {
                CurlDownloadFiles(
                    new[]
                    {
                        new Uri(mockzipurl),
                        new Uri(notmockzipurl)
                    },
                new CurlDownloadSettings
                {
                    OutputPaths = new FilePath[]
                    {
                        mockzipsavepath,
                        notmockzipsavepath
                    }
                });
            }
        }
    }
})
.ReportError(exception =>
{  
    Information(exception.Message);
});

Task("UnZipTask")
    .IsDependentOn("DownloadTask")
    .Does(() =>
{
    foreach (var item in Enum.GetValues(typeof(Enviornment)))
    {
        string[] targets = null;
        var outputdirectory = string.Empty;
        Information(string.Format("\n {0} ",item));
        switch (item)
        {
            case Enviornment.Android:
                targets = ANDROID_ARCHITECTURES;
                outputdirectory = ANDROID_DIR_NAME;
                break;
            case Enviornment.Ios:
                targets = IOS_ARCHITECTURES;
                outputdirectory = IOS_DIR_NAME;
                break;
            case Enviornment.Desktop:
                targets = DESKTOP_ARCHITECTURES;
                outputdirectory = DESKTOP_DIR_NAME;
                break;
        }

        CleanDirectories(string.Concat(outputdirectory, "/lib"));

        foreach (var target in targets)
        {
            var zipdirectorysource = Directory(string.Format("{0}/{1}/{2}", Native_DIR.Path,item, target));
            var zipfiles =  GetFiles(string.Format("{0}/*.*" ,zipdirectorysource));
            foreach (var zip in zipfiles)
            {
                var filename = zip.GetFilename();
                Information(" Unzipping : " + filename);
                var platformoutputdirecotory = new StringBuilder();
                platformoutputdirecotory.Append(outputdirectory);
                platformoutputdirecotory.Append("/lib");

                if(filename.ToString().Contains("mock"))
                {
                    platformoutputdirecotory.Append("/mock");
                }
                else
                {
                    platformoutputdirecotory.Append("/non-mock");
                }

                if(target.Equals(ANDROID_X86))
                    platformoutputdirecotory.Append("/x86");
                else if(target.Equals(ANDROID_ARMEABI_V7A))
                    platformoutputdirecotory.Append("/armeabi-v7a");

                if(target.Contains("osx"))
                {
                    var afile = GetFiles(string.Format("{0}/*.a", platformoutputdirecotory.ToString()));
                    DeleteFile(afile.ToArray()[0].FullPath);
                }
                Unzip(zip, platformoutputdirecotory.ToString());
            }
        }
    }
})
.ReportError(exception =>
{  
    Information(exception.Message);
});