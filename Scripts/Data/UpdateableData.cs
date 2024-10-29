using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateableData : ScriptableObject
{
    public event System.Action OnValuesUpdate;
    public bool autoUpdate;

    #if UNITY_EDITOR


    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {

            UnityEditor.EditorApplication.update += NotifiyOfUpdateValues;
        }
    }

    public void NotifiyOfUpdateValues()
    {
        UnityEditor.EditorApplication.update -= NotifiyOfUpdateValues;
        if (OnValuesUpdate != null)
        {
            OnValuesUpdate();
        }
    }

    #endif
}
