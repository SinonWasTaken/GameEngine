using System;

namespace Nekinu
{
    //Supposed to allow a variable to show up in the editor. Not sure if I got this working
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class SerializedPropertyAttribute : Attribute { }
}