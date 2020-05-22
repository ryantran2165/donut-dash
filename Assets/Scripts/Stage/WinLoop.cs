using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinLoop : MonoBehaviour
{
    [SerializeField] private GameObject winLoopAudio;
    [SerializeField] private Image white;

    private bool fadeToWhiteActivated;
    private float fadeTimer;
    private const float FADE_TIME = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Separate because when deactivated, still want to hear it continue for the fade out to white
        winLoopAudio.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Activate fade to white
        if (!fadeToWhiteActivated && Input.GetKeyDown(KeyCode.Space))
        {
            fadeToWhiteActivated = true;
        }

        if (fadeToWhiteActivated)
        {
            // Increase fade timer
            fadeTimer += Time.deltaTime;

            // Clamp alpha to max of 1f
            float alpha = Mathf.Min(fadeTimer / FADE_TIME, 1f);
            white.color = new Color(1f, 1f, 1f, alpha);

            // Done fading
            if (alpha == 1f)
            {
                // Activate object after boss fight
                MyGameManager.activateObjectsAfterWin = true;

                // Go back to DonutDash scene
                SceneManager.UnloadSceneAsync("BossFight");
                DonutDashSingleton.setActive(true);
            }
        }
    }
}
