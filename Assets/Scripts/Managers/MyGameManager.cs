using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> toActivate;
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private GameObject donutShop;
    [SerializeField] private PlayerMovement playerMovement;

    private bool ingame;
    public static bool skipCutscene;

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
            SpriteRenderer renderer = donutShop.GetComponent<SpriteRenderer>();
            donutShop.transform.position = new Vector3(ScreenUtility.getXRightOnscreen(renderer), donutShop.transform.position.y);

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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else // Pressed escape from main menu, exit game
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            DonutDashSingleton.setActive(false);
            SceneManager.LoadScene("BossFight", LoadSceneMode.Additive);
        }
    }

    public void setIngame(bool ingame)
    {
        this.ingame = ingame;
    }

}
