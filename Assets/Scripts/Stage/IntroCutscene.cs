using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Video done
        if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
        {
            // Activate all the deactivated objects
            foreach (GameObject toActivateObject in toActivate)
            {
                toActivateObject.SetActive(true);
            }

            // Activate player movement
            playerMovement.enabled = true;

            // Destroy this object
            Destroy(gameObject);
        }
    }
}
