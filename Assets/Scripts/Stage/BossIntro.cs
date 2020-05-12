using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossIntro : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDeActivate;

    // Start is called before the first frame update
    void Start()
    {
        // Deactivate objects
        foreach (GameObject toDeActivateObject in toDeActivate)
        {
            toDeActivateObject.SetActive(false);
        }

        // Destroy all falling obstacles (anvils and flaming donuts)
        GameObject[] fallingObstacles = GameObject.FindGameObjectsWithTag("FallingObstacle");
        foreach (GameObject fallingObstacle in fallingObstacles)
        {
            Destroy(fallingObstacle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onFinishTransition()
    {
        // Set to boss fight
        DonutDashSingleton.setActive(false);
        SceneManager.LoadScene("BossFight", LoadSceneMode.Additive);

        // Destroy this object
        Destroy(gameObject);
    }
}
