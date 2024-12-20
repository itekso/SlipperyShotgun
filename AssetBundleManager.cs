using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SlipperyShotgun
{
    public static class AssetBundleManager
    {
        private static AssetBundle? _bundle;

        public static AssetBundle LoadEmbeddedAssetBundle()
        {
            if (_bundle != null)
            {
                return _bundle;
            }

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith("slippery"));

            if (resourceName == null)
            {
                SlipperyShotgun.Logger.LogError("Failed to find embedded resource ending with 'slippery'");
                return null!;
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    SlipperyShotgun.Logger.LogError($"Failed to load embedded resource: {resourceName}");
                    return null!;
                }

                byte[] bundleData = new byte[stream.Length];
                stream.Read(bundleData, 0, bundleData.Length);

                _bundle = AssetBundle.LoadFromMemory(bundleData);
                if (_bundle == null)
                {
                    SlipperyShotgun.Logger.LogError("Failed to load AssetBundle from memory!");
                    return null!;
                }

                return _bundle;
            }
        }
    }
}