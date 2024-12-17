using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeCreator : MonoBehaviour
{
    public static List<ThemeInfo> list;

    public List<ThemeInfo> themeList;

    private void Awake()
    {
        list = themeList;
    }

    public static ThemeInfo getRandom()
    {
        int index = Random.Range(0, list.Count - 1);
        return list[index];
    }
}
