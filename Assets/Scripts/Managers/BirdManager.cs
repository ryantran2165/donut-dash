using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] private GameObject bird;

    private float timer;
    private float birdWidth;
    private const float MIN_SPAWN_TIME = 10f;
    private const float MAX_SPAWN_TIME = 15f;
    private const float MIN_SPAWN_Y = 12f;
    private const float MAX_SPAWN_Y = 16f;
    private const float MIN_VEL = 5f;
    private const float MAX_VEL = 10f;

    // Start is called before the first frame update
    void Start()
    {
        birdWidth = bird.GetComponent<SpriteRenderer>().bounds.size.x;
        timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease timer
        timer -= Time.deltaTime;

        // Timer is out, spawn new bird
        if (timer < 0)
        {
            // Get bird properties
            int birdDirection = Random.Range(0, 2);
            float spawnX = birdDirection == 0 ? ScreenUtility.getRightEdge() + birdWidth / 2f : ScreenUtility.getLeftEdge() - birdWidth / 2f;
            float spawnY = Random.Range(MIN_SPAWN_Y, MAX_SPAWN_Y);

            // Create bird
            GameObject birdObject = Instantiate(bird, new Vector3(spawnX, spawnY), Quaternion.identity);

            // Flip x scale if flying right
            if (birdDirection == 1)
            {
                birdObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            // Set directional velocity
            float randVel = Random.Range(MIN_VEL, MAX_VEL);
            Rigidbody2D rigidbody = birdObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(-randVel * birdObject.transform.localScale.x, 0);

            // Reset timer
            timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
        }
    }
}
