using UnityEngine;

public abstract class Animation : ScriptableObject
{
    public abstract void StartAnimation(GameObject gameObject);
    public abstract void StopAnimation(GameObject gameObject);
}