using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModiferCommand : BaseCommand
{
    public ModiferCommand(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
        Vector3 vector = argsToVector();
        manaCost = vector.x + vector.y + vector.z;
    }

    public ModiferCommand(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    protected virtual void apply(Vector3 modifyAmount)
    {

    }

    public virtual void applyToParent(Vector3 modifyAmount)
    {
        if (parent != null)
        {
            if (parent is ModiferCommand)
            {
                BaseCommand nextParent = parent.parent;
                while (nextParent is ModiferCommand)
                {
                    try
                    {
                        nextParent = nextParent.parent;
                    }
                    catch
                    {
                        break;
                    }
                    if (nextParent == null)
                    {
                        break;
                    }
                }
                if (nextParent is not ModiferCommand)
                {
                    ((ModiferCommand)nextParent.child).apply(modifyAmount);
                }
            } else
            {
                apply(modifyAmount);
            }
        }
    }

}
