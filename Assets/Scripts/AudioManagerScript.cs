using UnityEngine;
using System;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {
    public Sound[] sounds;
    public AudioSource music;

    private float initialMusicVolume;

    void Awake() {
        initialMusicVolume = music.volume;

        foreach (var s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
            s.source.loop = false;
        }

        UpdateVolume();
    }

    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void UpdateVolume() {
        music.volume = initialMusicVolume * PlayerPrefs.GetFloat("music", 0.5f);

        var volume = PlayerPrefs.GetFloat("effects", 0.5f);
        foreach (var s in sounds) s.source.volume = s.volume * volume;
    }

    public void StopMusic() {
        foreach (Sound s in sounds) s.source.Pause();
        StartCoroutine(nameof(StopMusicInner));
        Play("gameover");
    }

    private IEnumerator StopMusicInner() {
        while (music.volume > 0.001f) {
            music.pitch /= 1.01f;
            music.volume /= 1.015f;

            yield return new WaitForSecondsRealtime(1 / 60f);
        }

        music.Stop();
    }
}
