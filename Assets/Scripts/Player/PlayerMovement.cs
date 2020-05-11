using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1000f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AudioClip sticWalkSound;
    [SerializeField] private AudioClip thicWalkSound;
    [SerializeField] private AudioClip thiccWalkSound;

    private AudioSource audioSource;
    private Player player;
    private Rigidbody2D rigidBody;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool jump;
    private float horizontalMove;
    private Vector2 movement;
    private float horzExtentHalf;
    private const float LEFT_BOUNDARY_OFFSET = 0.5f;

    private const float STIC_VOLUME = 0.5f;
    private const float THIC_VOLUME = 0.5f;
    private const float THICC_VOLUME = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        rigidBody = GetComponent<Rigidbody2D>();
        movement = new Vector2();
        float vertExtentHalf = camera.orthographicSize;
        horzExtentHalf = vertExtentHalf * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        // Poll input
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        // Cast circle to check if grounded
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundTransform.position, .5f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
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

        float leftEdge = camera.transform.position.x - horzExtentHalf;
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

        // Jump, velocity < 1f to make sure no double jump
        if (isGrounded && jump && rigidBody.velocity.y < 1f)
        {
            isGrounded = false;
            rigidBody.AddForce(Vector2.up * jumpForce);
        }
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

}
