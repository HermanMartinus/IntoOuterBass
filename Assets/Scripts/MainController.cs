
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{

    [SerializeField] GameObject bassShip;
    [SerializeField] GameObject box;
    [SerializeField] GameObject platform;
    [SerializeField] BeatManager beatManager;
    [SerializeField] List<Sprite> boxes;
    [SerializeField] List<GameObject> obsticles;
    [SerializeField] Transform obsticleContainer;


    public AudioClip activeMusic;
    public float timeBetweenPlatforms = 0.2f;
    float dropTime = 0.2f;

    public Vector2 holeSizeRange = new Vector2(1.3f, 0.7f);
    float holeSize = 1.5f;
    bool altenator = false;
    public float boxSpeed = 1f;
    float startTime;
    public bool playing = false;
    public bool ended = false;
    float timeLeft = 100;

    public bool _simplified = false;
    public static bool simplified;
    public int lives = 5;
    public Transform lifeContainer;
    public Sprite lifeSprite;
    public GameObject patreonButton;
    public GameObject retryButton;
    public Text hermansText;
    bool started = false;

    [SerializeField] GameObject leaderBoard;
    [SerializeField] GameObject gameUi;

    public static MainController Instance;

    private void Awake()
    {
        simplified = _simplified;
        Instance = this;
        if(!simplified)
            activeMusic = LoadedClips.Instance.selectedTrack.clip;
        else
        {
            UpdateLives();
        }
        beatManager.SetClip(activeMusic);
    }

    private void Start()
    {
        Time.timeScale = 1;

    }

    void GameStart ()
	{
        if (!simplified)
            SoundManager.Instance.StopMusic();
        playing = true;

        beatManager.Play();

        startTime = Time.time;

        dropTime = timeBetweenPlatforms;

        StartCoroutine(MessageSequence());
    }

    void Update()
    {
     
        if (!playing && !ended)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
            return;
        }
        if (!started) return;

        dropTime -= Time.deltaTime;
        if (dropTime <= 0f && !ended)
        {
            GameObject spawnedPlatform = Instantiate(platform, obsticleContainer);
            spawnedPlatform.transform.position = new Vector2(0, 7.55f);
            spawnedPlatform.GetComponent<Rigidbody2D>().velocity = Vector2.down * boxSpeed;
            Vector2 direction = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

            spawnedPlatform.GetComponent<Rigidbody2D>().AddTorque(direction.x * 1);

            dropTime = timeBetweenPlatforms;

            DestroyAll(GameObject.FindGameObjectsWithTag("Platform"), -10);
            DestroyAll(GameObject.FindGameObjectsWithTag("Box"), -10);
        }

        if (!BaseShip.Instance.spinning && Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }

        if (!ended && !BeatManager.Instance.IsPlaying())
        {
            SongCompleted();
        }
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
    
    public void PreBeat ()
	{
        if (ended || !started) return;
        GameObject spawnedBox = Instantiate(box, obsticleContainer);
        spawnedBox.transform.position = new Vector2(altenator ? Random.Range(1.1f, 2f) : -Random.Range(1.1f, 2f), 7.4f);
        spawnedBox.GetComponent<Rigidbody2D>().velocity = Vector2.down * boxSpeed;
        Instantiate(obsticles[Random.Range(0, obsticles.Count)], spawnedBox.transform);
        Vector2 direction = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));

        spawnedBox.GetComponent<Rigidbody2D>().AddTorque(direction.x * 1);


        RemovePlatform(new Vector2(0, 7f));

        altenator = !altenator;
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

    public void Crash()
    {
        lives--;
        if(lives < 1 && !ended)
        {
            playing = false;
            ended = true;
            Destroy(BaseShip.Instance.gameObject);
            Time.timeScale = 1;
            StartCoroutine(Died());
            foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
            {
                audioSource.pitch = 1;
            }
        }
        UpdateLives();
    }

    void UpdateLives()
    {
        foreach(Transform life in lifeContainer)
        {
            Destroy(life.gameObject);
        }
        for (int i = 0; i < lives; i++)
        {
            GameObject life = new GameObject();
            life.name = "life";
            life.transform.parent = lifeContainer;
            life.AddComponent<SpriteRenderer>().sprite = lifeSprite;
            life.GetComponent<SpriteRenderer>().sortingOrder = 2;
            life.transform.localScale = Vector2.one * 0.7f;
            life.transform.position = new Vector2(1.6f - (i * 0.5f), -3.8f);
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("SearchMenu");
    }

    public void Patreon()
    {
        Application.OpenURL("https://patreon.com/herman_martinus");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Simplified");
    }

    void SongCompleted()
    {
        playing = false;
        ended = true;
        if (simplified)
        {
            StartCoroutine(SimpleEndSequence());
        }
        else
        {
            ShowLeaderBoard();
            SoundManager.Instance.StartMusic();
        }
    }

    IEnumerator MessageSequence()
    {
        hermansText.gameObject.SetActive(true);

        Text message = hermansText.GetComponent<Text>();
        yield return new WaitForSeconds(2.7f);
        message.text = "Into Outer Bass";
        yield return new WaitForSeconds(3);
        message.text = "";
        yield return new WaitForSeconds(10);
        message.text = "a game by Herman Martinus";
        started = true;
        yield return new WaitForSeconds(3);
        message.text = "";

    }

    IEnumerator SimpleEndSequence()
    {
        GameObject hermansFace = GameObject.Find("HermansFace");
        patreonButton.SetActive(false);
        retryButton.SetActive(false);
        hermansText.gameObject.SetActive(true);

        Text message = hermansText.GetComponent<Text>();
        yield return new WaitForSeconds(2);
        hermansFace.GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        message.text = "Wow, I'm impressed...";
        yield return new WaitForSeconds(4);
        message.text = "You finished the entire track...";
        yield return new WaitForSeconds(4);
        message.text = "Which is like half an hour long...";
        yield return new WaitForSeconds(4);
        message.text = "Well done!";
        yield return new WaitForSeconds(4);
        message.text = "I'm Herman...";
        yield return new WaitForSeconds(4);
        message.text = "The creator of this game...";
        yield return new WaitForSeconds(4);
        message.text = "Just here to congratulate you...";
        yield return new WaitForSeconds(5);
        message.text = "I hope you had fun!";
        patreonButton.SetActive(true);
        retryButton.SetActive(true);
    }

    IEnumerator Died()
    {
        GameObject hermansFace = GameObject.Find("HermansFace");
        patreonButton.SetActive(false);
        retryButton.SetActive(false);
        hermansText.gameObject.SetActive(true);

        Text message = hermansText.GetComponent<Text>();
        yield return new WaitForSeconds(2);
        hermansFace.GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        message.text = "Hey...";
        yield return new WaitForSeconds(4);
        message.text = "Looks like you crashed too many times...";
        yield return new WaitForSeconds(4);
        message.text = "Guess your journey ends here...";
        yield return new WaitForSeconds(4);
        message.text = "Fret not!";
        yield return new WaitForSeconds(4);
        message.text = "You can always try again!";
        patreonButton.SetActive(true);
        retryButton.SetActive(true);
    }

    void ShowLeaderBoard()
    {
        leaderBoard.GetComponent<Leaderboard>().ShowLeaderboard(FindObjectOfType<Score>().score);
        leaderBoard.SetActive(true);
        gameUi.SetActive(false);

        bassShip.GetComponent<BaseShip>().ended = true;
    }
}
