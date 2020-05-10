using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private ParticleSystem eatParticleSystem;
    [SerializeField] private GameObject deathSound;
    [SerializeField] private GameObject eatSound;
    [SerializeField] private Camera camera;

    private Rigidbody2D rigidbody;
    
    private Vector2 movement;
    private float verticalMovement;
    private Vector3 minPos;
    private Vector3 maxPos;

    private int score;

    private const float DIST_FROM_SCREEN = 2f;
    private const float SPEED = 2000f;
    private const float MAX_SPEED = 10f;

    private const int SCORE_TO_WIN = 3;

    // Start is called before the first frame update
    void Start()
    {
        // Set player position
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float playerX = ScreenUtility.getXLeftOnscreen(renderer, camera) + DIST_FROM_SCREEN;
        transform.position = new Vector3(playerX, 0);

        rigidbody = GetComponent<Rigidbody2D>();
        movement = new Vector2();

        minPos = new Vector3(playerX, ScreenUtility.getYDownOnscreen(renderer, camera));
        maxPos = new Vector3(playerX, ScreenUtility.getYUpOnscreen(renderer, camera));
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
        // Destroy the donut
        Destroy(collision.gameObject);

        if (collision.CompareTag("RegularDonut"))
        {
            // Regular = +1 to score
            Instantiate(eatParticleSystem, transform.position, Quaternion.identity);
            Instantiate(eatSound, transform.position, Quaternion.identity);
            score++;

            // Win
            if (score == SCORE_TO_WIN)
            {
                // Play burp sound = death sound
                GameObject burpSound = Instantiate(deathSound, transform.position, Quaternion.identity);

                // Go back to DonutDash scene
                SceneManager.UnloadSceneAsync("BossFight");
                DonutDashSingleton.setActive(true);
            }
        }
        else if (collision.CompareTag("FlamingDonut"))
        {
            // Flaming = dead
            Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
            Instantiate(deathSound, transform.position, Quaternion.identity);

            // Go back to DonutDash scene
            SceneManager.UnloadSceneAsync("BossFight");
            DonutDashSingleton.setActive(true);
        }
    }
}
