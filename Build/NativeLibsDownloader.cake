var SAFE_API_LIB_TAG = "9a41307";
var AUTH_LIB_TAG = "183939d";

var S3_DOWNLOAD_BASE_URL = "https://safe-api.s3.amazonaws.com/";
var S3_AUTH_DOWNLOAD_BASE_URL = "https://safe-client-libs.s3.amazonaws.com/";

var LIB_DIR_NAME = "../SafeApp.AppBindings/NativeLibs/";
var ANDROID_DIR_NAME = $"{LIB_DIR_NAME}Android";
var IOS_DIR_NAME = $"{LIB_DIR_NAME}iOS";
var DESKTOP_DIR_NAME = $"{LIB_DIR_NAME}Desktop";

var AUTH_LIB_DIR_NAME = "../Tests/SafeApp.Tests.Core/NativeLibs/";
var NATIVE_DIR = Directory($"{System.IO.Path.GetTempPath()}SafeApiNativeLibs");
var NATIVE_AUTH_DIR = Directory($"{System.IO.Path.GetTempPath()}SafeAuthNativeLibs");

var ANDROID_ARMEABI_V7A = "armv7-linux-androideabi";
var ANDROID_x86_64 = "x86_64-linux-android";
var ANDROID_ARCHITECTURES = new string[] {
  ANDROID_ARMEABI_V7A,
  ANDROID_x86_64
};
var IOS_ARCHITECTURES = new string[] {
  "apple-ios"
};
var DESKTOP_ARCHITECTURES = new string[] {
  "x86_64-unknown-linux-gnu",
  "x86_64-apple-darwin",
  "x86_64-pc-windows-msvc"
};
var All_ARCHITECTURES = new string[][] {
  ANDROID_ARCHITECTURES,
  IOS_ARCHITECTURES,
  DESKTOP_ARCHITECTURES
};

enum Environment {
  Android,
  Ios,
  Desktop
}

var varients = new string[] {
  "",
  "dev" 
};

Task("Download-Libs")
  .Does(() => {
    foreach(var item in Enum.GetValues(typeof (Environment)))
    {
      string[] targets = null;
      Information($"\n{item} ");
      switch (item) 
      {
      case Environment.Android:
        targets = ANDROID_ARCHITECTURES;
        break;
      case Environment.Ios:
        targets = IOS_ARCHITECTURES;
        break;
      case Environment.Desktop:
        targets = DESKTOP_ARCHITECTURES;
        break;
      }

      foreach(var target in targets) {
        foreach (var varient in varients)
        {
          var targetDirectory = $"{NATIVE_DIR.Path}/{item}/{target}";
          var zipFileName = varient == "" ? $"safe-ffi-{SAFE_API_LIB_TAG}-{target}.zip" : $"safe-ffi-{SAFE_API_LIB_TAG}-{target}-{varient}.zip";
          var zipFileDownloadUrl = $"{S3_DOWNLOAD_BASE_URL}{zipFileName}";
          var zipSavePath = $"{targetDirectory}/{zipFileName}";

          if(!DirectoryExists(targetDirectory))
            CreateDirectory(targetDirectory);
          else
          {
            var existingNativeLibs = GetFiles($"{targetDirectory}/*.zip");
            if(existingNativeLibs.Count > 0)
            foreach(var lib in existingNativeLibs) 
            {
              if(!lib.GetFilename().ToString().Contains(SAFE_API_LIB_TAG))
                DeleteFile(lib.FullPath);
            }
          }

          Information("Downloading : {0}", zipFileName);
          if(!FileExists(zipSavePath))
          {
            DownloadFile(zipFileDownloadUrl, File(zipSavePath));
          }
          else
          {
            Information("File already exists");
          }
        }
      }
    }
  })
  .ReportError(exception => {
    Information(exception.Message);
  });

Task("UnZip-Libs")
  .IsDependentOn("Download-Libs")
  .Does(() => {
    foreach(var item in Enum.GetValues(typeof(Environment))) {
      string[] targets = null;
      var outputDirectory = string.Empty;
      Information($"\n {item} ");
      switch (item) 
      {
      case Environment.Android:
        targets = ANDROID_ARCHITECTURES;
        outputDirectory = ANDROID_DIR_NAME;
        break;
      case Environment.Ios:
        targets = IOS_ARCHITECTURES;
        outputDirectory = IOS_DIR_NAME;
        break;
      case Environment.Desktop:
        targets = DESKTOP_ARCHITECTURES;
        outputDirectory = DESKTOP_DIR_NAME;
        break;
      }

      CleanDirectories(outputDirectory);

      foreach(var target in targets) {
        var zipSourceDirectory = Directory($"{NATIVE_DIR.Path}/{item}/{target}");
        var zipFiles = GetFiles($"{zipSourceDirectory}/*.*");
        foreach(var zip in zipFiles) 
        {
          var filename = zip.GetFilename();
          Information(" Unzipping : " + filename);
          var platformOutputDirectory = new StringBuilder();
          platformOutputDirectory.Append(outputDirectory);
          
          if(filename.ToString().Contains("dev")) 
          {
            platformOutputDirectory.Append("/mock");
          } 
          else 
          {
            platformOutputDirectory.Append("/non-mock");
          }
          
          if(target.Equals(ANDROID_ARMEABI_V7A))
            platformOutputDirectory.Append("/armeabi-v7a");
          else if(target.Equals(ANDROID_x86_64))
            platformOutputDirectory.Append("/x86_64");

          Unzip(zip, platformOutputDirectory.ToString());

          var unZippedFiles = GetFiles($"{platformOutputDirectory.ToString()}/*.*");
          foreach (var file in unZippedFiles)
          {
            MoveFile(file.FullPath, file.FullPath.Replace("safe_ffi", "safe_api"));
          }
        }
      }
    }
  })
  .ReportError(exception => {
    Information(exception.Message);
  });

Task("Download-Auth-Libs")
  .Does(() => {
    var targetDirectory = $"{NATIVE_AUTH_DIR.Path}";
    
    if(!DirectoryExists(targetDirectory))
      CreateDirectory(targetDirectory);
    else
    {
      var existingNativeLibs = GetFiles($"{targetDirectory}/*.zip");
      if(existingNativeLibs.Count > 0)
      foreach(var lib in existingNativeLibs) 
      {
        DeleteFile(lib.FullPath);
      }
    }
    
    foreach(var item in DESKTOP_ARCHITECTURES)
    {
      var zipFileName = $"safe_authenticator-{AUTH_LIB_TAG}-{item}.zip";
      var zipFileDownloadUrl = $"{S3_AUTH_DOWNLOAD_BASE_URL}{zipFileName}";
      var zipSavePath = $"{targetDirectory}/{zipFileName}";

      Information("Downloading : {0}", zipFileName);
      if(!FileExists(zipSavePath))
      {
        DownloadFile(zipFileDownloadUrl, File(zipSavePath));
      }
      else
      {
        Information("File already exists");
      }
    }
  })
  .ReportError(exception => {
    Information(exception.Message);
  });


  Task("UnZip-Auth-Libs")
  .IsDependentOn("Download-Auth-Libs")
  .Does(() => {
    CleanDirectories(AUTH_LIB_DIR_NAME);
    var targetDirectory = $"{NATIVE_AUTH_DIR.Path}";

    var zipFiles = GetFiles($"{targetDirectory}/*.*");
    foreach(var zip in zipFiles) 
    {
      var filename = zip.GetFilename();
      Information(" Unzipping : " + filename);
      Unzip(zip, AUTH_LIB_DIR_NAME);
    }
  })
  .ReportError(exception => {
    Information(exception.Message);
  });