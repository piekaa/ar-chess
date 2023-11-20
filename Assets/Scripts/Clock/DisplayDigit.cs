using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayDigit : MonoBehaviour
{
    /**
     *   _ 0
     *  || 1 2
     *   _ 3
     *  || 4 5
     *   _ 6
     */
    [SerializeField] private MeshRenderer[] lines;

    [SerializeField] private DigitMaterials digitMaterials;

    private Dictionary<char, List<int>> digitLines = new()
    {
        { '0', new() { 0, 1, 2, 4, 5, 6 } },
        { '1', new() { 2, 5 } },
        { '2', new() { 0, 2, 3, 4, 6 } },
        { '3', new() { 0, 2, 3, 5, 6 } },
        { '4', new() { 1, 2, 3, 5 } },
        { '5', new() { 0, 1, 3, 5, 6 } },
        { '6', new() { 0, 1, 3, 4, 5, 6 } },
        { '7', new() { 0, 2, 5 } },
        { '8', new() { 0, 1, 2, 3, 4, 5, 6 } },
        { '9', new() { 0, 1, 2, 3, 5, 6 } },
        { ' ', new() { } },
        
        { 'a', new() {4} },
        { 'b', new() {1} },
        { 'c', new() {0} },
        { 'd', new() {2} },
        { 'e', new() {5} },
        { 'f', new() {6} },
    };

    public char toDisplay = ' ';

    void Update()
    {
        foreach (var line in lines)
        {
            line.material = digitMaterials.Off;
        }

        foreach (var lineIndex in digitLines.ContainsKey(toDisplay) ? digitLines[toDisplay] : new List<int>())
        {
            lines[lineIndex].material = digitMaterials.On;
        }
    }
}