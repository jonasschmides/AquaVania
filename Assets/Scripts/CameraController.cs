using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    public float minX = 0;
    public float minY = 0;
    public float maxX = 0;
    public float maxY = 0;

    private Vector3 oldPos;

    // Use this for initialization
    void Start()
    {
        if (maxX == 0) maxX = float.MaxValue;
        if (maxY == 0) maxY = float.MaxValue;

        offset = transform.position;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float newX = Mathf.Max(minX, Mathf.Min(maxX, player.transform.position.x));
        float newY = Mathf.Max(minY, Mathf.Min(maxY, player.transform.position.y));

        var newPos = Vector3.Lerp(oldPos, new Vector3(newX, newY), 0.95f);
        oldPos = newPos;

        transform.position = newPos + offset;
    }
}
