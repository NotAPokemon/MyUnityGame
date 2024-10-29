using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrainData : UpdateableData
{
    public bool useFalloff;
    public float uniformScale;
    public float MeshHeightMultiplyer;
    public AnimationCurve meshcurve;
    public float minHeight
    {
        get
        {
            return uniformScale * meshcurve.Evaluate(0) * MeshHeightMultiplyer;
        }
    }

    public float maxHeight
    {
        get
        {
            return uniformScale * meshcurve.Evaluate(1) * MeshHeightMultiplyer;
        }
    }


}
