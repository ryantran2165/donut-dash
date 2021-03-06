﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject collisionSound;

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
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            Instantiate(collisionSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
