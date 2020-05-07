using System;

#pragma warning disable 1591

namespace SafeApp.AppBindings
{
    public static class AppResolver
    {
#if NETSTANDARD
        private static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
              "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
#else
        private static readonly Lazy<IAppBindings> Implementation = new Lazy<IAppBindings>(
          CreateBindings,
          System.Threading.LazyThreadSafetyMode.PublicationOnly);

        private static IAppBindings CreateBindings()
        {
            return new AppBindings();
        }
#endif

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
    }
}
#pragma warning restore 1591
