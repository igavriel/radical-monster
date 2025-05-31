using UnityEngine;

static public class Util
{
    public static void AssertObject(Object obj, string message)
    {
        if (obj == null)
        {
            Debug.LogError(message);
            throw new UnityException(message);
        }
    }
}
