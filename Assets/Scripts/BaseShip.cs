using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : MonoBehaviour {

    [SerializeField] float jumpTime = 0.2f;
    bool altenator = false;
    public bool testing = false;
    Rigidbody2D rb;
    float t;
    Vector3 startPosition;
    Vector3 target;
    float timeToReachTarget;
    public float yPosition = -3;
    public float lanePositionX = 2f;
    public bool spinning = false;
    bool jumping = false;
    public float rotateSpeed = 500f;
    [SerializeField] GameObject shield;
    bool shieldUp = false;
    float lastHitTime = 0f;
    public float shieldCooldown = 30f;
    float shieldTime = 0f;
    public List<float> speedSteps = new List<float>();
    public int level = 0;
    [SerializeField] GameObject explosion;
    [SerializeField] Transform speakers;
    public bool ended = false;
    public bool upgrading = false;
    [SerializeField] List<Sprite> ships = new List<Sprite>();

    public OnJumpEventHandler onJump;
    [System.Serializable]
    public class OnJumpEventHandler : UnityEngine.Events.UnityEvent {}

    public OnFailEventHandler onFail;
    [System.Serializable]
    public class OnFailEventHandler : UnityEngine.Events.UnityEvent {}

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        lastHitTime = Time.time;
        level = 0;
    }


    void Update () {


        InputManager();

        foreach (Transform speaker in speakers)
        {
            speaker.localScale = Vector2.Lerp(speaker.localScale, Vector2.one, 0.2f);
        }

        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, Easing.Quintic.Out(t));

        if (spinning)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * 400 * Easing.Circular.InOut(Time.deltaTime));
        }
        else if (upgrading)
        {
            transform.Rotate(Vector3.up * rotateSpeed * 200 * Easing.Circular.InOut(Time.deltaTime));
        }
        else
        {
            RotateShip(0);
            shield.transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
        }

       

        shield.SetActive(shieldUp);

        if(Time.time - shieldTime > shieldCooldown)
        {
            shieldUp = true;
            shieldTime = Time.time;
        }

        Leveling();
    }

    IEnumerator Upgrade(bool wait = true)
    {
        upgrading = true;
        if (wait)
        {
            yield return new WaitForSeconds(0.3f);
        }

        foreach (Transform speaker in speakers)
        {
            speaker.gameObject.SetActive(speaker.name == "speaker" + level);
        }

        yield return new WaitForSeconds(0.3f);

        upgrading = false;
    }

    void Leveling()
    {
        float timeSinceLastHit = Time.time - lastHitTime;


        if (timeSinceLastHit > speedSteps[5])
        {
            if (level != 5)
            {
                level = 5;
                StartCoroutine(Upgrade());
            }
        }
        else if (timeSinceLastHit > speedSteps[4])
        {
            if (level != 4)
            {
                level = 4;
                StartCoroutine(Upgrade());
            }
        }
        else if (timeSinceLastHit > speedSteps[3])
        {
            if (level != 3)
            {
                level = 3;
                StartCoroutine(Upgrade());
            }
        }
        else if (timeSinceLastHit > speedSteps[2])
        {
            if (level != 2)
            {
                level = 2;
                StartCoroutine(Upgrade());
            }
        }
        else if (timeSinceLastHit > speedSteps[1])
        {
            if (level != 1)
            {
                level = 1;
                StartCoroutine(Upgrade());
            }
        }
        else
        {
            if (level != 0)
            {
                level = 0;
                StartCoroutine(Upgrade(false));
            }
        }
    }

    void InputManager()
    {

        if (Input.GetMouseButtonDown(0) && !ended)
        {
            testing = false;
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            testing = true;
        }
    }

    void RotateShip (float degrees)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, 0, degrees);
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotateSpeed);
    }

    public void JumpBeat ()
    {
        if (testing)
        {
            Jump();

        }
    }

    public void ActualBeat ()
    {
        foreach(Transform speaker in speakers)
        {
            speaker.localScale = Vector2.one * Random.Range(1.2f, 1.5f);
        }
    }

    public void Jump()
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = jumpTime;
        target = new Vector2(altenator ? -lanePositionX : lanePositionX, yPosition);
        jumping = true;
        StartCoroutine("EndJump", jumpTime);
        onJump.Invoke();
        altenator = !altenator;
    }


    IEnumerator EndJump(float _jumpTime)
    {
        yield return new WaitForSeconds(_jumpTime);
        jumping = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.timeScale > 0.09f)
        {
            if(shieldUp)
            {
                Instantiate(explosion, collision.transform);
                collision.GetComponent<SpriteRenderer>().enabled = false;
                collision.GetComponent<Collider2D>().enabled = false;
                shieldUp = false;
            }
            else
            {
                Time.timeScale = 0.5f;

                if (level > 0)
                {
                    Transform speakerBin = GameObject.Find("speakerBin").transform;
                    speakerBin.position = transform.position;
                    foreach (Transform child in speakers.Find("speaker" + level))
                    {
                        GameObject droppedSpeaker = Instantiate(child.gameObject, speakerBin);
                        droppedSpeaker.AddComponent<Rigidbody2D>().AddTorque(Random.Range(-200, 200));
                    }
                }


                StartCoroutine("ResetTime");
                lastHitTime = Time.time;
                foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
                {
                    audioSource.pitch = 0.5f;
                }
                onFail.Invoke();
                spinning = true;

            }
            shieldTime = Time.time;
            StartCoroutine("ReneableCollider");
        }
    }

    IEnumerator ReneableCollider()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(1f);
        foreach(Transform junkSpeaker in GameObject.Find("speakerBin").transform)
        {
            Destroy(junkSpeaker.gameObject);
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        //FindObjectOfType<MainController>().ResetDifficulty();
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.pitch = 1;
        }
        spinning = false;
    }
}
