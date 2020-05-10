using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour
{
    [SerializeField] private GameObject bird;
    [SerializeField] private Transform parent;

    private SpriteRenderer renderer;
    private float timer;
    private const float MIN_SPAWN_TIME = 10f;
    private const float MAX_SPAWN_TIME = 15f;
    private const float MIN_SPAWN_Y = 12f;
    private const float MAX_SPAWN_Y = 16f;
    private const float MIN_VEL = 5f;
    private const float MAX_VEL = 10f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = bird.GetComponent<SpriteRenderer>();
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
            float spawnX = birdDirection == 0 ? ScreenUtility.getXRightOffscreen(renderer) : ScreenUtility.getXLeftOffscreen(renderer);
            float spawnY = Random.Range(MIN_SPAWN_Y, MAX_SPAWN_Y);

            // Create bird
            GameObject birdObject = Instantiate(bird, new Vector3(spawnX, spawnY), Quaternion.identity, parent);

            // Flip x scale if flying right
            if (birdDirection == 1)
            {
                birdObject.transform.localScale = new Vector3(-1, 1, 1);
            }

            // Set directional velocity
            float randVel = Random.Range(MIN_VEL, MAX_VEL);
            Rigidbody2D rigidbody = birdObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(-randVel * birdObject.transform.localScale.x, 0);

            // Save the velocity for when leaving main DonutDash scene
            birdObject.GetComponent<Bird>().saveVelocity();

            // Reset timer
            timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
        }
    }
}
