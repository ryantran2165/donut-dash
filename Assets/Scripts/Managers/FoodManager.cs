using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> foods;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera camera;

    private float horzExtentHalf;
    private float lastSpawnX;
    private float nextSpawnInterval;
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
    }

    // Update is called once per frame
    void Update()
    {
        float rightEdge = camera.transform.position.x + horzExtentHalf;

        if (rightEdge - lastSpawnX > nextSpawnInterval)
        {
            // +1 just so player can't see the spawning
            float spawnX = rightEdge + 1f;
            bool isOverPit = tilemap.GetTile(new Vector3Int((int) spawnX, 0, 0)) == null;
            float spawnY = Random.Range(isOverPit ? MIN_FOOD_Y_PIT : MIN_FOOD_Y_GROUND, MAX_FOOD_Y);
            GameObject randomFood = foods[Random.Range(0, foods.Count)];
            Instantiate(randomFood, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
            lastSpawnX = spawnX;
            nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
        }
    }
}
