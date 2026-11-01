using UnityEngine;

namespace Veyrath.World
{
    public sealed class VeyrathAudioFeedback : MonoBehaviour
    {
        private AudioSource source;
        private AudioClip tone;

        public void Initialize()
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            tone = AudioClip.Create("Veyrath generated feedback", 2205, 1, 44100, false);
            float[] samples = new float[2205];
            for (int i = 0; i < samples.Length; i++) samples[i] = Mathf.Sin(i * 0.12f) * (1f - i / (float)samples.Length) * 0.12f;
            tone.SetData(samples, 0);
        }

        public void PlayEvent()
        {
            if (source != null && tone != null) source.PlayOneShot(tone);
        }
    }
}
