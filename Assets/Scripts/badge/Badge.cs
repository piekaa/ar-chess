using System;
using UnityEngine;
using UnityEngine.UI;

public class Badge : EventListener
{
    [SerializeField] private Text text;

    [SerializeField] private Material badgeNameMaterial;
    
    
    // https://forum.unity.com/threads/ui-text-to-texture.397135/
    
    private void LateUpdate()
    {
        badgeNameMaterial.mainTexture = text.mainTexture;
    }
}
