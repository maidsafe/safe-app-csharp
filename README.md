# sn_csharp [![NuGet](https://img.shields.io/nuget/v/MaidSafe.SafeApp.svg)](https://www.nuget.org/packages/MaidSafe.SafeApp)

.NET wrapper package for [sn_api](https://github.com/maidsafe/sn_api/).

> [sn_api](https://github.com/maidsafe/sn_api/) is a native library which exposes high level API for application development on Safe Network. It exposes API for authorisation and to manage data on the network.

**Maintainer:** Ravinder Jangra (ravinder.jangra@maidsafe.net)

## Build Status

| CI service | Status |
|---|---|
| Azure DevOps | [![Build Status](https://dev.azure.com/maidsafe/SafeApp/_apis/build/status/Test%20%26%20Release%20CI?branchName=master)](https://dev.azure.com/maidsafe/SafeApp/_build/latest?definitionId=21&branchName=master) | |

## Table of Contents

- [sn_csharp ![NuGet](https://www.nuget.org/packages/MaidSafe.SafeApp)](#sn_csharp-img-srchttpsimgshieldsionugetvmaidsafesafeappsvg-altnuget)
  - [Build Status](#build-status)
  - [Table of Contents](#table-of-contents)
  - [Supported Platforms](#supported-platforms)
  - [API Usage](#api-usage)
    - [Using Mock API](#using-mock-api)
    - [Authentication](#authentication)
  - [Documentation](#documentation)
  - [Development](#development)
    - [Project structure](#project-structure)
    - [Interoperability between C# managed and unmanaged code](#interoperability-between-c-managed-and-unmanaged-code)
    - [Interfacing with Safe Client Libs](#interfacing-with-safe-client-libs)
    - [Tests](#tests)
    - [Packaging](#packaging)
    - [Tools required](#tools-required)
  - [Useful resources](#useful-resources)
  - [Copyrights](#copyrights)
  - [Further Help](#further-help)
  - [License](#license)
  - [Contributing](#contributing)

This project contains the C# bindings and API wrappers for the [sn_api](https://github.com/maidsafe/sn_api/) and mock [safe_authenticator](https://github.com/maidsafe/safe_client_libs/tree/master/safe_authenticator). The native libraries, bindings and API wrapper are built and published as a NuGet package. The latest version can be fetched from the [MaidSafe.SafeApp NuGet package](https://www.nuget.org/packages/MaidSafe.SafeApp/).

At a very high level, this package includes:

* C# API for devs for easy app development.
* sn_api and mock safe_authenticator bindings. These bindings are one to one mapping to the FFI functions exposed from sn_api and safe_authenicator native libraries.
* Native libraries generated from [sn_api](https://github.com/maidsafe/sn_api) containing required logic to connect, read and write data on the Safe Network.

## Supported Platforms

* Xamarin.Android ( >=5.0. ABI: armeabi-v7a, x86_64)
* Xamarin.iOS ( >= 1.0, ABI: ARM64, x64)
* .NET Standard 2.0 (for usage via portable libs)
* .NET Core 2.2 (for use via .NET Core targets. Runtime support limited to x64)
* .NET Framework 4.7.2 (for use via classic .NET Framework targets. Platform support limited to x64)

## API Usage

To develop desktop and mobile apps for the Safe Network install the latest [MaidSafe.SafeApp](https://www.nuget.org/packages/MaidSafe.SafeApp/) package from NuGet.

This package provides support for mock and non-mock network. By default, non-mock API are used in the package.

### Using Mock API

* Mock API can be used by adding a `SAFE_APP_MOCK` flag in your project properties at **Properties > Build > conditional compilation symbols**.
* When the mock feature is used, a local mock node file is generated which simulates network operations used to store and retrieve data. The app will then interface with this file rather than the live Safe network.

### Authentication

* Applications must be authenticated via the Safe Authenticator to work with the Safe Network.
* The desktop authenticator is packed and shipped with the [Safe browser](https://github.com/maidsafe/sn_browser/releases/latest).
* On mobile devices, use the [Safe Authenticator](https://github.com/maidsafe/sn_authenticator_mobile/releases/latest) mobile application.

## Documentation

The documentation for the latest `sn_csharp` API is available at [https://maidsafe.github.io/sn_csharp/](https://maidsafe.github.io/sn_csharp/).

We use [DocFX](https://github.com/dotnet/docfx) to generate static HTML API documentation pages from XML code comments. The API docs are generated and published automatically during the CI build.

To generate a local copy of the API docs, [install DocFX](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#2-use-docfx-as-a-command-line-tool) and run the following command:

```
docfx .\docs\docfx.json
```

## Development

### Project structure

* **SafeApp:** C# API for sn_api
  * Fetch, Inspect, Files, Keys, Wallet, XorUrl
* **SafeApp.AppBindings:**
  * sn_api and safe_app IPC bindings generated from sn_api and safe_client_libs
  * Contains native libraries for the platform
* **SafeApp.MockAuthBindings:**
  * Mock Safe authentication C# API
  * mock safe_authenticator bindings generated from safe_client_libs
  * Classes required for mock auth functionality
* **SafeApp.Core:** Contains
  * Constants used in SafeApp
  * Binding utilities and helper functions

### Interoperability between C# managed and unmanaged code

[Platform invoke](https://www.mono-project.com/docs/advanced/pinvoke/) is a service that enables managed code to call unmanaged functions that are implemented in dynamic link libraries or native libraries. It locates and invokes an exported function and marshals its arguments (integers, strings, arrays, structures, and so on) across the interoperation boundary as needed. Check links in [useful resources](#Useful-resources) section to know more about how P/Invoke works in different .NET environments and platforms.

### Interfacing with Safe Client Libs

The package uses native code written in Rust and compiled into platform specific code. Learn more about the safe_client_libs in [the Safe client libraries wiki](https://github.com/maidsafe/safe_client_libs/wiki).

Instructions to update the bindings can be found in the [Update Bindings file](./UpdateBindings.md).

### Tests

We use shared unit tests for `safe_app` and mock `safe_authenticator` API which can be run on all supported platforms.

### Packaging

Instructions to generate the NuGet package can be found in the [Package Instructions file](
https://github.com/maidsafe/sn_csharp/blob/master/PackageInstructions.txt).

### Tools required

* [Visual Studio](https://visualstudio.microsoft.com/) 2017 or later editions with the following workloads installed:
  * [Mobile development with .NET (Xamarin)](https://visualstudio.microsoft.com/vs/visual-studio-workloads/)
  * [.NET desktop development (.NET framework)](https://visualstudio.microsoft.com/vs/visual-studio-workloads/)
  * [.NET Core](https://dotnet.microsoft.com/download)
* [Docfx](https://github.com/dotnet/docfx) - to generate the API documentation
* [Cake](https://cakebuild.net/) - Cross-platform build script tool used to build the projects and run the tests.

## Useful resources

* [Using High-Performance C++ Libraries in Cross-Platform Xamarin.Forms Applications](https://devblogs.microsoft.com/xamarin/using-c-libraries-xamarin-forms-apps/)
* [Native interoperability](https://docs.microsoft.com/en-us/dotnet/standard/native-interop/)
* [Interop with Native Libraries](https://www.mono-project.com/docs/advanced/pinvoke/)
* [Using Native Libraries in Xamarin.Android](https://docs.microsoft.com/en-us/xamarin/android/platform/native-libraries)
* [Referencing Native Libraries in Xamarin.iOS](https://docs.microsoft.com/en-us/xamarin/ios/platform/native-interop)

## Copyrights

Copyrights in the Safe Network are retained by their contributors. No copyright assignment is required to contribute to this project.

## Further Help

Get your developer related questions clarified on [Safe Dev Forum](https://forum.safedev.org/). If you're looking to share any other ideas or thoughts on the Safe Network you can reach out on [Safe Network Forum](https://safenetforum.org/)

## License

This Safe Network library is dual-licensed under the Modified BSD ([LICENSE-BSD](LICENSE-BSD) https://opensource.org/licenses/BSD-3-Clause) or the MIT license ([LICENSE-MIT](LICENSE-MIT) https://opensource.org/licenses/MIT) at your option.

## Contributing

Want to contribute? Great :tada:

There are many ways to give back to the project, whether it be writing new code, fixing bugs, or just reporting errors. All forms of contributions are encouraged!

For instructions on how to contribute, see our [Guide to contributing](https://github.com/maidsafe/QA/blob/master/CONTRIBUTING.md).
