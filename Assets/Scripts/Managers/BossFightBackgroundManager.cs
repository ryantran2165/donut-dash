using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossFightBackgroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> repeatingObjects;
    [SerializeField] private List<int> speeds;
    [SerializeField] private GameObject tilemapObject1;
    [SerializeField] private GameObject tilemapObject2;

    private GameObject[] curRepeatingObjects;
    private SpriteRenderer[] curRepeatingRenderers;
    private SpriteRenderer[] repeatingRenderers;

    private GameObject curTilemapObject;
    private TilemapRenderer curTilemapRenderer;
    private TilemapRenderer tilemapRenderer1;
    private TilemapRenderer tilemapRenderer2;

    private const float REPEAT_OFFSET = -0.3f;
    private float cameraLeftEdge;

    // Start is called before the first frame update
    void Start()
    {
        cameraLeftEdge = ScreenUtility.getLeftEdge();

        int repeating = repeatingObjects.Count;
        curRepeatingObjects = new GameObject[repeating / 2];
        curRepeatingRenderers = new SpriteRenderer[repeating / 2];
        repeatingRenderers = new SpriteRenderer[repeating];

        // Setup for repeating objects
        for (int i = 0; i < repeating; i++)
        {
            GameObject repeatingObject = repeatingObjects[i];
            repeatingRenderers[i] = repeatingObject.GetComponent<SpriteRenderer>();

            // Each repeating object comes in pairs, initially every even index is the current one
            if (i % 2 == 0)
            {
                curRepeatingObjects[i / 2] = repeatingObject;
                curRepeatingRenderers[i / 2] = repeatingRenderers[i];
            }

            // Give them velocity
            Rigidbody2D rigidbody = repeatingObject.GetComponent<Rigidbody2D>();
            rigidbody.velocity = new Vector2(speeds[i / 2], 0f);
        }

        // Setup for tilemap
        tilemapRenderer1 = tilemapObject1.GetComponent<TilemapRenderer>();
        tilemapRenderer2 = tilemapObject2.GetComponent<TilemapRenderer>();

        curTilemapObject = tilemapObject1;
        curTilemapRenderer = tilemapRenderer1;

        Rigidbody2D rigidbody1 = tilemapObject1.GetComponent<Rigidbody2D>();
        rigidbody1.velocity = new Vector2(speeds[0], 0f);

        Rigidbody2D rigidbody2 = tilemapObject2.GetComponent<Rigidbody2D>();
        rigidbody2.velocity = new Vector2(speeds[0], 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Repeat repeating objects
        for (int i = 0; i < curRepeatingObjects.Length; i++)
        {
            GameObject curRepeatingObject = curRepeatingObjects[i];
            SpriteRenderer curRepeatingRenderer = curRepeatingRenderers[i];

            // Current repeating object passed out of view
            if (curRepeatingRenderer.bounds.max.x < cameraLeftEdge)
            {
                // Move current repeating object to end of other repeating object
                Vector3 curPos = curRepeatingObject.transform.position;
                curRepeatingObject.transform.position = new Vector3(curPos.x + curRepeatingRenderer.bounds.size.x * 2 + REPEAT_OFFSET, curPos.y);

                // Swap current object and renderer
                curRepeatingObjects[i] = curRepeatingObject == repeatingObjects[i * 2] ? repeatingObjects[i * 2 + 1] : repeatingObjects[i * 2];
                curRepeatingRenderers[i] = curRepeatingRenderer == repeatingRenderers[i * 2] ? repeatingRenderers[i * 2 + 1] : repeatingRenderers[i * 2];
            }
        }

        // Tilemap
        if (curTilemapRenderer.bounds.max.x < cameraLeftEdge)
        {
            // Move current repeating object to end of other repeating object
            Vector3 curPos = curTilemapRenderer.transform.position;
            curTilemapObject.transform.position = new Vector3(curPos.x + curTilemapRenderer.bounds.size.x * 2, curPos.y);

            // Swap current object and renderer
            curTilemapObject = curTilemapObject == tilemapObject1 ? tilemapObject2 : tilemapObject1;
            curTilemapRenderer = curTilemapRenderer == tilemapRenderer1 ? tilemapRenderer2 : tilemapRenderer1;
        }
    }
}
