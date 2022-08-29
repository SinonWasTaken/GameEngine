using Nekinu_Soft;
using Nekinu_Soft.Scene_Manager;
using Shader_Test.Scripts;

public class Program
{
    public static void Main(string[] args)
    {
        Window window = new Window("Shader test", 1024, 768);
        
        MasterRenderer.Add_Renderer(new Basic_Renderer());
        Init_Scenes();
        
        window.Run();
    }

    private static void Init_Scenes()
    {
        Scene scene = new Scene("");
        Entity camera = new Entity("Camera");
        camera.AddComponent<Camera>();
        scene.Add_Entity(camera);
        
        SceneManager.Add_Scene(scene);
        SceneManager.Load_Scene(0);
    }
}