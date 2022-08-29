using NekinuSoft;
using OpenTK.Mathematics;
using Vector3 = NekinuSoft.Vector3;
using Vector4 = NekinuSoft.Vector4;
using Matrix4x4 = System.Numerics.Matrix4x4;

public class ShadowBox
{
    private const float OFFSET = 10;
    private const float SHADOW_DISTANCE = 100;

    private float minX, maxX;
    private float minY, maxY;
    private float minZ, maxZ;

    private Matrix4 lightViewMatrix;
    private Camera camera;

    private float farHeight, farWidth, nearHeight, nearWidth;

    public ShadowBox(Matrix4 lightView, Camera camera)
    {
        lightViewMatrix = lightView;
        this.camera = camera;
        calculateWidthsAndHeights();
    }

    public void update()
    {
        Matrix4 rotation = calculateCameraRotationMatrix();

        Vector4 fin = Transform(rotation, Vector4.Forward());
        
        Vector3 forwardVector = new Vector3(fin.x, fin.y, fin.z);

        Vector3 toFar = new Vector3(forwardVector);
        toFar *= SHADOW_DISTANCE;
        Vector3 toNear = new Vector3(forwardVector);
        toNear *= 0.001f;
        Vector3 centerNear = toNear + camera.Parent.Transform.position;
        Vector3 centerFar = toFar + camera.Parent.Transform.position;

        Vector4[] points = calculateFrustumVertices(rotation, forwardVector, centerNear,
            centerFar);

        bool first = true;
        foreach (Vector4 point in points) 
        {
            if (first) 
            {
                minX = point.x;
                maxX = point.x;
                minY = point.y;
                maxY = point.y;
                minZ = point.z;
                maxZ = point.z;
                first = false;
                continue;
            }
            if (point.x > maxX) 
            {
                maxX = point.x;
            }
            else if (point.x < minX) 
            {
                minX = point.x;
            }
            if (point.y > maxY) 
            {
                maxY = point.y;
            }
            else if (point.y < minY) 
            {
                minY = point.y;
            }
            if (point.z > maxZ) 
            {
                maxZ = point.z;
            }
            else if (point.z < minZ) 
            {
                minZ = point.z;
            }
        }
        maxZ += OFFSET;
    }
    
    protected Vector3 getCenter() 
    {
        float x = (minX + maxX) / 2f;
        float y = (minY + maxY) / 2f;
        float z = (minZ + maxZ) / 2f;
        Vector4 cen = new Vector4(x, y, z, 1);
        Matrix4 invertedLight = new Matrix4();
        invertedLight = Matrix4.Invert(lightViewMatrix);

        Vector4 fin = Transform(invertedLight, cen);
        
        return new Vector3(fin.x, fin.y, fin.z);
    }

    public float get_Width()
    {
        return maxX - minX;
    }

    public float get_Height()
    {
        return maxY - minY;
    }

    public float get_Z()
    {
        return maxZ - minZ;
    }
    
    private Vector4[] calculateFrustumVertices(Matrix4 rotation, Vector3 forwardVector, Vector3 centerNear, Vector3 centerFar)
    {
        Vector4 fin = Transform(rotation, Vector4.Up());
        
        Vector3 upVector = new Vector3();
        Vector3 rightVector = Vector3.Cross(forwardVector, upVector);
        Vector3 downVector = new Vector3(-upVector.x, -upVector.y, -upVector.z);
        Vector3 leftVector = new Vector3(-rightVector.x, -rightVector.y, -rightVector.z);
        Vector3 farTop = centerFar + new Vector3(upVector.x * farHeight, upVector.y * farHeight, upVector.z * farHeight);
        Vector3 farBottom = centerFar + new Vector3(downVector.x * farHeight, downVector.y * farHeight, downVector.z * farHeight);
        Vector3 nearTop = centerNear + new Vector3(upVector.x * nearHeight, upVector.y * nearHeight, upVector.z * nearHeight);
        Vector3 nearBottom = centerNear + new Vector3(downVector.x * nearHeight, downVector.y * nearHeight, downVector.z * nearHeight);
        
        Vector4[] points = new Vector4[8];
        points[0] = calculateLightSpaceFrustumCorner(farTop, rightVector, farWidth);
        points[1] = calculateLightSpaceFrustumCorner(farTop, leftVector, farWidth);
        points[2] = calculateLightSpaceFrustumCorner(farBottom, rightVector, farWidth);
        points[3] = calculateLightSpaceFrustumCorner(farBottom, leftVector, farWidth);
        points[4] = calculateLightSpaceFrustumCorner(nearTop, rightVector, nearWidth);
        points[5] = calculateLightSpaceFrustumCorner(nearTop, leftVector, nearWidth);
        points[6] = calculateLightSpaceFrustumCorner(nearBottom, rightVector, nearWidth);
        points[7] = calculateLightSpaceFrustumCorner(nearBottom, leftVector, nearWidth);
        return points;
    }
    
    private Vector4 calculateLightSpaceFrustumCorner(Vector3 startPoint, Vector3 direction, float width) 
    {
        Vector3 point = startPoint + new Vector3(direction.x * width, direction.y * width, direction.z * width);
        Vector4 point4f = new Vector4(point.x, point.y, point.z, 1f);
        point4f = Transform(lightViewMatrix, point4f);
        return point4f;
    }
    
    private Matrix4 calculateCameraRotationMatrix() 
    {
        Matrix4 rotation = new Matrix4();

        rotation = NekinuSoft.Matrix4x4.CreateRotationMatrixEuler(camera.Parent.Transform.rotation);
        
        return rotation;
    }
    
    private void calculateWidthsAndHeights() 
    {
        farWidth = (float) (SHADOW_DISTANCE * Math.Tan(NekinuSoft.Math.Math.ToRadians * camera.Fov));
        nearWidth = (float) (0.0001f * Math.Tan(NekinuSoft.Math.Math.ToRadians * camera.Fov));

        farHeight = farWidth / WindowSize.AspectRatio;
        nearHeight = nearWidth / WindowSize.AspectRatio;
    }
    
    private Vector4 Transform(Matrix4 mat, Vector4 vec)
    {
        Matrix4x4 inverted = new Matrix4x4(mat.M11, mat.M12, mat.M13, mat.M14,
            mat.M21, mat.M22, mat.M23, mat.M24, mat.M31,
            mat.M32, mat.M33, mat.M34, mat.M41, mat.M42,
            mat.M43, mat.M44);
        
        System.Numerics.Vector4 ray =
            System.Numerics.Vector4.Transform(new System.Numerics.Vector4(vec.x, vec.y, vec.z, vec.w), inverted);

        return new Vector4(ray.X, ray.Y, ray.Z, ray.W);
    }
}