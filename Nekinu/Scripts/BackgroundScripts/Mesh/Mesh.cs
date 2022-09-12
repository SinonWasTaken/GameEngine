using Nekinu;
using Newtonsoft.Json;

namespace NekinuSoft
{
    public class Mesh : Component, EditorUpdate
    {
        //the location where the mesh is located, or the mesh name
        [SerializedProperty] private string mesh_location;
        
        //Contains information on the mesh
        [JsonIgnore]
        public VAO vao { get; private set; }
        [JsonIgnore]
        //The amount of vertices in the mesh
        public int vertex_count { get; private set; }

        //empty constructor for saving and loading
        public Mesh() { }

        public override void Awake()
        {
            base.Awake();

            //Generates the mesh when the game starts, if the mesh has been loaded from a save
            if (mesh_location != String.Empty && vao == null)
            {
                Mesh mesh = MeshLoader.MeshLoader.loadOBJ(mesh_location);
                vao = mesh.vao;
                vertex_count = mesh.vertex_count;
            }
        }
        
        //Only called in the editor
        public void EditorAwake()
        {
            Awake();
        }

        //Binds the mesh for use
        public void Bind()
        {
            if (vao != null)
            {
                vao.Bind();
                vao.BindVertexAttribute();
            }
        }

        //Unbinds the mesh when it is no longer being used
        public void Unbind()
        {
            if (vao != null)
            {
                vao.UnbindVertexAttribute();
                vao.Unbind();
            }
        }

        //Creates the mesh and fills in the information
        public static Mesh CreateNewMesh(string location, int[] indicies, float[] pos, float[] text, float[] normal, float[] tangents)
        {
            VAO vao = new VAO();
            vao.createVAO();
            vao.Bind();
            vao.bindIndicesBuffer(indicies);
            vao.storeData(0, 3, pos);
            vao.storeData(1, 2, text);
            vao.storeData(2, 3, normal);
            vao.storeData(3, 3, tangents);
            vao.Unbind();

            Mesh mesh = new Mesh();
            mesh.mesh_location = location;
            mesh.vao = vao;
            mesh.vertex_count = indicies.Length;
            
            Cache.AddMesh(mesh);
            
            return mesh;
        }

        public string MeshLocation => mesh_location;

        //Removes the mesh from the memory
        public void CleanUp()
        {
            vao.CleanUp();
        }
    }
}