using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour
{
    public NavMeshAgent opponentAgent;
    public GameObject target, speedBoostObject;
    bool speedboostTrue = false, bumperObsTrue = false;
    public float speedboostTimer, bumperObsTimer;
    float speed;
    Vector3 startPos;

    float speedBoostTimerAtStart, bumperObsTimerAtStart;

    bool endOfTheGame = false;
    void Start()
    {
        startPos = transform.position;
        opponentAgent = GetComponent<NavMeshAgent>();
        speed = opponentAgent.speed;
        speedBoostObject.SetActive(false);


        speedBoostTimerAtStart = speedboostTimer;
        bumperObsTimerAtStart = bumperObsTimer;
        endOfTheGame = false;
    }


    void Update()
    {
        if(endOfTheGame)
        {
            opponentAgent.enabled = false;
            return;
        }

        opponentAgent.SetDestination(target.transform.position);

        if (speedboostTrue)
        {
            speedboostTimer -= Time.deltaTime;
            if (speedboostTimer <= 0)
            {
                opponentAgent.speed = speed;
                speedboostTimer = speedBoostTimerAtStart;
                speedboostTrue = false;
                speedBoostObject.SetActive(false);
            }
        }

        if(bumperObsTrue)
        {
            bumperObsTimer -= Time.deltaTime;
            if(bumperObsTimer <= 0)
            {
                opponentAgent.speed = speed;
                bumperObsTimer = bumperObsTimerAtStart;
                bumperObsTrue = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("speedboost"))
        {
            if (bumperObsTrue)
            {
                bumperObsTimer = bumperObsTimerAtStart;
                bumperObsTrue = false;
                opponentAgent.speed = speed;
            }

            if (speedboostTrue)
            {
                speedboostTimer = speedBoostTimerAtStart;
                return;
            }

            speedBoostObject.SetActive(true);
            opponentAgent.speed += 5;
            speedboostTrue = true;
        }
        else if (other.CompareTag("BumperObs"))
        {
            if (speedboostTrue)
            {
                speedboostTimer = speedBoostTimerAtStart;
                speedboostTrue = false;
                opponentAgent.speed = speed;
            }

            if (bumperObsTrue)
            {
                bumperObsTimer = bumperObsTimerAtStart;
                return;
            }

            opponentAgent.speed -= 5;
            bumperObsTrue = true;
            speedBoostObject.SetActive(false);
        }
        else if (other.CompareTag("End"))
        {
            speedBoostObject.SetActive(false);
            Debug.Log("Congrats!..");
            endOfTheGame = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Touched the obstacle!..");
            transform.position = startPos;
        }
    }

}
