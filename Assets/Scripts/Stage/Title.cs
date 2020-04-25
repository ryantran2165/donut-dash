using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private RuntimeAnimatorController transitionAnimation;
    [SerializeField] private MyGameManager gameManager;

    private Animator animator;
    private bool inTransition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // Get default sprite sizes
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float width = renderer.bounds.size.x;
        float height = renderer.bounds.size.y;

        // Get camera sizes
        float camHeight = Camera.main.orthographicSize * 2f;
        float camWidth = camHeight / Screen.height * Screen.width;

        // Set title position to center of screen (same as camera)
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);

        // Stretch title sprite to screen
        transform.localScale = new Vector3(camWidth / width, camHeight / height, 1f);
    }

    // Update is called once per frame
    void Update()
    {
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

                // Change to transition animation
                animator.runtimeAnimatorController = transitionAnimation;
                inTransition = true;
            }
            else if (Input.GetKeyDown(KeyCode.C)) // Change to credits scene
            {
                SceneManager.LoadScene("CreditScene");
            }
        }
    }

    // Called by Animation Event attached to Title Transition Animation
    public void onFinishTransition()
    {
        // Activate all the deactivated objects
        foreach (GameObject toActivateObject in toActivate)
        {
            toActivateObject.SetActive(true);
        }

        // Destroy this title object
        Destroy(gameObject);
    }
}
