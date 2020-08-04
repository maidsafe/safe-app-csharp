# Updating safe_app_csharp bindings from safe_ffi

* Updating `safe_app` bindings in `safe_app_csharp`
  * Generate bindings for `safe-ffi`.
  * Update bindings in `SafeApp.AppBindings` project with new generated `safe-ffi` bindings.
  * Update manual files in `SafeApp.AppBindings` project.
  * Update core files in `SafeApp.Core` project.
* Updating `safe_authenticator` bindings in `safe_app_csharp`
  * Generate bindings for `safe-ffi`.
  * Update bindings in `SafeAuthenticator/AuthBindings.cs` file with new generated `safe-ffi` bindings.
  * Update manual files in `SafeAuthenticator` project.

***Note:** Make sure the changes made in the manual files in `safe_app_csharp` are synced with `safe-ffi` and vice versa.*
