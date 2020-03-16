# safe_app_csharp Change Log

## [0.3.1] - RC 1

### Changes

- Updated native libs to support baby fleming network.
- Updated the `safe-api` native libs to baby-fleming-mobile branch commit [#b3862a1](https://github.com/maidsafe/safe-api/commit/b3862a13efb100de8977da2a87cb28ad5288e2b0).

## [0.3.0]

### Changes

- Updated minimum supported version for the desktop and mobile platforms. 
- Added new simplied APIs for pWeb and test SafeCoins using `[safe-api](https://github.com/maidsafe/safe-api/tree/master/safe-ffi)`.
  - Connect API: add binding and implementation for the connect APIs.
  - Fetch API: add binding and implementation for the Fetch and Inspect APIs.
  - Files API: add binding and implementation for the Files APIs.
  - Keys API: add binding and implementation for the keys APIs.
  - NRS API: add binding and implementation for the NRS APIs.
  - Wallet API: add binding and implementation for the Wallet APIs.
  - XorUrl API: add binding and implementation for the XorUrlEncoder APIs.
  - Added `SetConfigurationFilePathAsync` API to set the config location path for the local vault connection.
  - Added `AppConfigDirPathAsync` API to get the current app configuration directory path.
  - Added `AuthApp` API to use cli service for app authentication on desktop.
- Renamed `SafeApp.Utilites` project to `SafeApp.Core`.
- Updated `MockAuthBindings` and mock authenticator bindings to latest `safe_authenticator`.
- Updated `AppKeys` bindings to the latest `safe_client_libs`.
- Updated `AppBindings` and `MockAuthBindings` project to use multi-target style project.
- Updated Tests to verify the working of all the new APIs.
- Removed `safe_app` bindings and wrapper APIs.
- Removed `JetBrains.Annotations` package and related code.
- Refactored solution to remove `Newtonsoft.Json` dependency.
- Refactored solution to use solution level pros and code analyzer.
- Updated solution to use C# 7.3.
- Fixed iOS native library loading issue.
- Fixes NuGet package generation issue.
- Moved Azure DevOps CI setup to the repo.
- Updated the `safe-api` native libs to master branch commit [#6cf0aa6](https://github.com/maidsafe/safe-api/commit/6cf0aa67879b1398c2365c07d8cc4f1e54cfe2de).

## [0.3.0] - RC 3

### Changes

- Updated `FilesPutPublishedImmutableAsync` API to support `DryRun` feature.
- Added `AppConfigDirPathAsync` API to get the current app configuration directoruy path.
- Removed `JetBrains.Annotations` package and related code.
- Updated `AppKeys` bindings to the latest `safe_client_libs`.
- Updated the `safe-api` native libs to master branch commit [#9a41307](https://github.com/maidsafe/safe-api/commit/9a41307efb5d8989386f227a9a1e3ec819c12054).

## [0.3.0] - RC 2

### Changes

- Added new AuthApp API to use cli service for app authentication on desktop.
- Updated APIs to use FilesMap structure instead of JSON strings.
- Fixed iOS native library loading issue.
- Fixes NuGet package generation issue.
- Refactored solution to remove `Newtonsoft.Json` dependency.
- Updated the `safe-api` native libs to master branch commit [#43c371](https://github.com/maidsafe/safe-api/tree/43c3712712fa2094cedf4078c6cac0e223843fbe).

## [0.3.0] - RC 1

### Changes

- Added new simplied APIs for pWeb and test SafeCoins from `[safe-api](https://github.com/maidsafe/safe-api/tree/master/safe-ffi)`.
  - Connect API: add binding and implementation for the connect APIs.
  - Fetch API: add binding and implementation for the Fetch and Inspect APIs.
  - Files API: add binding and implementation for the Files APIs.
  - Keys API: add binding and implementation for the keys APIs.
  - NRS API: add binding and implementation for the NRS APIs.
  - Wallet API: add binding and implementation for the Wallet APIs.
  - XorUrl API: add binding and implementation for the XorUrlEncoder APIs.
  - Added `SetConfigurationFilePathAsync` API to set the config location path for the local vault connection.
- Removed LL `safe_app` API.
- Updated `MockAuthBindings` and mock authenticator bindings to latest `safe_authenticator`.
- Updated `AppBindings` and `MockAuthBindings` project to use multi-target style project.
- Renamed `SafeApp.Utilites` project to `SafeApp.Core`.
- Updated Tests to verify the working of all the new APIs.

## [0.2.3]

### Changed

- Updated `safe_app` bindings to v0.9.1
- Updated mock authenticator bindings to v0.9.1

## [0.2.2]

### Changed

- Removed IntPtr.Zero check from `SafeAppPtr` implicit operator function.

## [0.2.1]

### Changed

- Update projects to generate and add API documentation.
- Update .NET Core target file to include native libraries in the project.
- Added seperate NativeHandles for and marked Zero NativeHandle obsolete.

## [0.2.0]

### Added

- Android x86_64 platform support.
- API documentation.

### Changed

- Updated safe_app bindings to v0.9.0 of [safe_app](https://github.com/maidsafe/safe_client_libs/tree/master/safe_app).
- NFS APIs (dir_delete, dir_update) updated to return new file version.
- Updated mock authenticator bindings to v0.9.0 of [safe_authenticator](https://github.com/maidsafe/safe_client_libs/tree/master/safe_authenticator).
- Update ShareMDataRequest to hold the list of MDataResponse.
- Changed parameter encodedReq to encodedResponse in function DecodeIpcMessageAsync.
- Added new null reference native handles (AnyOne, EmptyMDataEntries, EmptyMDataPermissions) and marked zero native handle obselete.
- Removed SetAdditionalSearchPath call from InitLoggingAsync.
- Renamed SetLogOutputPathAsync to GetLogOutputPathAsync.
- Removed Android x86 platform support.

### Fixed

- Fixes to use `FfiResultULongFromUIntPtrCb` instead of `FfiResultULongCb` in appbindings so `usize` gets marshalled to `uint` pointer.
- Fixed Typo in GetAccountInfoAsync.
- Updated GetOutputFilePath to return Task\<string>.
- Removed unused _appPtr from MDataInfoActions class.
- Fixed .NET Core target file to make NuGet package work with .NET Core projects.

## [0.1.1]

- Fixed delegate garbage collection issue.

## [0.1.0]

- API scope expanded to cover all the exposed APIs from safe_client_libs ([#6be5558](https://github.com/maidsafe/safe_client_libs/tree/6be5558)).
- Support updated to cover:
  - DotNet Framework
  - DotNet Core
- Adds support for using mock/non mock libraries based on configuration.
- Test cases using Nunit3 for the APIs to be able to test in all supported platforms.

## [0.0.1]

- Support for API scope from Messages mobile example
- Targets: Android/iOS
- Nuget Package Setup
