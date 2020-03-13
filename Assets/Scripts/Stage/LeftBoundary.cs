using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftBoundary : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private float horzExtentHalf;

    // Start is called before the first frame update
    void Start()
    {
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        // Set transform position to follow left edge of camera
        float leftEdge = camera.transform.position.x - horzExtentHalf;
        transform.position = new Vector3(leftEdge, transform.position.y, transform.position.z);
    }
}
