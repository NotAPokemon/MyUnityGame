using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCommand
{
    string[] args;
    public BaseCommand child;
    bool structured;
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

    public BaseCommand getChild(int index, int iteration = 0) 
    { 
        if (index == iteration)
        {
            return this;
        }
        return child.getChild(index, iteration +1);
    }
    
}
