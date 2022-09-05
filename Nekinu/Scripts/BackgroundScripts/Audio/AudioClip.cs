using System.Runtime.InteropServices;
using OpenTK.Audio.OpenAL;

namespace NekinuSoft
{
    public class AudioClip
    {
        //The id of the audioclip
        public int id { get; private set; }

        //the location of the audio clip in the files
        public string clip { get; private set; }

        //Constructor
        public AudioClip(string clip)
        {
            //saves the location of the audio clip for later
            this.clip = clip;

            //loads the audio clip into the game using the clip location
            loadSoundClip(clip);
        }

        //Constructor
        public AudioClip(Stream stream, string clip_name)
        {
            //saves the location of the audio clip for later
            clip = clip_name;
            
            //loads the audio clip into the game by finding it in the engine resources
            loadSoundClip(stream);
        }

        private void loadSoundClip(string source)
        {
            AudioClip c = Cache.AudioClipExists(source); 
            
            //There was duplicate code here before 9/4/2022
            //Removed the duplicate code, and created a separate method.
            //Loads the audio clip, and checks if the audio clip is the right format
            checkIfAudioIsCorrectFormat(c);
        }
        
        private void loadSoundClip(Stream source)
        {
            AudioClip c = Cache.AudioClipExists(clip);

            //There was duplicate code here before 9/4/2022
            //Removed the duplicate code, and created a separate method.
            //Loads the audio clip, and checks if the audio clip is the right format
            checkIfAudioIsCorrectFormat(c);
        }

        private void checkIfAudioIsCorrectFormat(AudioClip clip)
        {
            //Performs the check if the audio clip doesn't exist in the game
            if (clip == null)
            {
                //Generates an id
                id = AL.GenBuffer();

                //The varaibles that will help determine if the audio clip is usable. Not entirely sure how this works
                int channels = 0;
                int bits = 0;
                int rate = 0;

                byte[] bytes = new byte[0];

                try
                {
                    //opens up a stream, containing information from the audio clip 
                    using (StreamReader stream = new StreamReader(clip.clip))
                    {
                        //https://github.com/mono/opentk/blob/master/Source/Examples/OpenAL/1.1/Playback.cs
                        if (stream != null)
                        {
                            using (BinaryReader reader = new BinaryReader(stream.BaseStream))
                            {
                                //I'm not really sure what this all does. Reads the audio data and determines if it is a valid format
                                // RIFF header
                                string signature = new string(reader.ReadChars(4));
                                if (signature != "RIFF")
                                    throw new NotSupportedException("Specified stream is not a wave file.");

                                int riff_chunck_size = reader.ReadInt32();

                                string format = new string(reader.ReadChars(4));
                                if (format != "WAVE")
                                    throw new NotSupportedException("Specified stream is not a wave file.");

                                // WAVE header
                                string format_signature = new string(reader.ReadChars(4));
                                if (format_signature != "fmt ")
                                    throw new NotSupportedException("Specified wave file is not supported.");

                                int format_chunk_size = reader.ReadInt32();
                                int audio_format = reader.ReadInt16();
                                int num_channels = reader.ReadInt16();
                                int sample_rate = reader.ReadInt32();
                                int byte_rate = reader.ReadInt32();
                                int block_align = reader.ReadInt16();
                                int bits_per_sample = reader.ReadInt16();

                                string data_signature = new string(reader.ReadChars(4));
                                if (data_signature != "data")
                                    throw new NotSupportedException("Specified wave file is not supported.");

                                int data_chunk_size = reader.ReadInt32();

                                channels = num_channels;
                                bits = bits_per_sample;
                                rate = sample_rate;

                                bytes = reader.ReadBytes((int) reader.BaseStream.Length);
                            }
                        }
                        
                        IntPtr data = Marshal.AllocHGlobal(bytes.Length);
                        Marshal.Copy(bytes, 0, data, bytes.Length);

                        AL.BufferData(id, GetSoundFormat(channels, bits), data, bytes.Length, rate);
                    }
                }
                //Occurs if there is an error when loading the audio file
                catch (Exception e)
                {
                    //Writes to the console
                    Console.WriteLine($"Error loading audio! {e}");
                    //Generates a user crash report that can be read after the program has closed
                    Crash_Report.generate_crash_report($"Error loading audio! {e}");
                    //Closes the program with an error code -100, which is my code for audio error
                    Environment.Exit(-100);
                }

                //If the audio clip loads properly, store the data in a cache, so the program doesn't load the same audio clip again
                Cache.AddAudioClip(this);
            }
            else
            {
                //if the audio clip already exists, then set this id to the id of the audio clip that already exists
                id = clip.id;
            }
        }
        
        //Not sure what this does exaclty. Not mine
        //https://github.com/mono/opentk/blob/master/Source/Examples/OpenAL/1.1/Playback.cs
        private ALFormat GetSoundFormat(int channels, int bits)
        {
            switch (channels)
            {
                case 1: return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
                case 2: return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
                default: throw new NotSupportedException("The specified sound format is not supported.");
            }
        }
    }
}