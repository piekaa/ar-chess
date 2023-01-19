using UnityEngine;

public abstract class Animation : ScriptableObject
{
    public abstract void StartAnimation(GameObject gameObject);
    public abstract void StopAnimation(GameObject gameObject);
    
    protected Empty GetEmpty(GameObject gameObject)
    {
        var empty = gameObject.GetComponent<Empty>();

        if (empty == null)
        {
            empty = gameObject.AddComponent<Empty>();
        }

        return empty;
    }
}