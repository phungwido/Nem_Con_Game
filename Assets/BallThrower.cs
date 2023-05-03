using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    public GameObject Ball;

    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos;
    private Vector2 endPos;

    public float MinSwipDist = 0;
    private float BallVelocity = 50;
    private float BallSpeed = 500;
    public float MaxBallSpeed = 350;
    private Vector3 angle;

    private bool thrown, holding;
    private Vector3 newPosition;
    Rigidbody rb;

    [SerializeField] Camera cameraAR;


    void Start()
    {
        setupBall();
    }

    void setupBall()
    {
        //GameObject _ball = GameObject.FindGameObjectWithTag("Player");
        //Ball = _ball;
        rb = Ball.GetComponent<Rigidbody>();
        ResetBall();
    }

    void ResetBall()
    {
        angle = Vector3.zero;
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        BallSpeed = 0;
        startTime = 0;
        endTime = 0;
        swipeDistance = 0;
        swipeTime = 0;
        thrown = holding = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        //Ball.transform.position = transform.position;
    }
    void PickupBall()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cameraAR.nearClipPlane * 5f;
        newPosition = cameraAR.ScreenToWorldPoint(mousePos);
        Ball.transform.localPosition = Vector3.Lerp(Ball.transform.localPosition, newPosition, 80f * Time.deltaTime);
    }
    private void Update()
    {
        if (holding)
            PickupBall();

        if (thrown)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cameraAR.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;

            if (Physics.Raycast(ray, out _hit, 100f))
            {
                if (_hit.transform == Ball.transform)
                {
                    startTime = Time.time;
                    startPos = Input.mousePosition;
                    holding = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            endTime = Time.time;
            endPos = Input.mousePosition;
            swipeDistance = (endPos - startPos).magnitude;
            swipeTime = endTime - startTime;
            Debug.Log("Kokoko");
            //throw ball
            //CalSpeed();
            //CalAngle();
            rb.AddForce(new Vector3((angle.x * BallSpeed), (angle.y * BallSpeed / 3), (angle.z * BallSpeed) * 2));
            rb.useGravity = true;
            holding = false;
            thrown = true;
            Invoke("ResetBall", 4f);
            //if (swipeTime < 0.5f && swipeDistance > 30f)
            //{
                
            //}
            //else
            //    ResetBall();
        }
    }
    private void CalAngle()
    {
        angle = cameraAR.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50f, (cameraAR.nearClipPlane + 5)));
    }

    void CalSpeed()
    {
        if (swipeTime > 0)
            BallVelocity = swipeDistance / (swipeDistance - swipeTime);

        BallSpeed = BallVelocity * 40;

        if (BallSpeed <= MaxBallSpeed)
        {
            BallSpeed = MaxBallSpeed;
        }
        swipeTime = 0;
    }
}
