using System;
using UnityEngine;

public class ArSymulator : MonoBehaviour
{
    private void Start()
    {
        EventSystem.Instance.Fire(EventName.ARSpawn, new EventData(Vector3.zero, Quaternion.identity));
    }
}
