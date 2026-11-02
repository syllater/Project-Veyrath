using UnityEngine;
using Veyrath.Core;

namespace Veyrath.World
{
    public sealed class VeyrathAudioManager : MonoBehaviour
    {
        private AudioSource effects;
        private AudioSource music;
        private AudioClip fallbackTone;
        public float MusicIntensity { get; private set; }

        public void Initialize()
        {
            effects = gameObject.AddComponent<AudioSource>();
            music = gameObject.AddComponent<AudioSource>();
            effects.playOnAwake = false; music.loop = true; music.playOnAwake = false;
            fallbackTone = AudioClip.Create("Veyrath silent-safe fallback", 441, 1, 44100, false);
        }

        public bool PlayEffect(AudioClip clip, float volume = 1f)
        {
            if (effects == null) Initialize();
            if (clip == null) return false;
            effects.PlayOneShot(clip, Mathf.Clamp01(volume)); return true;
        }

        public bool PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (music == null) Initialize();
            if (clip == null) { music.Stop(); return false; }
            music.clip = clip; music.volume = Mathf.Clamp01(volume); music.Play(); return true;
        }

        public void SetMusicIntensity(float intensity) { MusicIntensity = Mathf.Clamp01(intensity); }
        public void StopMusic() { if (music != null) music.Stop(); }
        public bool HasSafeFallback { get { return fallbackTone != null; } }
    }
}
