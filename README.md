# safe_app_csharp

|NuGet Package|CI - NetCore master|
|:-----------:|:-----------------:|
|[![NuGet](https://img.shields.io/nuget/v/MaidSafe.SafeApp.svg)](https://www.nuget.org/packages/MaidSafe.SafeApp)|[![Build status](https://ci.appveyor.com/api/projects/status/x3m722rvosw2coao/branch/master?svg=true)](https://ci.appveyor.com/project/MaidSafe-QA/safe-app-csharp/branch/master)|

safe_app CSharp Library. Currently supports
- Xamarin.Android ( >=4.1.2. ABI: armeabi-v7a, x86)
- Xamarin.iOS ( >= 1.0, ABI: ARM64, x64)
- netstandard1.3 (for usage via portable libs)
- netcoreapp1.0 (for use via NET Core targets. Runtime support limited to x64)
- netframework46 (for use via classic NET Framework targets. Platform support limited to x64)

**Note**: Just adding a package dependency to the Portable Library will not be enough. Package also needs to be added to the corresponding app projects for platform specific Native libraries to get included.

| [MaidSafe website](https://maidsafe.net) | [SAFE Dev Forum](https://forum.safedev.org) | [SAFE Network Forum](https://safenetforum.org) |
|:----:|:----:|:----:|


# TODO
- [ ] Extend Native API Scope to full Alpha-2 client APIs.
- [ ] Tests
  - [x] Setup test targets in currently supported platforms
  - [x] Setup basic CI suite - currently supports NetCore
  - [ ] Expand test cases to cover the Managed API scope
- [ ] Mock integration
  - [x] Support mock network via `SAFE_APP_MOCK` conditional symbol
  - [ ] Publish package including mock network support.
- [ ] Add other compatible target frameworks
  - [x] NETCore
  - [x] NetFramework
  - [ ] Xamarin.Mac
- [ ] Managed API Docs.
- [ ] Code Samples.

## License

Licensed under either of

* the MaidSafe.net Commercial License, version 1.0 or later ([LICENSE](LICENSE))
* the General Public License (GPL), version 3 ([COPYING](COPYING) or http://www.gnu.org/licenses/gpl-3.0.en.html)

at your option.

## Contribution

Unless you explicitly state otherwise, any contribution intentionally submitted for inclusion in the
work by you, as defined in the MaidSafe Contributor Agreement ([CONTRIBUTOR](CONTRIBUTOR)), shall be
dual licensed as above, and you agree to be bound by the terms of the MaidSafe Contributor Agreement.
