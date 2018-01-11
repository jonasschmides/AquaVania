using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	private Vector3 offset;
	private float offsetZ;

	// Use this for initialization
	void Start () {
		offset = transform.position - player.transform.position;
		offsetZ = - offset[1];
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Vector3 position = player.transform.position + offset;
		position [1] = offsetZ;
		transform.position = position;
	}
}
