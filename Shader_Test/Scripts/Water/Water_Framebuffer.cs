using OpenTK.Graphics.ES30;

public class Water_Framebuffer
{
    protected const int REFLECTION_WIDTH = 320;
    protected const int REFLECTION_HEIGHT = 180;
	
    protected const int REFRACTION_WIDTH = 1280;
    protected const int REFRACTION_HEIGHT = 720;
    
    private int reflectionFrameBuffer;
    private int reflectionTexture;
    private int reflectionDepthBuffer;
	
    private int refractionFrameBuffer;
    private int refractionTexture;
    private int refractionDepthTexture;

    public Water_Framebuffer()
    {
	    init_Reflection_Buffer();
	    init_Refraction_Buffer();
    }

    public void Bind_Reflection()
    {
	    GL.BindTexture(TextureTarget.Texture2D, 0);
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, reflectionFrameBuffer);
    }
    
    public void Bind_Refraction()
    {
	    GL.BindTexture(TextureTarget.Texture2D, 0);
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, refractionFrameBuffer);
    }

    public void Unbind()
    {
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }
    
    private void init_Reflection_Buffer()
    {
	    reflectionFrameBuffer = create_Frame_Buffer();
	    reflectionTexture = create_Texture_Attachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
	    reflectionDepthBuffer = create_Depth_Buffer(REFLECTION_WIDTH, REFLECTION_HEIGHT);
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    private void init_Refraction_Buffer()
    {
	    refractionFrameBuffer = create_Frame_Buffer();
	    refractionTexture = create_Texture_Attachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
	    refractionDepthTexture = create_Depth_Texture_Attachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); 
    }
    
    private int create_Depth_Buffer(int width, int height) 
    {
	    int depthBuffer = GL.GenRenderbuffer();
	    
	    GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
	    GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent32f, width, height);
	    GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
	    
	    return depthBuffer;
    }
    private int create_Frame_Buffer()
    {
	    int id = GL.GenFramebuffer();
	    GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
	    
	    DrawBufferMode[] mode = new DrawBufferMode[1];
	    mode[0] = DrawBufferMode.ColorAttachment0;
	    
	    GL.DrawBuffers(reflectionFrameBuffer, mode);

	    return id;
    }
    private int create_Texture_Attachment(int w, int h)
    {
	    int texture = GL.GenTexture();
	    GL.BindTexture(TextureTarget.Texture2D, texture);
	    GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgb, w, h, 0, PixelFormat.Rgb, PixelType.UnsignedByte, IntPtr.Zero);
	    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);
	    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
	    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget2d.Texture2D, texture, 0);

	    return texture;
    }

    private int create_Depth_Texture_Attachment(int w, int h)
    {
	    int texture = GL.GenTexture();
	    GL.BindTexture(TextureTarget.Texture2D, texture);
	    GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.DepthComponent32f, w, h, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
	    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);
	    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
	    GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget2d.Texture2D, texture, 0);

	    return texture;
    }

    public int Reflection_Texture => reflectionTexture;
    public int Refraction_Texture => refractionTexture;
    
    public void CleanUp()
    {
	    GL.DeleteBuffer(reflectionFrameBuffer);
	    GL.DeleteTexture(reflectionTexture);
	    GL.DeleteRenderbuffer(reflectionDepthBuffer);
	    GL.DeleteBuffer(refractionFrameBuffer);
	    GL.DeleteTexture(refractionTexture);
	    GL.DeleteRenderbuffer(refractionDepthTexture);
    }
}