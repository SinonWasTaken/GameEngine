using FileBrowser;
using ImGuiNET;
using NekinuSoft.Scene_Manager;

namespace NekinuSoft.Editor
{
    internal class DockPanel : IEditorPanel
    {
        //Docks a editor windows to the window
        private Entity selectedEntity;
        //all editor windows
        private Dictionary<string, List<IEditorPanel>> editor_tabs;

        //is the game playing
        public static bool isPlaying = false;

        ImGuiWindowFlags window_flags;

        public override void Init()
        {
            //Initializes a dictionary
            editor_tabs = new Dictionary<string, List<IEditorPanel>>();

            //Adds a menu bard
            window_flags = ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoDocking;

            //Cant collapse, the window has no title, the window cant be resized, and it cants be moved
            window_flags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            //no background colors
            window_flags |= ImGuiWindowFlags.NoBackground;
            
            HierarchyPanel.ItemSelected += HierarchyPanelOnItemSelected;
        }

        //Called when a user clicks on a new ui entity
        private void HierarchyPanelOnItemSelected(Entity entity)
        {
            selectedEntity = entity;
        }

        public override void Render()
        {
            //Gets the main window
            ImGuiViewportPtr port = ImGui.GetMainViewport();
            //Gets the next editor window position
            ImGui.SetNextWindowPos(port.Pos);
            //Gets the next editor window size
            ImGui.SetNextWindowSize(port.Size);

            //Gets the next window id
            ImGui.SetNextWindowViewport(port.ID);
            //No window rounding
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            //there is no border
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);

            //There is no padding between windows
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, System.Numerics.Vector2.Zero);
            bool open = true;

            //Creates a new window
            ImGui.Begin("DockSpace", ref open, window_flags);
            //Removes the 3 style vars from above
            ImGui.PopStyleVar();
            ImGui.PopStyleVar(2);

            //Gets the dock id
            uint id = ImGui.GetID("MyDockSpace");
            
            //Docking over the window
            ImGui.DockSpace(id);
            ImGui.DockSpaceOverViewport(port);

            //Begins rendering a menu bar
            if (ImGui.BeginMenuBar())
            {
                //Creates a new menu item called file
                if (ImGui.BeginMenu("File"))
                {
                    //Creates a new menu item that will create new scenes
                    if (ImGui.MenuItem("New Scene"))
                    {
                        //Creates a new scene
                        Scene scene = new Scene("Scene");
                        //Creates the scene camera
                        Entity camera = new Entity("Camera");
                        camera.AddComponent<Camera>();
                        //A test entity
                        Entity yest = new Entity("Test");
                        camera.AddChild(yest);
                        scene.AddEntity(camera);
                        scene.AddEntity(yest);
                        
                        //Adds the scene to the scene manager and loads it
                        SceneManager.AddScene(scene);
                        SceneManager.LoadScene(0);
                        /*Thread thre = new Thread(new ThreadStart(() =>
                        {
                            FileBrowser.MainWindow.saveFile("T", ".tile", Directory.GetCurrentDirectory());
                        }));
                        
                        thre.SetApartmentState(ApartmentState.STA);
                        thre.Start();*/
                    }

                    //Saves the scene
                    if (ImGui.MenuItem("Save Scene"))
                    {
                        //Opens the file browser, so the user can choose where to save the scene
                        Thread thre = new Thread(new ThreadStart(() =>
                        {
                            MainWindow.saveFile("Save", ".scene", Directory.GetCurrentDirectory());
                        }));
                        
                        thre.SetApartmentState(ApartmentState.STA);
                        thre.Start();
                    }
                    ImGui.EndMenu();
                }

                //Will create a new entity in the scene
                if (ImGui.BeginMenu("GameObject"))
                {
                    //Creates a blank entity
                    if (ImGui.MenuItem("Create Empty"))
                    {
                        //Only if there is a loaded scene
                        if (SceneManager.hasLoadedScene != false)
                        {
                            //If an entity is not selected
                            if (selectedEntity == null)
                            {
                                //Add the entity to the scene
                                SceneManager.AddEntityToScene(new Entity("Empty", new Transform()));
                            }
                            else
                            {
                                //Adds the entity to a parent entity
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
