using NekinuSoft;
using OpenTK.Mathematics;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Vector2 = NekinuSoft.Vector2;
using Vector3 = NekinuSoft.Vector3;
using Vector4 = NekinuSoft.Vector4;

public static class Screen
{
    //Converts the point on the screen into world position
    public static Vector3 ScreenToWorld(Camera camera)
    {
        return calculate_ray(camera);
    }

    //Draws a ray from the mouse position to the world
    private static Vector3 calculate_ray(Camera camera)
    {
        float mouse_X = Input.Get_Mouse_X;
        float mouse_Y = Input.Get_Mouse_Y;

        //Not sure how this works
        Vector2 normalized_position = normalize_screen_position(mouse_X, mouse_Y);
        Vector4 clip = new Vector4(normalized_position.x, normalized_position.y, -1, 1);
        Vector4 eyes = to_eye_space(clip, camera);
        Vector3 world = to_world_coords(eyes, camera);

        return world;
    }

    private static Vector3 to_world_coords(Vector4 eye, Camera camera)
    {
        Matrix4 inverted_view = Matrix4.Invert(camera.View);

        Matrix4x4 inverted = new Matrix4x4(inverted_view.M11, inverted_view.M12, inverted_view.M13, inverted_view.M14,
            inverted_view.M21, inverted_view.M22, inverted_view.M23, inverted_view.M24, inverted_view.M31,
            inverted_view.M32, inverted_view.M33, inverted_view.M34, inverted_view.M41, inverted_view.M42,
            inverted_view.M43, inverted_view.M44);

        System.Numerics.Vector4 ray =
            System.Numerics.Vector4.Transform(new System.Numerics.Vector4(eye.x, eye.y, eye.z, eye.w), inverted);

        Vector3 mouse = new Vector3(ray.X, ray.Y, ray.Z);
        
        return mouse.Normalize();
    }

    private static Vector4 to_eye_space(Vector4 clip, Camera camera)
    {
        Matrix4 inverted_projection = Matrix4.Invert(camera.Projection);

        Matrix4x4 inverted = new Matrix4x4(inverted_projection.M11, inverted_projection.M12, inverted_projection.M13, inverted_projection.M14,
            inverted_projection.M21, inverted_projection.M22, inverted_projection.M23, inverted_projection.M24, inverted_projection.M31,
            inverted_projection.M32, inverted_projection.M33, inverted_projection.M34, inverted_projection.M41, inverted_projection.M42,
            inverted_projection.M43, inverted_projection.M44);
        
        System.Numerics.Vector4 ray =
            System.Numerics.Vector4.Transform(new System.Numerics.Vector4(clip.x, clip.y, clip.z, clip.w), inverted);

        return new Vector4(ray.X, ray.Y, -1, 0);
    }

    private static Vector2 normalize_screen_position(float x_pos, float y_pos)
    {
        float x = (2f * x_pos) / WindowSize.Width - 1;
        float y = (2f * y_pos) / WindowSize.Height - 1;

        //might need to change this to x, -y
        return new Vector2(x, -y);
    }
}