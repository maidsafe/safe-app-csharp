using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SafeApp.AppBindings;

// ReSharper disable ConvertToLocalFunction

namespace SafeApp.MData
{
    /// <summary>
    /// Holds a mutation action to be done to the MutableData entries within one transaction on the network.
    /// Used to change the content of the entries.
    /// </summary>
    [PublicAPI]
    public class MDataEntryActions
    {
        private static readonly IAppBindings AppBindings = AppResolver.Current;
        private SafeAppPtr _appPtr;

        /// <summary>
        /// Initialises an MDataEntryAction object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal MDataEntryActions(SafeAppPtr appPtr)
        {
            _appPtr = appPtr;
        }

        /// <summary>
        /// Add a new Remove-Action in the transaction for removing an existing entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">Corresponding entry key to remove.</param>
        /// <param name="version">The version successor, to handle the concurrency issue.</param>
        /// <returns></returns>
        public Task DeleteAsync(NativeHandle entryActionsH, List<byte> entKey, ulong version)
        {
            return AppBindings.MDataEntryActionsDeleteAsync(_appPtr, entryActionsH, entKey, version);
        }

        private Task FreeAsync(ulong entryActionsH)
        {
            return AppBindings.MDataEntryActionsFreeAsync(_appPtr, entryActionsH);
        }

        /// <summary>
        /// Add a new Insert-Action in the transaction for inserting a new entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">The key to be inserted.</param>
        /// <param name="entVal">The value to be inserted.</param>
        /// <returns></returns>
        public Task InsertAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal)
        {
            return AppBindings.MDataEntryActionsInsertAsync(_appPtr, entryActionsH, entKey, entVal);
        }

        /// <summary>
        /// Get a new handle to perform Mutable Data entry transaction actions.
        /// </summary>
        /// <returns>New Handle to perform entry actions.</returns>
        public async Task<NativeHandle> NewAsync()
        {
            var entryActionsH = await AppBindings.MDataEntryActionsNewAsync(_appPtr);
            return new NativeHandle(_appPtr, entryActionsH, FreeAsync);
        }

        /// <summary>
        /// Add a Update-Action in the transaction to update an existing entry.
        /// </summary>
        /// <param name="entryActionsH">EntryActions handle.</param>
        /// <param name="entKey">The key for which the value to be updated.</param>
        /// <param name="entVal">New value.</param>
        /// <param name="version">The version successor, to handle the concurrency issue.</param>
        /// <returns></returns>
        public Task UpdateAsync(NativeHandle entryActionsH, List<byte> entKey, List<byte> entVal, ulong version)
        {
            return AppBindings.MDataEntryActionsUpdateAsync(_appPtr, entryActionsH, entKey, entVal, version);
        }
    }
}
