using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    [SerializeField] private Tile roadTile;
    [SerializeField] private Tile dirtTile;
    [SerializeField] private Tile spikeTile;
    [SerializeField] private Player player;
    [SerializeField] private Camera camera;

    private Tilemap tilemap;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints;

    private float horzExtentHalf;
    private const int GENERATE_BUFFER = 5;
    private int nextX = 40;
    private int previousType = TYPE_NORMAL;
    private const int TYPE_NORMAL = 0;
    private const int TYPE_PIT = 1;
    private Vector2[] probabilities;

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
        probabilities[0] = new Vector2(TYPE_NORMAL, .9f);
        probabilities[1] = new Vector2(TYPE_PIT, .1f);
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
        // 3-high normal, road on top followed by 2 dirts
        tilemap.SetTile(new Vector3Int(nextX, 0, 0), roadTile);
        tilemap.SetTile(new Vector3Int(nextX, -1, 0), dirtTile);
        tilemap.SetTile(new Vector3Int(nextX, -2, 0), dirtTile);

        // Modify end point
        edgeColliderPoints[edgeColliderPoints.Count - 1] = new Vector2(nextX + 1, 0.9f);

        // Advance next spawn x by 1
        nextX++;

        previousType = TYPE_NORMAL;
    }

    private void addPit()
    {
        // 2-wide spike pit
        tilemap.SetTile(new Vector3Int(nextX, -2, 0), spikeTile);
        tilemap.SetTile(new Vector3Int(nextX + 1, -2, 0), spikeTile);

        // Add 4 new points before the last point
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX, 0.9f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX, -1.7f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX + 2, -1.7f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(nextX + 2, 0.9f));

        // Advance next spawn x by 2
        nextX += 2;

        previousType = TYPE_PIT;
    }

}
