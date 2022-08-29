using System.ComponentModel;
using NekinuSoft.Editor;
using NekinuSoft.Scene_Manager;
using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NekinuSoft
{
    public class Editor_Window : GameWindow
    {
        private Time time;

        public static Editor_Window window;
        
        private EditorRenderer render;

        public static FrameBuffer buffer;

        public Editor_Window(string title) : base(GameWindowSettings.Default,
            NativeWindowSettings.Default)
        {
            FrameBuffer.FrameBufferSpecification spec = new FrameBuffer.FrameBufferSpecification();
            spec.height = Size.Y;
            spec.width = Size.X;
            
            WindowSize.UpdateSize(Size.X, Size.Y);
            
            buffer = new FrameBuffer(spec);
            Title = title;

            WindowState = WindowState.Maximized;

            window = this;

            //CursorGrabbed = true;

            Context.MakeCurrent();
            MakeCurrent();
            VSync = VSyncMode.Off;
            Run();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Debug.InitDebugLogging();
            
            SceneManager.InitSceneManager();
            
            ResourceGetter.Init_Assembly();
            
            MasterRenderer.InitMasterRenderer(new StandardRenderer());
            
            EditorRenderer.Init(this);
            
            Cache.InitCache();

            time = new Time();
            
            new Input(this);
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

            buffer.Bind();
            
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            GL.CullFace(CullFaceMode.Back);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            MasterRenderer.Render();
            buffer.Unbind();
            
            EditorRenderer.Render();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Size = e.Size;

            buffer.Resize(Size);

            GL.Viewport(0, 0, e.Width, e.Height);
            WindowSize.UpdateSize(e.Width, e.Height);
            
            EditorRenderer.OnResize(Size.X, Size.Y);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SceneManager.CloseScene();
            buffer.Delete();

            Cache.FlushCache();
            EditorRenderer.Dispose();
        }
    }
}