using OpenTK.Windowing.Desktop;

namespace NekinuSoft.Editor
{
    internal class EditorRenderer
    {
        private static List<IEditorPanel> editor_panels;

        private static ImGuiController controller;

        private static GameWindow window;

        public static void Init(GameWindow wind)
        {
            window = wind;

            editor_panels = new List<IEditorPanel>();

            controller = new ImGuiController(window.Size.X, window.Size.Y, wind);

            editor_panels.Add(new DockPanel());
            editor_panels.Add(new ContentBrowser());
            editor_panels.Add(new DebugPanel());
            editor_panels.Add(new ScenePanel());
            editor_panels.Add(new HierarchyPanel());
            editor_panels.Add(new PropertiesPanel());

            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Init();
            }
        }

        public static void Render()
        {
            controller.Update(window, Time.deltaTime);

            for (int i = 0; i < editor_panels.Count; i++)
            {
                editor_panels[i].Render();
            }

            controller.Render();
        }

        public static void OnResize(int width, int height)
        {
            controller?.WindowResized(width, height);
        }

        public static void Dispose()
        {
            controller.Dispose();
        }

        public static void addEditor(IEditorPanel editor)
        {
            editor.Init();
            editor_panels.Add(editor);
        }

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

        public static void removeEditor(IEditorPanel editor)
        {
            editor_panels.Remove(editor);
        }
    }
}
