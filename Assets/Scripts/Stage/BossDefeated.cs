using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeated : MonoBehaviour
{
    [SerializeField] private List<GameObject> toDeActivate;
    [SerializeField] private GameObject winScreen;

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
        
    }

    public void onFinish()
    {
        winScreen.SetActive(true);
        Destroy(gameObject);
    }
}
