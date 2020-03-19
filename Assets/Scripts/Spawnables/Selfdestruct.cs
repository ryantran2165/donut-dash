using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    private Camera camera;
    private float horzExtentHalf;
    private SpriteRenderer renderer;
    private BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        float rightEdge = renderer != null ? renderer.bounds.max.x : collider.bounds.max.x;
        float cameraLeftEdge = camera.transform.position.x - horzExtentHalf;

        // Destroy when out of screen
        if (rightEdge < cameraLeftEdge)
        {
            Destroy(gameObject);
        }
    }
}
