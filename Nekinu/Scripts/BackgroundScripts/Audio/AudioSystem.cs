using OpenTK.Audio.OpenAL;

namespace NekinuSoft
{
    public class AudioSystem
    {
        //The device that audio will play out of
        private static ALDevice device;
        
        //Called when the program is started
        public static void InitAudio()
        {
            //Gets the device that the audio will play out of
            string source = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);

            //Opens the device to recieve sound
            device = ALC.OpenDevice(source);

            //If there is no audio device, something is clearly wrong and the program needs to exit
            if(device == null)
            {
                //Generates a crash report
                Crash_Report.generate_crash_report($"Failed to load default OpenAl audio device! {source}");
                //Exits the program with an error code -101. My code for audio issues
                Environment.Exit(-101);
            }

            //Makes the current device the main device to play from
            ALC.MakeContextCurrent(ALC.CreateContext(device, new int[0]));

            //Sets the default position of the audio source in the world
            AL.Listener(ALListener3f.Position, 0, 0, 1.0f);
            //And sets how far the sound will travel to 1
            AL.Listener(ALListener3f.Velocity, 0, 0, 1.0f);

            //Dont remember what this model means, but it determines how sound is moved through out the game
            AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
        }

        //Used when the program ends
        public static void CleanAudio()
        {
            //Stops the device from playing the programs audio
            ALC.CloseDevice(device);
        }
    }
}
