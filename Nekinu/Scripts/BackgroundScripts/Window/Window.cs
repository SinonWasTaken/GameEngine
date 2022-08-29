using System.ComponentModel;
using NekinuSoft;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NekinuSoft
{
    public class Window : GameWindow
    {
        private Time time;

        public static Window window;

        public Window(string title, int width, int height, bool full_screen = false) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Title = title;

            WindowState = full_screen ? WindowState.Fullscreen : WindowState.Normal;

            ResourceGetter.Init_Assembly();

            Size = new Vector2i(width, height);
            GL.Viewport(0, 0, width, height);
            
            WindowSize.UpdateSize(width, height);

            Cache.InitCache();

            window = this;

            /*byte[] data;
            
            using (var stream = new MemoryStream())
            {
                Resources.Icon.Save(stream, ImageFormat.Png);
                data = stream.ToArray();
            }
            
            Icon = new WindowIcon(new Image(256,256, data));*/
            
            NekinuSoft.MouseState.Init();
            NekinuSoft.MouseState.Set_Mouse_Visible(true);
            
            time = new Time();
            
            MakeCurrent();
            VSync = VSyncMode.Off;
            
            SceneManager.InitSceneManager();
            MasterRenderer.InitMasterRenderer(new StandardRenderer());
            
            AudioSystem.InitAudio();
            
            new Input(this);
        }

        public void Start()
        {
            Run();
        }

        public void Grab_Cursor()
        {
            CursorGrabbed = true;
        }

        public void Release_Cursor()
        {
            CursorGrabbed = false;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            time.Update();

            SceneManager.UpdateScene();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            ProcessEvents();
            SwapBuffers();
            base.OnRenderFrame(args);

            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.Back);

            GL.ClearColor(0.34901960784313725490196078431373f, 0.85882352941176470588235294117647f, 0.81960784313725490196078431372549f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            MasterRenderer.Render();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Size = e.Size;

            GL.Viewport(0, 0, e.Width, e.Height);
            
            WindowSize.UpdateSize(e.Width, e.Height);
            
            MasterRenderer.Resize(e.Width, e.Height);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SceneManager.CloseScene();
            
            AudioSystem.CleanAudio();
            
            MasterRenderer.CloseRenderer();
            Cache.FlushCache();
        }
    }
}