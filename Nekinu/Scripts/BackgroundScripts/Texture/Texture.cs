namespace NekinuSoft
{
    public class Texture : Component, EditorUpdate
    {
        public string texture_file { get; private set; }
        public int id { get; private set; }

        public Texture(string location)
        {
            texture_file = location;
        }

        public override void Awake()
        {
            string texture_extension = texture_file.Split('.')[1];
            id = TextureLoader.TextureLoader.loadTexture($"{texture_file}.{texture_extension}", ResourceGetter.Get_Resource_File_Of_Type_Stream(texture_file, texture_extension));
        }
        
        public void EditorAwake()
        {
            Awake();
        }
    }
}