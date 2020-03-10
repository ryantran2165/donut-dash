using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int thiccLevel;
    private Rigidbody2D rigidBody;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addThicc(int thicc)
    {
        // Increase/decrease thicc level depending on food item
        thiccLevel += thicc;

        // Update mass
        rigidBody.mass += thicc / 10000f;

        // Update max speed
        playerMovement.changeMaxSpeed(-thicc / 5000f);
    }

}
