using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFightPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticleSystem;
    [SerializeField] private ParticleSystem eatParticleSystem;
    [SerializeField] private GameObject deathSound;
    [SerializeField] private GameObject eatSound;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject UI;
    [SerializeField] private Text gameOverText;
    [SerializeField] private GameObject bossOutro;

    [SerializeField] private int scoreToWin = 5;

    private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    
    private Vector2 movement;
    private float verticalMovement;
    private Vector3 minPos;
    private Vector3 maxPos;

    private int score;
    private bool isPlayerDead;

    private const float DIST_FROM_SCREEN = 2f;
    private const float SPEED = 2000f;
    private const float MAX_SPEED = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Set player position
        renderer = GetComponent<SpriteRenderer>();
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
        if (!isPlayerDead)
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
        else
        {
            // Destroy player once off screen
            if (renderer.bounds.max.y < ScreenUtility.getDownEdge(camera))
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isPlayerDead)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayerDead)
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
                if (score == scoreToWin)
                {
                    // Activate boss outro
                    bossOutro.SetActive(true);
                }
            }
            else if (collision.CompareTag("FlamingDonut"))
            {
                // Screen shake
                ScreenShake screenShake = camera.GetComponent<ScreenShake>();
                screenShake.enabled = true;
                screenShake.triggerShake();

                // Flag player as dead
                isPlayerDead = true;

                // Flaming = dead
                Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
                Instantiate(deathSound, transform.position, Quaternion.identity);

                // Change player rigid body type to Dynamic
                rigidbody.bodyType = RigidbodyType2D.Dynamic;

                // Turn gravity on for player
                rigidbody.gravityScale = 3f;

                // Add torque
                Vector3 force = new Vector2(Random.Range(0, 2) == 0 ? 50f : -50f, 0f);
                Vector2 pos = new Vector2(transform.position.x, renderer.bounds.max.y);
                rigidbody.AddForceAtPosition(force, pos);

                // Remove animation
                GetComponent<Animator>().enabled = false;

                // Reactive UI
                UI.SetActive(true);

                // Update high score
                int highscore = MyGameManager.updateHighScore(Player.totalScore);

                // Update GameOver text
                gameOverText.text = "The donut thief got away!\nHighscore: " + highscore + "\nScore: " + Player.totalScore + "\nPress 'R' to replay!";
            }
        }
    }

    public bool playerIsDead()
    {
        return isPlayerDead;
    }
}
