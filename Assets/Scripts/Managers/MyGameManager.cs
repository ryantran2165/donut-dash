using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour
{
    private bool ingame;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    public void setIngame(bool ingame)
    {
        this.ingame = ingame;
    }
}
