using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] AudioSource preBeatAudioSource;
    [SerializeField] AudioSource actualBeatAudioSource;

    public OnBeatEventHandler onPreBeat;
    public OnBeatEventHandler onActualBeat;
    public OnBeatEventHandler onJumpBeat;
    [System.Serializable]
    public class OnBeatEventHandler : UnityEngine.Events.UnityEvent {}

    public float timeDifference = 0.65f;

    public static BeatManager Instance;

    bool canSpawn = true;
    bool canJump = true;
    public float cooldownTime = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void SetClip(AudioClip clip)
    {
        preBeatAudioSource.clip = clip;
        actualBeatAudioSource.clip = clip;
    }

    public void Play()
    {
        preBeatAudioSource.Play();
        actualBeatAudioSource.PlayDelayed(timeDifference);
    }

    public void preBeatOnSpectrum(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }

    //This event will be called every frame while music is playing
    public void actualBeatOnSpectrum(float[] spectrum)
    {
        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }

    public void preBeatDetected()
    {
        if (canSpawn)
        {
            onPreBeat.Invoke();
            StartCoroutine(jumpBeatDetected());
            StartCoroutine(Cooldown());
        }
    }

    public void actualBeatDetected()
    {
        onActualBeat.Invoke();
    }

    IEnumerator jumpBeatDetected()
    {
        yield return new WaitForSeconds(timeDifference);
        onJumpBeat.Invoke();
    }

    IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }

    public bool IsPlaying()
    {
        return actualBeatAudioSource.isPlaying;
    }
}
