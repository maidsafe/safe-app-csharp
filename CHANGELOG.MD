# safe_app_csharp Change Log

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
