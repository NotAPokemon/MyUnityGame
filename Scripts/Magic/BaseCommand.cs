using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCommand
{
    protected string[] args;
    public BaseCommand child;
    public bool structured;
    public BaseCommand parent;
    public GameObject self;
    public float manaCost = 0;
    public BaseCommand(string[] args)
    {
        this.args = args;
        structured = false;
    }

    public BaseCommand(string[] args, BaseCommand child)
    {
        this.args = args;
        this.child = child;
        structured= true;
    }

    public void setChild(BaseCommand child)
    {
        this.child=child;
        structured=true;
    }

    public BaseCommand next()
    {
        return child;
    }

    protected Vector3 argsToVector(int index1 = 0, int index2 = 1, int index3 = 2)
    {
        return new Vector3(float.Parse(args[index1]), float.Parse(args[index2]), float.Parse(args[index3]));
    }

    public BaseCommand getChild(int index, int iteration = 0) 
    { 
        if (index == iteration)
        {
            return this;
        }
        return child.getChild(index, iteration +1);
    }

    public virtual void run()
    {

    }
    
}
