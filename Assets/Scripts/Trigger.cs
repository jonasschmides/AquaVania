using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { ON, OFF, TOGGLE };

public class Trigger : Activatable
{
    private AudioSource src;
    public AudioClip clip;

    public TriggerType tType;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        src.PlayOneShot(clip, 1f);
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, clip.length);

        switch (tType)
        {
            case TriggerType.ON:
                foreach (var i in toActivate)
                {
                    Activatable j = (Activatable)i.GetComponent("Activatable");
                    if (j == null)
                        Debug.LogError("Object is not Activatable!");
                    else
                        j.Activate();
                }
                break;
            case TriggerType.OFF:
                foreach (var i in toActivate)
                {
                    Activatable j = (Activatable)i.GetComponent("Activatable");
                    if (j == null)
                        Debug.LogError("Object is not Activatable!");
                    else
                        j.Deactivate();
                }
                break;
            case TriggerType.TOGGLE:
            default:
                foreach (var i in toActivate)
                {
                    Activatable j = (Activatable)i.GetComponent("Activatable");
                    if (j == null)
                        Debug.LogError("Object is not Activatable!");
                    else
                        j.Toggle();
                }
                break;
        }
    }
}