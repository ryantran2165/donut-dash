using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Title : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private MyGameManager gameManager;
    [SerializeField] private GameObject titleTransition;
    [SerializeField] private GameObject prescreen;

    private bool inTransition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Title isn't destroyed immediately, so transition video has time to prepare
        if (!inTransition)
        {
            // Change title animation to the transition
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set flag for ingame
                gameManager.setIngame(true);

                // Deactivate
                foreach (GameObject toDeActivateObject in toDeActivate)
                {
                    toDeActivateObject.SetActive(false);
                }

                // Destroy prescreen
                Destroy(prescreen);

                // Activate transition
                titleTransition.SetActive(true);

                // Flag transition
                inTransition = true;
            }
            else if (Input.GetKeyDown(KeyCode.C)) // Change to credits scene
            {
                DonutDashSingleton.setActive(false);
                SceneManager.LoadScene("CreditScene", LoadSceneMode.Additive);
            }
        }
        
    }
}
