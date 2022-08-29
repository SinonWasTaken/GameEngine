namespace NekinuSoft.UI
{
    public class Image : UI_Class
    {
        public Image() : base(new Vector4(1, 1, 1, 1)) { }

        public Image(string texture_name, string texture_extension, Vector4 color) : base(texture_name, texture_extension, color) { }
    }
}