using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	public bool IsActive = true;
	public bool IsSelected = false;

	public bool ScaleonHover = true;
	public float ScaleRatio = 1f;

	public AudioSource ButtonPress;
	public AudioSource ButtonSelect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(ScaleonHover == true){
			if (IsSelected == false) {
				this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (ScaleRatio, ScaleRatio, ScaleRatio), 0.25f * (Time.unscaledDeltaTime * 100f));
			} else {
				this.transform.localScale = Vector3.Lerp (this.transform.localScale, new Vector3 (ScaleRatio * 1.25f, ScaleRatio * 1.25f, ScaleRatio * 1.1f), 0.25f * (Time.unscaledDeltaTime * 100f));
			}
		}

		if (IsActive == false) {
			IsSelected = false;
		} else {
			if(Input.GetMouseButtonDown(0) && IsSelected == true){
				if(ButtonPress != null){
					ButtonPress.Play ();
				}
			}
		}
		
	}

	void OnEnabled(){
		IsSelected = false;
	}

	public void OnPointerEnter(){

		if(IsActive == true){
			IsSelected = true;
			if(ButtonSelect != null){
				ButtonSelect.Play ();
			}
		}

	}

	public void OnPointerLeave(){

		IsSelected = false;

	}

}
