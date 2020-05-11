using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Start boss fight
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Activate all objects that need to be activated
            foreach (GameObject toActivateObject in toActivate)
            {
                toActivateObject.SetActive(true);
            }

            // Destroy this UI object
            Destroy(gameObject);
        }
    }
}
