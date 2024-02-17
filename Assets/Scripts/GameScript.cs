using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class GameScript : MonoBehaviour {

	// References
	public PostProcessingProfile PPP1;
	// References

	// Options
	public int currentResolution = -1;
	public Vector2[] LoadedResolutions;
	public string Language = "English";
	public int HighScore = 0;
	public float[] Volumes = new float[] { 1f, 1f, 1f, -1}; // master, music, sounds, prev sounds
	public int ControlScheme = 0; // Mouse and Keyboard, Pointing
	public bool InvertedMouse = false;
    readonly Color[] PrevLight = new Color[2];
	int SwitchDesperate = 0;
	// Options

	// Game
	public int Level = 1;
	public int DifficultyLevel = 1;
	public int CurrentScore = 0;
    public int TempScore = 0;
	public int Mooney = 0;
    public int TempMooney = 0;
    public int Parachutes = 0;

	public string CurrentPlaneModel = "BP Mark.I";
	public string OwnedPlaneModels = "10000";
	public string CurrentEngineModel = "Basic Propeller";
	public string OwnedEngineModels = "10000";
	public string CurrentGunType = "Vickers";
	public string OwnedGundTypes = "10000";
	public string CurrentPresentCannonType = "Slingshot";
	public string OwnedPresentCannonTypes = "100";
	public string CurrentSpecialType = "None";
	public string OwnedSpecialTypes = "10000";
	public string CurrentAddition = "None";
	public string OwnedAdditions = "10000";
	public string CurrentPaint = "Basic Paint";
	public string OwnedPaints = "10000";
	// Game

	// Misc
	public string WhichMenuWindowToLoad = "";
	bool EreasePrefs = false;
	public int PreviousScore = 0;
	public string PreviousPlane = "";
    public bool HasDied = false;
	// Misc

	// Main Variables
	public string Version = "1.0";
	public string Build = "Screen";
	// Main Variables

	// Use this for initialization
	void Start () {

		if(this.gameObject.name == "GameScriptSTART"){
			if (GameObject.Find ("GameScript") == null) {
				this.gameObject.name = "GameScript";
				DontDestroyOnLoad (this.gameObject);
				// Load stuff
				if(PlayerPrefs.GetInt("HasSaved") == 1){
					currentResolution = PlayerPrefs.GetInt("Resolution");
					Language = PlayerPrefs.GetString ("Language");
					Volumes[0] = PlayerPrefs.GetFloat ("MasterVolume");
					Volumes[1] = PlayerPrefs.GetFloat ("AudioVolume");
					Volumes[2] = PlayerPrefs.GetFloat ("MusicVolume");
					HighScore = PlayerPrefs.GetInt ("HighScore");
					ControlScheme = PlayerPrefs.GetInt("Controls");

					if (PlayerPrefs.GetString ("Inverted") == "True") InvertedMouse = true;
					else InvertedMouse = false;

					if (PlayerPrefs.GetString ("Fullscreen") == "True") Screen.fullScreen = true;
					else Screen.fullScreen = false;
					
				}
				// Load stuff

				List<Vector2> loadResolutions = new();
				int defaulte = 0;
				int prefRR = -1;
				foreach(Resolution res in Screen.resolutions)
				if(res.refreshRate == prefRR || prefRR == -1) {
					loadResolutions.Add(new Vector2(res.width, res.height));
					defaulte++;
					if(res.width == 800 && currentResolution == -1) currentResolution = defaulte;
					if(prefRR == -1) prefRR = res.refreshRate;
				}
				LoadedResolutions = loadResolutions.ToArray();

			} else {
				Destroy (this.gameObject);
			}
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(GameObject.Find("RoundScript") != null){
			if(GameObject.Find("RoundScript").GetComponent<RoundScript>().enabled == false){
				GameObject.Find ("RoundScript").GetComponent<RoundScript> ().enabled = true;
			}
		}

		// Erase prefs
		if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.L)){
			if(EreasePrefs == false){
				EreasePrefs = true;
				PlayerPrefs.DeleteAll ();
				print("Data erased successfully");
			}
		}
		// Erase prefs

		// Set Graphics
		if(GameObject.Find("MainCamera") != null){
			
			if((QualitySettings.GetQualityLevel() == 0 || QualitySettings.GetQualityLevel() == 1) && GameObject.Find("MainCamera").GetComponent<Camera>().allowMSAA == true){
				GameObject.Find ("MainCamera").GetComponent<Camera> ().allowMSAA = false;
			} else if((QualitySettings.GetQualityLevel() == 2 || QualitySettings.GetQualityLevel() == 3) && GameObject.Find("MainCamera").GetComponent<Camera>().allowMSAA == false){
				GameObject.Find ("MainCamera").GetComponent<Camera> ().allowMSAA = true;
			}

			if (QualitySettings.GetQualityLevel () == 0 && SwitchDesperate != 0) {
				SwitchDesperate = 0;
				GameObject.Find ("MainCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Color;
				if(GameObject.Find("MainLight")){
					Light S = GameObject.Find("MainLight").GetComponent<Light>();
					PrevLight[0] = GameObject.Find ("MainCamera").GetComponent<Camera> ().backgroundColor;
					GameObject.Find ("MainCamera").GetComponent<Camera> ().backgroundColor = S.color;
					PrevLight[1] = RenderSettings.ambientLight;
					RenderSettings.ambientLight = S.color;
					S.intensity = 0f;
				}
			} else if (QualitySettings.GetQualityLevel () != 0 && SwitchDesperate != 1) {
				SwitchDesperate = 1;
				GameObject.Find ("MainCamera").GetComponent<Camera> ().clearFlags = CameraClearFlags.Skybox;
				if(GameObject.Find("MainLight")){
					Light S = GameObject.Find("MainLight").GetComponent<Light>();
					GameObject.Find ("MainCamera").GetComponent<Camera> ().backgroundColor = PrevLight[0];
					RenderSettings.ambientLight = PrevLight[1];
					S.intensity = 1f;
				}
			}

			if (QualitySettings.GetQualityLevel () == 3) {
				GameObject.Find ("MainCamera").GetComponent<PostProcessingBehaviour> ().enabled = true;
			} else {
				GameObject.Find ("MainCamera").GetComponent<PostProcessingBehaviour> ().enabled = false;
			}
				
		}
		// Set Graphics

		// Set Audios
		if(Volumes[3] != Volumes[0] + Volumes[1] + Volumes[2]) {
			Volumes[3] = Volumes[0] + Volumes[1] + Volumes[2];
			foreach(SoundControll A in FindObjectsOfType<SoundControll>()) A.Set();
		}
		// Set Audios
		
	}

	public string SetText(string English, string Polish){
		if(Language == "Polski"){
			return Polish;
		} else {
			return English;
		}
	}

	public void LoadLevel(string LevelName){
		SceneManager.LoadScene (LevelName);
		SwitchDesperate = 1;
	}

	public void SetGameOptions(string Which, string File){
		if(Which == "Empty"){
			PlayerPrefs.SetInt (File + "SavedGame", 0);
			Level = 1;
			DifficultyLevel = 1;
			CurrentScore = 0;
            TempScore = 0;
			Mooney = 0;
            TempMooney = 0;
            Parachutes = 3;
			CurrentPlaneModel = "BP Mark.I";
			OwnedPlaneModels = "100000000000000000000000000000";
			CurrentEngineModel = "Basic Propeller";
			OwnedEngineModels = "100000000000000000000000000000";
			CurrentGunType = "Vickers";
			OwnedGundTypes = "100000000000000000000000000000";
			CurrentPresentCannonType = "Slingshot";
			OwnedPresentCannonTypes = "100000000000000000000000000000";
			CurrentSpecialType = "None";
			OwnedSpecialTypes = "100000000000000000000000000000";
			CurrentAddition = "None";
			OwnedAdditions = "100000000000000000000000000000";
			CurrentPaint = "Basic Paint";
			OwnedPaints = "100000000000000000000000000000";
		} else if(Which == "Save"){
			PlayerPrefs.SetInt (File + "SavedGame", 1);
			PlayerPrefs.SetInt (File + "Level", Level);
			PlayerPrefs.SetInt (File + "DifficultyLevel", DifficultyLevel);
			PlayerPrefs.SetInt (File + "CurrentScore", CurrentScore);
			PlayerPrefs.SetInt (File + "Mooney", Mooney);
            PlayerPrefs.SetInt (File + "Parachutes", Parachutes);
			PlayerPrefs.SetString (File + "CurrentPlaneModel", CurrentPlaneModel);
			PlayerPrefs.SetString (File + "OwnedPlaneModels", OwnedPlaneModels);
			PlayerPrefs.SetString (File + "CurrentEngineModel", CurrentEngineModel);
			PlayerPrefs.SetString (File + "OwnedEngineModels", OwnedEngineModels);
			PlayerPrefs.SetString (File + "CurrentGunType", CurrentGunType);
			PlayerPrefs.SetString (File + "OwnedGunTypes", OwnedGundTypes);
			PlayerPrefs.SetString (File + "CurrentPresentCannonType", CurrentPresentCannonType);
			PlayerPrefs.SetString (File + "OwnedPresentCannonTypes", OwnedPresentCannonTypes);
			PlayerPrefs.SetString (File + "CurrentSpecialType", CurrentSpecialType);
			PlayerPrefs.SetString (File + "OwnedSpecialTypes", OwnedSpecialTypes);
			PlayerPrefs.SetString (File + "CurrentAddition", CurrentAddition);
			PlayerPrefs.SetString (File + "OwnedAdditions", OwnedAdditions);
			PlayerPrefs.SetString (File + "CurrentPaint", CurrentPaint);
			PlayerPrefs.SetString (File + "OwnedPaints", OwnedPaints);
		} else if(Which == "Load"){
			PlayerPrefs.SetInt (File + "SavedGame", 1);
			Level = PlayerPrefs.GetInt (File + "Level");
			DifficultyLevel = PlayerPrefs.GetInt (File + "DifficultyLevel");
			CurrentScore = PlayerPrefs.GetInt (File + "CurrentScore");
            TempScore = 0;
			Mooney = PlayerPrefs.GetInt (File + "Mooney");
            TempMooney = 0;
            Parachutes = PlayerPrefs.GetInt(File + "Parachutes");
			CurrentPlaneModel = PlayerPrefs.GetString (File + "CurrentPlaneModel");
			OwnedPlaneModels = PlayerPrefs.GetString (File + "OwnedPlaneModels");
			CurrentEngineModel = PlayerPrefs.GetString (File + "CurrentEngineModel");
			OwnedEngineModels = PlayerPrefs.GetString (File + "OwnedEngineModels");
			CurrentGunType = PlayerPrefs.GetString (File + "CurrentGunType");
			OwnedGundTypes = PlayerPrefs.GetString (File + "OwnedGunTypes");
			CurrentPresentCannonType = PlayerPrefs.GetString (File + "CurrentPresentCannonType");
			OwnedPresentCannonTypes = PlayerPrefs.GetString (File + "OwnedPresentCannonTypes");
			CurrentSpecialType = PlayerPrefs.GetString (File + "CurrentSpecialType");
			OwnedSpecialTypes = PlayerPrefs.GetString (File + "OwnedSpecialTypes");
			CurrentAddition = PlayerPrefs.GetString (File + "CurrentAddition");
			OwnedAdditions = PlayerPrefs.GetString (File + "OwnedAdditions");
			CurrentPaint = PlayerPrefs.GetString (File + "CurrentPaint");
			OwnedPaints = PlayerPrefs.GetString (File + "OwnedPaints");
		} else if (Which == "Erase"){
			PlayerPrefs.DeleteKey (File + "SavedGame");
			PlayerPrefs.DeleteKey (File + "Level");
			PlayerPrefs.DeleteKey (File + "DifficultyLevel");
			PlayerPrefs.DeleteKey (File + "CurrentScore");
			PlayerPrefs.DeleteKey (File + "Mooney");
            PlayerPrefs.DeleteKey (File + "Parachutes");
			PlayerPrefs.DeleteKey (File + "CurrentPlaneModel");
			PlayerPrefs.DeleteKey (File + "OwnedPlaneModels");
			PlayerPrefs.DeleteKey (File + "CurrentEngineModel");
			PlayerPrefs.DeleteKey (File + "OwnedEngineModels");
			PlayerPrefs.DeleteKey (File + "CurrentGunType");
			PlayerPrefs.DeleteKey (File + "OwnedGunTypes");
			PlayerPrefs.DeleteKey (File + "CurrentPresentCannonType");
			PlayerPrefs.DeleteKey (File + "OwnedPresentCannonTypes");
			PlayerPrefs.DeleteKey (File + "CurrentSpecialType");
			PlayerPrefs.DeleteKey (File + "OwnedSpecialTypes");
			PlayerPrefs.DeleteKey (File + "CurrentAddition");
			PlayerPrefs.DeleteKey (File + "OwnedAdditions");
			PlayerPrefs.DeleteKey (File + "CurrentPaint");
			PlayerPrefs.DeleteKey (File + "OwnedPaints");
		}
	}

	public void GainScore(int GainScore, int GainMooney, string special){

		if(special == ""){
			GameObject.Find("RoundScript").GetComponent<RoundScript>().TempScore += GainScore * DifficultyLevel;
            GameObject.Find("RoundScript").GetComponent<RoundScript>().TempMooney += GainMooney / DifficultyLevel;
		}

	}

	void OnApplicationQuit(){

		if(EreasePrefs == false){
			PlayerPrefs.SetInt ("HasSaved", 1);
			PlayerPrefs.SetInt ("Resolution", currentResolution);
			PlayerPrefs.SetString ("Language", Language);
			PlayerPrefs.SetInt ("Controls", ControlScheme);
			PlayerPrefs.SetString ("Inverted", InvertedMouse.ToString());
			PlayerPrefs.SetString ("Fullscreen", Screen.fullScreen.ToString());
			PlayerPrefs.SetFloat ("MasterVolume", Volumes[0]);
			PlayerPrefs.SetFloat ("AudioVolume", Volumes[1]);
			PlayerPrefs.SetFloat ("MusicVolume", Volumes[2]);
			PlayerPrefs.SetInt ("HighScore", HighScore);
		}

	}

}
