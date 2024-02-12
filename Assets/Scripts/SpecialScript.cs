using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialScript : MonoBehaviour {

	// References
	public GameObject SpecialObject;
	// References

	// Main Variables
	public string TypeofSpecial = "";
	public bool CausedByPlayer = false;
	// Main Variables

	// Explosion
	public float ExplosionRadius = 1f;
	public float ExplosionPower = 10f;
	public bool IsLethal = true;
	// Explosion

	// Use this for initialization
	void Start () {

		foreach(Transform ChooseSpecialObject in this.transform){
			if (ChooseSpecialObject.name == TypeofSpecial) {
				SpecialObject = ChooseSpecialObject.gameObject;
				ChooseSpecialObject.gameObject.SetActive (true);
			} else {
				ChooseSpecialObject.gameObject.SetActive (false);
			}
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Explosion
		if(TypeofSpecial == "Explosion"){
			SpecialObject.transform.localScale = new Vector3 (ExplosionRadius, ExplosionRadius, ExplosionRadius);
			// Shake Camera of a player
			if(GameObject.Find("MainPlane") != null){
				if(Vector3.Distance(this.transform.position, GameObject.Find("MainPlane").transform.position) <= ExplosionRadius * 10f && IsLethal == true){
					GameObject.Find ("MainPlane").GetComponent<PlayerScript> ().ShakePower = (1f - (Vector3.Distance(this.transform.position, GameObject.Find("MainPlane").transform.position) / (ExplosionRadius * 10f))) * (ExplosionPower / 10f);
					GameObject.Find ("MainPlane").GetComponent<PlayerScript> ().ShakeDecay = 0.1f;
				}
			}
			// Shake Camera of a player
			// Remove
			if(SpecialObject.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f && IsLethal == true){
				IsLethal = false;
			}
			if(SpecialObject.GetComponent<AudioSource>().isPlaying == false && SpecialObject.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f && SpecialObject.transform.GetChild(1).GetComponent<ParticleSystem>().particleCount <= 0f && SpecialObject.transform.GetChild(2).GetComponent<ParticleSystem>().particleCount <= 0f){
				Destroy (this.gameObject);
			}
			// Remove
		} else if(TypeofSpecial == "Flares"){
			// Remove
			if(SpecialObject.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f && IsLethal == true){
				IsLethal = false;
			}
			if(SpecialObject.GetComponent<AudioSource>().isPlaying == false && SpecialObject.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f && SpecialObject.transform.GetChild(1).GetComponent<ParticleSystem>().particleCount <= 0f){
				Destroy (this.gameObject);
			}
			// Remove
		}
		// Explosion
		
	}
}
