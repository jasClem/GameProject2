using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroller : MonoBehaviour {

    public float scrollSpeed = 1.0f;

	
	// Class to scroll the texture of our roads background
	void Update ()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
            //Variable for Unity's Mesh Renderer

        Material mat = mr.material;
            //Variable for Unity Material from the Mesh Renderer

        Vector2 offset = mat.mainTextureOffset;
            //Variable for Vector2 offset (x, y) to change our Material

        offset.x = transform.position.x / transform.localScale.x;
        offset.y = transform.position.y / transform.localScale.y;
            //Change offset for x and y positions to our current position divided by our localscale

        mat.mainTextureOffset = offset;
            //Set our Material with current offset.
	}
}
