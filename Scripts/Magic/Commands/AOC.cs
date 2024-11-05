using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOC : BaseCommand
{

    public AOC(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        manaCost = 1;
    }

    public AOC(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    public override void run()
    {
        Vector3 pos = argsToVector();
        self = GameObject.CreatePrimitive(PrimitiveType.Cube);
        self.transform.parent = parent != null ? parent.self.transform : MagicReader.spellParent.transform;
        self.transform.localPosition = pos;
    }


}
