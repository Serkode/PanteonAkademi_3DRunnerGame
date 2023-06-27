using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float runningSpeed;
    public float xSpeed;
    public float limitX;
    public GameObject speedBoostObject;
    float speedAtStart;

    bool speedboostTrue = false, bumperObsTrue = false;
    public float speedboostTimer, bumperObsTimer;
    float speedBoostTimerAtStart, bumperObsTimerAtStart;

    void Start()
    {
        speedAtStart = runningSpeed;
        speedBoostObject.SetActive(false);

        speedBoostTimerAtStart = speedboostTimer;
        bumperObsTimerAtStart = bumperObsTimer;
    }


    void Update()
    {
        SwipeCheck();
        SpeedBoostAndBumperObsCheck();

    }

    private void SpeedBoostAndBumperObsCheck()
    {
        if (speedboostTrue)
        {
            speedboostTimer -= Time.deltaTime;
            if (speedboostTimer <= 0)
            {
                runningSpeed = speedAtStart;
                speedboostTimer = speedBoostTimerAtStart;
                speedboostTrue = false;
                speedBoostObject.SetActive(false);
            }
        }


        if (bumperObsTrue)
        {
            bumperObsTimer -= Time.deltaTime;
            if (bumperObsTimer <= 0)
            {
                runningSpeed = speedAtStart;
                bumperObsTimer = bumperObsTimerAtStart;
                bumperObsTrue = false;
            }
        }
    }

    void SwipeCheck()
    {
        float newX = 0;
        float touchXDelta = 0;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            //Debug.Log(Input.GetTouch(0).deltaPosition.x / Screen.width);
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }
        newX = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX);

        Vector3 newPosition = new Vector3(newX,
                                          transform.position.y,
                                          transform.position.z + runningSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("speedboost"))
        {
            if (bumperObsTrue)
            {
                bumperObsTimer = bumperObsTimerAtStart;
                bumperObsTrue = false;
                runningSpeed = speedAtStart;
            }

            if (speedboostTrue)
            {
                speedboostTimer = speedBoostTimerAtStart;
                return;
            }

            speedBoostObject.SetActive(true);
            runningSpeed += 5;
            speedboostTrue = true;
        }

        if (other.CompareTag("BumperObs"))
        {
            if (speedboostTrue)
            {
                speedboostTimer = speedBoostTimerAtStart;
                speedboostTrue = false;
                runningSpeed = speedAtStart;
            }

            if (bumperObsTrue)
            {
                bumperObsTimer = bumperObsTimerAtStart;
                return;
            }

            runningSpeed -= 5;
            bumperObsTrue = true;
            speedBoostObject.SetActive(false);
        }

        if (other.CompareTag("End"))
        {
            Debug.Log("Congrats!..");
            speedBoostObject.SetActive(false);
        }
    }
}
