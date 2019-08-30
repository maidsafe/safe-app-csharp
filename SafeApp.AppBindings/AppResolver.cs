using System;

namespace SafeApp.AppBindings
{
    public static class AppResolver
    {
#if !NETSTANDARD
        private static readonly Lazy<IAppBindings> Implementation = new Lazy<IAppBindings>(
          CreateBindings,
          System.Threading.LazyThreadSafetyMode.PublicationOnly);
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

        private static IAppBindings CreateBindings()
        {
#if NETSTANDARD && !__DESKTOP__
            return null;
#else
            return new AppBindings();
#endif
        }

        private static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
              "This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
