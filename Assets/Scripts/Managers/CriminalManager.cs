using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CriminalManager : MonoBehaviour
{
    [SerializeField] private GameObject criminal;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Camera camera;

    private float horzExtentHalf;
    private float lastSpawnX;
    private float nextSpawnInterval;
    private const float MIN_SPAWN_INTERVAL = 10f;
    private const float MAX_SPAWN_INTERVAL = 30f;
    private const float SPAWN_Y = 3f;

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

            // Make sure not spawning criminal in pit
            if (tilemap.GetTile(new Vector3Int((int) spawnX, 0, 0)) != null)
            {
                Instantiate(criminal, new Vector3(spawnX, SPAWN_Y, 0f), Quaternion.identity);
                lastSpawnX = spawnX;
                nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
            }
        }
    }
}
