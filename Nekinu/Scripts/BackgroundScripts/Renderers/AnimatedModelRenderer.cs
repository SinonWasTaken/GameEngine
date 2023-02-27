namespace NekinuSoft.Renderer
{
    public class AnimatedModelRenderer : IRenderer
    {
        private AnimatedModelShader shader;

        public AnimatedModelRenderer()
        {
            shader = new AnimatedModelShader("","");
        }

        public override void Render(Camera camera)
        {
            shader.Bind();
            
            shader.Unbind();
        }
    }
}