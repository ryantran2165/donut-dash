using UnityEngine;

public class ScreenUtility
{
    public static float HORZ_EXT_HALF = Camera.main.orthographicSize * Screen.width / Screen.height;

    public static float getLeftEdge()
    {
        return Camera.main.transform.position.x - HORZ_EXT_HALF;
    }

    public static float getRightEdge()
    {
        return Camera.main.transform.position.x + HORZ_EXT_HALF;
    }
}
