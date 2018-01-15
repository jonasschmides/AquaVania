using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Activatable : MonoBehaviour
{
    public bool isActive;
    public Transform[] toActivate;

    public virtual void Activate()
    {
        isActive = true;
        foreach (var i in toActivate)
        {
            Activatable j = (Activatable)i.GetComponent("Activatable");
            j.Activate();
        }
    }
    public virtual void Deactivate()
    {
        isActive = false;
        foreach (var i in toActivate)
        {
            Activatable j = (Activatable)i.GetComponent("Activatable");
            j.Deactivate();
        }
    }
    public virtual void Toggle()
    {
        foreach (var i in toActivate)
        {
            Activatable j = (Activatable)i.GetComponent("Activatable");
            if (j.isActive)
                j.Deactivate();
            else
                j.Activate();
        }
        //if (isActive) Activate();
        //else Deactivate();
    }
}