using OpenTK.Graphics.ES30;

namespace NekinuSoft
{
    /// <summary>
    /// TODO:
    ///     Create a class called Cache_Dictionary.
    ///         This class will store the ids of a specific vao or vbo, and keep track of how many objects in the scene have the id. This will only apply to textures and audios clips. There will be too many blocks in the world to keep track of 
    /// </summary>
    public static class Cache
    {
        private static List<Mesh> mesh_vaos;
        private static List<Texture> texture_vbos;
        private static List<AudioClip> audio_ids;
        private static List<AudioSource> audio_sources;
        private static List<ShaderProgram> shaders;

        public static void InitCache()
        {
            mesh_vaos = new List<Mesh>();
            texture_vbos = new List<Texture>();
            
            audio_ids = new List<AudioClip>();
            audio_sources = new List<AudioSource>();
            
            shaders = new List<ShaderProgram>();
        }

        #region AudioSources

        public static void AddSource(AudioSource source)
        {
            audio_sources.Add(source);
        }

        public static void RemoveSource(AudioSource source)
        {
            audio_sources.Remove(source);
        }
        
        #endregion
        
        #region AudioClips

        public static void AddAudioClip(AudioClip audioClip)
        {
            audio_ids.Add(audioClip);
        }
        
        public static AudioClip AudioClipExists(string file)
        {
            for (int i = 0; i < audio_ids.Count; i++)
            {
                if (audio_ids[i].clip == file)
                    return audio_ids[i];
            }

            return null;
        }

        public static void RemoveAudioClip(AudioClip clip)
        {
            audio_ids.Remove(clip);
        }
        
        #endregion
        
        #region Mesh

        public static void AddMesh(Mesh mesh)
        {
            mesh_vaos.Add(mesh);
        }

        public static Mesh MeshExists(string file)
        {
            for (int i = 0; i < mesh_vaos.Count; i++)
            {
                if (mesh_vaos[i].MeshLocation == file)
                    return mesh_vaos[i];
            }

            return null;
        }

        public static void RemoveMesh(Mesh mesh)
        {
            mesh_vaos.Remove(mesh);
        }

        #endregion

        #region Textures

        public static void AddTexture(Texture texture)
        {
            texture_vbos.Add(texture);
        }

        public static Texture TextureExists(string file)
        {
            foreach (Texture texture in texture_vbos)
            {
                if (texture.texture_file == file)
                    return texture;
            }

            return null;
        }

        #endregion

        #region Shaders

        public static void AddShader(ShaderProgram shader)
        {
            shaders.Add(shader);
        }

        public static ShaderProgram ShaderExists(string vertrex, string fragment)
        {
            foreach (ShaderProgram program in shaders)
            {
                if (program.vertex_file == vertrex && program.fragment_file == fragment)
                    return program;
            }

            return null;
        }

        #endregion

        public static void FlushCache()
        {
            foreach (Mesh mesh in mesh_vaos)
            {
                GL.DeleteVertexArray(mesh.vao.vao);
            }

            foreach (Texture texture in texture_vbos)
            {
                GL.DeleteTexture(texture.id);
            }

            foreach (ShaderProgram program in shaders)
            {
                program.CleanUp();
            }

            foreach (AudioClip clip in audio_ids)
            {
                GL.DeleteBuffer(clip.id);
            }
        }
    }
}