using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Villian : MonoBehaviour
{
    [SerializeField] private GameObject flamingDonut;

    private GameObject player;

    private Rigidbody2D villianRigidbody;
    private Vector2 movement;
    private float horizontalMovement;
    private float flamingDonutTimer;
    private float targetVariation;
    private Vector2 offScreenTarget;
    private bool alreadySetVel;
    private float bossFightTimer = 15f;
    private const float MIN_SPAWN_TIME = 1f;
    private const float MAX_SPAWN_TIME = 2f;
    private const float SPEED = 500f;
    private const float MAX_SPEED = 5f;
    private const float MAX_TARGET_VARIATION = 4f;
    private const float ARRIVAL_THRESHOLD = 1f;
    private const float FLY_OFF_VEL = 20f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        villianRigidbody = GetComponent<Rigidbody2D>();
        flamingDonutTimer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
        targetVariation = Random.Range(-MAX_TARGET_VARIATION, MAX_TARGET_VARIATION);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Fly off
        if (player == null)
        {
            // Set fly off velocity
            if (!alreadySetVel)
            {
                alreadySetVel = true;

                // Target off screen
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                offScreenTarget = new Vector2(ScreenUtility.getXRightOffscreen(renderer), ScreenUtility.getYUpOffscreen(renderer));

                // Set velocity
                float dx = offScreenTarget.x - transform.position.x;
                float dy = offScreenTarget.y - transform.position.y;
                float magnitude = Mathf.Sqrt(dx * dx + dy * dy);
                villianRigidbody.bodyType = RigidbodyType2D.Kinematic;
                villianRigidbody.velocity = new Vector2(dx / magnitude * FLY_OFF_VEL, dy / magnitude * FLY_OFF_VEL);
            }

            // Destroy once off screen
            if ((Mathf.Abs(transform.position.x - offScreenTarget.x) < ARRIVAL_THRESHOLD) && (Mathf.Abs(transform.position.y - offScreenTarget.y) < ARRIVAL_THRESHOLD))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Calculate horizontal movement
            horizontalMovement = (player.transform.position.x - transform.position.x + targetVariation) * SPEED;

            // Decrease flaming donut timer
            flamingDonutTimer -= Time.deltaTime;

            // Timer is out, spawn new flaming donut
            if (flamingDonutTimer < 0)
            {
                // Create flaming donut
                GameObject flamingDonutObject = Instantiate(flamingDonut, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);

                // Set flaming donut's velocity to half of villian's
                Rigidbody2D donutRigidbody = flamingDonutObject.GetComponent<Rigidbody2D>();
                donutRigidbody.velocity = new Vector2(villianRigidbody.velocity.x / 2f, 0f);

                // Reset timer
                flamingDonutTimer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);

                // Reset target variation
                targetVariation = Random.Range(-MAX_TARGET_VARIATION, MAX_TARGET_VARIATION);
            }

            // Decrease boss fight timer
            bossFightTimer -= Time.deltaTime;

            // Start boss fight
            if (bossFightTimer < 0)
            {
                // Set to boss fight
                DonutDashSingleton.setActive(false);
                SceneManager.LoadScene("BossFight", LoadSceneMode.Additive);

                // Destroy villian
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            // Move horizontal
            movement.Set(horizontalMovement * Time.fixedDeltaTime, 0f);
            villianRigidbody.AddForce(movement);

            // Clamp velocity
            if (villianRigidbody.velocity.x > MAX_SPEED)
            {
                villianRigidbody.velocity = new Vector2(MAX_SPEED, 0);
            }
            else if (villianRigidbody.velocity.x < -MAX_SPEED)
            {
                villianRigidbody.velocity = new Vector2(-MAX_SPEED, 0);
            }
        }
    }
}
