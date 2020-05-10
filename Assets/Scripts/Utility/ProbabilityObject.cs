using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProbabilityObject
{
    [SerializeField] private GameObject gameObject;
    [SerializeField] private float probability;

    public ProbabilityObject(GameObject gameObject, float probability)
    {
        this.gameObject = gameObject;
        this.probability = probability;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public float getProbability()
    {
        return probability;
    }

    public static GameObject getRandom(List<ProbabilityObject> probObjects)
    {
        int index = 0;
        float rand = Random.value;

        while (rand > 0)
        {
            // Out of objects, just return the last object
            if (index >= probObjects.Count)
            {
                break;
            }

            rand -= probObjects[index].probability;
            index++;
        }

        index--;
        return probObjects[index].gameObject;
    }


}
