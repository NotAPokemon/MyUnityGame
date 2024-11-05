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
        if (parent != null)
        {
            try
            {
                self.transform.parent = parent.self.transform;
            }
            catch
            {
                self.transform.parent = MagicReader.spellParent.transform;
            }

        }
        else
        {
            self.transform.parent = MagicReader.spellParent.transform;
        }
        self.transform.localPosition = pos;
    }


}
