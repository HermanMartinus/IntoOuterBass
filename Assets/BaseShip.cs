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
    bool spinning = false;
    bool jumping = false;
    public float rotateSpeed = 500f;

    // Use this for initialization
    void Start ()
    {
        FindObjectOfType<MainController>().onJumpBeat.AddListener(Beat);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {

       
        if (Input.GetMouseButtonDown(0))
        {
            Jump();

        }
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
                RotateShip(altenator ? -20 * Easing.Quadratic.In(Time.deltaTime*50) : 20 * Easing.Quadratic.In(Time.deltaTime*50));
            }
            else
            {
                RotateShip(0);
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
            Jump();
        }
    }

    void Jump()
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = jumpTime;
        target = new Vector2(altenator ? -1 : 1, yPosition);
        altenator = !altenator;
        jumping = true;
        StartCoroutine("EndJump");
    }

    IEnumerator EndJump()
    {
        yield return new WaitForSeconds(jumpTime);
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

            spinning = true;
        }
    }

    IEnumerator ResetTime()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1;
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            audioSource.pitch = 1f;
        }
        spinning = false;
    }
}
