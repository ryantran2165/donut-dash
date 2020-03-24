using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private GameObject donut;
    [SerializeField] private GameObject coffee;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera camera;

    private GameObject[] foods;

    private float[] probabilities;
    private const float DONUT_PROBABILITY = 0.8f;
    private const float COFFEE_PROBABILITY = 0.2f;

    private float horzExtentHalf;
    private float lastSpawnX;
    private float nextSpawnInterval;
    private float foodWidth;
    private const float MIN_SPAWN_INTERVAL = 5f;
    private const float MAX_SPAWN_INTERVAL = 10f;
    private const float MIN_FOOD_Y_GROUND = 2.5f;
    private const float MIN_FOOD_Y_PIT = 4.5f;
    private const float MAX_FOOD_Y = 6f;

    // Start is called before the first frame update
    void Start()
    {
        float vertExtentHalf = camera.orthographicSize;
        horzExtentHalf = vertExtentHalf * Screen.width / Screen.height;
        lastSpawnX = Random.Range(0f, horzExtentHalf);
        nextSpawnInterval = horzExtentHalf * 2f;

        foods = new GameObject[2];
        foods[0] = donut;
        foods[1] = coffee;

        probabilities = new float[2];
        probabilities[0] = DONUT_PROBABILITY;
        probabilities[1] = COFFEE_PROBABILITY;

        foodWidth = Mathf.Max(donut.GetComponent<SpriteRenderer>().bounds.size.x, coffee.GetComponent<SpriteRenderer>().bounds.size.x);
    }

    // Update is called once per frame
    void Update()
    {
        float rightEdge = camera.transform.position.x + horzExtentHalf;

        if (rightEdge - lastSpawnX > nextSpawnInterval)
        {
            // +1 just so player can't see the spawning
            float spawnX = rightEdge + foodWidth;
            bool isOverPit = tilemap.GetTile(new Vector3Int((int) spawnX, 0, 0)) == null;
            float spawnY = Random.Range(isOverPit ? MIN_FOOD_Y_PIT : MIN_FOOD_Y_GROUND, MAX_FOOD_Y);
            Instantiate(getRandomFood(), new Vector3(spawnX, spawnY), Quaternion.identity);
            lastSpawnX = spawnX;
            nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
        }
    }

    private GameObject getRandomFood()
    {
        int index = 0;
        float rand = Random.value;

        while (rand > 0)
        {
            rand -= probabilities[index];
            index++;
        }

        index--;
        return foods[index];
    }
}
