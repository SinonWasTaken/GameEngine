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
        //A list of all 3D models loaded into the game
        private static List<Mesh> mesh_vaos;
        //A list of all textures loaded into the game
        private static List<Texture> texture_vbos;
        //A list of all audio clips loaded into the game
        private static List<AudioClip> audio_ids;
        //A list of all audio sources in a scene
        private static List<AudioSource> audio_sources;
        //A list of all shaders loaded into the game
        private static List<ShaderProgram> shaders;

        //Initalizes the cache
        public static void InitCache()
        {
            mesh_vaos = new List<Mesh>();
            texture_vbos = new List<Texture>();
            
            audio_ids = new List<AudioClip>();
            audio_sources = new List<AudioSource>();
            
            shaders = new List<ShaderProgram>();
        }

        #region AudioSources

        //Adds an audio source to memory
        public static void AddSource(AudioSource source)
        {
            audio_sources.Add(source);
        }

        //Removes an audio source from the list
        public static void RemoveSource(AudioSource source)
        {
            audio_sources.Remove(source);
        }
        
        #endregion
        
        #region AudioClips

        //Adds an audio clip to the list
        public static void AddAudioClip(AudioClip audioClip)
        {
            audio_ids.Add(audioClip);
        }
        
        //Checks to see if a audio clip with the same name exists
        public static AudioClip AudioClipExists(string file)
        {
            for (int i = 0; i < audio_ids.Count; i++)
            {
                //If an audio clip with same name exists
                if (audio_ids[i].clip == file)
                    //Then return it
                    return audio_ids[i];
            }

            //If not, the return nothing
            return null;
        }

        //Removes the audio clip from the list
        public static void RemoveAudioClip(AudioClip clip)
        {
            audio_ids.Remove(clip);
        }
        
        #endregion
        
        #region Mesh

        //Adds a 3D model to the list
        public static void AddMesh(Mesh mesh)
        {
            mesh_vaos.Add(mesh);
        }

        //similar method to the AudioClipExists method, but with 3D models 
        public static Mesh MeshExists(string file)
        {
            for (int i = 0; i < mesh_vaos.Count; i++)
            {
                if (mesh_vaos[i].MeshLocation == file)
                    return mesh_vaos[i];
            }

            return null;
        }

        //Removes the model from the list
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

        //Removes all loaded data from memory
        public static void FlushCache()
        {
            //Deletes meshes from the cache
            foreach (Mesh mesh in mesh_vaos)
            {
                mesh.CleanUp();
            }

            //deletes textures from memory
            foreach (Texture texture in texture_vbos)
            {
                GL.DeleteTexture(texture.id);
            }

            //Deletes shaders from memory
            foreach (ShaderProgram program in shaders)
            {
                program.CleanUp();
            }

            //Deletes audio clips from memory
            foreach (AudioClip clip in audio_ids)
            {
                GL.DeleteBuffer(clip.id);
            }
        }
    }
}