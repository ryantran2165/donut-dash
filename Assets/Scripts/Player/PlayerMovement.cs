using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1000f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private Camera camera;
    [SerializeField] private AudioClip sticWalkSound;
    [SerializeField] private AudioClip thicWalkSound;
    [SerializeField] private AudioClip thiccWalkSound;
    [SerializeField] private AudioClip sticJumpSound;
    [SerializeField] private AudioClip thicJumpSound;
    [SerializeField] private AudioClip thiccJumpSound;
    [SerializeField] private ParticleSystem jumpParticleSystem;
    [SerializeField] private ParticleSystem walkParticleSystem;

    private AudioSource audioSource;
    private Player player;
    private Rigidbody2D rigidBody;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool jump;
    private float horizontalMove;
    private Vector2 movement;
    private const float LEFT_BOUNDARY_OFFSET = 0.5f;

    private const float STIC_VOLUME = 0.5f;
    private const float THIC_VOLUME = 0.5f;
    private const float THICC_VOLUME = 0.5f;

    private Animator animator;
    private bool alreadySpawnedWalkParticle;
    private float particleY;
    private const float PARTICLE_Y_OFFSET = -0.5f;

    private const float SPEED_BOOST = 1000f;    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        movement = new Vector2();

        // Get jump particle system spawn y position
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        particleY = renderer.bounds.min.y + PARTICLE_Y_OFFSET;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn walk particle system
        int animationProgress = ((int) (animator.GetCurrentAnimatorStateInfo(0).normalizedTime * 100)) % 100;
        if (animationProgress >= 50)
        {
            if (!alreadySpawnedWalkParticle)
            {
                // Only spawn one walk particle for every animation cycle
                alreadySpawnedWalkParticle = true;

                // Create walk particle system
                ParticleSystem walkParticle = Instantiate(walkParticleSystem, new Vector3(transform.position.x, particleY), Quaternion.identity);

                // Rotate particle system 180 degrees if player is walking left
                if (transform.localScale.x < 0)
                {
                    walkParticle.transform.Rotate(new Vector3(0f, 180f, 0f), Space.Self);
                }
            }
        } else
        {
            alreadySpawnedWalkParticle = false;
        }

        // Poll input
        horizontalMove = Input.GetAxisRaw("Horizontal") * (speed + (player.isPlayerCoffeeBoosted() ? SPEED_BOOST : 0));
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        // Walk sound
        if (horizontalMove != 0 && isGrounded && !audioSource.isPlaying)
        {
            if (player.getThiccLevel() < Player.THIC_THRESHOLD) // Stic
            {
                audioSource.PlayOneShot(sticWalkSound, STIC_VOLUME);
            }
            else if (player.getThiccLevel() < Player.THICC_THRESHOLD) // Thic
            {
                audioSource.PlayOneShot(thicWalkSound, THIC_VOLUME);
            } else // Thicc+
            {
                audioSource.PlayOneShot(thiccWalkSound, THICC_VOLUME);
            }
        }

        // Left screen boundary
        float leftEdge = ScreenUtility.getLeftEdge(camera);
        if (player.transform.position.x < leftEdge + LEFT_BOUNDARY_OFFSET)
        {
            player.transform.position = new Vector3(leftEdge + LEFT_BOUNDARY_OFFSET, player.transform.position.y);
        }
    }

    private void FixedUpdate()
    {
        // Move horizontal
        movement.Set(horizontalMove * Time.fixedDeltaTime, 0);
        rigidBody.AddForce(movement);

        // Clamp velocity
        if (rigidBody.velocity.x > maxSpeed)
        {
            rigidBody.velocity = new Vector2(maxSpeed, rigidBody.velocity.y);
        }
        else if (rigidBody.velocity.x < -maxSpeed)
        {
            rigidBody.velocity = new Vector2(-maxSpeed, rigidBody.velocity.y);
        }

        //Flip sprite horizontal
        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 flippedScale = transform.localScale;
            flippedScale.x *= -1;
            transform.localScale = flippedScale;
        }

        // Jump, velocity check to ensure no double jump
        if (isGrounded && jump && rigidBody.velocity.y < 1f)
        {
            // Add jump force
            rigidBody.AddForce(Vector2.up * jumpForce);

            // Create jump particle system
            Instantiate(jumpParticleSystem, new Vector3(transform.position.x, particleY), Quaternion.identity);

            // Jump sound
            if (player.getThiccLevel() < Player.THIC_THRESHOLD) // Stic
            {
                audioSource.PlayOneShot(sticJumpSound);
            }
            else if (player.getThiccLevel() < Player.THICC_THRESHOLD) // Thic
            {
                audioSource.PlayOneShot(thicJumpSound);
            }
            else // Thicc+
            {
                audioSource.PlayOneShot(thiccJumpSound);
            }
        }

        // If player requested another jump before the player hits the ground, jump request is still reset
        jump = false;
    }

    public void updateMaxSpeed(float dMaxSpeed)
    {
        maxSpeed += dMaxSpeed;
    }

    public void updateJumpForce(float dJumpForce)
    {
        jumpForce += dJumpForce;
    }

    public bool playerIsGrounded()
    {
        return isGrounded;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
