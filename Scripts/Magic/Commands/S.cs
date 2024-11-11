using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S : ModiferCommand
{
    public S(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        Vector3 vector = argsToVector();
        manaCost = (vector.x + vector.y + vector.z) / 10;
    }

    public S(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    protected override void apply(Vector3 modifyAmount)
    {
        try
        {
            parent.self.transform.localScale = modifyAmount;
            self = new GameObject("S");
            self.transform.parent = parent.self.transform;
            self.transform.localPosition = Vector3.zero;
        }
        catch
        {

        }
        
    }

    public override void run()
    {
        applyToParent(argsToVector());
    }

}
