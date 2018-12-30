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

    public float leTimeScale = 1;

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
    }


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
            if (jumping)
            {
                RotateShip(altenator ? -20 * Easing.Quintic.In(Time.deltaTime*50) : 20 * Easing.Quintic.In(Time.deltaTime*50));
            }
            else
            {
                RotateShip(0);
            }
        }
        
    }

    void InputManager()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x < Screen.width / 4)
            {
                Jump(new Vector2(-2.5f, yPosition), jumpTime);
                altenator = false;
            }
            else if (Input.mousePosition.x > (Screen.width / 4)*3)
            {
                Jump(new Vector2(2.5f, yPosition), jumpTime);
                altenator = true;
            }
            else
            {
                Jump(new Vector2(altenator ? -lanePositionX : lanePositionX, yPosition), jumpTime);
                altenator = !altenator;
            }

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
            Time.timeScale = 0.5f;
            StartCoroutine("ResetTime");
            foreach(AudioSource audioSource in FindObjectsOfType<AudioSource>())
            {
                audioSource.pitch = 0.5f;
            }
            onFail.Invoke();
            spinning = true;
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = leTimeScale;
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.pitch = leTimeScale;
        }
        spinning = false;
    }
}
