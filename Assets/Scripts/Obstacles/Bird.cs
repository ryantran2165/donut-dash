using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private GameObject birdPoop;

    private Rigidbody2D birdRigidBody;
    private float birdWidth;
    private float timer;
    private const float MIN_SPAWN_TIME = .5f;
    private const float MAX_SPAWN_TIME = 1.5f;

    private Vector2 savedVelocity = new Vector2();

    void Awake()
    {
        birdRigidBody = GetComponent<Rigidbody2D>();
        birdWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
    }

    void OnEnable()
    {
        // Coming back from boss fight, bird loses its velocity, so have to set again
        birdRigidBody.velocity = savedVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease timer
        timer -= Time.deltaTime;

        // Timer is out, spawn new bird poop
        if (timer < 0)
        {
            float spawnX = transform.position.x + .25f * birdWidth * transform.localScale.x;
            float spawnY = transform.position.y;

            // Create bird poop
            GameObject birdPoopObject = Instantiate(birdPoop, new Vector3(spawnX, spawnY), Quaternion.identity);

            // Set the poop's x-scale
            float scale = birdPoopObject.transform.localScale.x;
            birdPoopObject.transform.localScale = new Vector3(transform.localScale.x < 0 ? scale : -scale, scale, scale);

            // Set poop's velocity to half of bird's
            Rigidbody2D poopRigidBody = birdPoopObject.GetComponent<Rigidbody2D>();
            poopRigidBody.velocity = new Vector2(birdRigidBody.velocity.x / 2f, 0f);

            // Reset timer
            timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
        }
    }

    public void saveVelocity()
    {
        // Save velocity for when leaving main DonutDas scene
        savedVelocity = birdRigidBody.velocity;
    }
}
