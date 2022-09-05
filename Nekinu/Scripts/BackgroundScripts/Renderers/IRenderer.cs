using OpenTK.Mathematics;

namespace NekinuSoft.Renderer
{
    //Base class for any render class
    public class IRenderer
    {
        public virtual void Render(Camera camera) { }

        public virtual void Render() { }

        public virtual void OnResize(Vector2i size) { }
        
        public virtual void Close() { }
    }
}