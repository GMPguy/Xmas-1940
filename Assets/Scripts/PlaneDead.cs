using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDead : MonoBehaviour {

	public bool isMine = false;
	public GameObject PlaneModel;
    public GameObject Parachuter;
	public Vector3 PreviousRotation;
	public float PreviousSpeed = 0f;
	float decay = 1f;
	bool Fell = false;
	public GameObject Effect;
	public string Paint = "Basic Paint";
	float GoBackToMenu = 5f;
    public bool IsGameOver = false;
    int FreeTheParachuter = 50;

	// Use this for initialization
	void Start () {

        if (isMine && IsGameOver == false) {
            GameObject.Find("GameScript").GetComponent<GameScript>().GetComponent<GameScript>().SetGameOptions("Save", "TEST");
        } else if (isMine && IsGameOver){
			GameObject.Find("GameScript").GetComponent<GameScript>().GetComponent<GameScript>().SetGameOptions("Empty", "TEST");
			GameObject.Find("GameScript").GetComponent<GameScript>().GetComponent<GameScript>().SetGameOptions("Erase", "TEST");
		} else if (!isMine){
			int[] maxDeads = new int[]{1, 4, 11, 101};
			if(GameObject.FindGameObjectsWithTag("DeadPlane").Length >= maxDeads[QualitySettings.GetQualityLevel()]) 
				Destroy(GameObject.FindGameObjectsWithTag("DeadPlane")[0]);
		}

		foreach(Transform cleanP in PlaneModel.transform){
			if(cleanP.name == "Propeller" || cleanP.name == "Basic Propeller" || cleanP.name == "Double Propeller" || cleanP.name == "Jet Engine" || cleanP.name == "Double Jet Engine" || cleanP.name == "Magic Reindeer Dust"){
				Destroy(cleanP.gameObject);
			}
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if ((isMine && IsGameOver == false) || (!isMine && Time.timeSinceLevelLoad % 1f < 0.3f && this.transform.position.y > 100f)) {
            if (FreeTheParachuter > 1) {
                FreeTheParachuter -= 1;
            } else if(FreeTheParachuter == 1) {
                Parachuter.transform.parent = null;
                Parachuter.transform.position = this.transform.position;
                Parachuter.GetComponent<AudioSource>().Play();
                FreeTheParachuter = 0;
            } else {
                if (Parachuter.transform.position.y > 0f) {
                    Parachuter.transform.localScale = Vector3.Slerp(Parachuter.transform.localScale, new Vector3(5f, 5f, 5f), 0.5f);
                    Parachuter.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
                    Parachuter.transform.Translate(0f, 0f, -0.25f);
                } else {
                    Parachuter.transform.localScale = Vector3.Slerp(Parachuter.transform.localScale, new Vector3(5f, 5f, 0.5f), 0.5f);
                }
            }
        }

		if(isMine){
			GameObject.Find ("MainCamera").transform.LookAt (this.transform.position, this.transform.up * 1f);
        	if (GoBackToMenu > 0f) {
				GoBackToMenu -= 0.01f;
			} else {
            	if (IsGameOver == true) {
        	        GameObject.Find("GameScript").GetComponent<GameScript>().LoadLevel("MainMenu");
    	            GameObject.Find("GameScript").GetComponent<GameScript>().WhichMenuWindowToLoad = "GameOver";
	            } else {
            	    GameObject.Find("GameScript").GetComponent<GameScript>().LoadLevel("MainMenu");
        	        GameObject.Find("GameScript").GetComponent<GameScript>().WhichMenuWindowToLoad = "CampaignMessage";
    	        }
			}
		}

		if (Fell == false) {
			this.transform.position += this.transform.forward * (PreviousSpeed /90f);
			this.transform.Rotate (PreviousRotation * decay);
			this.transform.eulerAngles += new Vector3 (1f - decay, 0f, 0f);
			this.transform.position += Vector3.down * (5f - (decay * 5f));
			decay = Mathf.MoveTowards (decay, 0f, 0.0005f);
			if(this.transform.position.y <= 1f){
				Fell = true;
				decay = 0f;
				for(int Ground = Random.Range(3, 5); Ground > 0; Ground --){
					GameObject Smoke = Instantiate (Effect) as GameObject;
					Smoke.GetComponent<EffectScript> ().TypeofEffect = "BullethitGround";
					Smoke.transform.position = new Vector3 (this.transform.position.x + Random.Range (-1f, 1f), 0f, this.transform.position.z + Random.Range (-1f, 1f));
				}
			}
		} else {
			this.transform.position = new Vector3 (this.transform.position.x, 0f, this.transform.position.z);
		}
		
	}
}
