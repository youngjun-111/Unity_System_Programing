using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class CustomTools : Editor
{
   [MenuItem("치트/잼 10,000 추가")]
   public static void AddUserGem()  
    {
        var Gem = long.Parse(PlayerPrefs.GetString("Gem"));
        Gem += 10000;

        PlayerPrefs.SetString("Gem", Gem.ToString());
        PlayerPrefs.Save();
    }

    [MenuItem("치트/골드 100,000 추가")]
    public static void AddUserGold()
    {
        var Gold = long.Parse(PlayerPrefs.GetString("Gold"));
        Gold += 100000;

        PlayerPrefs.SetString("Gold", Gold.ToString());
        PlayerPrefs.Save();
    }
}
#endif
