using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCommand : BaseCommand
{

    public MeshRenderer MeshRenderer;

    public ObjectCommand(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        Vector3 vector = argsToVector();
        manaCost = vector.x + vector.y + vector.z;
    }

    public ObjectCommand(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    public override void run()
    {
        base.run();
        MeshRenderer = self.GetComponent<MeshRenderer>();
    }

}
