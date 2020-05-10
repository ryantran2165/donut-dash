using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestruct : MonoBehaviour
{
    [SerializeField] private int mode;

    private SpriteRenderer renderer;
    private BoxCollider2D collider;
    private const float OFFSET = 5f;

    private const int LEFT_ONLY_MODE = 0;
    private const int RIGHT_ONLY_MODE = 1;
    private const int BOTH_MODE = 2;
    private const int CREDITS_MODE = 3;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rendererLeftEdge = renderer != null ? renderer.bounds.min.x : collider.bounds.min.x;
        float rendererRightEdge = renderer != null ? renderer.bounds.max.x : collider.bounds.max.x;

        // Destroy when out of screen
        switch (mode)
        {
            case LEFT_ONLY_MODE:
                if (rendererRightEdge < ScreenUtility.getLeftEdge() - OFFSET)
                {
                    Destroy(gameObject);
                }
                break;
            case RIGHT_ONLY_MODE:
                if (rendererLeftEdge > ScreenUtility.getRightEdge() + OFFSET)
                {
                    Destroy(gameObject);
                }
                break;
            case BOTH_MODE:
                if ((rendererLeftEdge > ScreenUtility.getRightEdge() + OFFSET) ||
                    (rendererRightEdge < ScreenUtility.getLeftEdge() - OFFSET))
                {
                    Destroy(gameObject);
                }
                break;
            case CREDITS_MODE:
                float rendererDownEdge = renderer.bounds.min.y;
                float rendererUpEdge = renderer.bounds.max.y;

                if ((rendererLeftEdge > ScreenUtility.getRightEdge() + OFFSET) ||
                    (rendererRightEdge < ScreenUtility.getLeftEdge() - OFFSET) ||
                    (rendererDownEdge > ScreenUtility.getUpEdge() + OFFSET) ||
                    (rendererUpEdge < ScreenUtility.getDownEdge() - OFFSET))
                {
                    Destroy(gameObject);
                }
                break;
        }
        
    }
}
