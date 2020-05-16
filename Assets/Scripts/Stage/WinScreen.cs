using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fadeToWhiteRenderer;

    private bool activateFadeToWhite;
    private float fadeTimer;

    private const float FADE_TIME = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (activateFadeToWhite)
        {
            // Increase fade timer
            fadeTimer += Time.deltaTime;

            // Clamp alpha to max of 1f
            float alpha = Mathf.Min(fadeTimer / FADE_TIME, 1f);
            fadeToWhiteRenderer.color = new Color(1f, 1f, 1f, alpha);

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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            activateFadeToWhite = true;
        }
    }
}
