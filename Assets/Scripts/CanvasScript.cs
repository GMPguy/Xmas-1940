﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour {

	// References
	public GameScript GS;
    // While Playing
    public Camera MainCamera;
    public PlayerScript player;
	public GameObject PlayerHud;
	public Image HealthBar;
	public Image FuelBar;
	public Text Throttle;
	public Text Speed;
	public Text Altitude;
	public Image GunCooldown;
	public Image PresentCooldown;
	public Image SpecialCooldown;
	public Text AmmoText;
	public Image Crosshair;
	public Image PresentsHud;
	public Text CurrentLevelText;
	public Image SteeringCircle;
	public Text MooneyScore;
    public Image PIcon;
    public Text PText;
	// Flash
	public Image FlashImage;
	public float DisappearSpeed = 1f;
    // Flash
    float StartingTextDisplay = 5f;
    // Radar/Map
    public GameObject Radar;
    public GameObject Map;
    public GameObject AirplaneMark;
    public GameObject AAGunMark;
    public GameObject BalloonMark;
    public GameObject HomeMark;
    public float RadarDistance = 1000f;
    public float Refresh = 1f;
    // Radar/Map
	// While Playing
	public RoundScript RS;
	public Text Info;
	public GameObject Musics;
	// References

	// Misc
	GameObject CurrentMusic;
	// Misc

	// Use this for initialization
	void Start () {

        GS = GameObject.Find("GameScript").GetComponent<GameScript>();
        RS = GameObject.Find("RoundScript").GetComponent<RoundScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        MainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update () {

		Music ();

		// Whiles
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			PlayerHud.SetActive (true);
			Time.timeScale = 1f;
		} else {
			PlayerHud.SetActive (false);
		}
		// Whiles

		// While Playing
		if(player && PlayerHud.activeSelf == true){

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Map")) {
                if (RadarDistance == 100f) {
                    RadarDistance = 500f;
                } else if (RadarDistance == 500f) {
                    RadarDistance = 1000f;
                } else if (RadarDistance == 1000f) {
                    RadarDistance = 2000f;
                } else if (RadarDistance == 2000f) {
                    RadarDistance = 5000f;
                } else if (RadarDistance == 5000f) {
                    RadarDistance = 100f;
                }
            }

            if (StartingTextDisplay > 0f){
                StartingTextDisplay -= 0.01f * (Time.deltaTime * 100f);
                switch(GS.Level){
                    case 1:
                        SetInfoText("Deliver the presents!", "Dostarcz prezenty!", new Color32(255, 255, 255, 255), 1f);
                        break;
                    case 2:
                        SetInfoText("Watch out for the enemy airplanes!", "Uważaj na wrogie samoloty!", new Color32(255, 255, 255, 255), 1f);
                        break;
                    case 5:
                        SetInfoText("Watch out for the AA Guns, and the new Messerschmitt K4 planes!", "Uważaj na Bronie Przeciwlotnicze, i na nowe Messerschmitt K4", new Color32(255, 255, 255, 255), 1f);
                        break;
                    case 10:
                        SetInfoText("Watch out for the Balloons, and the new Messerschmitt 110 planes!", "Uważaj na Balony, i na nowe Messerschmitt 110!", new Color32(255, 255, 255, 255), 1f);
                        break;
                    case 20:
                        SetInfoText("Watch out for the new Messerschmitt Me 262! These are Jets!", "Uważaj na nowego Messerschmitt Me 262! To są odrzutowce!", new Color32(255, 255, 255, 255), 1f);
                        break;
                    default:
                        SetInfoText("Mission begins, good luck!", "Misja się zaczęła, powodzenia!", new Color32(255, 255, 255, 255), 1f);
                        break;
                }
            }

			HealthBar.fillAmount = player.Health / player.MaxHealth;
			HealthBar.transform.GetChild (0).GetComponent<Text> ().text = Mathf.Round(player.Health).ToString () + " / " + player.MaxHealth;
			FuelBar.fillAmount = player.Fuel / player.MaxFuel;
			FuelBar.transform.GetChild (0).GetComponent<Text> ().text = Mathf.Round(player.Fuel).ToString () + " / " + player.MaxFuel;
			Throttle.text = (Mathf.Round(player.Throttle * 100f)) + "%";
			Speed.text = GS.SetText("Speed:\n" + Mathf.Round(player.Speed).ToString() + " KM/H", "Prędkość:\n" + Mathf.Round(player.Speed).ToString() + " KM/H");
			Altitude.text = GS.SetText("Altitude:\n" + Mathf.Round(player.transform.position.y).ToString() + " MASL", "Wysokość:\n" + Mathf.Round(player.transform.position.y).ToString() + " NPM");
			MooneyScore.text = GS.SetText("Mooney: " + (GS.Mooney + RS.TempMooney) + "\nScore: " + (GS.CurrentScore + RS.TempScore), "Piniądze: " + (GS.Mooney + RS.TempMooney) + "\nWynik: " + (GS.CurrentScore + RS.TempScore));
            if (Input.GetButton("Map") && !Input.GetKey(KeyCode.LeftShift)) {
                RadarMap("Map");
            } else {
                RadarMap("Radar");
            }

			if(player.GunCooldown <= 0f && (player.Ammo > 0))
            {
				GunCooldown.fillAmount = 1f;
				GunCooldown.color = new Color32 (255, 255, 255, 255);
			} else {
				GunCooldown.fillAmount = 1f - (player.GunCooldown / player.MaxGunCooldown);
				GunCooldown.color = new Color32 (155, 155, 155, 255);
			}

			if(player.PresentCooldown <= 0f)
            {
				PresentCooldown.fillAmount = 1f;
				PresentCooldown.color = new Color32 (255, 255, 255, 255);
			} else {
				PresentCooldown.fillAmount = 1f - (player.PresentCooldown / player.MaxPresentCooldown);
				PresentCooldown.color = new Color32 (155, 155, 155, 255);
			}

			if(player.SpecialCooldown <= 0f && (player.Ammo >= player.SpecialRequiredAmmo)){
				SpecialCooldown.fillAmount = 1f;
				SpecialCooldown.color = new Color32 (255, 255, 255, 255);
			} else {
				SpecialCooldown.fillAmount = 1f - (player.SpecialCooldown / player.MaxSpecialCooldown);
				SpecialCooldown.color = new Color32 (155, 155, 155, 255);
			}

			AmmoText.text = player.Ammo.ToString();
			if((player.Stalling > 0f) || (player.Speed >= (player.MaxSpeed * 1.75f))){
				Speed.color = new Color32 (255, 0, 0, 255);
			} else {
				Speed.color = new Color32 (255, 255, 255, 255);
			}

			if (RS.GetComponent<RoundScript> ().State == "Deliver Presents") {
				PresentsHud.gameObject.SetActive(true);
				PresentsHud.transform.GetChild(0).GetComponent<Text>().text = GS.SetText( "Houses left:\n" + GameObject.FindGameObjectsWithTag ("HomeUnchecked").Length, "Pozostało:\n" + GameObject.FindGameObjectsWithTag ("HomeUnchecked").Length);
			} else {
				PresentsHud.gameObject.SetActive (false);
			}

			CurrentLevelText.text = GS.SetText ("Level: " + GS.Level, "Poziom: " + GS.Level);
			FlashImage.color = Color32.Lerp (FlashImage.color, new Color32((byte)FlashImage.color.r, (byte)FlashImage.color.g, (byte)FlashImage.color.b, 0), DisappearSpeed * (Time.unscaledDeltaTime * 100f));

            if (GS.Parachutes > 0) {
                PIcon.color = new Color32(255, 255, 255, 125);
                PText.text = GS.Parachutes.ToString();
                PText.color = new Color32(255, 255, 255, 125);
            } else {
                PIcon.color = new Color32(255, 0, 0, 125);
                PText.text = GS.Parachutes.ToString();
                PText.color = new Color32(255, 0, 0, 125);
            }

            // Cursors and steering wheel
            if(player.ControlType == "Point"){

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                SteeringCircle.rectTransform.anchoredPosition = Vector2.zero;
                SteeringCircle.GetComponent<Image>().color = new Color(1f,1f,1f, Vector3.Angle(player.transform.forward, player.PointThere.forward) / 10f);
			
                if(player.WhichCamera == "Turret"){
                    Crosshair.transform.position = MainCamera.WorldToScreenPoint(MainCamera.transform.position + MainCamera.transform.forward);
                    Crosshair.transform.localScale = Vector3.one;
                } else {
                    Crosshair.transform.position = MainCamera.WorldToScreenPoint(player.transform.position + player.transform.forward * 900f);
                    if(player.WhichCamera == "Aim") Crosshair.transform.localScale = Vector3.one;
                    else Crosshair.transform.localScale = Vector3.one/2f;
                }

            } else {

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;

                SteeringCircle.rectTransform.anchorMin = new Vector2 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
	    		SteeringCircle.rectTransform.anchorMax = new Vector2 (Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
    			SteeringCircle.rectTransform.anchoredPosition = new Vector2 (0f, 0f);
			
                if(player.WhichCamera == "Turret" || player.WhichCamera == "Aim"){
                    Crosshair.transform.position = MainCamera.WorldToScreenPoint(MainCamera.transform.position + MainCamera.transform.forward);
                    Crosshair.transform.localScale = Vector3.one;
                } else {
                    Crosshair.transform.position = MainCamera.WorldToScreenPoint(player.transform.position + player.transform.forward * 100f);
                    Crosshair.transform.localScale = Vector3.one/2f;
                }

            }

		}
		// While Playing
		
	}

    void RadarMap(string WhichOne){

        if (Refresh > 0f) {
            Refresh -= 1f;
        } else {
            Refresh = Mathf.Clamp(Mathf.Floor(RS.GetComponent<RoundScript>().Level / 10f), 1f, 100f);
        }

        if (WhichOne == "Radar") {
            Radar.transform.localScale = Vector3.Lerp(Radar.transform.localScale, new Vector3(1f, 1f, 1f), 0.25f * (Time.unscaledDeltaTime * 100f));
            Map.transform.localScale = Vector3.Lerp(Map.transform.localScale, new Vector3(0f, 0f, 0f), 0.25f * (Time.unscaledDeltaTime * 100f));

            if (Refresh == 0f) {
                foreach (Transform Obj in Radar.transform) {
                    if (Obj.name == "You") {
                        Obj.transform.localEulerAngles = new Vector3(0f, 0f, player.transform.eulerAngles.y * -1f);
                    } else if (Obj.name == "Radar") {
                        Obj.transform.localEulerAngles = new Vector3(0f, 0f, GameObject.Find("MainCamera").transform.eulerAngles.y * -1f);
                    } else if (Obj.name == "RadarDistanceText") {
                        Obj.GetComponent<Text>().text = RadarDistance + "m";
                    } else {
                        Destroy(Obj.gameObject);
                    }
                }
                List<GameObject> TheMarkings = new List<GameObject>();
                foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Foe")) {
                    if (Enemy.GetComponent<EnemyVesselScript>().Health > 0f) {
                        TheMarkings.Add(Enemy);
                    }
                }
                foreach (GameObject Home in GameObject.FindGameObjectsWithTag("HomeUnchecked")) {
                    TheMarkings.Add(Home);
                }
                if (GameObject.Find("Portal") != null) {
                    TheMarkings.Add(GameObject.Find("Portal"));
                }
                foreach (GameObject Marker in TheMarkings) {
                    if (Vector3.Distance(new Vector3(Marker.transform.position.x, 0f, Marker.transform.position.z), new Vector3(player.transform.position.x, 0f, player.transform.position.z)) < RadarDistance) {
                        GameObject PickMarker = null;
                        Color32 PickColor = new Color32(0, 0, 0, 0);
                        float MarkerRotation = 0f;
                        if (Marker.GetComponent<EnemyVesselScript>() != null) {
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt K4" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt 110" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt Me 262") {
                                PickMarker = AirplaneMark;
                                PickColor = new Color32(255, 0, 0, 255);
                                MarkerRotation = Marker.transform.eulerAngles.y * -1f;
                            }
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "AA Gun") {
                                PickMarker = AAGunMark;
                                PickColor = new Color32(255, 0, 0, 255);
                            }
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Balloon") {
                                PickMarker = BalloonMark;
                                PickColor = new Color32(255, 0, 0, 255);
                            }
                        } else if (Marker.GetComponent<HomeScript>() != null) {
                            PickMarker = HomeMark;
                            PickColor = new Color32(0, 255, 0, 255);
                        } else if (Marker.name == "Portal") {
                            PickMarker = HomeMark;
                            PickColor = new Color32(0, 255, 255, 255);
                        }
                        Vector3 DesiredPosition = Marker.transform.position - player.transform.position;
                        GameObject Mark = Instantiate(PickMarker) as GameObject;
                        Mark.transform.SetParent(Radar.transform);
                        Mark.transform.position = Radar.transform.position;
                        Mark.transform.localScale *= this.transform.localScale.y;
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0.5f + (DesiredPosition.x / (RadarDistance * 2f)), 0.5f + (DesiredPosition.z / (RadarDistance * 2f)));
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f + (DesiredPosition.x / (RadarDistance * 2f)), 0.5f + (DesiredPosition.z / (RadarDistance * 2f)));
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Mark.transform.GetChild(0).localEulerAngles = new Vector3(0f, 0f, MarkerRotation);
                        if (Vector3.Distance(new Vector3(Marker.transform.position.x, 0f, Marker.transform.position.z), new Vector3(player.transform.position.x, 0f, player.transform.position.z)) < (RadarDistance * 0.75f)) {
                            Mark.transform.GetChild(0).GetComponent<Image>().color = new Color(PickColor.r, PickColor.g, PickColor.b, 1f);
                        } else {
                            Mark.transform.GetChild(0).GetComponent<Image>().color = new Color(PickColor.r, PickColor.g, PickColor.b, 1f - (((Vector3.Distance(new Vector3(Marker.transform.position.x, 0f, Marker.transform.position.z), new Vector3(player.transform.position.x, 0f, player.transform.position.z)) / RadarDistance) - 0.75f) * 4f));
                        }
                    }
                }
            }
        } else if (WhichOne == "Map") {
            Radar.transform.localScale = Vector3.Lerp(Radar.transform.localScale, new Vector3(0f, 0f, 0f), 0.25f * (Time.unscaledDeltaTime * 100f));
            Map.transform.localScale = Vector3.Lerp(Map.transform.localScale, new Vector3(1f, 1f, 1f), 0.25f * (Time.unscaledDeltaTime * 100f));

            if (Refresh == 0f) {
                foreach (Transform Obj in Map.transform) {
                    Destroy(Obj.gameObject);
                }
                List<GameObject> TheMarkings = new List<GameObject>();
                foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Foe")) {
                    TheMarkings.Add(Enemy);
                }
                foreach (GameObject Home in GameObject.FindGameObjectsWithTag("HomeUnchecked")) {
                    TheMarkings.Add(Home);
                }
                if (GameObject.Find("Portal") != null) {
                    TheMarkings.Add(GameObject.Find("Portal"));
                }
                if (player != null) {
                    TheMarkings.Add(player.gameObject);
                }
                foreach (GameObject Marker in TheMarkings) {
                    if (Marker.transform.position.x > -2500f && Marker.transform.position.x < 2500f && Marker.transform.position.z > -2500f && Marker.transform.position.z < 2500f) {
                        GameObject PickMarker = null;
                        Color32 PickColor = new Color32(0, 0, 0, 0);
                        float MarkerRotation = 0f;
                        if (Marker.GetComponent<EnemyVesselScript>() != null) {
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt K4" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt 110" || Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Messerschmitt Me 262") {
                                PickMarker = AirplaneMark;
                                PickColor = new Color32(255, 0, 0, 255);
                                MarkerRotation = Marker.transform.eulerAngles.y * -1f;
                            }
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "AA Gun") {
                                PickMarker = AAGunMark;
                                PickColor = new Color32(255, 0, 0, 255);
                            }
                            if (Marker.GetComponent<EnemyVesselScript>().TypeofVessel == "Balloon") {
                                PickMarker = BalloonMark;
                                PickColor = new Color32(255, 0, 0, 255);
                            }
                        } else if (Marker.GetComponent<HomeScript>() != null) {
                            PickMarker = HomeMark;
                            PickColor = new Color32(0, 255, 0, 255);
                        } else if (Marker.name == "Portal") {
                            PickMarker = HomeMark;
                            PickColor = new Color32(0, 255, 255, 255);
                        } else if (Marker.GetComponent<PlayerScript>() != null) {
                            PickMarker = AirplaneMark;
                            PickColor = new Color32(255, 255, 255, 255);
                            MarkerRotation = Marker.transform.eulerAngles.y * -1f;
                        }
                        Vector3 DesiredPosition = Marker.transform.position;
                        GameObject Mark = Instantiate(PickMarker) as GameObject;
                        Mark.transform.SetParent(Map.transform);
                        Mark.transform.position = Map.transform.position;
                        Mark.transform.localScale *= this.transform.localScale.y;
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchorMin = new Vector2(0.5f + (DesiredPosition.x / 1250f), 0.5f + (DesiredPosition.z / 1250f));
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f + (DesiredPosition.x / 1250f), 0.5f + (DesiredPosition.z / 1250f));
                        Mark.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                        Mark.transform.GetChild(0).localEulerAngles = new Vector3(0f, 0f, MarkerRotation);
                        Mark.transform.GetChild(0).GetComponent<Image>().color = PickColor;
                    }
                }
            }

        }

    }

    void FixedUpdate()
    {

        if (PlayerHud.activeSelf == true)
        {
            // Info
            if (Info.color.a > 0 && (Info.transform.localScale.x > 0.49f && Info.transform.localScale.x < 0.51f))
            {
                Info.color = Color32.Lerp(Info.color, new Color32((byte)Info.color.r, (byte)Info.color.g, (byte)Info.color.b, 0), 0.01f);
            }
            if (Info.transform.localScale.x != 0.5f)
            {
                Info.transform.localScale = Vector3.Lerp(Info.transform.localScale, new Vector3(0.5f, 0.5f, 0.5f), 0.01f);
            }
            // Info
        }
    }

    void Music(){
		
		bool ChangeMusic = false;
		if(CurrentMusic == null){
			ChangeMusic = true;
		} else if(CurrentMusic.GetComponent<AudioSource>().isPlaying == false){
			ChangeMusic = true;
		}

		if(ChangeMusic == true){
			List<GameObject> MusicCandidates = new List<GameObject>();
			foreach(Transform Music in Musics.transform){
				if(Music.gameObject != CurrentMusic){
					MusicCandidates.Add(Music.gameObject);
				}
			}
			CurrentMusic = MusicCandidates.ToArray()[Random.Range(0, MusicCandidates.Count)];
			CurrentMusic.GetComponent<AudioSource> ().Play ();
		}

		if(GameObject.Find("MainPlane") == null){
			foreach(Transform Music in Musics.transform){
				Music.gameObject.GetComponent<AudioSource> ().Stop ();
			}
		}

	}

	public void SetInfoText(string English, string Polish, Color32 TextColor, float Scale){
		Info.transform.localScale = new Vector3 (Scale / 2f, Scale / 2f, Scale / 2f);
        Info.text = GS.SetText(English, Polish);
		Info.color = TextColor;
	}

}