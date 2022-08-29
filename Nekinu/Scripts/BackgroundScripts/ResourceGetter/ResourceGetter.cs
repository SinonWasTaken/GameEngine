using System.Reflection;

namespace NekinuSoft
{
    public static class ResourceGetter
    {
        private static Assembly[] assemblies;

        public static void Init_Assembly()
        {
            assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

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