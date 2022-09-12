using OpenTK.Graphics.OpenGL;

//Not used
namespace NekinuSoft
{
    public class ShadowFrameBuffer
    {
        private int width, height;

        private int framebuffer;
        private int shadow_map;

        public ShadowFrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            init_frame_buffer();
        }

        private void init_frame_buffer()
        {
            framebuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            shadow_map = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, shadow_map);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent16, width, height, 0,
                PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Nearest);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, shadow_map, 0);
        }

        protected void bindFrameBuffer()
        {
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, framebuffer);
            GL.BindTexture(TextureTarget.Texture2D, shadow_map);
        }

        protected void unbindFrameBuffer()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
        }

        protected void cleanUp()
        {
            GL.DeleteFramebuffer(framebuffer);
            GL.DeleteTexture(shadow_map);
        }
    }
}