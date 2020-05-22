using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BossOutro : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private GameObject winLoop;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject black;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in toDeActivate)
        {
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Video done
        if (videoPlayer.isPrepared && !videoPlayer.isPlaying)
        {
            black.SetActive(true);
            winLoop.SetActive(true);
            Destroy(gameObject);
        }
    }
}
