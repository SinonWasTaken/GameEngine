using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace FileBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
        }

        [STAThread]
        public static Dictionary<Stream, string> getFile(string title, string file_extension, string starting_directory)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = title;
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Files (*" + file_extension + ") | *" + file_extension + "*";
            openFileDialog.InitialDirectory = starting_directory;
            openFileDialog.ShowDialog();
            try
            {
                Dictionary<Stream, string> a = new Dictionary<Stream, string>();
                a.Add(openFileDialog.OpenFile(), openFileDialog.FileName);
                return a;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error getting file! {0}", (object)ex));
                return null;
            }
        }

        [STAThread]
        public static string saveFile(string title, string save_extension, string default_directory)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = title;
            saveFileDialog.InitialDirectory = default_directory;
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = save_extension;
            saveFileDialog.Filter = $"File (* {save_extension}) | * {save_extension}";
            saveFileDialog.ShowDialog();
            return saveFileDialog.FileName;
        }
    }
}
