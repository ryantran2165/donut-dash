﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private GameObject donutShop;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Camera camera;
    [SerializeField] private List<GameObject> toActivateAfterBossFight;

    private bool ingame;
    public static bool skipCutscene;
    public static bool activateObjectsAfterWin;

    // Start is called before the first frame update
    void Start()
    {
        // Player pressed 'R' to replay
        if (skipCutscene)
        {
            skipCutscene = false;

            // Set ingame
            ingame = true;

            // Set donut shop position
            //SpriteRenderer renderer = donutShop.GetComponent<SpriteRenderer>();
            //donutShop.transform.position = new Vector3(ScreenUtility.getXRightOnscreen(renderer, camera), donutShop.transform.position.y);

            // Activate
            foreach (GameObject toActivate in toActivate)
            {
                toActivate.SetActive(true);
            }

            // Deactivate
            foreach (GameObject toDeActivate in toDeActivate)
            {
                toDeActivate.SetActive(false);
            }

            // Enable player movement
            playerMovement.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ingame) // Pressed escape from ingame, back to main menu
            {
                SceneManager.LoadScene("DonutDash");
            }
            else // Pressed escape from main menu, exit game
            {
                Application.Quit();
            }
        }

        // Activate bird manager after boss fight win
        if (activateObjectsAfterWin)
        {
            activateObjectsAfterWin = false;

            foreach (GameObject obj in toActivateAfterBossFight)
            {
                obj.SetActive(true);
            }
        }
    }

    public void setIngame(bool ingame)
    {
        this.ingame = ingame;
    }

    public static int updateHighScore(int score)
    {
        int highscore = PlayerPrefs.HasKey("highscore") ? Mathf.Max(PlayerPrefs.GetInt("highscore"), score) : score;
        PlayerPrefs.SetInt("highscore", highscore);
        PlayerPrefs.Save();
        return highscore;
    }
}
