using NekinuSoft;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = NekinuSoft.Window;

public class Camera_Movement : Component
{
    public float speed = 5;

    public float x, y;

    private Vector3 reF_velocity = new Vector3();

    private bool do_movement = true;
    
    public override void Update()
    {
        base.Update();

        if (Input.is_key_pressed(Keys.Escape))
        {
            do_movement = !do_movement;
            
            if (!do_movement)
            {
                Window.window.CursorVisible = true;
                Window.window.MousePosition = new OpenTK.Mathematics.Vector2(Input.Get_Mouse_X, Input.Get_Mouse_Y);
            }
            else
            {
                Window.window.CursorVisible = false;
                Window.window.CursorGrabbed = true;
            }
        }
        
        if (do_movement)
        {
            do_camera_rotation();

            do_camera_movement();
        }
    }

    private void do_camera_rotation()
    {
        Vector2 input = new Vector2(Input.Get_Mouse_Delta_X * speed, Input.Get_Mouse_Delta_Y * speed);
        
        if (input != Vector2.zero)
        {
            x += input.x;
            y += input.y;
            
            Vector3 target = new Vector3(y, x, 0);
            
            Parent.Transform.rotation = Vector3.SmoothDamp(Parent.Transform.rotation, target, ref reF_velocity, 0.01f);
        }
    }

    private void do_camera_movement()
    {
        if (Input.is_key_down(Keys.W))
        {
            Parent.Transform.position += Parent.Transform.forward * Time.deltaTime * speed;
        }
        else if (Input.is_key_down(Keys.S))
        {
            Parent.Transform.position -= Parent.Transform.forward * Time.deltaTime * speed;
        }

        if (Input.is_key_down(Keys.A))
        {
            Parent.Transform.position += Parent.Transform.right * Time.deltaTime * speed;
        }
        else if (Input.is_key_down(Keys.D))
        {
            Parent.Transform.position -= Parent.Transform.right * Time.deltaTime * speed;
        }

        if (Input.is_key_down(Keys.Space))
        {
            Parent.Transform.position += Parent.Transform.up * Time.deltaTime * speed;
        }
    }
}