
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{

    [SerializeField] GameObject beater;
    [SerializeField] GameObject box;
    [SerializeField] GameObject platform;

    [SerializeField] AudioSource beatGeneratorAudioSource;
    [SerializeField] AudioSource listenAudioSource;
    [SerializeField] List<Sprite> boxes;
    public float difficulty = 0.8f;
    public AudioClip activeMusic;
    public float platformDropTimer = 0.2f;
    public float beatTimeDifference = 2f;
    public Vector2 holeSizeRange = new Vector2(1.3f, 0.7f);
    float holeSize = 1.5f;
    bool altenator = false;
    public float cooldownTime = 0.5f;
    public float boxSpeed = 1f;
    bool canSpawn = true;
    float clipLength;
    float startTime;
    bool started = false;

    [Header("Events")]
    public OnJumpBeatEventHandler onJumpBeat;
    [System.Serializable]
    public class OnJumpBeatEventHandler : UnityEngine.Events.UnityEvent
    {

    }
    [Header("Events")]
    public OnBeatEventHandler onBeat;
    [System.Serializable]
    public class OnBeatEventHandler : UnityEngine.Events.UnityEvent
    {

    }

    private void Awake()
    {
        if (FindObjectOfType<LoadedClips>())
        {
            activeMusic = FindObjectOfType<LoadedClips>().clips[0];
        }

        beatGeneratorAudioSource.clip = activeMusic;
        listenAudioSource.clip = activeMusic;
    }

    private void Start()
    {
        
    }

    void GameStart ()
	{
        started = true;
		AudioProcessor processor = FindObjectOfType<AudioProcessor> ();
		processor.onBeat.AddListener (onOnbeatDetected);
		processor.onSpectrum.AddListener (onSpectrum);

        beatGeneratorAudioSource.Play();
        listenAudioSource.PlayDelayed(beatTimeDifference);

        startTime = Time.time;
        clipLength = activeMusic.length;
	}

    private void Update()
    {
        if (!started)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
            return;
        }

        platformDropTimer -= Time.deltaTime;
        if (platformDropTimer <= 0f)
        {
            GameObject spawnedPlatform = Instantiate(platform);
            spawnedPlatform.transform.position = new Vector2(0, 7.55f);
            spawnedPlatform.GetComponent<Rigidbody2D>().velocity = Vector2.down * boxSpeed;
            Vector2 direction = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

            spawnedPlatform.GetComponent<Rigidbody2D>().AddTorque(direction.x * 1);

            platformDropTimer = 0.2f;

            DestroyAll(GameObject.FindGameObjectsWithTag("Platform"), -10);
            DestroyAll(GameObject.FindGameObjectsWithTag("Box"), -10);
        }


        DificultyIncreaser();
    }

    void DestroyAll(GameObject [] objects, float yThreshold)
    {
        foreach (GameObject obj in objects)
        {
            if (obj.transform.position.y < yThreshold)
            {
                Destroy(obj);
            }
        }
    }

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    public void onOnbeatDetected ()
	{
        StartCoroutine("StandardBeat");
        if (canSpawn)
        {

            //Debug.Log("Beat!!!");
            GameObject spawnedBox = Instantiate(box);
            spawnedBox.transform.position = new Vector2(altenator ? Random.Range(1.1f, 2f) : -Random.Range(1.1f, 2f), 7.4f);
            spawnedBox.GetComponent<Rigidbody2D>().velocity = Vector2.down * boxSpeed;
            spawnedBox.GetComponent<SpriteRenderer>().sprite = boxes[Random.Range(0, boxes.Count)];
            Vector2 direction = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

            spawnedBox.GetComponent<Rigidbody2D>().AddTorque(direction.x * 1);


            RemovePlatform(new Vector2(0, 7f));

            StartCoroutine("JumpBeat", altenator);
            altenator = !altenator;

            StartCoroutine("Cooldown");
        }
    }

    void RemovePlatform(Vector2 boxPosition)
    {
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Platform"))
        {
            float distance = Vector3.Distance(t.transform.position, boxPosition);

            if (distance < holeSize)
            {
                Destroy(t);
            }
        }

    }

    IEnumerator Cooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }

    IEnumerator StandardBeat()
    {

        yield return new WaitForSeconds(beatTimeDifference);
        onBeat.Invoke();
    }

    IEnumerator JumpBeat(bool _altenator)
    {
        yield return new WaitForSeconds(beatTimeDifference);
        //Debug.Log("Jump Beat");
        onJumpBeat.Invoke();
    }


    void DificultyIncreaser()
    {
        float timeLeft = clipLength - (Time.time - startTime);
        float percentageCompleted = Mathf.Abs(((timeLeft / clipLength)-1));

        if(!beater.GetComponent<BaseShip>().spinning)
            Time.timeScale = 1 + (percentageCompleted* difficulty);

        holeSize = holeSizeRange.x - percentageCompleted*(holeSizeRange.x - holeSizeRange.y);
    }

    public void ResetDifficulty()
    {
        startTime = Time.time;
    }
    //This event will be called every frame while music is playing
    public void onSpectrum (float[] spectrum)
	{
		//The spectrum is logarithmically averaged
		//to 12 bands

		for (int i = 0; i < spectrum.Length; ++i) {
			Vector3 start = new Vector3 (i, 0, 0);
			Vector3 end = new Vector3 (i, spectrum [i], 0);
			Debug.DrawLine (start, end);
		}
	}

    public void Menu()
    {
        if(FindObjectOfType<LoadedClips>())
            FindObjectOfType<LoadedClips>().clips.Clear();
        SceneManager.LoadScene("Selection");
    }
}
