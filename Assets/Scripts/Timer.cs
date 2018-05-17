using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public static Timer instance;
    public Text timer;
    public float timelimit = 60f;
    public float resetTimer;

    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        instance = this;
        resetTimer = timelimit;
    }
	
	// Update is called once per frame
	void Update () {
        if (timelimit > 0)
			timelimit -= Time.deltaTime;
        else
            timer.color = Color.red;
           
        string timertext = timelimit.ToString("f1");
        timer.text = "Timer - " + timertext;
    }

    public void timer_reset()
    {
        timelimit = resetTimer;
        timer.color = Color.black;
    }
}
