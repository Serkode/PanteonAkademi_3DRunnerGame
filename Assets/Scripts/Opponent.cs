using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opponent : MonoBehaviour
{
    public NavMeshAgent opponentAgent;
    public GameObject target, speedBoostObject, gObj;
    bool speedboostTrue = false, bumperObsTrue = false;
    public float speedboostTimer, bumperObsTimer;
    float speed;
    Vector3 startPos;
    Vector3 stopPos;

    float speedBoostTimerAtStart, bumperObsTimerAtStart;

    bool endOfTheGame = false;


    private InGameRanking ig;
    public Animator anim;

    void Start()
    {
        startPos = transform.position;
        opponentAgent = GetComponent<NavMeshAgent>();
        speed = opponentAgent.speed;
        speedBoostObject.SetActive(false);
        stopPos = new Vector3(Random.Range(-3.0f, 3.0f), transform.position.y, target.transform.position.z);


        speedBoostTimerAtStart = speedboostTimer;
        bumperObsTimerAtStart = bumperObsTimer;
        endOfTheGame = false;

        ig = FindObjectOfType<InGameRanking>();
        anim = gObj.GetComponentInChildren<Animator>();
    }


    void Update()
    {

        opponentAgent.SetDestination(/*target.transform.position*/stopPos);

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

        if (endOfTheGame)
        {
            //opponentAgent.enabled = false;
            opponentAgent.speed = 0;
        }
        else if(GameManager.Instance.isGameOver)
        {
            opponentAgent.speed = 0;
            anim.SetBool("Win", false);
            anim.SetBool("Lose", true);
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

            if (ig.namesText[6].text == transform.name && PlayerPrefs.GetInt("FirsPlace") == 0)
            {
                PlayerPrefs.SetInt("FirstPlace", 1);
                transform.position = new Vector3(transform.position.x, transform.position.y, target.transform.position.z);
                speedBoostObject.SetActive(false);
                Debug.Log("Congrats!..");
                endOfTheGame = true;
                transform.Rotate(transform.rotation.x, 180, transform.rotation.z, Space.Self);

                anim.SetBool("Win", true);
                anim.SetBool("Lose", false);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, target.transform.position.z-1.0f);
                speedBoostObject.SetActive(false);
                Debug.Log("Congrats!..");
                endOfTheGame = true;
                transform.Rotate(transform.rotation.x, 180, transform.rotation.z, Space.Self);

                anim.SetBool("Win", false);
                anim.SetBool("Lose", true);
            }

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
