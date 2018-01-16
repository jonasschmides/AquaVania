using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaterCamera : MonoBehaviour {

	public Material material;

	void Start()
	{
		GetComponent<Camera>().cullingMask = 1 << 0;
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
