using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillScreen : MonoBehaviour
{
    [SerializeField] private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        // Get default sprite sizes
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float width = renderer.bounds.size.x;
        float height = renderer.bounds.size.y;

        // Get camera sizes
        float camHeight = camera.orthographicSize * 2f;
        float camWidth = camHeight / Screen.height * Screen.width;

        // Set position to center of screen (same as camera)
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y);

        // Stretch sprite to screen
        transform.localScale = new Vector3(camWidth / width, camHeight / height, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
