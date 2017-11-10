using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { ON, OFF, TOGGLE };

public class Trigger : MonoBehaviour
{

    private AudioSource src;
    public AudioClip clip;

    public TriggerType tType;
    public Transform[] toActivate;

    private List<IActivatable> _toActivate;


    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void Collect()
    {
        src.PlayOneShot(clip, 0.05f);
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, clip.length);



        switch (tType)
        {

            case TriggerType.ON:
                foreach (var i in toActivate)
                {
                    IActivatable j = (IActivatable)i.GetComponent("IActivatable");
                    if (j == null)
                        Debug.LogError("Object is not IActivatable!");
                    else
                        j.Activate();
                }
                break;
            case TriggerType.OFF:
                foreach (var i in toActivate)
                {
                    IActivatable j = (IActivatable)i.GetComponent("IActivatable");
                    if (j == null)
                        Debug.LogError("Object is not IActivatable!");
                    else
                        j.Deactivate();
                }
                break;
            case TriggerType.TOGGLE:
            default:
                foreach (var i in toActivate)
                {
                    IActivatable j = (IActivatable)i.GetComponent("IActivatable");
                    if (j == null)
                        Debug.LogError("Object is not IActivatable!");
                    else
                        j.Toggle();
                }
                break;
        }
    }
}
