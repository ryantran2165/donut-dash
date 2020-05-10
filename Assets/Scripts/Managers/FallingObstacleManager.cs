using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject fallingObstacle;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform parent;

    private float timer;
    private const float MIN_SPAWN_TIME = 1f;
    private const float MAX_SPAWN_TIME = 5f;
    private const float SPAWN_RANGE = 5f;
    private const float SPAWN_HEIGHT = 25f;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && player != null)
        {
            float randX = player.transform.position.x + Random.Range(-SPAWN_RANGE, SPAWN_RANGE);
            Instantiate(fallingObstacle, new Vector3(randX, SPAWN_HEIGHT), Quaternion.identity, parent);
            timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
        }
    }
}
