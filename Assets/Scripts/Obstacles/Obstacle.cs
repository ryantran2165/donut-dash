using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Screen shake
            ScreenShake screenShake = Camera.main.GetComponent<ScreenShake>();
            screenShake.enabled = true;
            screenShake.triggerShake();

            // Set game over
            GameObject playerObject = collision.gameObject;
            playerObject.GetComponent<Player>().setGameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Screen shake
            ScreenShake screenShake = Camera.main.GetComponent<ScreenShake>();
            screenShake.enabled = true;
            screenShake.triggerShake();

            // Set game over
            GameObject playerObject = collision.gameObject;
            playerObject.GetComponent<Player>().setGameOver();
        }
    }
}
