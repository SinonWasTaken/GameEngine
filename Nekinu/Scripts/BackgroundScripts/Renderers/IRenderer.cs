using OpenTK.Mathematics;

namespace NekinuSoft.Renderer
{
    public class IRenderer
    {
        public virtual void Render(Camera camera) { }

        public virtual void Render() { }

        public virtual void OnResize(Vector2i size) { }
        
        public virtual void Close() { }
    }
}