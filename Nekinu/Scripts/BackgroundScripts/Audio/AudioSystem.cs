using OpenTK.Audio.OpenAL;

namespace NekinuSoft
{
    public class AudioSystem
    {
        private static ALDevice device;
        
        public static void InitAudio()
        {
            string source = ALC.GetString(ALDevice.Null, AlcGetString.DefaultDeviceSpecifier);

            device = ALC.OpenDevice(source);

            if(device == null)
            {
                Crash_Report.generate_crash_report($"Failed to load default OpenAl audio device! {source}");
            }

            ALC.MakeContextCurrent(ALC.CreateContext(device, new int[0]));

            AL.Listener(ALListener3f.Position, 0, 0, 1.0f);
            AL.Listener(ALListener3f.Velocity, 0, 0, 1.0f);

            AL.DistanceModel(ALDistanceModel.InverseDistanceClamped);
        }

        public static void CleanAudio()
        {
            ALC.CloseDevice(device);
        }
    }
}
