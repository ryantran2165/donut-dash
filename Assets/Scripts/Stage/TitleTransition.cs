using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TitleTransition : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDestroy;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject titleLoop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy title once video is ready
        if (videoPlayer.isPrepared && titleLoop != null)
        {
            Destroy(titleLoop);
        }

        if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
        {
            // Activate all the deactivated objects
            foreach (GameObject toActivateObject in toActivate)
            {
                toActivateObject.SetActive(true);
            }

            // Destroy objects
            foreach (GameObject destroyObject in toDestroy)
            {
                Destroy(destroyObject);
            }

            // Destroy this title object
            Destroy(gameObject);
        }
    }
}
