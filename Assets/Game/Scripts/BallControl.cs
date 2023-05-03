using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class BallControl : MonoBehaviour
{
    Rigidbody rigid;
    public float timeLive = 6;

    float startTime, endTime, swipeDistance, swipeTime;
    private Vector3 startPos;
    private Vector3 endPos;

    public float MinSwipDist = 0;
    private float BallVelocity = 100;
    private float BallSpeed = 350;
    public float MaxBallSpeed = 350;
    private Vector3 angle;

    private bool thrown, holding;
    private Vector3 newPosition, resetPos;
    Rigidbody rb;
    [SerializeField] Camera cameraAR;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnThrow()
    {
        GameControl.Instance.OnThown();
        rb.isKinematic = false;
        transform.parent = null;
        Invoke("DestroyBall", timeLive);
        Invoke("CheckEnter", timeLive - 1);
    }
    bool isScore = false;

    void OnTriggerEnter(Collider collision)
    {
        if (isScore == false)
        {
            if (collision.gameObject.name == "Basket")
            {
                isScore = true;
                AudioManager.OnPlayWin();
                GameControl.Instance.OnScore();
                Debug.Log("kokoko" + isScore);
            }
        }

    }
    void CheckEnter()
    {
        if (isScore == false)
        {
            AudioManager.OnPlayLose();
            GameControl.Instance.ResetGame();
        }
    }
    void DestroyBall()
    {
        Destroy(gameObject);
    }

   

}