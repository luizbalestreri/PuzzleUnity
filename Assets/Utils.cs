using UnityEngine;
public static class Utils
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position){
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static Vector2 RoundVector2 (Vector2 vector2){
        float x = (float) System.Math.Round(vector2.x);
        float y = (float) System.Math.Round(vector2.y);
        return (new Vector2(x, y));
    }

    public static int FindIndex<T>(this T[] array, T item) {
        return System.Array.IndexOf(array, item);
    }

}
