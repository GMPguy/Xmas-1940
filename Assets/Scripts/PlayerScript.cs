using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour {

	// References
	public GameScript GS;
	public RoundScript RS;
	public GameObject PlaneModels;
	public GameObject CurrentPlane;
	public GameObject Camera;
	public GameObject AimPart;
    public GameObject Turret;
	public GameObject Projectile;
	public List<GameObject> Guns;
	public ParticleSystem Smoke;
	public ParticleSystem Sparkles;
    public ParticleSystem Wind;
    public ParticleSystem NearGroundSnow;
	public GameObject Alarms;
	public GameObject PresentCannon;
	public GameObject MainCanvas;
	public GameObject PlaneDead;
	public GameObject Effect;
	public GameObject Special;
    public GameObject FreeLook;
	// References

	// Plane options
	public string PlaneModel = "BP Mark.I";
	public string EngineModel = "Basic Propeller";
	public string GunType = "Vickers";
	public string PresentCannonType = "Slingshot";
	public string SpecialType = "None";
	public string Addition = "None";
	public string Paint = "Basic Paint";
	// Plane options

	// Stats
	public float MaxHealth = 100f;
	public float Health = 100f;

	public float MaxFuel = 180f;
	public float Fuel = 180f;

	public float Heat = 0f;
	public float MaxHeat = 1f;
	public float HeatDispare = 1f;
	public float MaxGunCooldown = 1f;
	public float GunCooldown = 0f;
	float AmountOfGuns = 0f;

	public float MaxPresentCooldown = 10f;
	public float PresentCooldown = 0f;

	public float MaxSpecialCooldown = 10f;
	public float SpecialCooldown = 0f;
	public int SpecialRequiredAmmo = 0;

	public float MaxSpeed = 100f;
	public float Speed = 0f;
	public float RotationSpeed = 1f;

	public string AirplaneClass;
	// Stats

	// Mechanics
	public Vector3 ElevatorFlapsRudder;
	public Vector2 SteerPosition;
	public float Throttle = 1f;
	public float Inverted = -1f;
	public float Stalling = 0f;
	public string ControlType = "Point";
	public Transform PointThere;
	Vector2 PTturn;
    public string WhichCamera = "Normal";
	Vector3 FlyingTowards;
	public Vector3 PrevPos;
	// Mechanics

	// Screen Shake
	public float ShakePower = 0f;
	public float ShakeDecay = 1f;
	Vector3 ShakeScreen;
	// Screen Shake

	// Misc
	public Color32 PresentColor1;
	public Color32 PresentColor2;
	public float AlarmCooldown = 0f;
	public float GunDistane = 0f;
	public float PresentCannonDistane = 0f;
	public float Desertion = 5f;
	float DesertionBoom = 0f;
	public Color32 PlaneColor1;
	public Color32 PlaneColor2;
	bool BeyondMap = false;
	public float Flares = 0f;
	public float Intro = 5f;
	int IntroZoom = 0;
	// Misc

	void Start () {

		GS = GameObject.Find("GameScript").GetComponent<GameScript>();
		RS = GameObject.Find("RoundScript").GetComponent<RoundScript>();

		PlaneModel = GS.CurrentPlaneModel;
		EngineModel = GS.CurrentEngineModel;
		GunType = GS.CurrentGunType;
		PresentCannonType = GS.CurrentPresentCannonType;
		SpecialType = GS.CurrentSpecialType;
		Addition = GS.CurrentAddition;
		Paint = GS.CurrentPaint;

		if (GS.InvertedMouse == true) Inverted = 1f;
		if (GS.ControlScheme == 1) ControlType = "Point";

		MainCanvas = GameObject.Find ("MainCanvas");
		PlaneSettings (1);
		SetStats ();

		PointThere.SetParent(null);
		IntroZoom = 0;//(int)Random.Range(0f, 2.9f);
		
	}

	void FixedUpdate () {

		PlaneSettings (0);
		if(Intro > 0f){
			Intro -= 0.02f;
		} else {
			Mechanics ();
			Controlling ();
		}

		// Values
		Health = Mathf.Clamp(Health, 0f, MaxHealth);
		Fuel = Mathf.Clamp(Fuel, 0f, MaxFuel);
		if(Heat > 0f){
			if(GunCooldown <= -0.9f) {
				float mainheat = Heat;
				Heat = Mathf.Clamp (Heat - (0.02f*(MaxHeat/HeatDispare)), 0f, MaxHeat*2f);
			}
			if(Heat > MaxHeat) {
				Heat = -MaxHeat - 10f;
				MainCanvas.GetComponent<CanvasScript> ().SetInfoText ("Gun overheated!", "Broń się przegrzała!", new Color32 (255, 0, 0, 255), 1.5f);
			}
		} else if (Heat <= -10f) {
			Heat = Mathf.Clamp (Heat + 0.02f*(MaxHeat/HeatDispare), -Mathf.Infinity, -10f);
			if(Heat >= -10f) Heat = 0f;
		}
        float LowFuelMultiplier;
        if (Fuel > (MaxFuel / 5f)) {
            if (Addition == "Boost") LowFuelMultiplier = 1.5f;
            else LowFuelMultiplier = 1f;
		} else {
            if (Addition == "Boost") LowFuelMultiplier = Fuel / MaxFuel / 5f * 1.5f;
            else LowFuelMultiplier = Fuel / (MaxFuel / 5f);
		}

		if (Health > (MaxHealth / 3f)) Throttle = Mathf.Clamp (Throttle, 0.004f,LowFuelMultiplier);
		else if ((Health / MaxHealth) <= (Fuel / MaxFuel)) Throttle = Mathf.Clamp (Throttle, 0.004f, (0.75f + (Health / MaxHealth * 0.25f)) * LowFuelMultiplier);

        if (Addition == "Boost") {
            if (Throttle > 1f) {
                ShakePower = (Throttle - 1f) * 0.5f;
                ShakeDecay = 0.1f;
                if (!Input.GetButton("Throttle")) Throttle -= 0.01f;
            }
        }

		if(GunCooldown > -1f)GunCooldown -= 0.02f;
		if(Flares > 0f) Flares -= 0.01f;
		if(PresentCooldown > 0f){
			if (AirplaneClass == "Bomber") {
				PresentCooldown -= 0.03f;
			} else {
				PresentCooldown -= 0.01f;
			}
		}
		if(SpecialCooldown > 0f) SpecialCooldown -= 0.01f;
		if(AlarmCooldown > 0f) AlarmCooldown -= 0.01f;
		// Values

		// Camera
		if(Intro <= 0f){
			if (ControlType == "Point") {

				if (Input.GetButton("Free Look")) {
	            	WhichCamera = "Free";
				} else if (Input.GetButton("Aim")) {
	    	        if (Addition == "Turret"){
    	    	        WhichCamera = "Turret";
					} else {
						WhichCamera = "Aim";
					}
				} else {
					WhichCamera = "Normal";
				}

				Camera.transform.position = this.transform.position + this.transform.up * 2f + PointThere.forward*-15f + ShakeScreen;
				Camera.transform.LookAt(PointThere.position + PointThere.forward*1000f);
				Cursor.lockState = CursorLockMode.Locked;

			} else {

				if (Input.GetButton("Free Look") || Quaternion.Angle(FreeLook.transform.localRotation, Quaternion.identity) > 1f) {
	    	        WhichCamera = "Free";

    	    	    // Rotate Camera
	            	Camera.transform.position = FreeLook.transform.position + this.transform.up*2f + (FreeLook.transform.forward * -15f);
	            	Camera.transform.LookAt (FreeLook.transform.position + FreeLook.transform.forward * 1000f, this.transform.up);
    	    	    if (Input.GetButton("Free Look")) FreeLook.transform.localRotation = Quaternion.Lerp(FreeLook.transform.localRotation, Quaternion.Euler(-90f * SteerPosition.y, 180f * SteerPosition.x, 0f), 0.1f);
        		    else FreeLook.transform.localRotation = Quaternion.Lerp(FreeLook.transform.localRotation, Quaternion.identity, 0.5f);
					// Rotate Camera
				} else if(Input.GetButton("Aim") && Addition == "Turret"){
        		    WhichCamera = "Turret";
        	        Camera.transform.position = Turret.transform.GetChild(0).position + (ShakeScreen / 10f);
    	        	Camera.transform.rotation = Turret.transform.GetChild(0).rotation;
		            Turret.transform.GetChild(0).localRotation = Quaternion.Euler(Mathf.Clamp((-60f * SteerPosition.y), -90f, 30f), 200f * SteerPosition.x, 0f);
	            	Turret.transform.GetChild(3).rotation = Quaternion.Slerp(Turret.transform.GetChild(3).rotation, Turret.transform.GetChild(0).rotation, 0.25f);

	        	    Turret.transform.GetChild(3).gameObject.SetActive(true);
    		        Turret.transform.GetChild(1).gameObject.SetActive(false);
    	            Turret.transform.GetChild(2).gameObject.SetActive(true);
				} else {
					if(Input.GetButton("Aim")) WhichCamera = "Aim";
    		        else WhichCamera = "Normal";
					float divASC = (Camera.GetComponent<Camera> ().fieldOfView-30f) / 30f;
	        	    Vector3 AddSteerCamera = this.transform.right * ((ElevatorFlapsRudder.z - ElevatorFlapsRudder.y) * -2f) + this.transform.up * (ElevatorFlapsRudder.x * -2f);
					Camera.transform.position = AddSteerCamera*divASC + (this.transform.position + ShakeScreen) + (this.transform.forward * -5f) + (this.transform.forward * (Speed / MaxSpeed) * -10f) + (this.transform.up * Mathf.Lerp(4f, 2f, divASC) );
					//Camera.transform.LookAt (Camera.transform.position + (this.transform.forward * 1f), this.transform.up * 1f);
					MainCanvas.GetComponent<CanvasScript> ().Crosshair.transform.localScale = Vector3.one*0.1f;
					Camera.transform.LookAt (FreeLook.transform.position + FreeLook.transform.forward * 1000f, this.transform.up);
				}

			}
		} else {

			switch(IntroZoom){
				case 0:
					Camera.transform.LookAt(this.transform.position + this.transform.up*2f + this.transform.forward*10f);
					Camera.transform.position = this.transform.position + this.transform.up*2f + this.transform.forward*-Mathf.Lerp(15f, -500f, Intro*Intro/25f) + this.transform.right*Mathf.Sin(Intro*Intro/25f)*250f;
					Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(60f, 10f, Intro/5f);
					break;
				case 1:
					Camera.transform.LookAt(this.transform.position + this.transform.up*2f + this.transform.forward*1000f);
					Camera.transform.position = this.transform.position + this.transform.up*Mathf.Lerp(2f, 10f, Intro/5f) + this.transform.forward*Mathf.Lerp(-15f, 5f, Intro/5f);
					Camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(60f, 10f, Intro/5f);
					break;
			}

		}

        float DesiredPOV;
        if (WhichCamera == "Aim" || WhichCamera == "Turret") {
			if (Addition == "Zoom") DesiredPOV = 10f;
			else DesiredPOV = 30f;
		} else {
			if(WhichCamera == "Free") DesiredPOV = 80f;
			else DesiredPOV = 60f;
            if (Addition == "Turret" && WhichCamera != "Turret") {
                Turret.transform.GetChild(3).gameObject.SetActive(false);
                Turret.transform.GetChild(1).gameObject.SetActive(true);
                Turret.transform.GetChild(2).gameObject.SetActive(false);
            }
		}
		Camera.GetComponent<Camera> ().fieldOfView = Mathf.Lerp(Camera.GetComponent<Camera> ().fieldOfView , DesiredPOV, 0.1f);
		// Camera

		// Visual Effects
		if (Health <= (MaxHealth / 3f)) {
			ParticleSystem myParticleSystem;
			ParticleSystem.EmissionModule emissionModule;
			myParticleSystem = Smoke.GetComponent<ParticleSystem> ();
			emissionModule = myParticleSystem.emission;
			emissionModule.rateOverTime = 200f - ((Health / (MaxHealth / 3f)) * 200f);
			Smoke.GetComponent<AudioSource> ().volume = (1f - (Health / (MaxHealth / 3f) * 1f)) * GS.Volumes[0]*GS.Volumes[2];
			Smoke.GetComponent<AudioSource> ().pitch = Random.Range (0.8f, 1.2f);
		} else {
			ParticleSystem myParticleSystem;
			ParticleSystem.EmissionModule emissionModule;
			myParticleSystem = Smoke.GetComponent<ParticleSystem> ();
			emissionModule = myParticleSystem.emission;
			emissionModule.rateOverTime = 0f;
			Smoke.GetComponent<AudioSource> ().volume = 0f;
		}

		if(Health > (MaxHealth / 5f)){
			Sparkles.GetComponent<ParticleSystem> ().Stop ();
		} else {
			Sparkles.GetComponent<ParticleSystem> ().Play ();
		}

		if((Health <= (MaxHealth / 5f)) || (Fuel <= (MaxFuel / 5f)) || BeyondMap == true){
			if(AlarmCooldown <= 0f){
				AlarmCooldown = 0.25f;
				Alarms.GetComponent<AudioSource> ().Play ();
				Alarms.GetComponent<Light> ().intensity = 5f;
			}
		} else if((Health < (MaxHealth / 3f)) || (Fuel < (MaxFuel / 3f))){
			if(AlarmCooldown <= 0f){
				AlarmCooldown = 2f;
				Alarms.GetComponent<AudioSource> ().Play ();
				Alarms.GetComponent<Light> ().intensity = 5f;
			}
		}
		if(Alarms.GetComponent<Light> ().intensity > 0f){
			Alarms.GetComponent<Light> ().intensity -= 0.1f;
		}

		if (PresentCooldown > 0f) {
			PresentCannon.transform.localScale = Vector3.zero;
			PresentColorGenerator ();
		} else {
			PresentCannon.transform.localScale = Vector3.Lerp (PresentCannon.transform.localScale, new Vector3(1f, 1f, 1f), 0.5f);;
			foreach(Material Mat in PresentCannon.transform.GetChild (0).GetComponent<MeshRenderer> ().materials){
				if(Mat.name == "Material1 (Instance)")Mat.color = PresentColor1;
				else if(Mat.name == "Material2 (Instance)")Mat.color = PresentColor2;
			}
		}

        float WindAlpha = Speed / MaxSpeed * GameObject.Find("RoundScript").GetComponent<RoundScript>().SnowIntensity;
		ParticleSystem.MainModule WC = Wind.main;
        WC.startColor = new Color(1f, 1f, 1f, WindAlpha * 0.001f);
		Wind.transform.eulerAngles = PointThere.eulerAngles;

		ParticleSystem.MainModule NGS = NearGroundSnow.main;
        if (this.transform.position.y > 100f) NGS.startColor = new Color(1f, 1f, 1f, 0f);
        else NGS.startColor = new Color(1f, 1f, 1f, (1f - (this.transform.position.y / 100f)) * 0.05f);
        Vector3 PlaneP = this.transform.position + (this.transform.forward * (Speed / 10f));
        NearGroundSnow.transform.position = new Vector3(PlaneP.x, 1f, PlaneP.z);
        NearGroundSnow.transform.eulerAngles = new Vector3(-90f, 0f, 0f);
		// Visual Effects

		// Screen Shake
		ShakePower = Mathf.MoveTowards(ShakePower, 0f, ShakeDecay);
		ShakeScreen = new Vector3 (Random.Range(-ShakePower, ShakePower), Random.Range(-ShakePower, ShakePower), Random.Range(-ShakePower, ShakePower));
		// Screen Shake

		// Dead
		if(Health <= 0f){

			GS.Parachutes -= 1;
            GameObject Boom = Instantiate(Special) as GameObject;
            Boom.GetComponent<SpecialScript>().TypeofSpecial = "Explosion";
            Boom.GetComponent<SpecialScript>().ExplosionPower = 1f;
            Boom.GetComponent<SpecialScript>().ExplosionRadius = 2f;
            Boom.transform.position = this.transform.position;

            GameObject Corpse = Instantiate(PlaneDead) as GameObject;
            Corpse.transform.position = this.transform.position;
            Corpse.transform.rotation = this.transform.rotation;
            Corpse.GetComponent<PlaneDead>().IsGameOver = false;
            Corpse.GetComponent<PlaneDead>().isMine = true;
            Corpse.GetComponent<PlaneDead>().PreviousSpeed = Speed;
            Corpse.GetComponent<PlaneDead>().PreviousRotation = ElevatorFlapsRudder;
			CurrentPlane.transform.SetParent(Corpse.transform);
			Corpse.GetComponent<PlaneDead>().PlaneModel = CurrentPlane;
            Destroy(this.gameObject);

            if (GS.Parachutes <= 0){
                GS.PreviousScore = GS.CurrentScore;
                GS.PreviousPlane = PlaneModel;
                if (GS.PreviousScore > GS.HighScore)
                    GS.HighScore = GS.PreviousScore;
            } else {
                GS.HasDied = true;
            }
		}
		// Dead

		// Beyond Map
		if (this.transform.position.x < RS.MapSize/-2f || this.transform.position.x > RS.MapSize/2f || this.transform.position.z < RS.MapSize/-2f || this.transform.position.z > RS.MapSize/2f) {
			BeyondMap = true;
			if (Desertion > 0f) {
				MainCanvas.GetComponent<CanvasScript> ().SetInfoText ("You're leaving the map! Go back, now!\n " + Mathf.Round (Desertion) + " seconds", "Opuszczasz mapę! Wracaj natychmiast!\n " + Mathf.Round (Desertion) + " sekundy", new Color32 (255, 0, 0, 255), 1f);
				Desertion -= 0.01f;
			} else {
				MainCanvas.GetComponent<CanvasScript> ().SetInfoText ("You'll be taken down for desertion", "Zostaniesz zdjęty za dezercję", new Color32 (255, 0, 0, 255), 1f);
				if (DesertionBoom > 0f) {
					DesertionBoom -= 0.01f;
				} else {
					DesertionBoom = Random.Range (0.05f, 0.5f);
					GameObject Boom = Instantiate (Special) as GameObject;
					Boom.GetComponent<SpecialScript>().TypeofSpecial = "Explosion";
					Boom.GetComponent<SpecialScript> ().ExplosionPower = 100f;
					Boom.transform.position = this.transform.position + new Vector3 (Random.Range(-6f ,6f), Random.Range(-6f ,6f), Random.Range(-6f ,6f));
				}
			}
		} else {
			BeyondMap = false;
			if (Desertion < 5f) {
				Desertion = 5f;
			}
		}
		// Beyond Map

		// Fuel
		if(Throttle > 0f && EngineModel != "Magic Reindeer Dust")
			Fuel -= Throttle / 100f * (float)GS.DifficultyLevel;
		// Fuel

	}

	void PlaneSettings (int DoOnce) {


		foreach(Transform SelectedPlane in PlaneModels.transform){
			if (SelectedPlane.gameObject.name == PlaneModel) {
				CurrentPlane = SelectedPlane.gameObject;
				foreach(Material Mat in SelectedPlane.GetChild(0).GetComponent<MeshRenderer>().materials){
					if(Mat.name == "PlaneColor1 (Instance)"){
						Mat.color = PlaneColor1;
					} else if(Mat.name == "PlaneColor2 (Instance)"){
						Mat.color = PlaneColor2;
					}
				}
			} else {
				SelectedPlane.gameObject.SetActive (false);
			}
		}

		if(CurrentPlane != null){
			foreach (Transform SelectedPart in CurrentPlane.transform) {
				switch(SelectedPart.name){
	                case "AimPart":
                    	AimPart = SelectedPart.gameObject;
						break;
            	    case "Turret":
        	            if (Addition == "Turret") {
    	                    SelectedPart.gameObject.SetActive(true);
	                        Turret = SelectedPart.gameObject;
                        	foreach (Material Mat in Turret.transform.GetChild(1).GetComponent<MeshRenderer>().materials) {
                    	        if (Mat.name == "PlaneColor1 (Instance)")
                	                Mat.color = PlaneColor1;
            	                else if (Mat.name == "PlaneColor2 (Instance)")
        	                        Mat.color = PlaneColor2;
    	                    }
	                        foreach (Material Mat in Turret.transform.GetChild(2).GetComponent<MeshRenderer>().materials) {
                            	if (Mat.name == "PlaneColor1 (Instance)")
                        	        Mat.color = PlaneColor1;
                    	        else if (Mat.name == "PlaneColor2 (Instance)")
                	                Mat.color = PlaneColor2;
            	            }
        	            } else {
    	                    SelectedPart.gameObject.SetActive(false);
	                    }
						break;
                	case "PresentCannon":
						PresentCannon = SelectedPart.gameObject;
						break;
					case "Basic Propeller":
						if (EngineModel == "Basic Propeller") {
							SelectedPart.gameObject.SetActive (true);
							SelectedPart.transform.Rotate (new Vector3 (Speed / -10f, 0f, 0f));
							SelectedPart.gameObject.GetComponent<AudioSource> ().pitch = (Speed / MaxSpeed / 2f) * Throttle;
							SelectedPart.gameObject.GetComponent<AudioSource> ().volume = GS.Volumes[0] * GS.Volumes[2];
						} else {
							SelectedPart.gameObject.SetActive (false);
							SelectedPart.gameObject.GetComponent<AudioSource> ().Stop ();
						}
						break;
					case "Double Propeller":
						if (EngineModel == "Double Propeller") {
							SelectedPart.gameObject.SetActive (true);
							foreach(Material Mat in SelectedPart.GetComponent<MeshRenderer>().materials){
								if(Mat.name == "PlaneColor1 (Instance)")
									Mat.color = PlaneColor1;
								else if(Mat.name == "PlaneColor2 (Instance)")
									Mat.color = PlaneColor2;
							}
							SelectedPart.transform.GetChild(0).Rotate (new Vector3 (Speed / -10f, 0f, 0f));
        	                SelectedPart.gameObject.GetComponent<AudioSource>().pitch = (Speed / MaxSpeed / 2f) * Throttle;
							SelectedPart.gameObject.GetComponent<AudioSource> ().volume = GS.Volumes[0] * GS.Volumes[2];
    	                } else {
							SelectedPart.gameObject.SetActive (false);
							SelectedPart.gameObject.GetComponent<AudioSource> ().Stop ();
						}
						break;
					case "Jet Engine":
						if (EngineModel == "Jet Engine") {
							SelectedPart.gameObject.SetActive (true);
							foreach(Material Mat in SelectedPart.GetComponent<MeshRenderer>().materials){
								if(Mat.name == "PlaneColor1 (Instance)")
									Mat.color = PlaneColor1;
								else if(Mat.name == "PlaneColor2 (Instance)")
									Mat.color = PlaneColor2;
							}
							ParticleSystem.MainModule JE = SelectedPart.transform.GetChild(0).GetComponent<ParticleSystem> ().main;
							JE.startColor = new Color(1f, 1f, 1f, (Mathf.Clamp((Speed / MaxSpeed) * (0.5f * Throttle), 0f, 255f)));
							SelectedPart.gameObject.GetComponent<AudioSource> ().pitch = Speed / MaxSpeed * Throttle;
							SelectedPart.gameObject.GetComponent<AudioSource> ().volume = GS.Volumes[0] * GS.Volumes[2];
						} else {
							SelectedPart.gameObject.SetActive (false);
							SelectedPart.gameObject.GetComponent<AudioSource> ().Stop ();
						}
						break;
					case "Double Jet Engine":
						if (EngineModel == "Double Jet Engine") {
							SelectedPart.gameObject.SetActive (true);
							foreach(Material Mat in SelectedPart.GetComponent<MeshRenderer>().materials){
								if(Mat.name == "PlaneColor1 (Instance)"){
									Mat.color = PlaneColor1;
								} else if(Mat.name == "PlaneColor2 (Instance)"){
									Mat.color = PlaneColor2;
								}
							}
							ParticleSystem.MainModule DJE = SelectedPart.transform.GetChild(0).GetComponent<ParticleSystem> ().main;
							DJE.startColor = new Color(1f, 1f, 1f, (Mathf.Clamp((Speed / MaxSpeed) * (0.5f * Throttle), 0f, 255f)));
        	                SelectedPart.gameObject.GetComponent<AudioSource> ().pitch = Speed / MaxSpeed * Throttle;
							SelectedPart.gameObject.GetComponent<AudioSource> ().volume = GS.Volumes[0] * GS.Volumes[2];
						} else {
							SelectedPart.gameObject.SetActive (false);
							SelectedPart.gameObject.GetComponent<AudioSource> ().Stop ();
						}
						break;
					case "Magic Reindeer Dust":
						if (EngineModel == "Magic Reindeer Dust") {
							SelectedPart.gameObject.SetActive (true);
							float SetPitch = Mathf.Clamp ((Speed / MaxSpeed) * Throttle, 0.75f, 2f);
							SelectedPart.gameObject.GetComponent<AudioSource> ().pitch = SetPitch;
							SelectedPart.gameObject.GetComponent<AudioSource> ().volume = GS.Volumes[0] * GS.Volumes[2];
						} else {
							SelectedPart.gameObject.SetActive (false);
							SelectedPart.gameObject.GetComponent<AudioSource> ().Stop ();
						}
						break;
					case "Gun":
						if(DoOnce == 1){
							Guns.Add (SelectedPart.gameObject);
							AmountOfGuns += 1;
						}
						break;
				}
			}
		}

	}

	void Mechanics () {

		// Set speed
		// Set X Angle
		float AngleX = 1f;
		if( this.transform.eulerAngles.x >= 0f &&  this.transform.eulerAngles.x <= 90f) AngleX = 0.5f + ((this.transform.eulerAngles.x / 90f) / 2f);
		else if( this.transform.eulerAngles.x >= 270f &&  this.transform.eulerAngles.x <= 360f) AngleX = ((270f - this.transform.eulerAngles.x) / -90f) / 2f;
        // Set X Angle

        float DesiredSpeed;
        if (AngleX > 0.5) DesiredSpeed = Mathf.Lerp(MaxSpeed * Throttle, MaxSpeed*3f, (AngleX - 0.5f) * 2f);
        else DesiredSpeed = Mathf.Lerp(-MaxSpeed, MaxSpeed * Throttle, AngleX * 2f);
        Speed = Mathf.MoveTowards (Speed, DesiredSpeed, MaxSpeed / 500f);
		// Set speed

		// Diving too fast
		if(Speed / MaxSpeed >= 1.75f){
            MainCanvas.GetComponent<CanvasScript>().SetInfoText("You're going too fast!", "Prędkość zbyt wyskoka!", new Color32(255, 0, 0, 255), 1f);
            Health -= Random.Range (MaxHealth / 1000f, MaxHealth / 500f);
			ShakePower = (1.75f - (Speed / MaxSpeed)) * 2f;
			ShakeDecay = 0.1f;
		}
		// Diving too fast

		// Move forward
		PrevPos = this.transform.position;
		FlyingTowards = Vector3.Lerp(FlyingTowards, this.transform.forward, Mathf.Clamp((Speed / MaxSpeed), 0f, 1f) * 0.03f);
		this.transform.position += FlyingTowards  * (Speed/90f);
		// Move forward

		// Chandelle
		float AngleZ = 0f;
		float CF = 0f;
        float AngleDecreaser;
        if (AngleX >= 0f && AngleX < 0.75f) AngleDecreaser = 1f;
		else AngleDecreaser = 1f - ((AngleX - 0.75f) / 0.25f);

		if( this.transform.eulerAngles.z >= 0f && this.transform.eulerAngles.z <= 90f){
			AngleZ = (this.transform.eulerAngles.z / 90f) * -1f;
			CF = 0f;
		} else if( this.transform.eulerAngles.z >= 270f && this.transform.eulerAngles.z <= 360f){
			AngleZ = 1f - ((270f - this.transform.eulerAngles.z) / -90f);
			CF = 0f;
		} else if( this.transform.eulerAngles.z > 90f && this.transform.eulerAngles.z < 270f){
			AngleZ = 0f;
			CF = 1f;
		}
		AngleZ *= AngleDecreaser;
		CF *= AngleDecreaser;
		// Chandelle
		// Flaps and rudder
		float RotationalSpeed = Speed / MaxSpeed;
		this.transform.Rotate(ElevatorFlapsRudder * (RotationSpeed * RotationalSpeed)); // Turning speed
		this.transform.Rotate(new Vector3 (0f, AngleZ / 2f * RotationSpeed, 0f));
		this.transform.eulerAngles += new Vector3 (CF / 2f * RotationSpeed, 0f, 0f);

		if(ControlType == "Point")
		{

			PointThere.transform.position = this.transform.position;
			PTturn = new Vector2(Mathf.Clamp(PTturn.x + Input.GetAxis("Mouse Y") * 10f * Inverted, -89f, 89f), PTturn.y + Input.GetAxis("Mouse X") * 10f);
			PointThere.rotation = Quaternion.Lerp(PointThere.rotation, Quaternion.Euler(PTturn), 0.1f);

			float TurnX = (this.transform.forward * 100f).y - (PointThere.forward*100f).y;
			float TurnY = Vector3.SignedAngle(this.transform.forward, PointThere.transform.forward*20f, Vector3.up);

			if(WhichCamera == "Free" || WhichCamera == "Turret")
				TurnX = TurnY = 0f;

			ElevatorFlapsRudder = Vector3.Lerp(
				ElevatorFlapsRudder,
				Vector3.Lerp(
					new Vector3(
						Mathf.Clamp(TurnX / 10f, -1f, 1f),
						Mathf.Clamp(TurnY / 10f, -1f, 1f),
						Mathf.Clamp(AngleZ * 10f, -1f, 1f)
					)
				,
					new Vector3(
						Mathf.Clamp((AngleX - 0.4f) * -3f + TurnX, -1f, 1f),
						0,//Mathf.Clamp(AngleX, -0.3f, 0.3f),
						Mathf.Clamp(Mathf.Lerp( TurnY / -3f , 0f, Mathf.Abs(AngleZ*2f) ), -1f, 1f)
					)
				,
				Mathf.Clamp((Mathf.Abs(TurnY)-10f) / 30f, 0f, 1f)
				),
				0.1f
			);

		}
		else
		{

			// Steering circle
			Vector2 SetSteer = new(
				Mathf.Clamp( Mathf.Lerp(SteerPosition.x, -1f + (Input.mousePosition.x / Screen.width * 2f), Time.deltaTime * 100f) , -1f , 1f ),
				Mathf.Clamp( Mathf.Lerp(SteerPosition.y,  (1f - (Input.mousePosition.y / Screen.height * 2f)) * Inverted  , Time.deltaTime * 100f) , -1f , 1f ));
			SteerPosition = SetSteer;
			// Steering circle

			if (WhichCamera == "Free" || WhichCamera == "Turret"){
	            ElevatorFlapsRudder = Vector3.Lerp(
					ElevatorFlapsRudder,
					new Vector3(
    	            	0f,
        	        	Input.GetAxis("Rudder") / 3f,
            	    	0f),
					0.1f);
	        } else if (Input.GetKey (KeyCode.LeftShift)) {
				ElevatorFlapsRudder = Vector3.Lerp(
					ElevatorFlapsRudder,
					new Vector3(
						SteerPosition.y * -1f,
						SteerPosition.x / 3f,
						0f),
					0.1f);
				ElevatorFlapsRudder = Vector3.Lerp(ElevatorFlapsRudder, Vector3.zero, Stalling /2f);
			} else {
				ElevatorFlapsRudder = Vector3.Lerp(
					ElevatorFlapsRudder,
					new Vector3(
						SteerPosition.y * -1f,
						Input.GetAxis("Rudder") / 3f,
						SteerPosition.x * -1f),
					0.1f);
				ElevatorFlapsRudder = Vector3.Lerp(ElevatorFlapsRudder, Vector3.zero, Stalling /2f);
			}

		}
		
		// Flaps and rudder

		// Lift force
		this.transform.position += Vector3.down*0.5f + Vector3.Lerp(this.transform.up, Vector3.down, Stalling / 3f) * Mathf.Lerp(0.25f, 0.75f, Speed/MaxSpeed);
		// Lift force

		// Stalling
		if(Speed <= (MaxSpeed / 3.9f))
			Stalling = Mathf.Clamp(Stalling + 0.02f, 0f, 5f);

		if(Stalling > 0f){
            MainCanvas.GetComponent<CanvasScript>().SetInfoText("You're stalling!", "Prędkość zbyt niska!", new Color32(255, 0, 0, 255), 1f);
            Stalling -= 0.01f;
			this.transform.position += Vector3.up * -0.1f;
			this.transform.eulerAngles += new Vector3(Mathf.Lerp(0f, Stalling, AngleX*10f), 0f, 0f);
		}
		// Stalling

	}

	void Controlling() {

		// Throttle
		Throttle += Input.GetAxis ("Throttle") / 100f;
		// Throttle

		// Shooting
		if(Input.GetButton("Fire")){
			if(GunCooldown <= 0f && Heat >= 0f){
                if (WhichCamera == "Turret") {
					Vector3 PickedPosition = Turret.transform.GetChild(0).GetChild(0).position;
					GameObject TurretBullet = Shoot("Main", PickedPosition, Turret.transform.GetChild(0).GetChild(0), PickedPosition + Camera.transform.forward);
                    GunCooldown = MaxGunCooldown;
					Heat += MaxGunCooldown;
                    if (GunType == "Paintball") TurretBullet.GetComponent<ProjectileScript>().PresentColor1 = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 0.5f);
                } else {
                    GameObject PickedGun = Guns.ToArray()[Random.Range(0, Guns.Count)];
					GameObject MainBullet = Shoot("Main", PickedGun.transform.position, PickedGun.transform, PickedGun.transform.position + this.transform.forward);
                    if (AirplaneClass == "Striker") {
                        GunCooldown = MaxGunCooldown / 2 / AmountOfGuns;
						Heat += MaxGunCooldown / 2 / AmountOfGuns;
                    } else {
                        GunCooldown = MaxGunCooldown / AmountOfGuns;
						Heat += MaxGunCooldown / AmountOfGuns;
                    }
                    if (GunType == "Paintball") MainBullet.GetComponent<ProjectileScript>().PresentColor1 = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 0.5f);
                }
				if(GunType == "Cannon" || GunType == "Jet Gun"){
					ShakePower = 0.5f;
					ShakeDecay = 0.05f;
				} else if(GunType == "Flak"){
					ShakePower = 1f;
					ShakeDecay = 0.1f;
				} else {
					ShakePower = 0.1f;
					ShakeDecay = 0.01f;
				}
			}
		}
		if(Input.GetButton("Present")){
			if(PresentCooldown <= 0f){
				PresentCooldown = MaxPresentCooldown;

				GameObject Present = Shoot("Present", PresentCannon.transform.position, PresentCannon.transform, PresentCannon.transform.position + this.transform.forward);
				if(PresentCannonType == "Homing Present"){
					float Distance = 400f;
					GameObject ChoosenTarget = null;
					foreach(GameObject HomeA in GameObject.FindGameObjectsWithTag("HomeUnchecked")){
						if(Vector3.Distance(this.transform.position, HomeA.transform.position) < Distance){
							Distance = Vector3.Distance(this.transform.position, HomeA.transform.position);
							ChoosenTarget = HomeA;
						}
					}
					if(ChoosenTarget != null){
						Present.transform.LookAt(ChoosenTarget.transform.position);
					}
				}
				if(PresentCannonType == "Slingshot" || PresentCannonType == "Cannon" || PresentCannonType == "Sniper Rifle" || PresentCannonType == "Homing Present"){
					ShakePower = 1f;
					ShakeDecay = 0.1f;
				}
			}
		}
		if(Input.GetButton("Special")){
            if (SpecialType == "None")
            {
                MainCanvas.GetComponent<CanvasScript>().SetInfoText("You don't have any special!", "Nie masz żadnego ekwipunku specjalnego!", new Color32(255, 0, 0, 255), 1f);
            }
            if (SpecialCooldown <= 0f){
				SpecialCooldown = MaxSpecialCooldown;
                if(SpecialType == "Wrenches"){
					Health += MaxHealth / 4f;
					MainCanvas.GetComponent<CanvasScript> ().FlashImage.color = new Color32 (0, 155, 0, 255);
					MainCanvas.GetComponent<CanvasScript> ().DisappearSpeed = 0.05f;
				} else if(SpecialType == "Fuel Tank"){
					Fuel += MaxFuel / 4f;
					MainCanvas.GetComponent<CanvasScript> ().FlashImage.color = new Color32 (0, 155, 255, 255);
					MainCanvas.GetComponent<CanvasScript> ().DisappearSpeed = 0.05f;
				} else if(SpecialType == "Homing Missile"){
					Shoot("Homing Missile", PresentCannon.transform.position + (this.transform.forward * 5f), PresentCannon.transform, this.transform.position + this.transform.forward);
				} else if(SpecialType == "Flares"){
					GameObject Bullet = Instantiate (Special) as GameObject;
					Bullet.transform.position = this.transform.position;
					Bullet.GetComponent<SpecialScript> ().TypeofSpecial = "Flares";
					Flares = 10f;
				} else if(SpecialType == "Brick"){
					Shoot("Brick", this.transform.position, this.transform, this.transform.position + this.transform.forward);
                }
			}
		}
		// Shooting

	}

	GameObject Shoot(string TypeofGun, Vector3 From, Transform GunFirePos, Vector3 There){
		GameObject Bullet = Instantiate(Projectile) as GameObject;
        Bullet.transform.position = From;
        Bullet.transform.LookAt(There);
		Bullet.GetComponent<ProjectileScript>().GunFirePos = GunFirePos;
        Bullet.GetComponent<ProjectileScript>().WhoShot = this.gameObject;
        Bullet.GetComponent<ProjectileScript>().PreviusSpeed = ((Speed * 0.003f) * 1.1f);
		switch(TypeofGun){
			case "Main":
				Bullet.GetComponent<ProjectileScript>().TypeofGun = GunType;
				break;
			case "Present":
				Bullet.GetComponent<ProjectileScript>().TypeofGun = "Present";
				Bullet.GetComponent<ProjectileScript> ().PresentRange = PresentCannonDistane;
				Bullet.GetComponent<ProjectileScript> ().PresentColor1 = PresentColor1;
				Bullet.GetComponent<ProjectileScript> ().PresentColor2 = PresentColor2;
				break;
			default:
				Bullet.GetComponent<ProjectileScript>().TypeofGun = TypeofGun;
				break;
		}
		return Bullet;
	}

	void PlaneData(float a, float b, float c, float d, string classa){
		MaxFuel = a; MaxHealth = b; RotationSpeed = c; MaxSpeed = d; AirplaneClass = classa;
	}

	void SetStats(){

		switch(PlaneModel){
			case "BP Mark.I": PlaneData(180f, 100f, 1f, 150f, "Fighter"); break;
			case "BP Bomber V.I": PlaneData(200f, 200f, 0.75f, 100f, "Bomber"); break;
			case "Monobird Striker": PlaneData(100f, 75f, 0.75f, 200f, "Striker"); break;
			case "Monobird B1": PlaneData(300f, 350f, 0.75f, 175f, "Bomber"); break;
			case "PukeFlame": PlaneData(110f, 100f, 0.75f, 300f, "Striker"); break;
			case "Tornado": PlaneData(200f, 150f, 1.25f, 180f, "Fighter"); break;
			case "Bob": PlaneData(500f, 500f, 0.5f, 200f, "Bomber"); break;
			case "Falcon G2": PlaneData(200f, 200f, 1.5f, 200f, "Fighter"); break;
			case "Falcon Dart": PlaneData(150f, 150f, 0.5f, 400f, "Striker"); break;
			case "SP Albatross": PlaneData(500f, 750f, 0.35f, 250f, "Bomber"); break;
			case "SP Arrow": PlaneData(150f, 175f, 0.5f, 600f, "Striker"); break;
			case "SP Eagle": PlaneData(250f, 250f, 2f, 225f, "Fighter"); break;
			case "Stolen Messerschmitt": PlaneData(200f, 150f, 1f, 180f, "Fighter"); break;
			case "Stolen Messerschmitt 110": PlaneData(100f, 100f, 1f, 200f, "Striker"); break;
			case "Stolen Messerschmitt Me 262": PlaneData(100f, 100f, 0.75f, 400f, "Striker"); break;
		}

		switch(EngineModel){
			case "Double Propeller": MaxSpeed *= 1.25f; RotationSpeed *= 1.25f; break;
			case "Jet Engine": MaxSpeed *= 1.5f; RotationSpeed *= 1.5f; break;
			case "Double Jet Engine": MaxSpeed *= 1.75f; RotationSpeed *= 1.75f; break;
			case "Magic Reindeer Dust": MaxSpeed *= 2f; RotationSpeed *= 2f; break;
		}

		float[] GTvars = new float[]{0f, 0f, 0f};
		switch(GunType){
			case "Vickers": GTvars = new float[]{400f, 0.1f, 3f, 5f}; break;
			case "M2 Browning": GTvars = new float[]{600f, 0.075f, 3f, 5f}; break;
			case "M3 Browning": GTvars = new float[]{600f, 0.075f, 5f, 5f}; break;
			case "Cannon": GTvars = new float[]{300f, 0.2f, 3f, 6f}; break;
			case "Flammable": GTvars = new float[]{400f, 0.1f, 3f, 5f}; break;
			case "Mugger Missiles": GTvars = new float[]{400f, 0.1f, 3f, 5f}; break;
			case "Flak": GTvars = new float[]{750f, 0.4f, 30f, 9999f}; break;
			case "Jet Gun": GTvars = new float[]{1000f, 0.05f, 7.5f, 5f}; break;
			case "Laser": GTvars = new float[]{500f, 0.075f, 15f, 9999f}; break;
	        case "Blue Laser": GTvars = new float[]{1000f, 0.075f, 15f, 9999f}; break;
			case "Paintball": GTvars = new float[]{600f, 0.1f, 5f, 10f}; break;
			case "Rocket": GTvars = new float[]{600f, 0.4f, 15f, 9999f}; break;
		}
		GunDistane = GTvars[0];
		MaxGunCooldown = GTvars[1];
		MaxHeat = GTvars[2];
		HeatDispare = GTvars[3];

		float[] PCvars = new float[]{0f, 0f};
		switch(PresentCannonType){
			case "Slingshot": PCvars = new float[]{400f, 10f}; break;
			case "Cannon": PCvars = new float[]{1000f, 10f}; break;
			case "Sniper Rifle": PCvars = new float[]{10000f, 30f}; break;
			case "Homing Present": PCvars = new float[]{400f, 30f}; break;
		}
		PresentCannonDistane = PCvars[0];
		MaxPresentCooldown = PCvars[1];

		float[] SpecVars = new float[]{0f, 0f};
		switch(SpecialType){
			case "None": SpecVars = new float[]{10f, 100000000f}; break;
			case "Wrenches": SpecVars = new float[]{10f, 25f}; break;
			case "Fuel Tank": SpecVars = new float[]{10f, 25f}; break;
			case "Homing Missile": SpecVars = new float[]{30f, 50f}; break;
			case "Flares": SpecVars = new float[]{30f, 25f}; break;
			case "Brick": SpecVars = new float[]{10f, 0f}; break;
		}
		MaxSpecialCooldown = SpecVars[0];
		SpecialRequiredAmmo = (int)SpecVars[1];

		Color32[] PC = new Color32[]{};
		switch(Paint){
			case "Basic Paint": PC = new Color32[]{ new(200, 200, 200, 255), new(200, 100, 100, 255)}; break;
			case "Present Colors": PC = new Color32[]{ new(0, 125, 0, 255), new(200, 0, 0, 255)}; break;
			case "Monochrome": PC = new Color32[]{ new(100, 100, 100, 255), new(55, 55, 55, 255)}; break;
			case "Night": PC = new Color32[]{ new(0, 100, 200, 255), new(5, 5, 55, 255)}; break;
			case "War Paint": PC = new Color32[]{ new(100, 175, 0, 255), new(55, 75, 55, 255)}; break;
			case "Toy Paint": PC = new Color32[]{ new(0, 100, 200, 255), new(200, 0, 0, 255)}; break;
			case "Girly": PC = new Color32[]{ new(200, 0, 200, 255), new(100, 0, 100, 255)}; break;
			case "Black and White": PC = new Color32[]{ new(225, 225, 225, 255), new(5, 5, 5, 255)}; break;
			case "Royal": PC = new Color32[]{ new(0, 75, 255, 255), new(255, 255, 0, 255)}; break;
	        case "Aggressive": PC = new Color32[]{ new(200, 0, 0, 255), new(5, 5, 5, 255)}; break;
    	    case "Desert": PC = new Color32[]{ new(255, 240, 220, 255), new(155, 115, 85, 255)}; break;
        	case "Rich": PC = new Color32[]{ new(255, 255, 255, 255), new(255, 190, 0, 255)}; break;
        }
		PlaneColor1 = PC[0];
		PlaneColor2 = PC[1];

        if (AirplaneClass == "Striker")MaxHeat *= 2;
        if (Addition == "Ammo Pack") MaxHeat *= 3;

		Health = MaxHealth;
		Fuel = MaxFuel;
		Heat = 0;
		Speed = MaxSpeed;


	}

	void PresentColorGenerator(){
		float PickHue = Random.Range(0f, 1f);
		PresentColor1 = Color.HSVToRGB(PickHue, 1f, 0.5f);
		PresentColor2 = Color.HSVToRGB((PickHue + 0.5f) % 1f, 1f, 0.5f);
	}

	public void Hurt(float Damage, int RedFlash, int PowerofShake){

		Health -= Damage * (float)GS.DifficultyLevel;
		if(RedFlash == 1){
			MainCanvas.GetComponent<CanvasScript> ().FlashImage.color = new Color32 (255, 0, 0, 75);
			MainCanvas.GetComponent<CanvasScript> ().DisappearSpeed = 0.01f;
		} else if(RedFlash == 2){
			MainCanvas.GetComponent<CanvasScript> ().FlashImage.color = new Color32 (255, 0, 0, 255);
			MainCanvas.GetComponent<CanvasScript> ().DisappearSpeed = 0.01f;
		}

		if(PowerofShake == 1){
			ShakePower = 1f;
			ShakeDecay = 0.1f;
		} else if(PowerofShake == 2){
			ShakePower = 1f;
			ShakeDecay = 0.05f;
		} else if(PowerofShake == 3){
			ShakePower = 3f;
			ShakeDecay = 0.1f;
		} else if(PowerofShake == 4){
			ShakePower = 3f;
			ShakeDecay = 0.05f;
		}

	}

    void OnTriggerEnter(Collider Col) {

        if (Col.tag == "Terrain" || Col.name == "Home") {
            if (Addition == "Damper") {
                Hurt(0f, 2, 4);
            } else {
                Hurt(Random.Range(25f, 50f), 2, 4);
            }
            this.transform.position += Vector3.up * 10f;
            this.transform.LookAt(this.transform.position + (Vector3.up * Random.Range(1f, 2f)) + this.transform.forward * 10f, this.transform.up);
            GameObject Hit = Instantiate(Effect) as GameObject;
            Hit.GetComponent<EffectScript>().TypeofEffect = "BullethitPlane";
            Hit.transform.position = this.transform.position;
		} else if(Col.name == "Explosion"){
			if(Col.transform.parent.GetComponent<SpecialScript>().IsLethal == true){
				Hurt (Col.transform.parent.GetComponent<SpecialScript>().ExplosionPower, 2, 3);
				GameObject Hit = Instantiate (Effect) as GameObject;
				Hit.GetComponent<EffectScript> ().TypeofEffect = "BullethitPlane";
				Hit.transform.position = this.transform.position;
			}
		} else if(Col.name == "Portal"){
			if(GameObject.Find("RoundScript").GetComponent<RoundScript>().State == "Success"){
				GameObject.Find ("RoundScript").GetComponent<RoundScript> ().State = "Left1";
			}
		} else if(Col.transform.parent != null){
			if(Col.transform.parent.name == "VesselModels"){
				if (Addition == "Damper") {
					Col.transform.parent.parent.GetComponent<EnemyVesselScript>().Health -= 20f;
				} else {
					Col.transform.parent.parent.GetComponent<EnemyVesselScript>().Health -= 10f;
				}
				Col.transform.parent.parent.GetComponent<EnemyVesselScript> ().HitByAPlayer = 5f;
				Hurt (Random.Range(25f, 50f), 2, 4);
				GameObject Hit = Instantiate (Effect) as GameObject;
				Hit.GetComponent<EffectScript> ().TypeofEffect = "BullethitPlane";
				Hit.transform.position = this.transform.position;
			}
		}

	}

    private void OnTriggerStay(Collider Col){
		if (Col.tag == "Cloud") {
            Flares = 1f;
            MainCanvas.GetComponent<CanvasScript>().SetInfoText("You're in a cloud", "Jesteś w chmurze", new Color32(225, 225, 255, 255), 1f);
        }
    }

}
