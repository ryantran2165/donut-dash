using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private GameObject deathSound;

    private Rigidbody2D rigidbody;
    
    private Vector2 movement;
    private float verticalMovement;
    private Vector3 minPos;
    private Vector3 maxPos;

    private const float DIST_FROM_SCREEN = 2f;
    private const float SPEED = 2000f;
    private const float MAX_SPEED = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Set player position
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float playerX = ScreenUtility.getXLeftOnscreen(renderer) + DIST_FROM_SCREEN;
        transform.position = new Vector3(playerX, 0);

        rigidbody = GetComponent<Rigidbody2D>();
        movement = new Vector2();

        minPos = new Vector3(playerX, ScreenUtility.getYDownOnscreen(renderer));
        maxPos = new Vector3(playerX, ScreenUtility.getYUpOnscreen(renderer));
    }

    // Update is called once per frame
    void Update()
    {
        // Poll input
        verticalMovement = Input.GetAxisRaw("Vertical") * SPEED;

        // Clamp position
        if (transform.position.y <= minPos.y)
        {
            transform.position = minPos;
        }
        else if (transform.position.y >= maxPos.y)
        {
            transform.position = maxPos;
        }
    }

    private void FixedUpdate()
    {
        // Move vertical
        movement.Set(0, verticalMovement * Time.fixedDeltaTime);
        rigidbody.AddForce(movement);

        // Clamp velocity
        if (rigidbody.velocity.y > MAX_SPEED)
        {
            rigidbody.velocity = new Vector2(0, MAX_SPEED);
        }
        else if (rigidbody.velocity.y < -MAX_SPEED)
        {
            rigidbody.velocity = new Vector2(0, -MAX_SPEED);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only collision possible is donut
        Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        Instantiate(deathSound, transform.position, Quaternion.identity);

        MySceneManager.LoadPreviousScene();
    }
}
