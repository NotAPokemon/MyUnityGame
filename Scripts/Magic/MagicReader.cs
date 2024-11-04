using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MagicReader : MonoBehaviour
{

    public static List<Action<MagicTokenizer>> callbacks = new List<Action<MagicTokenizer>>();
    public static List<MagicTokenizer> arg = new List<MagicTokenizer>();

    public void Start()
    {
        requestCommands("assets/magic/test.magic",test);
    }
    public static void requestCommands(string filePath, Action<MagicTokenizer> callback)
    {
        ThreadStart threadStart = delegate
        {
            commandThread(filePath, callback);
        };
        new Thread(threadStart).Start();
    }

    public static void commandThread(string filePath, Action<MagicTokenizer> callback)
    {
        MagicTokenizer test = new MagicTokenizer(filePath);
        test.structure();

        callbacks.Add(callback);
        arg.Add(test);
    }

    void test(MagicTokenizer commandWrapper)
    {
        Player.player.health -= 1;
    }

    public void Update()
    {
        if (callbacks.Count > 0)
        {
            callbacks[0].Invoke(arg[0]);
            arg.RemoveAt(0);
            callbacks.RemoveAt(0);
        }
    }
}
