using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace NekinuSoft.TextureLoader;

public class TextureLoader
{
    //https://stackoverflow.com/questions/11645368/opengl-c-sharp-opentk-load-and-draw-image-functions-not-working
    public static int loadTexture(string file)
    {
        Texture t = Cache.TextureExists(file);
        if (t == null)
        {
            try
            {
                int texture;
                StreamReader reader = new StreamReader(file);
                Bitmap bitmap = (Bitmap) Bitmap.FromStream(reader.BaseStream);
                    
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D((TextureTarget) TextureTarget.Texture2D, 0,  PixelInternalFormat.Rgba, data.Width,
                    data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                    data.Scan0);

                bitmap.UnlockBits(data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                return texture;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading texture! {e}");
                return -1;
            }
        }

        return t.id;
    }
    
    public static int loadTexture(string name, Stream stream)
    {
        Texture t = Cache.TextureExists(name);
        if (t == null)
        {
            try
            {
                int texture;
                StreamReader reader = new StreamReader(stream);
                Bitmap bitmap = (Bitmap) Bitmap.FromStream(reader.BaseStream);
                    
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                texture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, texture);

                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0,  PixelInternalFormat.Rgba, data.Width,
                    data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte,
                    data.Scan0);

                bitmap.UnlockBits(data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

                //Mip maps lowers texture resolution at a distance, reducing texture flickers at a distance
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureLodBias, -0.4f);
                
                GL.BindTexture(TextureTarget.Texture2D, 0);

                return texture;
            }
            catch (Exception e)
            {
                Crash_Report.generate_crash_report($"Error loading texture! {e}");
                Environment.Exit(-206);
                return -1;
            }
        }

        return t.id;
    }
}