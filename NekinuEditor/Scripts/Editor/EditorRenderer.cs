using OpenTK.Windowing.Desktop;

namespace NekinuSoft.Editor
{
    internal class EditorRenderer
    {
        //All initialized editor windows
        private static List<IEditorPanel> editor_panels;

        //Gets input from the editor
        private static ImGuiController controller;

        //The main window
        private static GameWindow window;

        //Starts the renderer
        public static void Init(GameWindow wind)
        {
            window = wind;

            //Initializes the editor window list
            editor_panels = new List<IEditorPanel>();

            //starts the editor input class
            controller = new ImGuiController(window.Size.X, window.Size.Y, wind);

            //Adds editor windows to the list
            editor_panels.Add(new DockPanel());
            editor_panels.Add(new ContentBrowser());
            editor_panels.Add(new DebugPanel());
            editor_panels.Add(new ScenePanel());
            editor_panels.Add(new HierarchyPanel());
            editor_panels.Add(new PropertiesPanel());

            //Initializes all editor windows
            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Init();
            }
        }

        //Renders
        public static void Render()
        {
            //updates the input
            controller.Update(window, Time.deltaTime);

            //renders the panels
            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Render();
            }

            controller.Render();
        }

        //Called when the window is resized
        public static void OnResize(int width, int height)
        {
            controller?.WindowResized(width, height);
        }

        //Disposes the input class
        public static void Dispose()
        {
            controller.Dispose();
        }

        //Adds a editor window and initializes it
        public static void addEditor(IEditorPanel editor)
        {
            editor.Init();
            editor_panels.Add(editor);
        }

        //Checks to see if the editorRender has a window already
        public static bool hasEditor<T>() where T : IEditorPanel
        {
            Type editor = typeof(T);
            for (int i = 0; i < editor_panels.Count; i++)
            {
                if((editor_panels[i].GetType() == editor))
                {
                    return true;
                }
            }

            return false;
        }
        
        //Gets an editor of a specific type
        public static T getEditor<T>() where T : IEditorPanel
        {
            Type editor = typeof(T);
            for (int i = 0; i < editor_panels.Count; i++)
            {
                if((editor_panels[i].GetType() == editor))
                {
                    return (T) editor_panels[i];
                }
            }

            return null;
        }

        //Removes and editor
        public static void removeEditor(IEditorPanel editor)
        {
            editor_panels.Remove(editor);
        }
    }
}
