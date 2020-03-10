using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 vec3;
    private float initialY;

    // Start is called before the first frame update
    void Start()
    {
        vec3 = new Vector3();
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Vector3 camPos = transform.position;
        Vector3 playerPos = player.transform.position;

        vec3.Set(Mathf.Max(camPos.x, playerPos.x), Mathf.Max(initialY, playerPos.y), camPos.z);
        transform.position = vec3;
    }
}
