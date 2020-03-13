using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Criminal : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private const float MIN_FORCE = 1000f;
    private const float MAX_FORCE = 1350f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rigidBody.AddForce(new Vector2(0f, Random.Range(MIN_FORCE, MAX_FORCE)));
        }
    }

}
