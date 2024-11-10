using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M : ModiferCommand
{
    Vector4 baseColor;
    Vector2 TextureMetalicSmoothness;
    Vector4 Emmission;

    public M(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        baseColor = argsToColor();
        TextureMetalicSmoothness = argsToVector(4, 5);
        Emmission = argsToEmission();
    }

    public M(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    
    Vector4 argsToColor()
    {
        Vector3 solid = argsToVector();
        Vector4 color = new Vector4(solid.x,solid.y,solid.z, argsToVector(3).x);
        return color;
    }

    Vector4 argsToEmission()
    {
        Vector3 color = argsToVector(6,7,8);
        return new Vector4(color.x, color.y, color.z, argsToVector(9).x);
    }

    protected override void apply(Vector3 modifyAmount)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = baseColor;
        mat.SetFloat("_Smoothness", TextureMetalicSmoothness.y);
        mat.SetFloat("_Metallic", TextureMetalicSmoothness.x);
        mat.SetColor("_EmissionColor", Emmission);
        ((ObjectCommand) parent).MeshRenderer.material = mat;
        self = new GameObject("M");
        self.transform.parent = parent.self.transform;
        self.transform.localPosition = Vector3.zero;
    }

    public override void applyToParent(Vector3 modifyAmount)
    {
        if (parent != null)
        {
            if (parent is not ObjectCommand)
            {
                try
                {
                    ((ModiferCommand)parent).applyToParent(modifyAmount);
                } catch
                {
                    ((ModiferCommand)parent.parent).applyToParent(modifyAmount);
                }
            }
            else
            {
                apply(modifyAmount);
            }
        }
    }

}
