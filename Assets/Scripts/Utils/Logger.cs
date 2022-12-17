using UnityEngine;

public class Logger
{
    private static bool enabled = true;

    public static void Log(object s)
    {
        if (enabled)
        {
            Debug.Log(s);
        }
    }
}