using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutDashSingleton : MonoBehaviour
{
    public static DonutDashSingleton instance = null;

    void Awake()
    {
        // Instance does not exist
        if (instance == null)
        {
            // Set the singleton instance
            instance = this;
        }
        else if (instance != this)
        {
            // New singleton attempt, destroy it
            Destroy(gameObject);
        }
    }

    public static void setActive(bool active)
    {
        instance.gameObject.SetActive(active);
    }
}
