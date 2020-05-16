using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int thiccValue;
    [SerializeField] private int scoreValue;
    [SerializeField] private ParticleSystem eatParticleSystem;
    [SerializeField] private GameObject consumeSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.addThicc(thiccValue);
            player.addFoodScore(scoreValue);
            Instantiate(eatParticleSystem, transform.position, Quaternion.identity);
            Instantiate(consumeSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
