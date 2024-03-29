﻿using Nekinu;

namespace NekinuSoft.UI
{
    //The base ui class
    public class UI_Class : Component
    {
        //The mesh of the ui
        private Mesh ui_mesh;
        //The texture of the ui
        private Texture ui_texture;
        
        [SerializedProperty] private Vector4 color;

        //Constructor
        public UI_Class(Vector4 color)
        {
            ui_mesh = MeshLoader.MeshLoader.loadOBJ(ResourceGetter.Get_Resource_File_Of_Type_String("Image", ".obj"));
            this.color = color;
        }

        public UI_Class(string texture_name, string texture_extension, Vector4 color)
        {
            ui_mesh = MeshLoader.MeshLoader.loadOBJ(ResourceGetter.Get_Resource_File_Of_Type_String("Image", ".obj"));
            ui_texture = new Texture($"{texture_name}.{texture_extension}");
            this.color = color;
        }

        public virtual void Is_Mouse_Over(Camera camera) { }

        public Mesh UiMesh => ui_mesh;
        public Texture UiTexture => ui_texture;

        public virtual Vector4 Color
        {
            set { color = value; }
            get { return color; }
        }
    }
}