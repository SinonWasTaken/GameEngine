using Nekinu;
using Newtonsoft.Json;
using OpenTK.Audio.OpenAL;

namespace NekinuSoft
{
    public class AudioSource : Component
    {
        //A property that tells Json to not write the variable to text when saving 
        [JsonIgnore]
        //The id of the audio source
        private int sourceID;

        //A property that should display this variable in the editor 
        [SerializedProperty]
        //how loud the audio is
        private float volume;
        
        public float Volume
        {
            get => volume;
            set
            {
                volume = value;
                AL.Source(sourceID, ALSourcef.Gain, volume);
            }
        }

        //The pitch of the audio
        [JsonProperty] [SerializedProperty] private float pitch;

        //Determines how the audio travels over distance
        [JsonProperty] [SerializedProperty] private float rollOff;
        [JsonProperty] [SerializedProperty] private float distance;
        [JsonProperty] [SerializedProperty] private float maxDistance;

        //used when an audio clip is playing
        [JsonIgnore]
        public bool isPlaying { get; private set; }

        //The current audio clip that is playing
        [JsonIgnore] [SerializedProperty] private AudioClip currentClip;

        //The audio clip that is queued to play next
        [JsonIgnore]
        private AudioClip queuedClip;

        //Default constructor. Sets everything to a default value
        public AudioSource()
        {
            volume = 1;
            pitch = 1;
            rollOff = 1;
            distance = 10;
            maxDistance = 20;

            sourceID = AL.GenSource();
            AL.Source(sourceID, ALSourcef.Pitch, pitch);

            AL.Source(sourceID, ALSourcef.MinGain, 0);
            AL.Source(sourceID, ALSourcef.Gain, volume);
            AL.Source(sourceID, ALSourcef.MaxGain, 1);

            AL.Source(sourceID, ALSourceb.Looping, false);

            AL.Source(sourceID, ALSource3f.Position, 0, 0, 0);
            AL.Source(sourceID, ALSource3f.Velocity, 0, 0, 0);

            AL.Source(sourceID, ALSourcef.RolloffFactor, rollOff);
            AL.Source(sourceID, ALSourcef.RolloffFactor, distance);
            AL.Source(sourceID, ALSourcef.MaxDistance, maxDistance);

            //Saves the audio source to a cache. Used to prevent the same data from being loaded again, and to easily remove the data from memory when not being used
            Cache.AddSource(this);
        }

        //Updates every frame
        public override void Update()
        {
            base.Update();

            //Sets the position of the audio. Used to make the source 3d
            AL.Source(sourceID, ALSource3f.Position, Parent.Transform.position.x, Parent.Transform.position.y, Parent.Transform.position.z); ;

            //Gets the state of the audio source. Stopped, Playing, Paused
            ALSourceState state = AL.GetSourceState(sourceID);

            //Sets the isPlaying boolean to true if the audio source. Makes things easier when I want to check if a audio clip is playing
            isPlaying = state == ALSourceState.Playing ? true : false;
        }

        public void AddClipToQueue(AudioClip clip)
        {
            //Queues an audio clip
            queuedClip = clip;
        }

        public void Play()
        {
            //If there is a clip queued, then
            if (queuedClip != null)
            {
                //set the current clip to the queued audio clip
                currentClip = queuedClip;
                //then remove it from the queue
                queuedClip = null;
            }

            //Hands off the clip to the proper play method
            Play(currentClip);
        }

        public void Play(AudioClip clip)
        {
            //if a audio clip is playing, stop it
            if (isPlaying)
                Stop();

            //set the current audio clip to the new audio clip
            currentClip = clip;

            //Loads the current clip into the source
            AL.Source(sourceID, ALSourcei.Buffer, currentClip.id);
            //then plays it
            AL.SourcePlay(sourceID);
        }

        //Pauses the audio clip
        public void Pause()
        {
            AL.SourcePause(sourceID);
        }

        //Stop the audio clip
        public void Stop()
        {
            AL.SourceStop(sourceID);
        }

        //Used when the 3D object is removed from the scene
        public override void OnDestroy()
        {
            CleanUp();
        }

        //Removes the audio source from memory
        public void CleanUp()
        {
            AL.DeleteSource(sourceID);
        }
    }
}