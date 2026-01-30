using UnityEngine;
using System.Collections;
using VRCampusTour.Utils;

namespace VRCampusTour.Core
{
    /// <summary>
    /// Manages all audio in the application
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambienceSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Settings")]
        [SerializeField] private float masterVolume = 1f;
        [SerializeField] private float musicVolume = 0.6f;
        [SerializeField] private float ambienceVolume = 0.4f;
        [SerializeField] private float sfxVolume = 0.8f;

        protected override void Awake()
        {
            base.Awake();
            InitializeAudioSources();
        }

        void InitializeAudioSources()
        {
            if (musicSource == null)
            {
                musicSource = CreateAudioSource("MusicSource");
                musicSource.loop = true;
            }

            if (ambienceSource == null)
            {
                ambienceSource = CreateAudioSource("AmbienceSource");
                ambienceSource.loop = true;
            }

            if (sfxSource == null)
            {
                sfxSource = CreateAudioSource("SFXSource");
            }

            UpdateVolumes();
        }

        AudioSource CreateAudioSource(string name)
        {
            GameObject audioObj = new GameObject(name);
            audioObj.transform.SetParent(transform);
            return audioObj.AddComponent<AudioSource>();
        }

        public void PlayMusic(AudioClip clip, bool fadeIn = true)
        {
            if (clip == null) return;

            if (fadeIn)
            {
                StartCoroutine(CrossfadeMusic(clip));
            }
            else
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }

        public void PlayAmbience(AudioClip clip, bool fadeIn = true)
        {
            if (clip == null) return;

            if (fadeIn)
            {
                StartCoroutine(CrossfadeAmbience(clip));
            }
            else
            {
                ambienceSource.clip = clip;
                ambienceSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip, volumeScale);
        }

        public void StopMusic(bool fadeOut = true)
        {
            if (fadeOut)
            {
                StartCoroutine(FadeOutAudio(musicSource, Constants.TRANSITION_AUDIO_FADE));
            }
            else
            {
                musicSource.Stop();
            }
        }

        public void StopAmbience(bool fadeOut = true)
        {
            if (fadeOut)
            {
                StartCoroutine(FadeOutAudio(ambienceSource, Constants.TRANSITION_AUDIO_FADE));
            }
            else
            {
                ambienceSource.Stop();
            }
        }

        IEnumerator CrossfadeMusic(AudioClip newClip)
        {
            // Fade out current
            if (musicSource.isPlaying)
            {
                yield return FadeOutAudio(musicSource, Constants.TRANSITION_AUDIO_FADE);
            }

            // Switch clip
            musicSource.clip = newClip;
            musicSource.Play();

            // Fade in new
            yield return FadeInAudio(musicSource, Constants.TRANSITION_AUDIO_FADE, musicVolume);
        }

        IEnumerator CrossfadeAmbience(AudioClip newClip)
        {
            if (ambienceSource.isPlaying)
            {
                yield return FadeOutAudio(ambienceSource, Constants.TRANSITION_AUDIO_FADE);
            }

            ambienceSource.clip = newClip;
            ambienceSource.Play();

            yield return FadeInAudio(ambienceSource, Constants.TRANSITION_AUDIO_FADE, ambienceVolume);
        }

        IEnumerator FadeOutAudio(AudioSource source, float duration)
        {
            float startVolume = source.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                yield return null;
            }

            source.volume = 0f;
            source.Stop();
        }

        IEnumerator FadeInAudio(AudioSource source, float duration, float targetVolume)
        {
            source.volume = 0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                source.volume = Mathf.Lerp(0f, targetVolume * masterVolume, elapsed / duration);
                yield return null;
            }

            source.volume = targetVolume * masterVolume;
        }

        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume * masterVolume;
        }

        public void SetAmbienceVolume(float volume)
        {
            ambienceVolume = Mathf.Clamp01(volume);
            ambienceSource.volume = ambienceVolume * masterVolume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            sfxSource.volume = sfxVolume * masterVolume;
        }

        void UpdateVolumes()
        {
            musicSource.volume = musicVolume * masterVolume;
            ambienceSource.volume = ambienceVolume * masterVolume;
            sfxSource.volume = sfxVolume * masterVolume;
        }
    }
}