using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MagicReader : MonoBehaviour
{

    public static List<Action<MagicTokenizer>> callbacks = new List<Action<MagicTokenizer>>();
    public static List<MagicTokenizer> arg = new List<MagicTokenizer>();
    public GameObject activeSpellParent;
    public static GameObject spellParent;
    public SetMagicKeys pathWrapper;

    public void Start()
    {
        spellParent = activeSpellParent;
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

    void runOnComplete(MagicTokenizer commandWrapper)
    {
        GameObject result =  commandWrapper.runAll();
        result.transform.SetParent(transform);
    }

    public void Update()
    {
        if (callbacks.Count > 0)
        {
            callbacks[0].Invoke(arg[0]);
            arg.RemoveAt(0);
            callbacks.RemoveAt(0);
        }
        if (!UIManager.UIOpen && pathWrapper.paths.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                requestCommands(pathWrapper.paths[0], runOnComplete);
            } else if (Input.GetKeyDown(KeyCode.G))
            {
                requestCommands(pathWrapper.paths[1], runOnComplete);
            } else if (Input.GetKeyDown(KeyCode.H))
            {
                requestCommands(pathWrapper.paths[2], runOnComplete);
            } else if (Input.GetKeyDown(KeyCode.J))
            {
                requestCommands(pathWrapper.paths[3], runOnComplete);
            }
        }
    }
}
