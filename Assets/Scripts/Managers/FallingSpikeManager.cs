using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeManager : MonoBehaviour
{
    [SerializeField] private GameObject fallingSpike;
    [SerializeField] private GameObject player;

    private float timer;
    private const float MIN_SPAWN_TIME = 1f;
    private const float MAX_SPAWN_TIME = 5f;
    private const float SPAWN_RANGE = 5f;
    private const float SPAWN_HEIGHT = 25f;

    // Start is called before the first frame update
    void Start()
    {
        setRandomTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && player != null)
        {
            float randX = player.transform.position.x + Random.Range(-SPAWN_RANGE, SPAWN_RANGE);
            Instantiate(fallingSpike, new Vector3(randX, SPAWN_HEIGHT, 0), Quaternion.identity);
            setRandomTimer();
        }
    }

    private void setRandomTimer()
    {
        timer = Random.Range(MIN_SPAWN_TIME, MAX_SPAWN_TIME);
    }
}
