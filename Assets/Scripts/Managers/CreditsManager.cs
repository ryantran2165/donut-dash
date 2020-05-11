using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> randomObjects;
    [SerializeField] private Camera camera;

    private float timer;

    private const float MIN_TIME = 0.25f;
    private const float MAX_TIME = 1f;
    private const float MIN_VEL = 5f;
    private const float MAX_VEL = 10f;
    private const float CENTER_RADIUS = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Return to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("DonutDash");
        }

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            // Set new spawn time
            timer = Random.Range(MIN_TIME, MAX_TIME);

            // Get random object to spawn
            GameObject randomObject = randomObjects[Random.Range(0, randomObjects.Count)];

            // Get spawn position
            SpriteRenderer renderer = randomObject.GetComponent<SpriteRenderer>();
            float x, y;
            if (Random.Range(0 ,2) == 0) // Spawn left/right
            {
                x = Random.Range(0, 2) == 0 ? ScreenUtility.getXLeftOffscreen(renderer, camera) : ScreenUtility.getXRightOffscreen(renderer, camera);
                y = Random.Range(ScreenUtility.getYDownOffscreen(renderer, camera), ScreenUtility.getYUpOffscreen(renderer, camera));
            }
            else // Spawn up/down
            {
                x = Random.Range(ScreenUtility.getXLeftOffscreen(renderer, camera), ScreenUtility.getXRightOffscreen(renderer, camera));
                y = Random.Range(0, 2) == 0 ? ScreenUtility.getYDownOffscreen(renderer, camera) : ScreenUtility.getYUpOffscreen(renderer, camera);
            }            

            // Spawn random object
            GameObject spawnedObject = Instantiate(randomObject, new Vector3(x, y), Quaternion.identity);

            // Set velocity to a target within range of center radius
            Rigidbody2D rigidbody = spawnedObject.GetComponent<Rigidbody2D>();
            float speedX = Random.Range(MIN_VEL, MAX_VEL);
            float speedY = Random.Range(MIN_VEL, MAX_VEL);
            float targetX = camera.transform.position.x + Random.Range(-CENTER_RADIUS, CENTER_RADIUS);
            float targetY = camera.transform.position.y + Random.Range(-CENTER_RADIUS, CENTER_RADIUS);
            float dirX = targetX - x;
            float dirY = targetY - y;
            float dirMag = Mathf.Sqrt(dirX * dirX + dirY * dirY);
            rigidbody.velocity = new Vector2(speedX * dirX / dirMag, speedY * dirY / dirMag);

        }
    }
}
