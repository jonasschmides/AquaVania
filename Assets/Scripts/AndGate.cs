using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndGate : Activatable
{
    private AudioSource src;
    public AudioClip clip;

    public TriggerType tType;

    public Activatable[] inputStates;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }


    override public void Activate()
    {
       // int k = 0;
        foreach (Activatable a in inputStates)
        {
           // Debug.Log(k++);
            if (!a.isActive) return;
        }

        src.PlayOneShot(clip,0.2f);

        base.Activate();
       
    }
}