using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC : BaseCommand
{

    public BC(string[] args) : base(args)
    {
        this.args = args;
        structured = false;
    }

    public BC(string[] args, BaseCommand child) : base(args, child)
    {
        this.args = args;
        this.child = child;
        structured = true;
    }

    

    public override void run()
    {
        Vector3 pos = argsToVector();
        self = new GameObject("BC");
        if (parent != null)
        {
            try
            {
                self.transform.parent = parent.self.transform;
            } catch
            {
                self.transform.parent = MagicReader.spellParent.transform;
            }
            
        } else
        {
            self.AddComponent<DestroyOnDone>();
            self.transform.parent = MagicReader.spellParent.transform;
        }
        
        self.transform.localPosition = pos;
        self.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
