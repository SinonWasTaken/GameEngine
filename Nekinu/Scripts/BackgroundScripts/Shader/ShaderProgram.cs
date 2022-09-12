using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace NekinuSoft
{
    //The base shader class
    public abstract class ShaderProgram
    {
        //The id for the shader program
        public int program_id { get; private set; }
        //The id for the vertex shader
        public int vertex { get; private set; }
        //The id for the geometry shader
        public int geometry { get; private set; }
        //The id for the fragment shader
        public int fragment { get; private set; }

        //The vertex shader files
        public string vertex_file { get; private set; }
        //The geometry shader files
        public string geomtry_file { get; private set; }
        //The fragment shader files
        public string fragment_file { get; private set; }

        public ShaderProgram(string vertex, string fragment)
        {
            //creates the shader program id
            program_id = GL.CreateProgram();

            //setting the vertex shader file
            vertex_file = vertex;
            //setting the fragment shader file
            fragment_file = fragment;

            //sets the vertex shader, giving it the shader information
            this.vertex = createShader(vertex, ShaderType.VertexShader);
            //sets the fragment shader, giving it the shader information
            this.fragment = createShader(fragment, ShaderType.FragmentShader);

            //Attaches both shaders to the main shader program
            GL.AttachShader(program_id, this.vertex);
            GL.AttachShader(program_id, this.fragment);

            //Binds the shader attributes. Position, normal, texture coordinates
            BindAttributes();

            //Links the shader program
            GL.LinkProgram(program_id);

            //Produces a log for the shader program
            string log = GL.GetProgramInfoLog(program_id);

            //if the log is not empty, then
            if (log != String.Empty)
            {
                //Generates a crash report
                Crash_Report.generate_crash_report($"Error linking program! {log}");
                //Exits the program with exit code -126. 
                Environment.Exit(-126);
            }

            //Validates the shader program
            GL.ValidateProgram(program_id);

            //Loads all shader uniforms
            GetAllUniformLocations();
        }

        //Method to create the shader
        private int createShader(string location, ShaderType type)
        {
            //Creates a shader, giving it a shader type, Vertex, Fragment or Geometry
            int shader = GL.CreateShader(type);

            //variable that contains the entire shader text
            string o = "";

            try
            {
                //GLSL doesnt have any way to include files, so this method does it for me
                o = getIncludeFiles(location);
            }
            //If the include file doesnt exist, or is broken, then throw an error and crash report
            catch (Exception e)
            {
                Crash_Report.generate_crash_report($"Error getting included shader! {e}");
                //Exit code -130 is my code for shader include error
                Environment.Exit(-130);
            }

            //If the shader isn't empty
            if (o != String.Empty)
            {
                //Loads the shader file
                GL.ShaderSource(shader, o);

                //Compiles the shader
                GL.CompileShader(shader);

                //int that produces an error code
                int i = 0;

                //Gets the shader status
                GL.GetShader(shader, ShaderParameter.CompileStatus, out i);

                //If the shader has failed to compile
                if (i != 1)
                {
                    //Gets the shader log
                    string log = GL.GetShaderInfoLog(shader);
                    //Generates a crash report
                    Crash_Report.generate_crash_report("Error compiling shader! " + log + " ShaderType: " + type.ToString() + " " + shader);
                    //Exit code -127 is the error code for shader compilation failure
                    Environment.Exit(-127);
                }

                //Returns the shader
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
            //then adds the constructs code after
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

        //Used to bind a shader attribute
        protected void BindAttribute(int attribute, string name)
        {
            GL.BindAttribLocation(program_id, attribute, name);
        }

        //Gets all uniform variables
        public virtual void GetAllUniformLocations() { }

        //Gets a uniform location
        protected int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(program_id, name);
        }

        //Loads a float into the shader
        protected void Uniform1f(int location, float value)
        {
            GL.Uniform1(location, value);
        }
        
        //Loads a vector2 into the shader
        protected void Uniform2f(int location, Vector2 value)
        {
            GL.Uniform2(location, value.x, value.y);
        }
        //Loads a vector2 into the shader with 2 floats
        protected void Uniform2f(int location, float x, float y)
        {
            GL.Uniform2(location, x, y);
        }

        //Loads a vector3 into the shader
        protected void Uniform3f(int location, Vector3 value)
        {
            GL.Uniform3(location, value.x, value.y, value.z);
        }
        //Loads a vector3 into the shader with 3 floats
        protected void Uniform3f(int location, float x, float y, float z)
        {
            GL.Uniform3(location, x, y, z);
        }

        //Loads a vector4 into the shader
        protected void Uniform4f(int location, Vector4 value)
        {
            GL.Uniform4(location, value.x, value.y, value.z, value.w);
        }
        //Loads a vector4 into the shader with 4 floats
        protected void Uniform4f(int location, float X, float Y, float Z, float W)
        {
            GL.Uniform4(location, X, Y, Z, W);
        }

        //Loads a matrix4 into the shader
        protected void UniformMatrix4(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, true, ref matrix);
        }

        //Loads a bool into the shader with a int value being passed
        protected void UniformBool(int location, bool value)
        {
            GL.Uniform1(location, value == true ? 1 : 0);
        }

        //Binds the shader program
        public void Bind()
        {
            GL.UseProgram(program_id);
        }

        //Unbinds the shader program
        public void Unbind()
        {
            GL.UseProgram(0);
        }

        //Cleans up the shader
        public void CleanUp()
        {
            //Removes the vertex shader from the shader program
            GL.DetachShader(program_id, vertex);
            //Removes the geometry shader from the shader program
            GL.DetachShader(program_id, geometry);
            //Removes the fragment shader from the shader program
            GL.DetachShader(program_id, fragment);

            //Deletes the vertex shader from memory
            GL.DeleteShader(vertex);
            //Deletes the fragment shader from memory
            GL.DeleteShader(fragment);
            //Deletes the geometry shader from memory
            GL.DeleteShader(geometry);

            //Deletes the shader program from memory
            GL.DeleteProgram(program_id);
        }
    }
}