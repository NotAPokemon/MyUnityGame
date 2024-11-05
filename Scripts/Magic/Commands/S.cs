using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class S : BaseCommand
{
    public S(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        Vector3 vector = argsToVector();
        manaCost = vector.x + vector.y + vector.z;
    }

    public S(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }


    public override void run()
    {
        parent.self.transform.localScale = argsToVector();
        self = new GameObject("S");
        self.transform.parent = parent.self.transform;
        self.transform.localPosition = Vector3.zero;
    }

}
