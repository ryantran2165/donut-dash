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
    private Tilemap tilemap;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> edgeColliderPoints;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeColliderPoints = new List<Vector2>(edgeCollider.points);
        addPit(8);
        edgeCollider.points = edgeColliderPoints.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }

    private void addPit(int x)
    {
        tilemap.SetTile(new Vector3Int(x, -2, 0), spikeTile);
        tilemap.SetTile(new Vector3Int(x + 1, -2, 0), spikeTile);

        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(x, 0.9f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(x, -1.7f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(x + 2, -1.7f));
        edgeColliderPoints.Insert(edgeColliderPoints.Count - 1, new Vector2(x + 2, 0.9f));
    }
}
