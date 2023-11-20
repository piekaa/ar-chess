using UnityEngine;

public class LCD : MonoBehaviour
{
    [SerializeField] private LCDChar[] line1;

    public string ToDisplay;


    private void Update()
    {
        foreach (var lcdChar in line1)
        {
            lcdChar.ToDisplay = (char)0;
        }

        for (var i = 0; i < line1.Length && i < ToDisplay.Length; i++)
        {
            line1[i].ToDisplay = ToDisplay[i];
        }
    }
}