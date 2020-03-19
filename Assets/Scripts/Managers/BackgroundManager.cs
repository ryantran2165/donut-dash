using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject background1;
    [SerializeField] private GameObject background2;
    [SerializeField] private List<GameObject> closeBackgroundObjects;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject player;

    private float horzExtentHalf;
    private GameObject curBackground;
    private SpriteRenderer curRenderer;
    private SpriteRenderer renderer1;
    private SpriteRenderer renderer2;
    private float backgroundWidth;
    private const float BACKGROUND_OFFSET = -0.3f;

    private Rigidbody2D playerRigidBody;
    private Rigidbody2D background1RigidBody;
    private Rigidbody2D background2RigidBody;
    private List<GameObject> spawnedObjects;
    private float lastSpawnX;
    private float nextSpawnInterval;
    private const float SPAWN_X_OFFSET = 3f;
    private const float MIN_SPAWN_INTERVAL = 6f;
    private const float MAX_SPAWN_INTERVAL = 10f;
    private const float BACKGROUND_SPEED = 0.8f;
    private const float CLOSE_BACKGROUND_SPEED = 0.5f;
    private const float MIN_SPAWN_Y = 3.5f;
    private const float MAX_SPAWN_Y = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;

        renderer1 = background1.GetComponent<SpriteRenderer>();
        renderer2 = background2.GetComponent<SpriteRenderer>();

        curBackground = background1;
        curRenderer = renderer1;
        backgroundWidth = curRenderer.bounds.size.x;

        playerRigidBody = player.GetComponent<Rigidbody2D>();
        background1RigidBody = background1.GetComponent<Rigidbody2D>();
        background2RigidBody = background2.GetComponent<Rigidbody2D>();
        spawnedObjects = new List<GameObject>();
        lastSpawnX = Random.Range(0f, horzExtentHalf);
        nextSpawnInterval = horzExtentHalf * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        float curBackgroundRightEdge = curRenderer.bounds.max.x;
        float cameraLeftEdge = camera.transform.position.x - horzExtentHalf;
        float cameraRightEdge = camera.transform.position.x + horzExtentHalf;

        // Spawn close background objects
        if (cameraRightEdge - lastSpawnX > nextSpawnInterval)
        {
            GameObject closeBackgroundObject = closeBackgroundObjects[Random.Range(0, closeBackgroundObjects.Count)];
            float spawnX = cameraRightEdge + SPAWN_X_OFFSET;
            float spawnY = Random.Range(MIN_SPAWN_Y, MAX_SPAWN_Y);
            GameObject spawnedObject = Instantiate(closeBackgroundObject, new Vector3(spawnX, spawnY), Quaternion.identity);
            spawnedObjects.Add(spawnedObject);
            lastSpawnX = spawnX;
            nextSpawnInterval = Random.Range(MIN_SPAWN_INTERVAL, MAX_SPAWN_INTERVAL);
        }

        // Current background passed out of view
        if (curBackgroundRightEdge < cameraLeftEdge)
        {
            // Move current background to end of other background
            Vector3 curPos = curBackground.transform.position;
            curBackground.transform.position = new Vector3(curPos.x + backgroundWidth * 2 + BACKGROUND_OFFSET, curPos.y);

            // Swap current backgrounds and renderers
            curBackground = curBackground == background1 ? background2 : background1;
            curRenderer = curRenderer == renderer1 ? renderer2 : renderer1;
        }

        // Remove self-destructed spawned objects
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            GameObject spawnedObject = spawnedObjects[i];

            if (spawnedObject == null)
            {
                spawnedObjects.RemoveAt(i);
            }
        }

        // Don't move backgrounds if game over, player moving backwards, or player is behind camera
        if (playerRigidBody == null || playerRigidBody.velocity.x < 0f || player.transform.position.x < camera.transform.position.x)
        {
            background1RigidBody.velocity = Vector2.zero;
            background2RigidBody.velocity = Vector2.zero;

            // Close background
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                GameObject spawnedObject = spawnedObjects[i];
                Rigidbody2D spawnedRigidBody = spawnedObject.GetComponent<Rigidbody2D>();
                spawnedRigidBody.velocity = Vector2.zero;
            }
        } else
        {
            // Move the backgrounds with the player at a percentage of the player's speed for parallax effect
            background1RigidBody.velocity = new Vector2(playerRigidBody.velocity.x * BACKGROUND_SPEED, 0f);
            background2RigidBody.velocity = new Vector2(playerRigidBody.velocity.x * BACKGROUND_SPEED, 0f);

            // Close background
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                GameObject spawnedObject = spawnedObjects[i];
                Rigidbody2D spawnedRigidBody = spawnedObject.GetComponent<Rigidbody2D>();
                spawnedRigidBody.velocity = new Vector2(playerRigidBody.velocity.x * CLOSE_BACKGROUND_SPEED, 0f);
            }
        }
    }
}
