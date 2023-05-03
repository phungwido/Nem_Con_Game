using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    public static GameControl Instance { get; private set; }
    public GameObject pfBall;
    public float timeSpawn = 5;

    bool hardMode, easyMode;

    public int time;

    public Text timetxt;
    public Image loseGame;
    public Image startLevel;
    public Image startGame;

    public bool cantPlay;

    public bool ballClick;

    public Rigidbody ball;
    public Slider powerSlider;
    public float throwForce;
    GameObject _obj;
    public bool planeClick;
    public bool onPlane;
    private void Awake()
    {
    }
    void Start () {
        Instance = this;
        score = 0;
        SpawnBall();
    }
	
	void SpawnBall()
    {
        _obj = Instantiate(pfBall, transform);
        _obj.SetActive(true);
        ball = _obj.GetComponent<Rigidbody>();
        ball.isKinematic = true;

    }
    public void OnThown()
    {
        Invoke("SpawnBall", timeSpawn);
    }

    int _score;
    int score {
        get {
            return _score;
        }
        set {
            _score = value;
            txtScore.text = "Score " + _score;
        }
    }
    public void OnScore()
    {
        score = score + 1;
    }
    public Text txtScore;
    public void ResetGame()
    {
        score = 0;
    }

    public void OnclickHardMode()
    {
        hardMode = true;
        time = 60;
        StartCoroutine(UpdateTimer(time));

    }

    public void OnclickEasyMode()
    {
        easyMode = true;
        time = 90;
        StartCoroutine(UpdateTimer(time));

    }

    public IEnumerator UpdateTimer(float time)
    {
        while (time > 0)
        {
            timetxt.text = "Time is " + time.ToString();
            if (onPlane)
            {
                time--;
                if (time <= 0)
                {
                    cantPlay = true;
                    Debug.Log("out of time");
                    loseGame.gameObject.SetActive(true);

                }
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }

    public void StartLevel()
    {
        startGame.gameObject.SetActive(false);
        startLevel.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        loseGame.gameObject.SetActive(false);
        startGame.gameObject.SetActive(true);

    }

    private Vector3 initialPosition;

    void Update()
    {
        throwForce = powerSlider.value * 100  ;
        if (Input.GetMouseButtonUp(0) && !planeClick)
        {
            initialPosition = _obj.transform.position;
            Vector3 destination = new Vector3(0, 1, 1);
            Vector3 direction = (destination - initialPosition).normalized;
            Vector3 force = direction * throwForce;
            ball.isKinematic= false;
            ball.AddForce(Vector3.up * 8);
            ball.AddForce(force);
        }
    }

}
