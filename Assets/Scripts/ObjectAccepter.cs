using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAccepter : MonoBehaviour
{
    public GameObject objOrigin;

    public GameObject marker;

    private bool _isHolding;

    private readonly Color _colorHolding = Color.green;
    private readonly Color _colorInactive = Color.red;

    private GameObject _itemRef;

    void Start()
    {
        marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
    }

    public void Hold(GameObject obj)
    {
        _isHolding = true;
        _itemRef = obj;
        obj.transform.position = objOrigin.transform.position;
        marker.gameObject.GetComponent<Renderer>().material.color = _colorHolding;
    }

    public GameObject GetItemRefBeforeRelease()
    {
        return _itemRef;
    }

    public void Release()
    {
        _isHolding = false;
        marker.gameObject.GetComponent<Renderer>().material.color = _colorInactive;
        _itemRef = null;
    }
}