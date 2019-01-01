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
    [SerializeField] GameObject explosion;


    public OnJumpEventHandler onJump;
    [System.Serializable]
    public class OnJumpEventHandler : UnityEngine.Events.UnityEvent
    {

    }

    public OnFailEventHandler onFail;
    [System.Serializable]
    public class OnFailEventHandler : UnityEngine.Events.UnityEvent
    {

    }

    // Use this for initialization
    void Start ()
    {
        FindObjectOfType<MainController>().onJumpBeat.AddListener(Beat);
        rb = GetComponent<Rigidbody2D>();
        lastHitTime = Time.time;
    }

    int level = 0;
    void Update () {


        InputManager();

        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, Easing.Quintic.Out(t));

        if (spinning)
        {
            transform.Rotate(Vector3.forward * rotateSpeed * 400 * Easing.Circular.InOut(Time.deltaTime));
        }
        else
        {
            RotateShip(0);
        }

        shield.SetActive(shieldUp);

        if(Time.time - shieldTime > shieldCooldown)
        {
            shieldUp = true;
            shieldTime = Time.time;
        }

        Leveling();

    }

    void Leveling()
    {
        float timeSinceLastHit = Time.time - lastHitTime;


        if (timeSinceLastHit > speedSteps[5])
        {
            if (level != 5)
            {
                level = 5;
            }
        }
        else if (timeSinceLastHit > speedSteps[4])
        {
            if (level != 4)
            {
                level = 4;
            }
        }
        else if (timeSinceLastHit > speedSteps[3])
        {
            if (level != 3)
            {
                level = 3;
            }
        }
        else if (timeSinceLastHit > speedSteps[2])
        {
            if (level != 2)
            {
                level = 2;
            }
        }
        else if (timeSinceLastHit > speedSteps[1])
        {
            if (level != 1)
            {
                level = 1;
            }
        }
        else
        {
            if (level != 0)
            {
                level = 0;
            }
        }
    }

    void InputManager()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //if (Input.mousePosition.x < Screen.width / 4)
            //{
            //    Jump(new Vector2(-2.5f, yPosition), jumpTime);
            //    altenator = false;
            //}
            //else if (Input.mousePosition.x > (Screen.width / 4)*3)
            //{
            //    Jump(new Vector2(2.5f, yPosition), jumpTime);
            //    altenator = true;
            //}
            //else
            //{
                Jump(new Vector2(altenator ? -lanePositionX : lanePositionX, yPosition), jumpTime);
                altenator = !altenator;
            //}

        }
    }

    void RotateShip (float degrees)
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, 0, degrees);
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * rotateSpeed);
    }

    void Beat ()
    {
        foreach (Box box in FindObjectsOfType<Box>())
        {
            box.transform.localScale = Vector2.one * 1.2f;
        }
        if (testing)
        {
            Jump(new Vector2(altenator ? -lanePositionX : lanePositionX, yPosition) , jumpTime);
            altenator = !altenator;
        }
    }

    public void Jump(Vector2 _target, float _jumpTime)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = _jumpTime;
        target = _target;
        jumping = true;
        StartCoroutine("EndJump", _jumpTime);
        onJump.Invoke();
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
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<MainController>().ResetDifficulty();
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.pitch = 1;
        }
        spinning = false;
    }
}
