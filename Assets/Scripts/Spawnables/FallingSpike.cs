using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] private ParticleSystem destroyParticleSystem;

    private const float MIN_GRAVITY = 0.75f;
    private const float MAX_GRAVITY = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = Random.Range(MIN_GRAVITY, MAX_GRAVITY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            Instantiate(destroyParticleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
