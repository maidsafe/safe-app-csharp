using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Nrs API.
    /// </summary>
    public class Nrs
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialise a NRS object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="appPtr">SafeApp pointer.</param>
        internal Nrs(SafeAppPtr appPtr)
            => _appPtr = appPtr;

        /// <summary>
        /// Parses a XorURl string into a XorUrlEncoder.
        /// </summary>
        /// <param name="url">XorURl string to parse.</param>
        /// <returns>New SafeUrl instance.</returns>
        public static Task<SafeUrl> ParseUrlAsync(string url)
            => AppBindings.ParseUrlAsync(url);

        /// <summary>
        /// Create a new NrsMapContainer.
        /// </summary>
        /// <param name="name">NrsMapContainer name, create fails if is already exists.</param>
        /// <param name="link">XorUrl to link to the NrsMapContainer.</param>
        /// <param name="directLink"></param>
        /// <param name="dryRun">Flag denoting whether container will be created locally.</param>
        /// <param name="setDefault">Flag to set the link as default entry.</param>
        /// <returns>
        /// New NrsMapContainer's XorUrl,
        /// new instance of ProcessedEntries containing the list of processed public names,
        /// NrsMap JSON string
        /// </returns>
        public Task<(string, ProcessedEntries, string)> CreateNrsMapContainerAsync(string name, string link, bool directLink, bool dryRun, bool setDefault)
            => AppBindings.CreateNrsMapContainerAsync(_appPtr, name, link, directLink, dryRun, setDefault);

        /// <summary>
        /// Add link to an existing NrsMapContainer.
        /// </summary>
        /// <param name="name">NrsMapContainer name, create fails if is already exists.</param>
        /// <param name="link">XorUrl to link to the NrsMapContainer.</param>
        /// <param name="directLink"></param>
        /// <param name="dryRun">Flag denoting whether container will be updated locally.</param>
        /// <param name="setDefault">Flag to set the link as default entry.</param>
        /// <returns>NrsMap JSON string, XorUrl, new version.</returns>
        public Task<(string, string, ulong)> AddToNrsMapContainerAsync(string name, string link, bool setDefault, bool directLink, bool dryRun)
            => AppBindings.AddToNrsMapContainerAsync(_appPtr, name, link, setDefault, directLink, dryRun);

        /// <summary>
        /// Remove an existing public name from a NrsMapContainer.
        /// </summary>
        /// <param name="name">Public name to remove.</param>
        /// <param name="dryRun">Flag denoting whether container will be updated locally.</param>
        /// <returns>NrsMap JSON string, XorUrl, new version.</returns>
        public Task<(string, string, ulong)> RemoveFromNrsMapContainerAsync(string name, bool dryRun)
            => AppBindings.RemoveFromNrsMapContainerAsync(_appPtr, name, dryRun);

        /// <summary>
        /// Get an NrsMapContainer from the network.
        /// </summary>
        /// <param name="url">XorUrl to fetch the NrsMapContainer.</param>
        /// <returns>NrsMap JSON string, NrsMapContainer's version.</returns>
        public Task<(string, ulong)> GetNrsMapContainerAsync(string url)
            => AppBindings.GetNrsMapContainerAsync(_appPtr, url);
    }
}
