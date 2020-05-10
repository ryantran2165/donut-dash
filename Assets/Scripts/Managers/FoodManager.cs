using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private List<ProbabilityObject> foodObjects;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Transform parent;

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
        lastSpawnX = Random.Range(0f, ScreenUtility.HORZ_EXT_HALF);
        nextSpawnInterval = ScreenUtility.HORZ_EXT_HALF * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        //float rightEdge = camera.transform.position.x + horzExtentHalf;
        float rightEdge = ScreenUtility.getRightEdge();

        if (rightEdge - lastSpawnX > nextSpawnInterval)
        {
            GameObject randomFood = ProbabilityObject.getRandom(foodObjects);
            SpriteRenderer renderer = randomFood.GetComponent<SpriteRenderer>();
            float spawnX = ScreenUtility.getXRightOffscreen(renderer);
            bool isOverPit = tilemap.GetTile(new Vector3Int((int) spawnX, 0, 0)) == null;
            float spawnY = Random.Range(isOverPit ? MIN_FOOD_Y_PIT : MIN_FOOD_Y_GROUND, MAX_FOOD_Y);
            Instantiate(randomFood, new Vector3(spawnX, spawnY), Quaternion.identity, parent);
            lastSpawnX = spawnX;
            nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
        }
    }
}
