using NekinuSoft;
using NekinuSoft.Renderer;
using OpenTK.Mathematics;

public class ShadowMapRenderer : IRenderer
{
    private const int Shadow_Map_Size = 2048;

    private ShadowFrameBuffer buffer;
    private ShadowBox box;
    private Matrix4 projection;
    private Matrix4 lightView = new Matrix4();
    private Matrix4 projection_View;
    //private Matrix4 offset = createOffset();

    private Camera camera;

    public ShadowMapRenderer()
    {
        buffer = new ShadowFrameBuffer(Shadow_Map_Size, Shadow_Map_Size);
    }

    //Used to render shadows. Clearly doesnt work
    
    /*public override void Render(Camera camera)
    {
        base.Render(camera);

        if (this.camera == null)
        {
            this.camera = camera;
            box = new ShadowBox(lightView, camera);
        }
        
        box.update();
        Vector3 sunPosition = sun.getPosition();
        Vector3 lightDirection = new Vector3(-sunPosition.x, -sunPosition.y, -sunPosition.z);
        prepare(lightDirection, shadowBox);
        entityRenderer.render(entities);
        finish();
    }

    public Matrix4f getToShadowMapSpaceMatrix() {
		return Matrix4f.mul(offset, projectionViewMatrix, null);
	}
	
	public void cleanUp() {
		shader.cleanUp();
		shadowFbo.cleanUp();
	}

	public int getShadowMap() {
		return shadowFbo.getShadowMap();
	}

	protected Matrix4f getLightSpaceTransform() {
		return lightViewMatrix;
	}

	private void prepare(Vector3f lightDirection, ShadowBox box) {
		updateOrthoProjectionMatrix(box.getWidth(), box.getHeight(), box.getLength());
		updateLightViewMatrix(lightDirection, box.getCenter());
		Matrix4f.mul(projectionMatrix, lightViewMatrix, projectionViewMatrix);
		shadowFbo.bindFrameBuffer();
		GL11.glEnable(GL11.GL_DEPTH_TEST);
		GL11.glClear(GL11.GL_DEPTH_BUFFER_BIT);
		shader.start();
	}

	private void finish() {
		shader.stop();
		shadowFbo.unbindFrameBuffer();
	}
	private void updateLightViewMatrix(Vector3 direction, Vector3 center) 
	{
		direction.Normalize();
		center = Vector3.negative(center);
		lightView = Matrix4.Identity;
		float pitch = (float) Math.Acos(new Vector2(direction.x, direction.z).Length());
			
		Matrix4f.rotate(pitch, new Vector3f(1, 0, 0), lightViewMatrix, lightViewMatrix);
		float yaw = (float) Math.toDegrees(((float) Math.atan(direction.x / direction.z)));
		yaw = direction.z > 0 ? yaw - 180 : yaw;
		Matrix4f.rotate((float) -Math.toRadians(yaw), new Vector3f(0, 1, 0), lightViewMatrix,
				lightViewMatrix);
		Matrix4f.translate(center, lightViewMatrix, lightViewMatrix);
	}

	private void updateOrthoProjectionMatrix(float width, float height, float length) 
	{
		projection = Matrix4.Identity;
		projection.M11 = 2f / width;
		projection.M22 = 2f / height;
		projection.M33 = -2f / length;
		projection.M44 = 1;
	}

	private static Matrix4 createOffset() 
	{
		Matrix4 offset = new Matrix4();
		offset.translate(new Vector3f(0.5f, 0.5f, 0.5f));
		offset.scale(new Vector3f(0.5f, 0.5f, 0.5f));
		return offset;
	}*/
}