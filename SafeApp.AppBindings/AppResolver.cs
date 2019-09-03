using System;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace SafeApp.AppBindings
{
    [PublicAPI]
    public static class AppResolver
    {
#if !NETSTANDARD
        private static readonly Lazy<IAppBindings> Implementation = new Lazy<IAppBindings>(
          CreateBindings,
          System.Threading.LazyThreadSafetyMode.PublicationOnly);
#endif

        [PublicAPI]
        public static IAppBindings Current
        {
            get
            {
#if NETSTANDARD
                throw NotImplementedInReferenceAssembly();
#else
                return Implementation.Value;
#endif
            }
        }

#if !NETSTANDARD
        private static IAppBindings CreateBindings()
        {
            return new AppBindings();
        }
#endif

        private static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
              "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
#pragma warning restore 1591
