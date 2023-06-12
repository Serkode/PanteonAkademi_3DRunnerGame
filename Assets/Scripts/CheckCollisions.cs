using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckCollisions : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI coinText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            //Debug.Log("Coin collected!..");
            AddCoin();
            //Destroy(other.gameObject);
            other.gameObject.SetActive(false);
        }
    }

    public void AddCoin()
    {
        score++;
        coinText.text = "Score: " + score.ToString();
    }
}
