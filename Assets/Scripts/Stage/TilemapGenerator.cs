using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tile sidewalkTile;
    [SerializeField] private Tile brokenLeftTile;
    [SerializeField] private Tile brokenRightTile;
    [SerializeField] private Tile roadTile;
    [SerializeField] private Tile dirtTile;
    [SerializeField] private Tile spikeTile;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject spikeCollider;

    private Tilemap tilemap;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints;

    private float horzExtentHalf;
    private int nextX = 40;
    private int previousType = TYPE_NORMAL;
    private Vector2[] probabilities;
    private const float NORMAL_PROBABILITY = 0.7f;
    private const float PIT_PROBABILITY = 0.3f;

    private const int GENERATE_BUFFER = 10;
    private const int TYPE_NORMAL = 0;
    private const int TYPE_PIT = 1;
    private const float ROAD_Y = 0.9f;
    private const float SPIKE_Y = -1.7f;

    private const int MIN_NORMAL_LENGTH = 2;
    private const int MAX_NORMAL_LENGTH = 5;
    private const int MIN_PIT_LENGTH = 2;
    private const int MAX_PIT_LENGTH = 5;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeColliderPoints = new List<Vector2>(edgeCollider.points);
        edgeCollider.points = edgeColliderPoints.ToArray();

        float vertExtentHalf = camera.orthographicSize;
        horzExtentHalf = vertExtentHalf * Screen.width / Screen.height;

        probabilities = new Vector2[2];
        probabilities[0] = new Vector2(TYPE_NORMAL, NORMAL_PROBABILITY);
        probabilities[1] = new Vector2(TYPE_PIT, PIT_PROBABILITY);
    }

    // Update is called once per frame
    void Update()
    {
        float rightEdge = camera.transform.position.x + horzExtentHalf;

        // Need to generate more
        if (nextX < rightEdge + GENERATE_BUFFER)
        {
            int randType = getRandomType();
            
            if (randType == TYPE_NORMAL)
            {
                addNormal();
            }
            else if (randType == TYPE_PIT && previousType != TYPE_PIT)
            {
                addPit();
            }

            edgeCollider.points = edgeColliderPoints.ToArray();
        }
    }

    private int getRandomType()
    {
        int index = 0;
        float rand = Random.value;

        while (rand > 0)
        {
            rand -= probabilities[index].y;
            index++;
        }

        index--;
        return (int) probabilities[index].x;
    }

    private void addNormal()
    {
        int length = Random.Range(MIN_NORMAL_LENGTH, MAX_NORMAL_LENGTH + 1);

        // 3-high normal, road on top followed by 2 dirts
        for (int i = 0; i < length; i++)
        {
            tilemap.SetTile(new Vector3Int(nextX + i, 1, 0), sidewalkTile);
            tilemap.SetTile(new Vector3Int(nextX + i, 0, 0), roadTile);
            tilemap.SetTile(new Vector3Int(nextX + i, -1, 0), dirtTile);
            tilemap.SetTile(new Vector3Int(nextX + i, -2, 0), dirtTile);
        }

        // Break sidewalk if previous was pit
        if (previousType == TYPE_PIT)
        {
            tilemap.SetTile(new Vector3Int(nextX, 0, 0), brokenRightTile);
        }

        // Modify end point
        edgeColliderPoints[edgeColliderPoints.Count - 1] = new Vector2(nextX + length, ROAD_Y);

        // Advance next spawn x by length
        nextX += length;

        previousType = TYPE_NORMAL;
    }

    private void addPit()
    {
        int length = Random.Range(MIN_PIT_LENGTH, MAX_PIT_LENGTH + 1);

        // Set spike pit tiles
        tilemap.SetTile(new Vector3Int(nextX - 1, 0, 0), brokenLeftTile);
        for (int i = 0; i < length; i++)
        {
            tilemap.SetTile(new Vector3Int(nextX + i, -2, 0), spikeTile);
        }

        // Add 4 new points before the last point
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX, ROAD_Y));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX, SPIKE_Y));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX + length, SPIKE_Y));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX + length, ROAD_Y));

        // Add collision
        BoxCollider2D boxCollider = spikeCollider.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(length, 1f);
        Instantiate(spikeCollider, new Vector3(nextX + length / 2f, -1f), Quaternion.identity);

        // Advance next spawn x by pit length
        nextX += length;

        previousType = TYPE_PIT;
    }

}
