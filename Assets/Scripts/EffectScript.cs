using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScript : MonoBehaviour {

	// Effect References
	public GameObject BulletHitPlane;
	public GameObject BullethitGround;
    public GameObject PaintballHit;
	// Effect References

	// main
	public string TypeofEffect = "None";
	// Main

	// Use this for initialization
	void Start () {

		if (TypeofEffect == "BullethitPlane") {
			BulletHitPlane.SetActive (true);
			BulletHitPlane.GetComponent<AudioSource> ().pitch = Random.Range (0.75f, 1.25f);
		} else if (TypeofEffect == "BullethitGround") {
			BullethitGround.SetActive (true);
			BullethitGround.GetComponent<AudioSource> ().pitch = Random.Range (0.75f, 1.25f);
		} else if (TypeofEffect == "PaintballHit") {
            PaintballHit.SetActive (true);
            PaintballHit.GetComponent<AudioSource> ().pitch = Random.Range (1.75f, 2f);
		}
		
	}
	
	// Update is called once per frame
	void Update () {

		if(TypeofEffect == "None"){
			Destroy (this.gameObject);
		} else if(TypeofEffect == "BullethitPlane"){
			if(BulletHitPlane.GetComponent<ParticleSystem>().particleCount <= 0f && BulletHitPlane.GetComponent<AudioSource>().isPlaying == false){
				Destroy (this.gameObject);
			}
		} else if(TypeofEffect == "BullethitGround"){
			if(BullethitGround.GetComponent<ParticleSystem>().particleCount <= 0f && BullethitGround.GetComponent<AudioSource>().isPlaying == false){
				Destroy (this.gameObject);
			}
		} else if(TypeofEffect == "PaintballHit"){
			if(PaintballHit.GetComponent<ParticleSystem>().particleCount <= 0f && PaintballHit.GetComponent<AudioSource>().isPlaying == false){
				Destroy (this.gameObject);
			}
		}
		
	}
}
