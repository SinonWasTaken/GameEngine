using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class FrameBuffer
    {
        //A struct that contains the width and height of the frame buffer
        public struct FrameBufferSpecification
        {
            public int width, height;
        }

        //The id of the frame buffer
        private int id;
        
        //the location where the color texture is stored
        public int colorBuffer { get; private set; }
        //Not used yet. Where the depth texture is stored
        private int depthBuffer;
        
        public FrameBufferSpecification specification { get; private set; }

        //Constructor
        public FrameBuffer(FrameBufferSpecification specification)
        {
            this.specification = specification;

            //used to set a default value to the buffer pointer
            colorBuffer = -1;
            depthBuffer = -1;
            
            //Creates the frame buffer
            Create();
        }

        //Enables the frame buffer to be used
        public void Bind()
        {
            //Binds the frame buffer
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, id);
            
            //Bind the 2 textures to be used
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
        }

        //Unbinds the frame buffer
        public void Unbind()
        {
            //Unbinds the buffer
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            
            //Unbinds both textures
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void Create()
        {
            GL.CreateFramebuffers(1, out id);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
            int colorID;
            GL.CreateTextures(TextureTarget.Texture2D, 1, out colorID);
            colorBuffer = colorID;

            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, specification.width, specification.height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.CreateTextures(TextureTarget.Texture2D, 1, out depthBuffer);
            GL.BindTexture(TextureTarget.Texture2D, depthBuffer);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Depth24Stencil8, specification.width, specification.height, 0, PixelFormat.DepthStencil, PixelType.UnsignedInt248, IntPtr.Zero);

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, TextureTarget.Texture2D, depthBuffer, 0);

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                Crash_Report.generate_crash_report(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer));
                Environment.Exit(-5);
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Delete()
        {
            //Removes the frame buffer and textures from memory
            GL.DeleteFramebuffer(id);
            GL.DeleteTexture(colorBuffer);
            GL.DeleteTexture(depthBuffer);
        }

        //Resizes the frame buffer
        public void Resize(Vector2i size)
        {
            //Unbinds the frame buffer
            Unbind();
            //Deletes the frame buffer information
            Delete();
            //Creates a new framebuffer specification
            FrameBufferSpecification spec = new FrameBufferSpecification();
            //Uses the new window size to resize the framebuffer
            spec.height = size.Y;
            spec.width = size.X;
            //sets the old specification to the new
            specification = spec;
            //Creates the new framebuffer
            Create();
        }
    }
}