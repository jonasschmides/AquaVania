﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccepter : Activatable
{
    public GameObject objOrigin;
    public GameObject marker;

    private readonly Color _colorHolding = Color.green;
    private readonly Color _colorInactive = Color.red;

    public GameObject itemRef;

    void Start()
    {
        if(itemRef != null)
        {
            Hold(itemRef);
            marker.gameObject.GetComponent<Renderer>().material.color = _colorHolding;
        }
        else
        {
            marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
        }
       
    }

    public void Hold(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.position = objOrigin.transform.position;
            itemRef = obj;
            marker.gameObject.GetComponent<Renderer>().material.color = _colorHolding;
            Activate();
        }
    }

    public GameObject GetItemRefBeforeRelease()
    {
        return itemRef;
    }

    public void Release()
    {
        marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
        itemRef = null;
        Deactivate();
    }
}