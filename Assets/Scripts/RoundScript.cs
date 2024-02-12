using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundScript : MonoBehaviour {

	// References
	public GameObject GameScript;
	public GameObject Player;
	public GameObject Enemy;
	public GameObject Home;
	public GameObject Portal;
    public GameObject Clouds;
    public ParticleSystem Snow;
	// References

	// Main Variables
	public string State = "Deliver Presents";
	public int Level = 1;
    public float SnowIntensity = 1f;
    public int TempScore = 0;
    public int TempMooney = 0;
    public int MapSize = 0;
	// Main Variables

	// Use this for initialization
	void Start () {

		GameScript = GameObject.Find ("GameScript");

		// Find Player
		Player = GameObject.Find("MainPlane");
		// Find Player

		// Set Level
		Level = GameScript.GetComponent<GameScript> ().Level;
        GameObject.FindObjectOfType<LandScript>().spawnLand("Plains", 20, Random.Range(1f, 999999f), 0);
        MapSize = (int)Mathf.Lerp(5000f, 20000f, (float)Level/20f);
        SetUp("Delivery", Level);
        Player.transform.position = new Vector3(MapSize/3f, 100f, MapSize/3f);
        // Set Level
		
	}

    void SetUp(string Mode, int LevelState){

        switch (Mode){
            case "Delivery":
                // Spawn Sequence
                List<Transform> placedHomes = new();
                for (int Begin = 0; Begin <= 6; Begin ++) {
                    if (Begin == 0) {
                        // Spawn homes
                        for (int Spawn = 1 + (LevelState / 4); Spawn > 0; Spawn--){
                            GameObject HomeA = Instantiate(Home) as GameObject;
                            HomeA.GetComponent<HomeScript>().HomeIndex = Spawn;
                            HomeA.transform.position = new Vector3(Random.Range(MapSize/-2f, MapSize/2f), 100f, Random.Range(MapSize/-2f, MapSize/2f));
                            placedHomes.Add(HomeA.transform);
                        }
                        // Spawn homes
                    } else if (Begin == 2){
                        // Spawn planes
                        List<string> PlanesToSpawn = new();
                        for (int Spawn = (int)((float)(LevelState / 2) * 1.5f); Spawn > 0; Spawn--)PlanesToSpawn.Add("Messerschmitt");
                        for (int Spawn = LevelState / 5; Spawn > 0; Spawn--)PlanesToSpawn.Add("Messerschmitt K4");
                        for (int Spawn = LevelState / 10; Spawn > 0; Spawn--)PlanesToSpawn.Add("Messerschmitt 110");
                        for (int Spawn = LevelState / 20; Spawn > 0; Spawn--)PlanesToSpawn.Add("Messerschmitt Me 262");
                        foreach(string SpawnThePlanes in PlanesToSpawn){
                            GameObject EnemyPlane = Instantiate(Enemy) as GameObject;
                            EnemyPlane.GetComponent<EnemyVesselScript>().TypeofVessel = SpawnThePlanes;
                            EnemyPlane.transform.position = placedHomes[(int)Random.Range(0f, placedHomes.ToArray().Length-0.1f)].position + new Vector3(Random.Range(-100f, 100f), Random.Range(100f, 1000f), Random.Range(-100f, 100f));
                        }
                         // Spawn planes
                    } else if (Begin == 4){
                        // Spawn aa guns
                        for (int Spawn = LevelState / 5; Spawn > 0; Spawn--) {
                            GameObject EnemyPlane = Instantiate(Enemy) as GameObject;
                            EnemyPlane.GetComponent<EnemyVesselScript>().TypeofVessel = "AA Gun";
                        }
                        // Spawn aa guns
                    } else if (Begin == 6){
                        // Spawn aa guns
                        for (int Spawn = LevelState / 10; Spawn > 0; Spawn--){
                            GameObject EnemyPlane = Instantiate(Enemy) as GameObject;
                            EnemyPlane.GetComponent<EnemyVesselScript>().TypeofVessel = "Balloon";
                        }
                        // Spawn aa guns
                    }
                }
                // Spawn Sequence
                break;
        }

        // Clouds
        SnowIntensity = Random.Range(3f, 10f);
        for (int SpawnClouds = Random.Range(5, 15); SpawnClouds > 0; SpawnClouds --) {
            GameObject SCloud = Instantiate(Clouds.transform.GetChild(0).gameObject) as GameObject;
            var main = SCloud.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(new Color(RenderSettings.fogColor.r / 1.5f, RenderSettings.fogColor.g / 1.5f, RenderSettings.fogColor.b / 1.5f, 0.5f), new Color(RenderSettings.fogColor.r, RenderSettings.fogColor.g, RenderSettings.fogColor.b, 0.75f));
            SCloud.transform.position = new Vector3(Random.Range(MapSize/-2f, MapSize/2f), Random.Range(100f, 3000f), Random.Range(MapSize/-2f, MapSize/2f));
            SCloud.transform.eulerAngles = new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 360f), Random.Range(-10f, 10f));
            SCloud.transform.localScale = new Vector3(Random.Range(1000f, 3000f), Random.Range(100f, 200f), Random.Range(1000f, 3000f));
        }
        // Clouds

    }

	void FixedUpdate () {

		GameScript = GameObject.Find ("GameScript");

        ParticleSystem myParticleSystem;
        ParticleSystem.EmissionModule emissionModule;
        myParticleSystem = Snow.GetComponent<ParticleSystem>();
        emissionModule = myParticleSystem.emission;
        emissionModule.rateOverTime = (SnowIntensity * 100f);

        if (State != "Success"){
			Portal.SetActive (false);
		} else if(Portal.activeInHierarchy == false){
			if(Player != null){
				Portal.SetActive (true);
				Portal.transform.position = new Vector3 (
					Mathf.Clamp ((Player.transform.position.x + Random.Range (-1000f, 1000f)), -2500f, 2500f),
					Random.Range (10f, 100f),
					Mathf.Clamp ((Player.transform.position.z + Random.Range (-1000f, 1000f)), -2500f, 2500f));
			}
		}

		if (State == "Deliver Presents" && GameObject.FindGameObjectsWithTag ("HomeUnchecked").Length <= 0f) {
			State = "Success";
			if(Player != null){
				Player.GetComponent<PlayerScript> ().MainCanvas.GetComponent<CanvasScript> ().SetInfoText ("Mission Accomplished!", "Misja Wykonana!", new Color32 (255, 255, 255, 255), 2f);
			}
		} else if (State == "Success"){
			if(Player != null){
				Portal.transform.GetChild (1).gameObject.SetActive (true);
				if(Vector3.Distance (Player.transform.position, Portal.transform.position) < 1000f){
					Portal.transform.GetChild (1).transform.GetChild (0).GetComponent<TextMesh> ().text = (int)(Vector3.Distance (Player.transform.position, Portal.transform.position)) + "m";
				} else {
					Portal.transform.GetChild (1).transform.GetChild (0).GetComponent<TextMesh> ().text = (int)((Vector3.Distance (Player.transform.position, Portal.transform.position)) / 1000f) + "km";
				}
			}
		} else if(State == "Left1"){
			GameScript.GetComponent<GameScript> ().LoadLevel("MainMenu");
			GameScript.GetComponent<GameScript> ().WhichMenuWindowToLoad = "CampaignMessage";
			State = "Left2";
			GameScript.GetComponent<GameScript> ().Level += 1;
            GameScript.GetComponent<GameScript>().CurrentScore += TempScore;
            GameScript.GetComponent<GameScript>().Mooney += TempMooney;
            TempScore = 0;
            TempMooney = 0;
        }

	}
}
