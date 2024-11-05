using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P : ModiferCommand
{
    public P(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        Vector3 vector = argsToVector();
        manaCost = (vector.x + vector.y + vector.z) / 10;
    }

    public P(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    protected override void apply(Vector3 modifyAmount)
    {
        parent.self.transform.localPosition = modifyAmount;
        self = new GameObject("P");
        self.transform.parent = parent.self.transform;
        self.transform.localPosition = Vector3.zero;
        self.transform.localRotation = Quaternion.Euler(Vector3.zero);
        self.transform.localScale = Vector3.one;
    }

    public override void run()
    {
        applyToParent(argsToVector());
    }
}
