using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightVillian : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flamingDonut;
    [SerializeField] private GameObject regularDonut;
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject flamingDonutSound;
    [SerializeField] private GameObject regularDonutSound;
    [SerializeField] private Camera camera;

    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;

    // Movement
    private Vector2 movement;
    private float verticalMovement;
    private Vector3 minPos;
    private Vector3 maxPos;

    // Spawn donut
    private float timer;
    private Vector2 donutVelocity = new Vector2(-20f, 0f);

    // Game over
    private Vector2 offScreenTarget;
    private bool alreadySetVel;

    private const float DIST_FROM_SCREEN = 2f;
    private const float SPEED = 250f;
    private const float MAX_SPEED = 10f;
    
    private const float MIN_TIME_BETWEEN_SHOTS = 0.25f;
    private const float MAX_TIME_BETWEEN_SHOTS = 1f;
    private const float REGULAR_DONUT_PROBABILITY = 0.15f;

    private const float FLY_OFF_VEL = 20f;
    private const float ARRIVAL_THRESHOLD = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Set villian position
        renderer = GetComponent<SpriteRenderer>();
        float villianX = ScreenUtility.getXRightOnscreen(renderer, camera) - DIST_FROM_SCREEN;
        transform.position = new Vector3(villianX, 0);

        rigidbody = GetComponent<Rigidbody2D>();
        movement = new Vector2();

        minPos = new Vector3(villianX, ScreenUtility.getYDownOnscreen(renderer, camera));
        maxPos = new Vector3(villianX, ScreenUtility.getYUpOnscreen(renderer, camera));
    }

    void Update()
    {
        if (player == null)
        {
            // Set fly off velocity
            if (!alreadySetVel)
            {
                alreadySetVel = true;

                // Target off screen
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                offScreenTarget = new Vector2(ScreenUtility.getXRightOffscreen(renderer, camera), ScreenUtility.getYUpOffscreen(renderer, camera));

                // Set velocity
                float dx = offScreenTarget.x - transform.position.x;
                float dy = offScreenTarget.y - transform.position.y;
                float magnitude = Mathf.Sqrt(dx * dx + dy * dy);
                rigidbody.bodyType = RigidbodyType2D.Kinematic;
                rigidbody.velocity = new Vector2(dx / magnitude * FLY_OFF_VEL, dy / magnitude * FLY_OFF_VEL);
            }

            // Destroy once off screen
            if ((Mathf.Abs(transform.position.x - offScreenTarget.x) < ARRIVAL_THRESHOLD) && (Mathf.Abs(transform.position.y - offScreenTarget.y) < ARRIVAL_THRESHOLD))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // Calculate vertical movement
            verticalMovement = (player.transform.position.y - transform.position.y) * SPEED;

            // Clamp position
            if (transform.position.y <= minPos.y)
            {
                transform.position = minPos;
            }
            else if (transform.position.y >= maxPos.y)
            {
                transform.position = maxPos;
            }

            // Shoot
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                // Reset timer
                timer = Random.Range(MIN_TIME_BETWEEN_SHOTS, MAX_TIME_BETWEEN_SHOTS);

                // Spawn donut and create sound
                GameObject donut;
                if (Random.Range(0f, 1f) < REGULAR_DONUT_PROBABILITY)
                {
                    // Regular
                    donut = Instantiate(regularDonut, new Vector3(renderer.bounds.min.x, transform.position.y), Quaternion.identity, parent);
                    Instantiate(regularDonutSound, transform.position, Quaternion.identity);
                }
                else
                {
                    // Flaming
                    donut = Instantiate(flamingDonut, new Vector3(renderer.bounds.min.x, transform.position.y), Quaternion.identity, parent);
                    Instantiate(flamingDonutSound, transform.position, Quaternion.identity);
                }


                // Set donut velocity
                Rigidbody2D donutRigidBody = donut.GetComponent<Rigidbody2D>();
                donutRigidBody.velocity = donutVelocity;
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
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
}
