using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    public float minX = 0;
    public float minY = 0;

    // Use this for initialization
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float newX = Mathf.Max(minX, player.transform.position.x);
        float newY = Mathf.Max(minY, player.transform.position.y);

        var newPos = new Vector3(newX, newY);

        transform.position = newPos + offset;
    }
}
