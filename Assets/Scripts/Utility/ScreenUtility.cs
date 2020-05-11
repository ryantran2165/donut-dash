using UnityEngine;

public class ScreenUtility
{
    public static float getHorzExtHalf(Camera camera)
    {
        return camera.orthographicSize * Screen.width / Screen.height;
    }

    public static float getLeftEdge(Camera camera)
    {
        return camera.transform.position.x - (camera.orthographicSize * Screen.width / Screen.height);
    }

    public static float getRightEdge(Camera camera)
    {
        return camera.transform.position.x + (camera.orthographicSize * Screen.width / Screen.height);
    }

    public static float getDownEdge(Camera camera)
    {
        return camera.transform.position.y - camera.orthographicSize;
    }

    public static float getUpEdge(Camera camera)
    {
        return camera.transform.position.y + camera.orthographicSize;
    }

    public static float getXLeftOffscreen(SpriteRenderer renderer, Camera camera)
    {
        return getLeftEdge(camera) - renderer.bounds.size.x / 2f;
    }

    public static float getXRightOffscreen(SpriteRenderer renderer, Camera camera)
    {
        return getRightEdge(camera) + renderer.bounds.size.x / 2f;
    }

    public static float getXLeftOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getLeftEdge(camera) + renderer.bounds.size.x / 2f;
    }

    public static float getXRightOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getRightEdge(camera) - renderer.bounds.size.x / 2f;
    }

    public static float getYDownOffscreen(SpriteRenderer renderer, Camera camera)
    {
        return getDownEdge(camera) - renderer.bounds.size.y / 2f;
    }

    public static float getYUpOffscreen(SpriteRenderer renderer, Camera camera)
    {
        return getUpEdge(camera) + renderer.bounds.size.y / 2f;
    }

    public static float getYDownOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getDownEdge(camera) + renderer.bounds.size.y / 2f;
    }

    public static float getYUpOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getUpEdge(camera) - renderer.bounds.size.y / 2f;
    }
}
