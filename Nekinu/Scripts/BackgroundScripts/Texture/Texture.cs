namespace NekinuSoft
{
    public class Texture : Component, EditorUpdate
    {
        //The textures name or location in the files
        public string texture_file { get; private set; }
        //The texture id
        public int id { get; private set; }

        //Default constructor
        public Texture(string location)
        {
            texture_file = location;
        }

        public override void Awake()
        {
            //Splits the texture name into the name and extension. png
            string texture_extension = texture_file.Split('.')[1];
            //Sets the texture id when loading the texture
            id = TextureLoader.TextureLoader.loadTexture($"{texture_file}.{texture_extension}", ResourceGetter.Get_Resource_File_Of_Type_Stream(texture_file, texture_extension));
        }
        
        //Called on awake
        public void EditorAwake()
        {
            Awake();
        }
    }
}