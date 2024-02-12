using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour {

	public GameObject Camera;
	public bool KeepScale = true;
	public float ScaleMultiplier = 1f;

	// Use this for initialization
	void Start () {

		Camera = GameObject.Find ("MainCamera");
		
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.LookAt (Camera.transform.position, Camera.transform.up * 1f);
		if(KeepScale == true){
			this.transform.localScale = new Vector3 (Vector3.Distance(this.transform.position, Camera.transform.position) * ScaleMultiplier, Vector3.Distance(this.transform.position, Camera.transform.position) * ScaleMultiplier, Vector3.Distance(this.transform.position, Camera.transform.position) * ScaleMultiplier);
		}
		
	}
}
