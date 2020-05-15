using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criminal : MonoBehaviour
{
    [SerializeField] private ParticleSystem jumpParticleSystem;

    private Rigidbody2D rigidBody;
    private Vector2 jumpForce;
    private const float MIN_FORCE = 1100f;
    private const float MAX_FORCE = 1300f;

    private float particleY;
    private const float PARTICLE_Y_OFFSET = -0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        jumpForce = new Vector2(0f, Random.Range(MIN_FORCE, MAX_FORCE));

        // Get jump particle system spawn y position
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        particleY = renderer.bounds.min.y + PARTICLE_Y_OFFSET;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Add force to jump again
            rigidBody.AddForce(jumpForce);

            // Create jump particle system
            Instantiate(jumpParticleSystem, new Vector3(transform.position.x, particleY), Quaternion.identity);
        }
    }

}
