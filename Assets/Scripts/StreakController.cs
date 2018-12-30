using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakController : MonoBehaviour {

    int streak = 0;
    private void Start()
    {
        FindObjectOfType<BaseShip>().onJump.AddListener(Jump);
        FindObjectOfType<BaseShip>().onFail.AddListener(Fail);
    }

    void Jump()
    {
        streak++;
        //Debug.Log("Jumps " + streak);
    }
    void Fail()
    {
        streak = 0;
        //Debug.Log("Fuck");
    }
}
