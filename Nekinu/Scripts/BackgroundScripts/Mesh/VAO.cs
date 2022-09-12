using OpenTK.Graphics.ES30;

namespace NekinuSoft
{
    public class VAO
    {
        //The object that contains mesh information
        public int vao { get; private set; }

        //Contains information on buffer objects, such as textures, normals etc..
        private List<int> vbos;

        public int VAOID => vao;

        public List<int> VBOS => vbos;

        public VAO()
        {
        }

        //Initializes the vao data
        public void createVAO()
        {
            vbos = new List<int>();
            vao = GL.GenVertexArray();
        }

        //Binds the vao for use
        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        //Binds the attributes of the mesh. textures, normals etc...
        public void BindVertexAttribute()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        //Unbinds the attributes
        public void UnbindVertexAttribute()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.DisableVertexAttribArray(i);
            }
        }

        //Unbinds the vao
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        //Binds the indices and adds them to the vbo buffer
        public void bindIndicesBuffer(int[] indicies)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(int), indicies,
                BufferUsageHint.StaticDraw);
            vbos.Add(vbo);
        }

        //Stores float (points, coordinates) information in the vao
        public void storeData(int number, int size, float[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }

        //Stores int (locations in memory) information in the vao
        public void storeIntData(int number, int size, int[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Int, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }

        //Removes all vao data from memory
        public void CleanUp()
        {
            GL.DeleteVertexArray(vao);

            for (int i = 0; i < vbos.Count; i++)
            {
                GL.DeleteBuffer(vbos[i]);
            }
        }
    }
}