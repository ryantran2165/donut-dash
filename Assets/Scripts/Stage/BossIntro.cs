using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class BossIntro : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate objects
        foreach (GameObject toDeActivateObject in toDeActivate)
        {
            toDeActivateObject.SetActive(false);
        }

        // Destroy obstacles so when player comes back, not instantly killed
        GameObject[] toDestroyObjects = GameObject.FindGameObjectsWithTag("DestroyOnBossIntro");
        foreach (GameObject toDestroy in toDestroyObjects)
        {
            Destroy(toDestroy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Video done
        if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
        {
            // Set to boss fight
            DonutDashSingleton.setActive(false);
            SceneManager.LoadScene("BossFight", LoadSceneMode.Additive);

            // Destroy this object
            Destroy(gameObject);
        }
    }
}
