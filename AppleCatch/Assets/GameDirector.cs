using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    GameObject timerText;
    GameObject pointText;

    float time = 30.0f;
    int point = 0;
    
    public void GetApple()
    {
        this.point += 100;
    }

    public void GetBomb()
    {
        this.point /=2;
    }

    void Start()
    {
        this.timerText = GameObject.Find("Time");
        this.pointText = GameObject.Find("Point");
    }

    void Update()
    {
        this.time -= Time.deltaTime;
        this.timerText.GetComponent<Text>().text = this.time.ToString("F1");
        this.pointText.GetComponent<Text>().text = this.point.ToString()+" point";
    }
}
