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
    private InGameRanking ig;
    private void Start()
    {
        startPos = transform.position;
        playerAnim = player.GetComponentInChildren<Animator>();
        ig = FindObjectOfType<InGameRanking>();
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
            if (ig.namesText[6].text == "Player")
            {
                PlayerFinished();
                playerAnim.SetBool("Win", true);
                playerAnim.SetBool("Lose", false);
            }
            else
            {
                PlayerFinished();
                playerAnim.SetBool("Lose", true);
                playerAnim.SetBool("Win", false);
            }
        }
    }

    void PlayerFinished()
    {
        playerController.runningSpeed = 0;
        playerController.enabled = false;
        transform.Rotate(transform.rotation.x, 180, transform.rotation.z, Space.Self);
        finishPanel.SetActive(true);
        GameManager.Instance.isGameOver = true;
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
