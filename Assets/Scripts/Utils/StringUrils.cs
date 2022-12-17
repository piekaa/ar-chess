using System;
using System.Linq;
using UnityEngine;

public class StringUtils
{
    public static string ToFirstUpper(string s)
    {
        return Char.ToUpper(s[0]) + s.Substring(1);
    }

    public static string ToFirstLower(string s)
    {
        return Char.ToLower(s[0]) + s.Substring(1);
    }

    public static string ToResourcePath(string path)
    { 
        if (!path.Contains("Resources/"))
        {
            Logger.Log(path + " is invalid resource path!!!");
            return "";
        }

        var afterResources = path.Split(new string[] {"Resources/"}, StringSplitOptions.None)[1];
        var dotIndex = afterResources.LastIndexOf('.');

        if (dotIndex == -1)
        {
            Logger.Log(path + " is invalid resource path!!!");
            return "";
        }

        return afterResources.Substring(0, dotIndex);
    }
    
    public static string FirstCharToUpper(string input)
    {
        return input.First().ToString().ToUpper() + input.Substring(1);
    }
}