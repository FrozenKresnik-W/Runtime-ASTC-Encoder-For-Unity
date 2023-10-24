using Aperture.Astcenc.Runtime;
using System.IO;
using UnityEngine;

public class Sample : MonoBehaviour
{
    /// <summary>
    /// input texture should be read/write enabled and uncompressed
    /// </summary>
    public Texture2D inputTexture;
    public Texture2D outputTexture;

    void Start()
    {
        using (AstcRaw raw = inputTexture.CompressTexture(TextureType.Default, BlockSize.ASTC_6x6, CompressQuality.Fast))
        {
            if(raw != null)
            {
                raw.ThrowIfDisposed();

                outputTexture = new Texture2D(inputTexture.width, inputTexture.width, TextureFormat.ASTC_6x6, inputTexture.mipmapCount > 0, false);
                outputTexture.name = inputTexture.name;
                outputTexture.LoadRawTextureData(raw);
                outputTexture.Apply(true, true);
            }
        }

        byte[] fileData = inputTexture.EncodeToASTC();
        string filePath = Path.Combine(Application.dataPath, "outputTexture.astc");
        using (FileStream fs = File.OpenWrite(filePath))
        {
            fs.Write(fileData);
            fs.Close();
        }
    }
}
