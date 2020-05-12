using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossFightUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private BossFightPlayer playerScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScript.playerIsDead())
        {
            // Start boss fight
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Activate all objects that need to be activated
                foreach (GameObject toActivateObject in toActivate)
                {
                    toActivateObject.SetActive(true);
                }

                // Deactivate UI
                gameObject.SetActive(false);
            }
        }
        else
        {
            // Replay
            if (Input.GetKeyDown(KeyCode.R))
            {
                MyGameManager.skipCutscene = true;
                SceneManager.LoadScene("DonutDash");
            }
        }
        
    }
}
