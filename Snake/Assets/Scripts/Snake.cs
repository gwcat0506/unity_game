using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    float speedMove = 3f;   // 이동속도
    float speedRot = 120f;  // 회전 속도
    
    bool isDead = false;    // 사망?

    Transform coin;

    List<Transform> tails = new List<Transform>();      //오브젝트의 Transform을 저장할 List 생성

    //UI
    GameObject panelOver;
    Text txtCoin;
    Text txtTime;
    int coinCnt = 0;
    float startTime;

    //Joystick
    GameObject panelStick;
    Joystick stick;
    bool isMobile;

    void Awake() 
    {
        InitGame();
        startTime = Time.time;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!isDead) {
            MoveHead();
            MoveTail();
            SetTime();
        }
    }

    // 머리 이동 및 회전
    void MoveHead() 
    {
        //이동
        float amount = speedMove * Time.deltaTime;
        transform.Translate(Vector3.forward * amount);          // 게임 화면의 전방(z축)으로 이동

        //회전       
        if(!isMobile) {
            amount = Input.GetAxis("Horizontal") * speedRot;    
        } else {
            amount = stick.Horizontal() * speedRot;
        }

        transform.Rotate(Vector3.up * amount * Time.deltaTime); // y축으로 회전
    }

    // 충돌의 판정과 처리
    void OnCollisionEnter (Collision other) 
    {
        switch (other.transform.tag) {
            case "Coin" :
                MoveCoin();   // 시험문제: 코인 위치를 이동하기 위한 함수 콜
                AddTail();   // 시험문제: 꼬리를 붙이기 위한 함수 콜
                break;
            case "Wall" :
            case "Tail" :
                isDead = true;   // 시험문제: 벽이나 꼬리에 부딪히면 사망!
                panelOver.SetActive(isDead);
                break;
        }
    }

    // Coin 이동
    void MoveCoin() 
    {
        //Coin을 이동할 범위 - 벽의 안쪽
        float x;
        float z;

        x = Random.Range(-9f,9f);    // 시험문제: 코인의 위치가 무작위로 벽 안에 위치할 수 있도록 x, z 위치를 생성
        z = Random.Range(-4f,4f);    // 코인이 벽면에 너무 붙어서 나오지 않도록 Scene에서 벽 위치를 확인하고, 적당히 조정

        coin.position = new Vector3(x,0,z);   // 코인의 위치를 지정
        coinCnt++;
    }

    // 게임 초기화
    void InitGame() {
        coin = GameObject.Find("Coin").transform;

        //UI Widget
        panelOver = GameObject.Find("PanelOver");
        panelOver.SetActive(false);

        txtCoin = GameObject.Find("TxtCoin").GetComponent<Text>();
        txtTime = GameObject.Find("TxtTime").GetComponent<Text>();

        // Mobile Device 인가?
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        // 테스트용 코드
        isMobile = true;
        panelStick = GameObject.Find("PanelStick");
        panelStick.SetActive(isMobile);
        stick = panelStick.transform.GetChild(0).GetComponent<Joystick>();
    }

    // 꼬리 추가
    void AddTail() {
        GameObject tail = Instantiate(Resources.Load("Tail")) as GameObject;    // Resources 폴더의 프리팹 Tail을 씬에 추가
        Vector3 pos = transform.position;                                       // 뱀의 머리 좌표

        //꼬리의 개수 구하기
        int cnt = tails.Count;         // List의 크기 구하기                                         

        if(cnt == 0) {
            tail.tag = "Untagged";      // 첫 번째 꼬리는 Tag를 제거해서 머리와 충돌 방지
        } else {
            pos = tails[cnt-1].position;    // 두 번째 이후부터는 마지막 고리 위치로 설정. List는 배열과 같은 형식으로 다룰 수 있음
        }

        tail.transform.position = pos;      // 씬에 추가한 고리의 위치 설정

        //꼬리의 색
        Color[] colors = {new Color(0, 0.5f, 0, 1), new Color(0, 0.5f, 1, 1)};  // 꼬리에 설정할 컬러를 배열로 만들기
        int n = cnt / 3 % 2;            // 0 or 1
        tail.GetComponent<Renderer>().material.color = colors[n];       // 꼬리의 머터리얼 컬러 설정

        tails.Add(tail.transform);          // 꼬리 붙임
    }

    // 꼬리 이동 및 회전
    void MoveTail() {
        Transform target = transform;       // 초기 이동 목표는 뱀의 머리

        foreach(Transform tail in tails) {      // tails에 저장되어 있는 모든 자료를 순차적으로 처리. tails의 자료를 하나식 변수 tail로 읽어옴
            Vector3 pos = target.position;      // 이동 목적지 좌표
            Quaternion rot = target.rotation;   // 이동 목적지 회전각

            //이동 및 회전: 선형 보간
            tail.position = Vector3.Lerp(tail.position, pos, 4 * Time.deltaTime);       
            tail.rotation = Quaternion.Lerp(tail.rotation, rot, 4 * Time.deltaTime);

            target = tail;              //처리를 마친 꼬리를 다음 꼬리의 목표로 설정
        }
    }

    //UI Time
    void SetTime() {
        txtCoin.text = coinCnt.ToString("Coin : 0");

        float span = Time.time - startTime;
        int h = Mathf.FloorToInt(span / 3600);
        int m = Mathf.FloorToInt(span / 60%60);
        float s = span % 60;

        txtTime.text = string.Format("Time : {0:0}:{1:0}:{2:00.0}", h, m, s);        
    }

    //Button Click
    public void OnButtonClick(Button button) {
        switch(button.name){
            case "BtnYes" :
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "BtnNo" :
                Application.Quit();
                break;    
        }
    }




}
