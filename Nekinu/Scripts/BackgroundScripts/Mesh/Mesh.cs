using Nekinu;
using Newtonsoft.Json;

namespace NekinuSoft
{
    public class Mesh : Component, EditorUpdate
    {
        [SerializedProperty] private string mesh_location;
        
        [JsonIgnore]
        public VAO vao { get; private set; }
        [JsonIgnore]
        public int vertex_count { get; private set; }

        public Mesh() { }

        public override void Awake()
        {
            base.Awake();

            if (mesh_location != String.Empty && vao == null)
            {
                Mesh mesh = MeshLoader.MeshLoader.loadOBJ(mesh_location);
                vao = mesh.vao;
                vertex_count = mesh.vertex_count;
            }
        }
        
        public void EditorAwake()
        {
            Awake();
        }

        public void Bind()
        {
            if (vao != null)
            {
                vao.Bind();
                vao.BindVertexAttribute();
            }
        }

        public void Unbind()
        {
            if (vao != null)
            {
                vao.UnbindVertexAttribute();
                vao.Unbind();
            }
        }

        public static Mesh CreateNewMesh(string location, int[] indicies, float[] pos, float[] text, float[] normal, float[] tangents)
        {
            VAO vao = new VAO();
            vao.createVAO();
            vao.Bind();
            vao.bindIndiciesBuffer(indicies);
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
    }
}