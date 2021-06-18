using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLEngine
{
    public class AssetPathGetter
    {
        /// <summary>
        /// This method is added so that the assets used for testing the engine can be distributed with the engine itself.
        /// Originally the assets file paths were hard coded to locations thoughout my pc with the intention of eventually having a small
        /// number of default textures and models which would have some relative paths. Most models and textures would need to be arguments
        /// from the software using the engine of course which is currently supported I think on everything.
        /// </summary>
        /// <returns>string the relative file path of the assets folder.</returns>
        public static string GetAssetsPath()
        {
            DirectoryInfo assetPath = System.IO.Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location);
            assetPath = System.IO.Directory.GetParent(assetPath.FullName);
            assetPath = System.IO.Directory.GetParent(assetPath.FullName);
            assetPath = System.IO.Directory.GetParent(assetPath.FullName);
            return assetPath.FullName + "\\Assets";
        }
    }
}
