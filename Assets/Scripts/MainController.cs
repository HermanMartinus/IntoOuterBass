﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField] GameObject leaderBoard;
    [SerializeField] GameObject gameUi;

    public static MainController Instance;

    private void Awake()
    {
        Instance = this;
        activeMusic = LoadedClips.Instance.selectedTrack.clip;
        beatManager.SetClip(activeMusic);
    }

    private void Start()
    {
        Time.timeScale = 1;

    }

    void GameStart ()
	{
        SoundManager.Instance.StopMusic();
        playing = true;

        beatManager.Play();

        startTime = Time.time;

        dropTime = timeBetweenPlatforms;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowLeaderBoard();
        }
        if (!playing && !ended)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();
            }
            return;
        }

        dropTime -= Time.deltaTime;
        if (dropTime <= 0f)
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

    public void Menu()
    {
        SceneManager.LoadScene("SearchMenu");
    }

    void SongCompleted()
    {
        playing = false;
        ended = true;
        ShowLeaderBoard();
        SoundManager.Instance.StartMusic();
    }

    void ShowLeaderBoard()
    {
        leaderBoard.GetComponent<Leaderboard>().ShowLeaderboard(FindObjectOfType<Score>().score);
        leaderBoard.SetActive(true);
        gameUi.SetActive(false);

        bassShip.GetComponent<BaseShip>().ended = true;
    }
}
