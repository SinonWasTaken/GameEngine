using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace NekinuSoft
{
    public abstract class ShaderProgram
    {
        public int program_id { get; private set; }
        public int vertex { get; private set; }
        public int geometry { get; private set; }
        public int fragment { get; private set; }

        public string vertex_file { get; private set; }
        public string geomtry_file { get; private set; }
        public string fragment_file { get; private set; }

        public ShaderProgram(string vertex, string fragment)
        {
            program_id = GL.CreateProgram();

            vertex_file = vertex;
            fragment_file = fragment;

            this.vertex = createShader(vertex, ShaderType.VertexShader);
            this.fragment = createShader(fragment, ShaderType.FragmentShader);

            GL.AttachShader(program_id, this.vertex);
            GL.AttachShader(program_id, this.fragment);

            BindAttributes();

            GL.LinkProgram(program_id);

            string log = GL.GetProgramInfoLog(program_id);

            if (log != String.Empty)
            {
                Crash_Report.generate_crash_report($"Error linking program! {log}");
                Console.WriteLine("Error linking program! " + log);
                Environment.Exit(-126);
            }

            GL.ValidateProgram(program_id);

            GetAllUniformLocations();
        }

        private int createShader(string location, ShaderType type)
        {
            int shader = GL.CreateShader(type);

            string o = "";

            try
            {
                o = getIncludeFiles(location);
            }
            catch (Exception e)
            {
                Crash_Report.generate_crash_report($"Error loading shader! {e}");
                Environment.Exit(-130);
            }

            if (o != String.Empty)
            {
                GL.ShaderSource(shader, o);

                GL.CompileShader(shader);

                int i = 0;

                GL.GetShader(shader, ShaderParameter.CompileStatus, out i);

                if (i != 1)
                {
                    string log = GL.GetShaderInfoLog(shader);
                    Crash_Report.generate_crash_report("Error compiling shader! " + log + " ShaderType: " + type.ToString() + " " + shader);
                    Environment.Exit(-127);
                }

                return shader;
            }
            else
            {
                Crash_Report.generate_crash_report($"Error! Shader: {shader} is empty!");
                Environment.Exit(-126);
                return -1;
            }
        }

        private string getIncludeFiles(string src)
        {
            //the embedded string 
            string out_string = new StringReader(src).ReadToEnd();

            //all lines in the embedded string
            List<string> lines = out_string.Split(Environment.NewLine).ToList();

            //lines that contain "Constructs". IDK What they are actually called, but an example is -- vec4 get_light_color(); --
            List<string> construct = new List<string>();
            //where as this list contains the actual code vec4 get_light_color() { code here; }
            List<string> lines_To_Add_At_End = new List<string>();
            
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("#include"))
                {
                    string file_to_get = lines[i].Split(" ")[1];
                    
                    string f = file_to_get.Split(".")[0].Replace("\"", "");
                    string extension = "." + file_to_get.Split(".")[1].Replace("\"", "");

                    string file = new StringReader(ResourceGetter.Get_Resource_File_Of_Type_String(f, extension)).ReadToEnd();

                    if (file != string.Empty)
                    {
                        string[] new_lines = file.Split(Environment.NewLine);
                        
                        construct.Add(new_lines[0]);
                        lines_To_Add_At_End.Add(new_lines[1]);
                    }
                    else
                    {
                        Console.WriteLine($"Error loading #include file! Could not find file {file_to_get}");
                    }

                    lines[i] = "";
                }
            }

            //removes the main method from the shader so the constructs and code can be added in the correct place
            int start = 0;

            List<string> main = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Contains("START"))
                {
                    start = i;
                    break;
                }
            }

            if (start != 0)
            {
                for (int i = start; i < lines.Count; i++)
                {
                    main.Add(lines[i]);
                }
            }

            if (start != 0)
            {
                lines.RemoveRange(start, lines.Count - start);
            }
            //end of the removing main method code

            //adds constructs first
            lines.AddRange(construct);
            //then re adds the main method
            lines.AddRange(main);
            //then adds the constucts code after
            lines.AddRange(lines_To_Add_At_End);

            out_string = "";

            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].Contains("START"))
                {
                    if (!lines[i].Contains("END"))
                    {
                        out_string += lines[i] + Environment.NewLine;
                    }
                }
            }

            return out_string;
        }

        public virtual void BindAttributes() { }

        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(program_id, attribute, name);
        }

        public virtual void GetAllUniformLocations() { }

        protected int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(program_id, name);
        }

        protected void Uniform1f(int location, float value)
        {
            GL.Uniform1(location, value);
        }
        
        protected void Uniform2f(int location, Vector2 value)
        {
            GL.Uniform2(location, value.x, value.y);
        }
        protected void Uniform2f(int location, float x, float y)
        {
            GL.Uniform2(location, x, y);
        }

        protected void Uniform3f(int location, Vector3 value)
        {
            GL.Uniform3(location, value.x, value.y, value.z);
        }
        protected void Uniform3f(int location, float x, float y, float z)
        {
            GL.Uniform3(location, x, y, z);
        }
        protected void Uniform3f(int location, Vector3 value, int size)
        {
            GL.Uniform3(location, value.x, value.y, value.z);
        }
        protected void Uniform3f(int location, float x, float y, float z, int size)
        {
            GL.Uniform3(location, x, y, z);
        }

        protected void Uniform4f(int location, Vector4 value)
        {
            GL.Uniform4(location, value.x, value.y, value.z, value.w);
        }
        protected void Uniform4f(int location, float X, float Y, float Z, float W)
        {
            GL.Uniform4(location, X, Y, Z, W);
        }
        protected void Uniform4f(int location, Vector4 value, int size)
        {
            GL.Uniform4(location, value.x, value.y, value.z, value.w);
        }
        protected void Uniform4f(int location, float X, float Y, float Z, float W, int size)
        {
            GL.Uniform4(location, X, Y, Z, W);
        }

        protected void UniformMatrix4(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, true, ref matrix);
        }

        protected void UniformBool(int location, bool value)
        {
            GL.Uniform1(location, value == true ? 1 : 0);
        }

        public void Bind()
        {
            GL.UseProgram(program_id);
        }

        public void Unbind()
        {
            GL.UseProgram(0);
        }

        public void CleanUp()
        {
            GL.DetachShader(program_id, vertex);
            GL.DetachShader(program_id, geometry);
            GL.DetachShader(program_id, fragment);

            GL.DeleteShader(vertex);
            GL.DeleteShader(fragment);
            GL.DeleteShader(geometry);

            GL.DeleteProgram(program_id);
        }
    }
}