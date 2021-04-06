using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranking : MonoBehaviour
{

    float playerTime = 100;
    float aiTime = 100;

    public GameManager gameManager;

    float time;

    private void Update()
    {
        //time = Time.timeSinceLevelLoad;
        if (playerTime < aiTime)
            gameManager.whoWon = "Won!!";
        else
            gameManager.whoWon = "Lost";

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            playerTime = Time.timeSinceLevelLoad;
        }
        else
        {
            aiTime = Time.timeSinceLevelLoad;
            Debug.Log("AI TIME: " + aiTime);
        }
    }
}
