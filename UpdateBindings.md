# Updating sn_csharp bindings from sn_ffi

* Updating `safe_app` bindings in `sn_csharp`
  * Generate bindings for `sn_ffi`.
  * Update bindings in `SafeApp.AppBindings` project with new generated `sn_ffi` bindings.
  * Update manual files in `SafeApp.AppBindings` project.
  * Update core files in `SafeApp.Core` project.
* Updating `safe_authenticator` bindings in `sn_csharp`
  * Generate bindings for `sn_ffi`.
  * Update bindings in `SafeAuthenticator/AuthBindings.cs` file with new generated `sn_ffi` bindings.
  * Update manual files in `SafeAuthenticator` project.

***Note:** Make sure the changes made in the manual files in `sn_csharp` are synced with `sn_ffi` and vice versa.*
