using System.ComponentModel;
using NekinuSoft;
using NekinuSoft.NyanToWorking.ServerSide;
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

        private Scene _scene = new Scene("Scene");
        Server server = new Server(5, 22);
        
        public Window(string title, int width, int height, bool full_screen = false) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            Entity camera = new Entity("Camera");
            camera.AddComponent<Camera>();
            
            _scene.AddEntity(camera);
            
            server.Start_Server();
            Debug.InitDebugLogging();
            Title = title;

            //Inits the resource getter class
            ResourceGetter.Init_Assembly();

            //Sets the window size
            Size = new Vector2i(width, height);
            //Puts the window to the middle of the screen
            GL.Viewport(0, 0, width, height);
            
            //Sets the window to fullscreen if the user wants it to be
            WindowState = full_screen ? WindowState.Fullscreen : WindowState.Normal;
            
            //Updates the window size class
            WindowSize.UpdateSize(width, height);

            //Inits the cache class
            Cache.InitCache();
            

            window = this;

            //Commented code was meant to produce the window icon, but it doesnt work
            
            /*byte[] data;
            
            using (var stream = new MemoryStream())
            {
                Resources.Icon.Save(stream, ImageFormat.Png);
                data = stream.ToArray();
            }
            
            Icon = new WindowIcon(new Image(256,256, data));*/

            //Starts the time
            time = new Time();
            
            //Makes the thread the main
            MakeCurrent();
            //Turns vsync off
            VSync = VSyncMode.Off;
            
            //Starts the scene
            SceneManager.InitSceneManager();
            //Adds a default renderer to the master renderer class
            MasterRenderer.InitMasterRenderer(new StandardRenderer());
            
            Entity model = new Entity("Model", new Transform(new Vector3(0,0, 10)));
            Mesh mesh = MeshLoader.MeshLoader.loadOBJ("world_vending_machine.obj");
            model.AddComponent(mesh);
            _scene.AddEntity(model);
            SceneManager.AddScene(_scene);
            SceneManager.LoadScene(0);
            
            //Starts the audio class
            AudioSystem.InitAudio();
            
            //Starts the input class, getting the inputs from this window
            new Input(this);
        }

        //External project use
        public void Start()
        {
            Run();
        }

        //When the program updates
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            time.Update();

            SceneManager.UpdateScene();
        }

        //When a frame is rendered
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //Enabled events
            ProcessEvents();
            //Prepares the frame to be rendered to
            SwapBuffers();
            base.OnRenderFrame(args);

            //Enables the ability to stops rendering the faces of a mesh
            GL.Enable(EnableCap.CullFace);
            //Allows the game to determine the depth of each rendered object
            GL.Enable(EnableCap.DepthTest);

            //Stops rendering the back side of a mesh
            GL.CullFace(CullFaceMode.Back);

            //Sets the background window color
            GL.ClearColor(0.34901960784313725490196078431373f, 0.85882352941176470588235294117647f, 0.81960784313725490196078431372549f, 1);
            //Clears the color of the window
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Renders the scene
            MasterRenderer.Render();
        }

        //When the window is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            Size = e.Size;

            GL.Viewport(0, 0, e.Width, e.Height);
            
            WindowSize.UpdateSize(e.Width, e.Height);
            
            MasterRenderer.Resize(e.Width, e.Height);
        }

        //When the window is closed
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            //Closes the loaded scene
            SceneManager.CloseScene();
            
            //Stops the audio class from producing audio
            AudioSystem.CleanAudio();
            
            //Stops rendering
            MasterRenderer.CloseRenderer();
            //Removes all loaded objects from memory
            Cache.FlushCache();
            server.End();
        }
    }
}