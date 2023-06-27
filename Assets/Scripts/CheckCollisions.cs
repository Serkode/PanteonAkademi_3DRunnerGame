using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CheckCollisions : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI coinText;
    public PlayerController playerController;
    Vector3 startPos;

    public int maxScore;

    public Animator playerAnim;
    public GameObject player, finishPanel;

    private void Start()
    {
        startPos = transform.position;
        playerAnim = player.GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            //Debug.Log("Coin collected!..");
            AddCoin();
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("End"))
        {
            Debug.Log("Congrats!..");
            playerController.runningSpeed = 0;
            playerController.enabled = false;
            transform.Rotate(transform.rotation.x, 180, transform.rotation.z, Space.Self);
            finishPanel.SetActive(true);

            if(score > maxScore)
            {
                Debug.Log("You Win!..");
                playerAnim.SetBool("Win", true);
                playerAnim.SetBool("Lose", false);
            }
            else
            {
                Debug.Log("You Lose!..");
                playerAnim.SetBool("Lose", true);
                playerAnim.SetBool("Win", false);
            }
        }
    }

    public void AddCoin()
    {
        score++;
        coinText.text = "Score: " + score.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Touched the obstacle!..");
            transform.position = startPos;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
