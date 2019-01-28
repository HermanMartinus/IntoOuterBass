using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    [System.Serializable]
    public struct SoundEffects
    {
        public string name;
        public AudioClip clip;
    }

    public string buttonSound = "";
    public List<AudioClip> soundEffects = new List<AudioClip>();

    // Singleton instance.
    public static SoundManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        AddButtonSounds();
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        AddButtonSounds();
    }

    void AddButtonSounds ()
    {
        foreach (Button button in Resources.FindObjectsOfTypeAll(typeof(Button)))
        {
            button.onClick.AddListener(ButtonClickHandler);
        }   
    }

    void ButtonClickHandler()
    {
        PlaySoundEffect(buttonSound);
    }

    public void PlaySoundEffect(string name, float timeDelay = 0, float volume = 1)
    {
        StartCoroutine(Play(soundEffects.Find((soundEffect) => soundEffect.name == name), timeDelay, volume));
    }

    // Play a single clip through the sound effects source.
    IEnumerator Play(AudioClip clip, float timeDelay, float volume)
    {
        yield return new WaitForSeconds(timeDelay);

        GameObject audioObject = new GameObject();
        audioObject.transform.parent = transform;
        audioObject.name = clip.name + "AudioSource";
        AudioSource newAudioSource = audioObject.AddComponent<AudioSource>();
        newAudioSource.clip = clip;
        newAudioSource.volume = volume;
        newAudioSource.Play();
        StartCoroutine(DestroyAudioSource(audioObject, clip.length));
    }

    IEnumerator DestroyAudioSource(GameObject audioObject, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(audioObject);
    }

    // Play a single clip through the music source.
    void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }
}