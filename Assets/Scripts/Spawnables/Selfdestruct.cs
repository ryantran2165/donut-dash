using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    private Camera camera;
    private float colliderRightEdge;
    private float horzExtentHalf;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        colliderRightEdge = collider.bounds.max.x;
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        float cameraLeftEdge = camera.transform.position.x - horzExtentHalf;

        // Destroy spike collider when out of screen
        if (colliderRightEdge < cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}
