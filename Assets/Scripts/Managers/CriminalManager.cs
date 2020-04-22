using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CriminalManager : MonoBehaviour
{
    [SerializeField] private GameObject criminal;
    [SerializeField] private Tilemap tilemap;

    private float lastSpawnX;
    private float nextSpawnInterval;
    private float criminalWidth;
    private const float MIN_SPAWN_INTERVAL = 10f;
    private const float MAX_SPAWN_INTERVAL = 30f;
    private const float SPAWN_Y = 3f;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnX = Random.Range(0f, ScreenUtility.HORZ_EXT_HALF);
        nextSpawnInterval = ScreenUtility.HORZ_EXT_HALF * 2f;
        criminalWidth = criminal.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float rightEdge = ScreenUtility.getRightEdge();

        if (rightEdge - lastSpawnX > nextSpawnInterval)
        {
            float spawnX = rightEdge + criminalWidth;

            // Make sure not spawning criminal in pit
            if (tilemap.GetTile(new Vector3Int((int) spawnX, 0, 0)) != null)
            {
                Instantiate(criminal, new Vector3(spawnX, SPAWN_Y), Quaternion.identity);
                lastSpawnX = spawnX;
                nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
            }
        }
    }
}
