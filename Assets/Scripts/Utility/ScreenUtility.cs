﻿using UnityEngine;

public class ScreenUtility
{
    public static float HORZ_EXT_HALF = Camera.main.orthographicSize * Screen.width / Screen.height;

    public static float getLeftEdge()
    {
        return Camera.main.transform.position.x - HORZ_EXT_HALF;
    }

    public static float getLeftEdge(Camera camera)
    {
        return camera.transform.position.x - (camera.orthographicSize * Screen.width / Screen.height);
    }

    public static float getRightEdge()
    {
        return Camera.main.transform.position.x + HORZ_EXT_HALF;
    }

    public static float getRightEdge(Camera camera)
    {
        return camera.transform.position.x + (camera.orthographicSize * Screen.width / Screen.height);
    }

    public static float getDownEdge()
    {
        return Camera.main.transform.position.y - Camera.main.orthographicSize;
    }

    public static float getDownEdge(Camera camera)
    {
        return camera.transform.position.y - camera.orthographicSize;
    }

    public static float getUpEdge()
    {
        return Camera.main.transform.position.y + Camera.main.orthographicSize;
    }

    public static float getUpEdge(Camera camera)
    {
        return camera.transform.position.y + camera.orthographicSize;
    }

    public static float getXLeftOffscreen(SpriteRenderer renderer)
    {
        return getLeftEdge() - renderer.bounds.size.x / 2f;
    }

    public static float getXRightOffscreen(SpriteRenderer renderer)
    {
        return getRightEdge() + renderer.bounds.size.x / 2f;
    }

    public static float getXLeftOnscreen(SpriteRenderer renderer)
    {
        return getLeftEdge() + renderer.bounds.size.x / 2f;
    }

    public static float getXLeftOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getLeftEdge(camera) + renderer.bounds.size.x / 2f;
    }

    public static float getXRightOnscreen(SpriteRenderer renderer)
    {
        return getRightEdge() - renderer.bounds.size.x / 2f;
    }

    public static float getXRightOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getRightEdge(camera) - renderer.bounds.size.x / 2f;
    }

    public static float getYDownOffscreen(SpriteRenderer renderer)
    {
        return getDownEdge() - renderer.bounds.size.y / 2f;
    }

    public static float getYUpOffscreen(SpriteRenderer renderer)
    {
        return getUpEdge() + renderer.bounds.size.y / 2f;
    }

    public static float getYDownOnscreen(SpriteRenderer renderer)
    {
        return getDownEdge() + renderer.bounds.size.y / 2f;
    }

    public static float getYDownOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getDownEdge(camera) + renderer.bounds.size.y / 2f;
    }

    public static float getYUpOnscreen(SpriteRenderer renderer)
    {
        return getUpEdge() - renderer.bounds.size.y / 2f;
    }

    public static float getYUpOnscreen(SpriteRenderer renderer, Camera camera)
    {
        return getUpEdge(camera) - renderer.bounds.size.y / 2f;
    }

    public static float getCenterX()
    {
        return Camera.main.transform.position.x;
    }

    public static float getCenterY()
    {
        return Camera.main.transform.position.y;
    }
}