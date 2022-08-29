using FileBrowser;
using ImGuiNET;
using NekinuSoft.Scene_Manager;

namespace NekinuSoft.Editor
{
    internal class DockPanel : IEditorPanel
    {
        private Entity selectedEntity;
        private Dictionary<string, List<IEditorPanel>> editor_tabs;

        public static bool isPlaying = false;

        ImGuiWindowFlags window_flags;

        private bool getting_name = false;

        public override void Init()
        {
            editor_tabs = new Dictionary<string, List<IEditorPanel>>();

            window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;

            window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            window_flags |= ImGuiWindowFlags.NoBackground;
            
            HierarchyPanel.ItemSelected += HierarchyPanelOnItemSelected;
        }

        private void HierarchyPanelOnItemSelected(Entity entity)
        {
            selectedEntity = entity;
        }

        public override void Render()
        {
            ImGuiViewportPtr port = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(port.Pos);
            ImGui.SetNextWindowSize(port.Size);

            ImGui.SetNextWindowViewport(port.ID);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, System.Numerics.Vector2.Zero);
            bool open = true;

            ImGui.Begin("DockSpace", ref open, window_flags);
            ImGui.PopStyleVar();
            ImGui.PopStyleVar(2);

            uint id = ImGui.GetID("MyDockSpace");
            ImGui.DockSpace(id);

            ImGui.DockSpaceOverViewport(port);

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New Scene"))
                    {
                        Scene scene = new Scene("Scene");
                        Entity camera = new Entity("Camera");
                        camera.AddComponent<Camera>();
                        Entity yest = new Entity("Test");
                        camera.AddChild(yest);
                        scene.AddEntity(camera);
                        scene.AddEntity(yest);
                        
                        SceneManager.AddScene(scene);
                        SceneManager.LoadScene(0);
                        /*Thread thre = new Thread(new ThreadStart(() =>
                        {
                            FileBrowser.MainWindow.saveFile("T", ".tile", Directory.GetCurrentDirectory());
                        }));
                        
                        thre.SetApartmentState(ApartmentState.STA);
                        thre.Start();*/
                    }

                    if (ImGui.MenuItem("Save Scene"))
                    {
                        Thread thre = new Thread(new ThreadStart(() =>
                        {
                            MainWindow.saveFile("Save", ".scene", Directory.GetCurrentDirectory());
                        }));
                        
                        thre.SetApartmentState(ApartmentState.STA);
                        thre.Start();
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("GameObject"))
                {
                    if (ImGui.MenuItem("Create Empty"))
                    {
                        if (SceneManager.hasLoadedScene != false)
                        {
                            if (selectedEntity == null)
                            {
                                SceneManager.AddEntityToScene(new Entity("Empty", new Transform()));
                            }
                            else
                            {
                                Entity child = new Entity("Empty", new Transform(selectedEntity.Transform.position, selectedEntity.Transform.rotation, Vector3.one));
                                selectedEntity.AddChild(child);
                                SceneManager.AddEntityToScene(child);
                            }
                        }
                        else
                        {
                            Debug.WriteErrorLog("there is no scene to add entities to!");
                        }
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }

            ImGui.End();
        }
    }
}
