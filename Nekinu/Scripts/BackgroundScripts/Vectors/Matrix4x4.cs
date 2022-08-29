using OpenTK.Mathematics;

namespace NekinuSoft
{
    public class Matrix4x4
    {
        public static Matrix4 entityTransformationMatrix(Entity Parent, Transform transform)
        {
            Matrix4 result;

            Matrix4 transformMatrix =
                Matrix4.CreateTranslation(transform.position.x, transform.position.y, transform.position.z);

            Matrix4 rot = CreateRotationMatrixEuler(transform.rotation);

            Matrix4 scale = Matrix4.CreateScale(transform.scale.x, transform.scale.y, transform.scale.z);

            result = Matrix4.Mult(scale, rot);
            result = Matrix4.Mult(result, transformMatrix);

            if (Parent != null)
            {
                result *= Parent.TransformationMatrix;
            }

            return result;
        }

        public static Matrix4 Create_UI_Matrix(Entity entity)
        {
            Vector3 new_position = new Vector3(entity.Transform.position.x / (WindowSize.Width / 2f),
                entity.Transform.position.y / (WindowSize.Height / 2f), 0f);

            Vector3 new_rotation = new Vector3(entity.Transform.rotation.x, entity.Transform.rotation.y, 0);
            
            Matrix4 transformMatrix =
                Matrix4.CreateTranslation(new_position.x, new_position.y, new_position.z);
            
            Matrix4 rot = CreateRotationMatrixEuler(new_rotation);

            Matrix4 scale = Matrix4.CreateScale(entity.Transform.scale.x / WindowSize.AspectRatio, entity.Transform.scale.y, 1);
            
            Matrix4 result = Matrix4.Mult(scale, rot);
            
            return Matrix4.Mult(result, transformMatrix);
        }

        public static Matrix4 Create_Default_Matrix(Vector3 position)
        {
            Matrix4 result; 
            Matrix4 transformMatrix =
                Matrix4.CreateTranslation(position.x, position.y, position.z);

            Matrix4 rot = CreateRotationMatrixEuler(Vector3.zero);

            Matrix4 scale = Matrix4.CreateScale(1, 1, 1);

            result = Matrix4.Mult(scale, rot);
            result = Matrix4.Mult(result, transformMatrix);

            return result;
        }
        
        public static Matrix4 CreateRotationMatrixEuler(Vector3 rotation)
        {
            Matrix4 x = Matrix4.CreateRotationX(rotation.x * Math.Math.ToRadians);
            Matrix4 y = Matrix4.CreateRotationY(rotation.y * Math.Math.ToRadians);
            Matrix4 z = Matrix4.CreateRotationZ(rotation.z * Math.Math.ToRadians);

            return z * y * x;
        }

        public static Matrix4 CreateCameraRotationMatrixEuler(Vector3 rotation)
        {
            Matrix4 x = Matrix4.CreateRotationX(rotation.x * Math.Math.ToRadians);
            Matrix4 y = Matrix4.CreateRotationY((rotation.y - 180) * Math.Math.ToRadians);
            Matrix4 z = Matrix4.CreateRotationZ(rotation.z * Math.Math.ToRadians);

            return z * y * x;
        }

        public static Matrix4 entityTransformationMatrix(Transform transform)
        {
            return entityTransformationMatrix(null, transform);
        }

        public static Matrix4 cameraTransformationMatrix(Transform transform)
        {
            return cameraTransformationMatrix(null, transform);
        }

        public static Matrix4 cameraTransformationMatrix(Entity Parent, Transform transform)
        {
            Matrix4 transformMatrix =
                Matrix4.CreateTranslation(-transform.position.x, -transform.position.y, -transform.position.z);

            Matrix4 rot = CreateCameraRotationMatrixEuler(transform.rotation);

            Matrix4 result = transformMatrix * rot;

            if (Parent != null)
            {
                result *= Parent.TransformationMatrix;
            }

            return result;
        }
    }
}