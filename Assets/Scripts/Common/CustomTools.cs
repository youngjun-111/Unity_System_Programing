using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CustomTools : Editor
{
   [MenuItem("CustomTools/Add User Gem (+10000)")]
   public static void AddUserGem()  
    {
        var Gem = long.Parse(PlayerPrefs.GetString("Gem"));
        Gem += 10000;

        PlayerPrefs.SetString("Gem", Gem.ToString());
        PlayerPrefs.Save();
    }

    [MenuItem("CustomTools/Add User Gold (+100000)")]
    public static void AddUserGold()
    {
        var Gold = long.Parse(PlayerPrefs.GetString("Gold"));
        Gold += 100000;

        PlayerPrefs.SetString("Gold", Gold.ToString());
        PlayerPrefs.Save();
    }
}
#endif
