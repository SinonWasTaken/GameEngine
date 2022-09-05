using System;

namespace Nekinu
{
    //I have a custom editor for this engine. It would allow me to place objects in the scene, while the game isnt playing. This would allow variables to be edited within the editor
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class SerializedPropertyAttribute : Attribute { }
}