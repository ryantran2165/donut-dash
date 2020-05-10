using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private static bool destroyed = false;
    private static object lockObject = new object();
    private static MySceneManager instance;
    private Stack<int> loadedLevels;

    public static MySceneManager Instance
    {
        get
        {
            if (destroyed)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(MySceneManager) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (lockObject)
            {
                if (instance == null)
                {
                    // Search for existing instance
                    instance = (MySceneManager) FindObjectOfType(typeof(MySceneManager));

                    // Create new instance if one doesn't already exist
                    if (instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<MySceneManager>();
                        singletonObject.name = typeof(MySceneManager).ToString() + " (Singleton)";

                        // Make instance persistent
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return instance;
            }
        }
    }

    public static Scene GetActiveScene()
    {
        return SceneManager.GetActiveScene();
    }

    public static void LoadScene(int buildIndex)
    {
        Instance.loadedLevels.Push(GetActiveScene().buildIndex);
        SceneManager.LoadScene(buildIndex);
    }

    public static void LoadScene(string sceneName)
    {
        Instance.loadedLevels.Push(GetActiveScene().buildIndex);
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadPreviousScene()
    {
        if (Instance.loadedLevels.Count > 0)
        {
            SceneManager.LoadScene(Instance.loadedLevels.Pop());
        }
        else
        {
            Debug.LogError("No previous scene loaded");
        }
    }

    private void Awake()
    {
        loadedLevels = new Stack<int>();
    }

    private void OnApplicationQuit()
    {
        destroyed = true;
    }

    private void OnDestroy()
    {
        destroyed = true;
    }
}
