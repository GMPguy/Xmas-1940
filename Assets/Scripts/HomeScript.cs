using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScript : MonoBehaviour {

	// References
	public GameObject MarkerNear;
	public GameObject MarkerFar;
	public GameObject Player;
	// References

	// Main Variables
	public int HomeIndex = 1;
	public bool GotPresent = false;
	// Main Variables

	// Lamps
	int LampState = 1;
	float LampCooldown = 1f;
	// Lamps

	// Use this for initialization
	void Start () {

		// Place Down
		RaycastHit PlacementPoin;
		Ray PlaceDowm = new Ray (this.transform.position, Vector3.up * -1f);
		if(Physics.Raycast(PlaceDowm, out PlacementPoin, 1000f)){
			this.transform.position = PlacementPoin.point;
		}
		// Place Down
		this.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
		// Choose Position

		// Set Colors
		foreach (Material Mat in this.transform.GetChild(0).GetComponent<MeshRenderer>().materials) {
			if (Mat.name == "HomeColor (Instance)") {
				Mat.color = new Color32 ((byte)Random.Range(25, 255), (byte)Random.Range(25, 255), (byte)Random.Range(25, 255), (byte)255);
			}
		}
		// Set Colors
		
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.Find ("MainPlane")) {
			Player = GameObject.Find ("MainPlane");
		} else {
			Player = null;
		}

		if (Player != null && GotPresent == false) {
			if (Vector3.Distance (this.transform.position, Player.transform.position) <= Player.GetComponent<PlayerScript> ().PresentCannonDistane) {
			    MarkerNear.transform.GetChild (0).GetComponent<TextMesh> ().text = (((Vector3.Distance(this.transform.position, Player.transform.position)) / 1000f).ToString() + "000").Substring(0, 4) + "km";
				MarkerNear.SetActive (true);
				MarkerFar.SetActive (false);
			} else {
			    MarkerFar.transform.GetChild (0).GetComponent<TextMesh> ().text = (((Vector3.Distance(this.transform.position, Player.transform.position)) / 1000f).ToString() + "000").Substring(0, 4) + "km";
                MarkerNear.SetActive (false);
				MarkerFar.SetActive (true);
			}
		} else {
			MarkerNear.SetActive (false);
			MarkerFar.SetActive (false);
		}

		if(GotPresent == false){
			this.tag = "HomeUnchecked";
		} else if(GotPresent == true){
			this.tag = "HomeChecked";
		}

		// Lamps
		if (LampCooldown > 0f) {
			LampCooldown -= 0.01f * (Time.deltaTime * 100f);
		} else {
			LampCooldown = 1f;
			if(LampState != 4){
				LampState += 1;
			} else {
				LampState = 1;
			}
		}
		foreach(Material Mat in this.transform.GetChild(0).GetComponent<MeshRenderer>().materials){
			if(Mat.name == "Lamp1 (Instance)"){
				if(LampState == 1){
					Mat.color = new Color32 (0, 255, 0, 255); // green
				} else if(LampState == 2){
					Mat.color = new Color32 (255, 0, 0, 255); // red
				} else if(LampState == 3){
					Mat.color = new Color32 (0, 255, 255, 255); // blue
				} else if(LampState == 4){
					Mat.color = new Color32 (255, 255, 0, 255); // yellow
				}
			} else if(Mat.name == "Lamp2 (Instance)"){
				if(LampState == 1){
					Mat.color = new Color32 (255, 0, 0, 255); // red
				} else if(LampState == 2){
					Mat.color = new Color32 (0, 255, 255, 255); // blue
				} else if(LampState == 3){
					Mat.color = new Color32 (255, 255, 0, 255); // yellow
				} else if(LampState == 4){
					Mat.color = new Color32 (0, 255, 0, 255); // green
				}
			} else if(Mat.name == "Lamp3 (Instance)"){
				if(LampState == 1){
					Mat.color = new Color32 (0, 255, 255, 255); // blue
				} else if(LampState == 2){
					Mat.color = new Color32 (255, 255, 0, 255); // yellow
				} else if(LampState == 3){
					Mat.color = new Color32 (0, 255, 0, 255); // green
				} else if(LampState == 4){
					Mat.color = new Color32 (255, 0, 0, 255); // red
				}
			} else if(Mat.name == "Lamp4 (Instance)"){
				if(LampState == 1){
					Mat.color = new Color32 (255, 255, 0, 255); // yellow
				} else if(LampState == 2){
					Mat.color = new Color32 (0, 255, 0, 255); // green
				} else if(LampState == 3){
					Mat.color = new Color32 (255, 0, 0, 255); // red
				} else if(LampState == 4){
					Mat.color = new Color32 (0, 255, 255, 255); // blue
				}
			}
		}
		// Lamps
		
	}

	public void PresentGot(){

		if(GotPresent == false){
			GotPresent = true;
			if(Player != null){
				GameObject.Find ("GameScript").GetComponent<GameScript> ().GainScore (10, 50, "");
				Player.GetComponent<PlayerScript> ().MainCanvas.GetComponent<CanvasScript> ().FlashImage.color = new Color32 (0, 155, 0, 255);
				Player.GetComponent<PlayerScript> ().MainCanvas.GetComponent<CanvasScript> ().DisappearSpeed = 0.01f;
				Player.GetComponent<PlayerScript> ().MainCanvas.GetComponent<CanvasScript> ().SetInfoText ("Present Delivered", "Prezent Dostarczony!", new Color32(0, 225, 0, 255), 2f);
				Player.GetComponent<PlayerScript> ().Health += Random.Range (Player.GetComponent<PlayerScript> ().MaxHealth / 8f, Player.GetComponent<PlayerScript> ().MaxHealth / 4f);
				Player.GetComponent<PlayerScript> ().Ammo += Random.Range (Player.GetComponent<PlayerScript> ().MaxAmmo / 8, Player.GetComponent<PlayerScript> ().MaxAmmo / 4);
				Player.GetComponent<PlayerScript> ().Fuel += (Player.GetComponent<PlayerScript> ().MaxFuel / 4f) * GameObject.Find("GameScript").GetComponent<GameScript>().DifficultyLevel;
			}
		}

	}

}
