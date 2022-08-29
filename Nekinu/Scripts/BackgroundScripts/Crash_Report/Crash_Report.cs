namespace NekinuSoft
{
    public class Crash_Report
    {
        //Creates a crash report for the user to read or send
        public static void generate_crash_report(object error)
        {
            //Gets the current date/time
            DateTime time = DateTime.Now;

            //If there aren't any crash reports, create a directory to store them
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "/Crash Reports"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/Crash Reports");
            }

            //Removes any ':' and splits the time
            string[] n = time.ToString().Replace(":", "_").Split('/');

            string name = "";

            //Creates the crash report name
            for (int i = 0; i < n.Length; i++)
            {
                name += n[i];
                if (i < n.Length - 1)
                {
                    name += " ";
                }
            }

            //Creates a file for the program to write to.
            FileStream stream = File.Create($"{Directory.GetCurrentDirectory()}/Crash Reports/{name}.crsh");

            //Loads the filestream
            StreamWriter writer = new StreamWriter(stream);

            //and writes the error to the file
            writer.WriteLine(error.ToString());

            //before closing them file
            writer.Close();
        }
    }
}