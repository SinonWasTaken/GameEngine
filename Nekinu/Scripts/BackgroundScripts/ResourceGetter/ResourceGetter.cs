using System.Reflection;

namespace NekinuSoft
{
    //All resources are stored in an embedded file. This class allows users to load files into their games 
    public static class ResourceGetter
    {
        //An array of assemblies. assemblies are, as far as a I can tell, project files. The engine is one assembly, the project that is using the engine is another assembly
        private static Assembly[] assemblies;

        //Loads all assemblies
        public static void Init_Assembly()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        //Gets a string resource file from the assemblies with its name and extension
        public static string Get_Resource_File_Of_Type_String(string name, string extension)
        {
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    string[] lines= s.Split('.');
                
                    string line = lines[lines.Length - 2];

                    if (line == name)
                    {
                        if (s.EndsWith(extension))
                        {
                            using (Stream stream = assembly.GetManifestResourceStream(s))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            
            return string.Empty;
        }
        //Gets a stream resource file from the assemblies with its name and extension
        public static Stream Get_Resource_File_Of_Type_Stream(string name, string extension)
        {
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    string[] lines= s.Split('.');
                
                    string line = lines[lines.Length - 2];

                    if (line == name)
                    {
                        if (s.EndsWith(extension))
                        {
                            return assembly.GetManifestResourceStream(s);
                        }
                    }
                }
            }
            
            return null;
        }

        //Gets all string resource files from the assemblies with an extension
        public static string[] Get_Resource_File_Of_Type_String(string extension)
        {
            List<string> files = new List<string>();
            
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    if (s.EndsWith(extension))
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(s))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                files.Add(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            
            return files.ToArray();
        }
        //Gets all stream resource files from the assemblies with an extension
        public static Stream[] Get_Resource_File_Of_Type_Stream(string extension)
        {
            List<Stream> files = new List<Stream>();
            
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    if (s.EndsWith(extension))
                    {
                        files.Add(assembly.GetManifestResourceStream(s));
                    }
                }
                
            }
            
            return files.ToArray();
        }

        //Gets all string resource files from the assemblies within a folder
        public static string[] Get_All_Resource_Files_Of_Type_String(string folder_name)
        {
            List<string> files = new List<string>();
            
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    if (s.Contains(folder_name))
                    {
                        using (Stream stream = assembly.GetManifestResourceStream(s))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                files.Add(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            
            return files.ToArray();
        }
        
        //Gets a string resource file from the assemblies within a folder, a file name and an extension
        public static string Get_Resource_Files_From_Folder_Of_Type_String(string folder_name, string file, string extension)
        {
            foreach (Assembly assembly in assemblies)
            {
                string[] strings = assembly.GetManifestResourceNames();

                foreach (string s in strings)
                {
                    if (s.Contains(folder_name))
                    {
                        if (s.Contains(file) && s.Contains(extension))
                        {
                            using (Stream stream = assembly.GetManifestResourceStream(s))
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    return reader.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}