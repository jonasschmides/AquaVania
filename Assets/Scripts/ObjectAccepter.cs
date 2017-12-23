using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccepter : Activatable
{
    public GameObject objOrigin;
    public GameObject marker;

    private readonly Color _colorHolding = Color.green;
    private readonly Color _colorInactive = Color.red;

    private GameObject _itemRef;

    void Start()
    {
        marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
    }

    public void Hold(GameObject obj)
    {
        if (obj != null)
        {
            obj.transform.position = objOrigin.transform.position;
            _itemRef = obj;
            marker.gameObject.GetComponent<Renderer>().material.color = _colorHolding;
            Activate();
        }
    }

    public GameObject GetItemRefBeforeRelease()
    {
        return _itemRef;
    }

    public void Release()
    {
        marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
        _itemRef = null;
        Deactivate();
    }
}