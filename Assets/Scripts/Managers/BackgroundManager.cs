using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject background1;
    [SerializeField] private GameObject background2;
    [SerializeField] private Camera camera;

    private float horzExtentHalf;
    private GameObject curBackground;
    private float curBackgroundRightEdge;
    private float backgroundWidth;
    private const float BACKGROUND_OFFSET = -0.3f;

    // Start is called before the first frame update
    void Start()
    {
        horzExtentHalf = camera.orthographicSize * Screen.width / Screen.height;

        curBackground = background1;
        SpriteRenderer renderer = curBackground.GetComponent<SpriteRenderer>();
        curBackgroundRightEdge = renderer.bounds.max.x;
        backgroundWidth = renderer.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float cameraLeftEdge = camera.transform.position.x - horzExtentHalf;

        // Current background passed out of view
        if (curBackgroundRightEdge < cameraLeftEdge)
        {
            // Move current background to end of other background
            Vector3 curPos = curBackground.transform.position;
            curBackground.transform.position = new Vector3(curPos.x + backgroundWidth * 2 + BACKGROUND_OFFSET, curPos.y, curPos.z);

            // Swap current backgrounds
            curBackground = curBackground == background1 ? background2 : background1;
            SpriteRenderer renderer = curBackground.GetComponent<SpriteRenderer>();
            curBackgroundRightEdge = renderer.bounds.max.x;

        }
    }
}
