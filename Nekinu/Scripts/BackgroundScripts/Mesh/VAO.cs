using OpenTK.Graphics.ES30;

namespace NekinuSoft
{
    public class VAO
    {
        public int vao { get; private set; }

        private List<int> vbos;

        public int VAOID => vao;

        public List<int> VBOS => vbos;

        public VAO()
        {
        }

        public void createVAO()
        {
            vbos = new List<int>();
            vao = GL.GenVertexArray();
        }

        public void Bind()
        {
            GL.BindVertexArray(vao);
        }

        public void BindVertexAttribute()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.EnableVertexAttribArray(i);
            }
        }

        public void UnbindVertexAttribute()
        {
            for (int i = 0; i < vbos.Count; i++)
            {
                GL.DisableVertexAttribArray(i);
            }
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void bindIndiciesBuffer(int[] indicies)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(int), indicies,
                BufferUsageHint.StaticDraw);
            vbos.Add(vbo);
        }

        public void storeData(int number, int size, float[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }

        public void storeIntData(int number, int size, int[] data)
        {
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * 4, data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(number, size, VertexAttribPointerType.Int, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            vbos.Add(vbo);
        }

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