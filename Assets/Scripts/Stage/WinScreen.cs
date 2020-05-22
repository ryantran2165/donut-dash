using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winScreenAudio;
    [SerializeField] private GameObject fadeToWhite;

    // Start is called before the first frame update
    void Start()
    {
        // Separate because when deactivated, still want to hear it continue for the fade out to white
        winScreenAudio.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fadeToWhite.SetActive(true);
            Destroy(gameObject);
        }
    }
}
