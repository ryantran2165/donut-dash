using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> repeatingObjects;
    [SerializeField] private List<ProbabilityObject> spawnableObjects;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;

    private float horzExtentHalf;
    private GameObject[] curRepeatingObjects;
    private SpriteRenderer[] curRepeatingRenderers;
    private SpriteRenderer[] repeatingRenderers;
    private const float REPEAT_OFFSET = -0.3f;

    private Rigidbody2D playerRigidBody;
    private List<Rigidbody2D> rigidBodies;
    private float lastSpawnX;
    private float nextSpawnInterval;
    private float lastLightX;
    private const float MIN_LIGHT_INTERVAL = 5f;

    private const float LAYER_1_SPEED = 0.7f;
    private const float LAYER_2_SPEED = 0.5f;
    private const float LAYER_3_SPEED = 0.25f;
    private const float LAYER_4_SPEED = 0.0f;
    private const float LAYER_5_SPEED = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;

        int repeating = repeatingObjects.Count;
        curRepeatingObjects = new GameObject[repeating / 2];
        curRepeatingRenderers = new SpriteRenderer[repeating / 2];
        repeatingRenderers = new SpriteRenderer[repeating];
        rigidBodies = new List<Rigidbody2D>();

        for (int i = 0; i < repeating; i++)
        {
            GameObject repeatingObject = repeatingObjects[i];
            repeatingRenderers[i] = repeatingObject.GetComponent<SpriteRenderer>();
            rigidBodies.Add(repeatingObject.GetComponent<Rigidbody2D>());

            // Each repeating object comes in pairs, initially every even index is the current one
            if (i % 2 == 0)
            {
                curRepeatingObjects[i / 2] = repeatingObject;
                curRepeatingRenderers[i / 2] = repeatingRenderers[i];
            }
        }

        playerRigidBody = player.GetComponent<Rigidbody2D>();
        
        lastSpawnX = Random.Range(0f, horzExtentHalf);
        nextSpawnInterval = horzExtentHalf * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        float cameraLeftEdge = camera.transform.position.x - horzExtentHalf;
        float cameraRightEdge = camera.transform.position.x + horzExtentHalf;

        // Spawnable objects
        if (cameraRightEdge - lastSpawnX > nextSpawnInterval)
        {
            GameObject toSpawnObject = ProbabilityObject.getRandom(spawnableObjects);

            // Make sure lights don't spawn too close to each other
            bool isLight = toSpawnObject.transform.childCount > 0 ? toSpawnObject.transform.GetChild(0).CompareTag("Light") : false;

            if (!isLight || cameraRightEdge - lastLightX > MIN_LIGHT_INTERVAL)
            {
                GameObject spawnedObject = Instantiate(toSpawnObject);
                float width = spawnedObject.GetComponent<SpriteRenderer>().bounds.size.x;
                float spawnX = cameraRightEdge + width / 2;
                spawnedObject.transform.position = new Vector3(spawnX, spawnedObject.transform.position.y);
                rigidBodies.Add(spawnedObject.GetComponent<Rigidbody2D>());
                lastSpawnX = spawnX;
                nextSpawnInterval = width / 2;

                if (isLight)
                {
                    lastLightX = spawnX;
                }
            }
        }

        // Repat repeating objects
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

        // Remove self-destructed spawned objects
        for (int i = rigidBodies.Count - 1; i >= 0; i--)
        {
            Rigidbody2D spawnedRigidBody = rigidBodies[i];

            if (spawnedRigidBody == null)
            {
                rigidBodies.RemoveAt(i);
            }
        }

        for (int i = 0; i < rigidBodies.Count; i++)
        {
            // Don't move backgrounds if game over, player moving backwards, or player is behind camera
            if (playerRigidBody == null || playerRigidBody.velocity.x < 0f || player.transform.position.x < camera.transform.position.x)
            {
                rigidBodies[i].velocity = Vector2.zero;
            }
            else
            {
                rigidBodies[i].velocity = new Vector2(playerRigidBody.velocity.x * getSpeed(rigidBodies[i].tag), 0f);
            }
        }
    }

    private float getSpeed(string tag)
    {
        switch (tag)
        {
            case "Layer 1":
                return LAYER_1_SPEED;
            case "Layer 2":
                return LAYER_2_SPEED;
            case "Layer 3":
                return LAYER_3_SPEED;
            case "Layer 4":
                return LAYER_4_SPEED;
            case "Layer 5":
                return LAYER_5_SPEED;
            default:
                return 0f;
        }
    }
}
