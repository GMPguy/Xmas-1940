using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	//References
	public GameScript GS;
	public GameObject Windows;
	public GameObject AnchorUp;
	public GameObject AnchorDown;
	// Loading
	public Text LoadingText;
	public Text LoadingTipText;
	// Loading
	// Prologue
	public Text PrologueText;
	public Image PrologueBlackImage;
	// Prologue
	// Start
	public Text VersionName;
	// Start
	// Main
	public GameObject StartButton;
	public GameObject OptionButton;
	public GameObject CreditsButton;
	public GameObject ExitButton;
	// Main
	// Play
	public GameObject NewGameButton;
	public GameObject ContinueGameButton;
	public GameObject PlayBackButton;
	public Text HighScores;
	// Play
	// New Game
	public GameObject CNormalButton;
	public GameObject CHardButton;
	public GameObject NewGameBack;
	// New Game
	// New Game Info
	public GameObject NGInfoText;
	public int NGWchicPage = 1;
	public GameObject NGBegin;
	// New Game Info
	// Credits
	public Text Credits;
	public GameObject CreditsBack;
	// Credits
	// Options
	public GameObject MusicVolumeButton;
	public GameObject VolumeButton;
	public GameObject InvertedButton;
	public GameObject LanguageButton;
	public GameObject OptionsBack;
	// Options
	// Campaign Main
	public GameObject CMPlayButton;
	public GameObject CMCustomizeButton;
	public GameObject CMQuitButton;
    public GameObject CMMessageButton;
	public Text CMInfo;
    public Image CMPImage;
    public Text CMPText;
	// Campaign Main
	// Campaign Customization
	public GameObject CustomizationBack;
	public Text CCInfo;
    public Image CCPImage;
    public Text CCPText;
	public Text CurrentCustomizationOptionText;
	public GameObject CustomizationButtons;
	public GameObject CustomizationButtons2;
	public GameObject CC2Prev;
	public GameObject CC2Next;
	public int WhichPage = 1;
	public GameObject CustomizationInfo;
	public string currentCustomizationOption = "";
	public Text OptionInfoName;
	public Text OptionInfoDescription;

	public GameObject PlaneinCustomization;
	public string PCPlaneModel = "";
	public float PCPlaneModelDTN = 0f;
	public string PCEngineModel = "";
	public float PCEngineModelDTN = 0f;
	public string PCPaint = "";
	public float PCPaintDTN = 0f;
    // Campaign Customization
    // Campaign Message
    public Text MessageText;
    public GameObject CloseMessage;
    // Campaign Message
	// Game Over
	public Image GameOverImage;
	public Text GameOverText;
	public Text GameOverText2;
	public Text GameOverText3;
	public GameObject DeadPlanes;
	public GameObject DeadCamera;
	// Game Over
	public Camera MainCamera;
	public GameObject MainMenuPlane;
	public GameObject StartLands;
	public float StartLandsPosition = 0f;
	string SLand1 = "EmptyLand";
	int SRLand1 = 1;
	string SLand2 = "EmptyLand";
	int SRLand2 = 1;
	string SLand3 = "EmptyLand";
	int SRLand3 = 1;
	public AudioSource Click;
	public GameObject Musics;
	//References

	// Variables
	string CurrentWindow = "Prologue";
	float Loading = 3f;
	int LoadingTip = 1;
	// variables

	// Misc
	Vector3 SetPos;
	float CampaignCamRotation = 0f;
    float CapmaignFreeRotation = 0f;

	string WindowToLoad = "";
	float TimeUntilLoad = 0f;

	public float EraseOptionInfo = 0f;

	string CurrentMusic = "";
	// Misc

	// Use this for initialization
	void Start () {

		Cursor.lockState = CursorLockMode.None;
		
		SetPos = new Vector3(0f, -3.5f, 0f);

		GS = GameObject.Find("GameScript").GetComponent<GameScript>();

		LoadingTip = Random.Range (1, 7);
		Loading = Random.Range (1f, 3f);

		// Set sky color
		RenderSettings.skybox.SetColor("_Tint", new Color32(0, 0, 75, 255));
		RenderSettings.skybox.SetColor("_SkyTint", new Color32(0, 0, 75, 255));
		RenderSettings.skybox.SetColor("_GroundColor", new Color32(0, 25, 55, 255));
		RenderSettings.ambientLight = new Color32 (0, 25, 55, 255);
		RenderSettings.fogColor = new Color32 (0, 25, 55, 255);
		RenderSettings.fogEndDistance = 1000f;
		GameObject.Find ("MainLight").GetComponent<Light> ().color = new Color32 (0, 55, 75, 255);
		GameObject.Find ("MainLight").transform.eulerAngles = new Vector3 (10f, 0f, 0f);
		// Set sky color

		Cursor.visible = true;
		
	}
	
	// Update is called once per frame
	void Update () {

        SettingMessage();
        if (GS.WhichMenuWindowToLoad != ""){
			CurrentWindow = GS.WhichMenuWindowToLoad;
			GS.WhichMenuWindowToLoad = "";
		}

		// Set Windows
		if (Loading > 0f) {
			foreach (Transform Window in Windows.transform) {
				if (Window.name == "Loading") {
					Window.position = AnchorUp.transform.position;
				} else {
					Window.position = AnchorDown.transform.position;
				}
			}
		} else {
			LoadingTip = Random.Range (1, 7);
			foreach (Transform Window in Windows.transform) {
				if (Window.name == CurrentWindow) {
					if (Window.name == "Prologue" || Window.name == "GameOver") {
						Window.position = AnchorUp.transform.position;
					} else {
						Window.position = Vector3.Lerp (Window.position, AnchorUp.transform.position, 0.1f * (Time.deltaTime * 100f));
					}
				} else {
					if (Window.name == "Loading") {
						Window.position = AnchorDown.transform.position;
					} else {
						Window.position = Vector3.Lerp (Window.position, AnchorDown.transform.position, 0.1f * (Time.deltaTime * 100f));
					}
				}
			}
		}
        // Set Windows

        // Windows actions
        if (Loading > 0f) {
            Loading -= 0.01f;
            LoadingText.text = GS.SetText("Loading", "Wczytywanie");
			switch(LoadingTip){
	            case 1:
    	            LoadingTipText.text = GS.SetText("While dogfightig, try shooting in advance if you're not directly behind/in front of your opponent!", "Gdy strzelasz do ruchomych objektów, spróbuj strzelać z wyprzedzeniem, jeśli nie jesteś prosto za/przed celem");
					break;
				case 2:
            	    LoadingTipText.text = GS.SetText("Delivering presents restores some of your health, fuel, and ammo", "Dostarczanie prezentów odnawia ci trochę zdrowia, paliwa, oraz ammunicji");
	            	break;
				case 3:
    	            LoadingTipText.text = GS.SetText("Try not to dogfight with too many planes at once!", "Nie próbuj walczyć z zbyt dużą ilością samolotów!");
					break;
				case 4:
            	    LoadingTipText.text = GS.SetText("While shooting or delivering present, aim from your gun perspective (RMB on default), for better accuracy", "Podczas strzelania lub dostarczania prezentów, celuj z perspektywy swojej broni (Przytrzymując PPM), dla lepszej celności");
	            	break;
				case 5:
    	            LoadingTipText.text = GS.SetText("Colliding with anything usually damages the plane, or destroys it immediately. Be careful when you're flying straight at any object!", "Kolizje z czymkolwiek przeważnie uszkadzają lub niszczą twój samolot. Bądź ostrożny gdy lecisz prosto na coś!");
					break;
				case 6:
            	    LoadingTipText.text = GS.SetText("Having speed greater than 175% damages the plane. If you have speed lower than 25%, you'll be unable to rotate your aircraft", "Jeśli twoja prędkość wyniesie więcej niż 175%, zaczniesz tracić zdrowie. Jeśli zaś będzie wynosić mniej niż 25%, nie będziesz mógł obracać swojej maszyny");
					break;
            }
        } else if (CurrentWindow == "Prologue") {

            if (CurrentMusic != "Prologue") {
                PlayMusic("Prologue");
            }

            PrologueBlackImage.color = new Color32(0, 0, 0, (byte)((Vector3.Distance(PrologueText.transform.position, AnchorUp.transform.position) / (Screen.height * 0.75f)) * 255f));
            PrologueText.transform.position = Vector3.MoveTowards(PrologueText.transform.position, AnchorUp.transform.position, ((float)Screen.height / 5000f) * (Time.deltaTime * 100f));
            PrologueText.text = GS.SetText(
                "Let me tell you a Christmas story:\n\n It is year 1940. It has been about a year, since the German invasion of Poland, and the following beginning of the Second World War. The German forces have been continuing to fight and invade other countries ever since, and it doesn't look like it's going to end anytime soon. Although Germany's occupying almost the entirety of europe, the fight is still present everywhere: on the land, on the sea, and in the sky. It is now December 25th, and Christmas broke out. It is a time, when Santa Claus is flying in his sleigh, and is delivering presents to the kind children. But since there is a grand conflict going on, he cannot be flying in his sleigh, because that's too dangerous! For that reason, he has replaced his sleigh and his reindeers, with a modern, heavily armed, military-class airplane. Now, he'll deliver the presents without getting stopped, and if any nazi scum will try to ruin the christmas spirit, that scum shall bite the dust! Or maybe Santa Claus will be the one who's gonna be taken down? Find it out, in...",
                "Pozwól że opowiem ci świąteczną opowieść:\n\n Jest rok 1940. Minął rok od Niemieckiej inwazji na Polskę, oraz rozpoczęcia drugiej wojny światowej. Siły niemieckie dalej kontynuują inwazję na nie swoje ziemie, i wydawać by się mogło, że taki stan rzeczy prędko się nie zmieni. Pomimo tego, iż Niemcy okupują już prawie całą europę, walka dalej jest kontynuowana: na ziemi, na wodzie, i w powietrzu. Nadszedł jednak 25'ego grudnia, a wraz z nim, Święta Bożego Narodzenia. Jest to czas, w którym Święty Mikołaj lata na swych saniach, i rozdaje prezenty grzecznym dziecią. Jednak, ze względu na wielki konflikt zbrojeniowy, nie może latać on w swych saniach, gdyż to jest niebiezpieczne! Z tego powodu, zastąpił on swoje sanie i renifery, świetnie uzbrojonym samolotem klasy wojskowej. Teraz, może w spokoju porozdawać dziecią ich prezenty, a jeśli jakiś szkop zechce zrujnować świąteczny nastrój, będzie on gryzł piach! A może to jednak Święty Mikołaj będzie tym, którego zestrzelą? Zaraz się tego dowiemy, w...");
            if (Input.anyKeyDown) {
                CurrentWindow = "Start";
            }

        } else if (CurrentWindow == "Start") {

            VersionName.text = GS.SetText("Version " + GS.Version, "Version " + GS.Version);
            MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, GameObject.Find("StartCamera").transform.position, 0.1f * (Time.deltaTime * 100f));
            MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, GameObject.Find("StartCamera").transform.rotation, 0.1f * (Time.deltaTime * 100f));
            if (Input.anyKeyDown) {
                CurrentWindow = "Main";
            }

        } else if (CurrentWindow == "Main") {

            StartButton.GetComponent<Text>().text = GS.SetText("Play", "Graj");
            OptionButton.GetComponent<Text>().text = GS.SetText("Options", "Opcje");
            CreditsButton.GetComponent<Text>().text = GS.SetText("Credits", "Lista Płac");
            ExitButton.GetComponent<Text>().text = GS.SetText("Exit", "Wyjdź");

            if (StartButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Play";
            } else if (OptionButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Options";
            } else if (CreditsButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Credits";
            } else if (ExitButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                Application.Quit();
            }

        } else if (CurrentWindow == "Play") {

            NewGameButton.GetComponent<Text>().text = GS.SetText("New Game", "Nowa Gra");
            if (PlayerPrefs.HasKey("TESTSavedGame")) {
                ContinueGameButton.GetComponent<Text>().text = GS.SetText("Continue Game", "Kontynuuj grę");
                ContinueGameButton.GetComponent<ButtonScript>().IsActive = true;
            } else {
                ContinueGameButton.GetComponent<Text>().text = GS.SetText("", "");
                ContinueGameButton.GetComponent<ButtonScript>().IsActive = false;
            }
            ExitButton.GetComponent<Text>().text = GS.SetText("Exit", "Wyjdź");
            HighScores.text = GS.SetText("High Score: " + GS.HighScore, "Najwyższy Wynik: " + GS.HighScore);

            if (NewGameButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "NewGame";
            } else if (ContinueGameButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0) && PlayerPrefs.HasKey("TESTSavedGame")) {
                CurrentWindow = "CampaignMain";
                GS.SetGameOptions("Load", "TEST");
            } else if (PlayBackButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Main";
            }

        } else if (CurrentWindow == "NewGame") {

            CNormalButton.GetComponent<Text>().text = GS.SetText("Normal", "Średni");
            CHardButton.GetComponent<Text>().text = GS.SetText("Hard", "Trudny");
            ExitButton.GetComponent<Text>().text = GS.SetText("Exit", "Wyjdź");

            if (CNormalButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "NewGameInfo";
                GS.SetGameOptions("Empty", "");
                GS.DifficultyLevel = 1;
                GS.Parachutes = 3;
            } else if (CHardButton.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "NewGameInfo";
                GS.SetGameOptions("Empty", "");
                GS.DifficultyLevel = 2;
                GS.Parachutes = 1;
            } else if (NewGameBack.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                CurrentWindow = "Play";
            }

        } else if (CurrentWindow == "NewGameInfo") {

            if (NGWchicPage == 1) {
                NGInfoText.GetComponent<Text>().text = GS.SetText(
                    "Quick Tutorial:\nYour objective is to deliver presents to the marked houses. The position of the houses are random, and they are scattered all around the map. A house is marked with a green dot, which has a text below it, that says how far from it you are. In order to deliver a present to a house, you need to get close enough to the house (if the dot turns into rectangle, it means you can reach it), then you aim and fire your present cannon at it (SPACE on default). Once all presents have been delivered, portal will appear near you, through which you leave the level.",
                    "Szybki Poradnik:\nTwoim zadaniem jest dostarczenie prezentów do wyznaczonych domów. Pozycja ów domów jest losowa, i są rozsiane po całej mapie. Domy są zaznaczone zieloną kropką, która ma tekst pod sobą, na którym napisany jest dystans pomiędzy tobą a domem. Aby dostarczyć prezent, musisz podlecieć wystarczająco blisko (jeśli kropka zamieni się w kwadrat, jesteś wystarczająco blisko), po czym musisz wycelować, i wystrzelić prezent ze swojej wyrzutni prezentów (SPACJA). Po dostarczeniu wszystkich prezentów, pojawi się portal, przez który opuszczasz poziom.");
                NGBegin.GetComponent<Text>().text = GS.SetText("Next", "Dalej");
            } else if (NGWchicPage == 2) {
                NGInfoText.GetComponent<Text>().text = GS.SetText(
                    "Quick Tutorial:\nYou control your airplane with mouse and keyboard. Moving your mouse vertically, turns your airplane up and down, while moving mouse horizontally, makes your airplane roll left and right. You can change your speed, by changing throttle, with W and S buttons. In order to turn your aircraft left and right (turn, not roll), you need to hold A or D button. You also have a gun, present cannon, and a special. To fire your gun, you need to hold LMB, and need to have more than 0 ammo. Press SPACE to fire your present cannon (you have infinite amount of presents), and (if you have any) press Q to use your special.",
                    "Szybki Poradnik:\nSwój samolot kontrolujesz myszką i klawiaturą. Wertykalny ruch myszki, obraca twój samolot w góre i w dół, natomiast horyzontalny, kręci twoim samolotem w lewo i w prawo. Swoją prędkość możesz zmienić za pomocą klawiszy W i S. Aby obrócić swój samolot w lewo i prawo (obrócić, nie kręcić), musisz przytrzymać klawisz A, albo D. W samolocie posiadasz broń, wyrzutnie prezentów, i ekwipunek specjalny. Aby użyć broni, musisz mieć amunicję, i musisz przytrzymać LPM. Wyrzutnia prezentów ma nielimitowaną amunicję, i używasz ją za pomocą SPACJI. Ekwipunek specjalny (jeśli masz jakiś), używaż za pomocą Q.");
                NGBegin.GetComponent<Text>().text = GS.SetText("Next", "Dalej");
            } else if (NGWchicPage == 3) {
                NGInfoText.GetComponent<Text>().text = GS.SetText(
                    "Quick Tutorial:\nAfter you complete a level, you get sent back to menu, where you can: upgrade your plane, save your progress, read new messages, and start another level. We'll see how many levels you can complete before you die. Oh, about that, death is pernament. You have a few parachutes, if you crash, you lose one parachute, if you crash without having any parachutes, you start from begining.\n\nThat's all, good luck!",
                    "Szybki Poradnik:\nPo przejściu poziomu, zostajesz odsyłany do menu, gdzie możesz: ulepszyć swój samolot, zapisać postęp, przeczytać nowe wiadomości, i rozpocząć nowy poziom. Zobaczymy ile poziomów uda ci się zaliczyć przed śmiercią. O, a co do tego, śmierć jest pernamenta. Masz kilka spadochronów, jeśli się rozbijesz, tracisz jeden spadochron, jeśli się rozbijesz nie mając żadnego spadochronu, zaczynasz od początku.\n\nTo wszystko, powodzenia!");
                NGBegin.GetComponent<Text>().text = GS.SetText("Begin", "Rozpocznij");
            }

            if (NGBegin.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)) {
                if (NGWchicPage == 3) {
                    WindowToLoad = "MainGame";
                    TimeUntilLoad = Random.Range(1f, 3f);
                } else {
                    NGWchicPage += 1;
                }
            }

        } else if (CurrentWindow == "CampaignMain") {

            CMPlayButton.GetComponent<Text>().text = GS.SetText("Play (Level " + GS.Level + ")", "Graj (Poziom " + GS.Level + ")");
            CMCustomizeButton.GetComponent<Text>().text = GS.SetText("Customize", "Edytuj Samolot");
            CMQuitButton.GetComponent<Text>().text = GS.SetText("Save'n'Quit", "Zapisz i Wyjdź");
            CMInfo.text = GS.SetText("Score: " + GS.CurrentScore + "\nMooney: " + GS.Mooney, "Wynik: " + GS.CurrentScore + "\nPiniądze: " + GS.Mooney);
            if (GS.Parachutes > 0){
                CMPText.text = GS.Parachutes.ToString();
                CMPText.color = new Color32(255, 255, 255, 255);
                CMPImage.color = new Color32(255, 255, 255, 255);
            } else {
                CMPText.text = GS.Parachutes.ToString();
                CMPText.color = new Color32(255, 0, 0, 255);
                CMPImage.color = new Color32(255, 0, 0, 255);
            }

            if (MessageText.text != "") CMMessageButton.SetActive(true);
            else CMMessageButton.SetActive(false);

			if(Input.GetMouseButtonDown(0)){
	            if (CMPlayButton.GetComponent<ButtonScript>().IsSelected == true) {
    	            WindowToLoad = "MainGame";
        	        TimeUntilLoad = Random.Range(1f, 3f);
            	} else if (CMCustomizeButton.GetComponent<ButtonScript>().IsSelected == true) {
                	CurrentWindow = "CampaignCustomization";
	            }else if (CMMessageButton.GetComponent<ButtonScript>().IsSelected == true) {
    	            CurrentWindow = "CampaignMessage";
        	    } else if (CMQuitButton.GetComponent<ButtonScript>().IsSelected == true) {
            	    CurrentWindow = "Main";
                	GS.SetGameOptions("Save", "TEST");
            	}
			}

        } else if (CurrentWindow == "CampaignMessage"){

            CloseMessage.GetComponent<Text>().text = GS.SetText("Close", "Zamknij");

            if (MessageText.text == "") {
                CurrentWindow = "CampaignMain";
            }

            if (CloseMessage.GetComponent<ButtonScript>().IsSelected == true && Input.GetMouseButtonDown(0)){
                GS.HasDied = false;
                CurrentWindow = "CampaignMain";
            }

        } else if (CurrentWindow == "CampaignCustomization") {
			
			CustomizationBack.GetComponent<Text> ().text = GS.SetText ("Back", "Wróć");
			CCInfo.text = GS.SetText ("Score: " + GS.CurrentScore + "\nMooney: " + GS.Mooney, "Wynik: " + GS.CurrentScore + "\nPiniądze: " + GS.Mooney);
            if (GS.Parachutes > 0) {
                CCPText.text = GS.Parachutes.ToString();
                CCPText.color = new Color32(255, 255, 255, 255);
                CCPImage.color = new Color32(255, 255, 255, 255);
            } else {
                CCPText.text = GS.Parachutes.ToString();
                CCPText.color = new Color32(255, 0, 0, 255);
                CCPImage.color = new Color32(255, 0, 0, 255);
            }

            CampaignCustomization ();

			if (CustomizationBack.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				if (currentCustomizationOption == "") {
					CurrentWindow = "CampaignMain";
				} else {
					currentCustomizationOption = "";
				}
			}

			if (EraseOptionInfo > 0f) {
				EraseOptionInfo -= 1f;
			} else {
				OptionInfoName.text = "";
				OptionInfoDescription.text = "";
			}

		} else if (CurrentWindow == "Options") {

			MusicVolumeButton.GetComponent<Text> ().text = GS.SetText ("Music Volume: " + (int)(GS.MusicVolume * 100f), "Głośność Muzyki: " + (int)(GS.MusicVolume * 100f));
			VolumeButton.GetComponent<Text> ().text = GS.SetText ("Sound Effects: " + (int)(GS.AudioVolume * 100f), "Głośność Dźwięków: " + (int)(GS.AudioVolume * 100f));
			LanguageButton.GetComponent<Text> ().text = GS.SetText ("Language: " + GS.Language, "Język: " + GS.Language);
			InvertedButton.GetComponent<Text> ().text = GS.SetText ("Inverted Mouse: " + GS.InvertedMouse, "Odwrócona Mysz: " + GS.InvertedMouse);
			OptionsBack.GetComponent<Text> ().text = GS.SetText ("Back", "Wróć");

			if (LanguageButton.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				if (GS.Language == "English") GS.Language = "Polski";
				else GS.Language = "English";
			} else if (VolumeButton.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				GS.AudioVolume += (1f + GS.AudioVolume + 0.1f) % 1f;
			} else if (MusicVolumeButton.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				GS.MusicVolume += (1f + GS.MusicVolume + 0.1f) % 1f;
			} else if (InvertedButton.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				GS.InvertedMouse = !GS.InvertedMouse;
			} else if (OptionsBack.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				CurrentWindow = "Main";
			}

		} else if (CurrentWindow == "Credits") {

			Credits.text = GS.SetText (
				"Credits\n\n\nGame created by:\nGMPguy\n\nTools used:\nUnity\nBlender\nPaint 3D\nAudacity\n\nMusic used:\n\"Almost New\" \"Take a Chance\" \"The Reveal\" \"The Descent\" \"Dream Culture\" \"Five Armies\" \"Chase\"\n Kevin MacLeod (incompetech.com) Licensed under Creative Commons: By Attribution 3.0 License http://creativecommons.org/licenses/by/3.0/\n\n\nCreated for Game Off 2018", 
				"Lista Płac\n\n\nGra zrobiona przez:\nGMPguy\n\nUżyte narzędzia:\nUnity\nBlender\nPaint 3D\n\nUżyta muzyka:\n\"Almost New\" \"Take a Chance\" \"The Reveal\" \"The Descent\" \"Dream Culture\" \"Five Armies\" \"Chase\"\n Kevin MacLeod (incompetech.com) Licensed under Creative Commons: By Attribution 3.0 License http://creativecommons.org/licenses/by/3.0/\n\n\nGra stworzona na Game Off 2018");
			CreditsBack.GetComponent<Text> ().text = GS.SetText ("Back", "Wróć");

			if (CreditsBack.GetComponent<ButtonScript> ().IsSelected == true && Input.GetMouseButtonDown (0)) {
				CurrentWindow = "Main";
			}

		} else if (CurrentWindow == "GameOver") {

			if(CurrentMusic != "GameOver"){
				PlayMusic ("GameOver");
			}

			GameOverText.text = GS.SetText ("Game Over", "Koniec Gry");
			GameOverText2.text = GS.SetText ("Score: " + GS.PreviousScore + "\nPress ESC", "Wynik: " + GS.PreviousScore + "\nNaciśnij ESC");
			GameOverText.transform.position = Vector3.MoveTowards (GameOverText.transform.position, AnchorUp.transform.position, ((float)Screen.height / 1000f) * (Time.deltaTime * 100f));
			GameOverImage.color = new Color32 (0, 0, 0, (byte)(128f + ((Vector3.Distance (GameOverText.transform.position, AnchorUp.transform.position) / (Screen.height * 0.5f)) * 128f)));

			MainCamera.transform.position = DeadCamera.transform.position;
			MainCamera.transform.rotation = DeadCamera.transform.rotation;

			foreach(Transform ChoosenPlane in DeadPlanes.transform){
				if (ChoosenPlane.name == GS.PreviousPlane) {
					ChoosenPlane.gameObject.SetActive (true);
				} else {
					ChoosenPlane.gameObject.SetActive (false);
				}
			}

			if(GameOverText.transform.position == AnchorUp.transform.position && GameOverText2.color.a < 255){
				GameOverText2.color += new Color32 (0, 0, 0, 5);
			}

			if(Input.GetKeyDown(KeyCode.Escape)){
				CurrentWindow = "Start";
			}

		}
		// Windows actions

		// Main Menu Plane
		if(CurrentWindow == "Start" || CurrentWindow == "Main" || CurrentWindow == "Options" || CurrentWindow == "NewGame" || CurrentWindow == "Play" || CurrentWindow == "NewGameInfo" || CurrentWindow == "Credits"){
			
			if(CurrentMusic != "Main"){
				PlayMusic ("Main");
			}

			MainCamera.transform.position = Vector3.Lerp(MainCamera.transform.position, GameObject.Find ("StartCamera").transform.position, 0.1f * (Time.deltaTime * 100f));
			MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, GameObject.Find ("StartCamera").transform.rotation, 0.1f * (Time.deltaTime * 100f));

			if (StartLandsPosition < 1000f) {
				foreach(Transform Land in StartLands.transform){
					if(Land.name == "Land1"){
						foreach(Transform LiL1 in Land){
							if(LiL1.name == SLand1){
								LiL1.gameObject.SetActive(true);
								LiL1.eulerAngles = new Vector3 (-90f, 0f, (float)SRLand1 * 90f);
							} else {
								LiL1.gameObject.SetActive(false);
							}
						}
					} else if(Land.name == "Land2"){
						foreach(Transform LiL2 in Land){
							if(LiL2.name == SLand2){
								LiL2.gameObject.SetActive(true);
								LiL2.eulerAngles = new Vector3 (-90f, 0f, (float)SRLand2 * 90f);
							} else {
								LiL2.gameObject.SetActive(false);
							}
						}
					} else if(Land.name == "Land3"){
						foreach(Transform LiL3 in Land){
							if(LiL3.name == SLand3){
								LiL3.gameObject.SetActive(true);
								LiL3.eulerAngles = new Vector3 (-90f, 0f, (float)SRLand3 * 90f);
							} else {
								LiL3.gameObject.SetActive(false);
							}
						}
					}
				}
				StartLandsPosition += 10f * (Time.deltaTime * 100f);
				StartLands.transform.position = new Vector3 (0f, 2000f, 0f + StartLandsPosition);
			} else {
				StartLandsPosition = 0f;

				SLand3 = SLand2;
				SRLand3 = SRLand2;
				SLand2 = SLand1;
				SRLand2 = SRLand1;
				SRLand1 = Random.Range (1, 4);
				int SSLand1 = Random.Range (0, 3);
				if(SSLand1 == 0){
					SLand1 = "EmptyLand";
				} else if(SSLand1 == 1){
					SLand1 = "Forest";
				} else if(SSLand1 == 2){
					SLand1 = "Village";
				}

			}

		}
		if (Vector3.Distance(MainMenuPlane.transform.position, MainMenuPlane.transform.parent.position + SetPos) < 0.2f) {
			SetPos = new Vector3(
				0f,
				Mathf.Clamp(SetPos.y + Random.Range(-0.5f, 0.5f), -3f, -4f),
				Mathf.Clamp(SetPos.z + Random.Range(-0.5f, 0.5f), -4f, 4f)
			);
		}
		MainMenuPlane.transform.GetChild (0).Rotate (-20f * (Time.fixedDeltaTime * 100f), 0f, 0f);
		MainMenuPlane.transform.LookAt (MainMenuPlane.transform.parent.position, Vector3.left);
		MainMenuPlane.transform.position = Vector3.MoveTowards (MainMenuPlane.transform.position, MainMenuPlane.transform.parent.position + SetPos, 0.0025f * (Time.deltaTime * 100f));
        // Main Menu Plane

        // Campaign Menu
        // Free rotation
        if (Input.GetAxis("Rudder") != 0f){
            CampaignCamRotation -= Input.GetAxis("Rudder");
            CapmaignFreeRotation = 5f;
        }
        // Free roation
        if (CampaignCamRotation < 360f) {
            if (CapmaignFreeRotation > 0f){
                CapmaignFreeRotation -= 0.01f * (Time.deltaTime * 100f);
            } else {
                CampaignCamRotation += 0.1f * (Time.deltaTime * 100f);
            }
		} else if (CampaignCamRotation > 360f){
            CampaignCamRotation = 0f;
        } else if (CampaignCamRotation < 0f){
            CampaignCamRotation = 360f;
        }
        if (CurrentWindow == "CampaignMain" || CurrentWindow == "CampaignCustomization" || CurrentWindow == "CampaignMessage") {

			if(CurrentMusic != "Campaign" && Loading <= 0f){
				PlayMusic ("Campaign");
			}

			GameObject.Find ("CampaignCamera").transform.rotation = Quaternion.Euler (0f, CampaignCamRotation, 0f);
			MainCamera.transform.position = GameObject.Find ("CampaignCamera").transform.position + GameObject.Find ("CampaignCamera").transform.forward * 10f + Vector3.up * 5f;
			MainCamera.transform.LookAt (GameObject.Find ("CampaignCamera").transform.position + (Vector3.up * 2f));

			PCPlaneModel = GS.CurrentPlaneModel;

			PCEngineModel = GS.CurrentEngineModel;

			PCPaint = GS.CurrentPaint;

			foreach(Transform SelectedPlane in PlaneinCustomization.transform){
				if(SelectedPlane.name == PCPlaneModel){
					SelectedPlane.gameObject.SetActive(true);
					foreach(Transform Part in SelectedPlane){
						if (Part.name == PCEngineModel || Part.name == PCPlaneModel || (Part.name == "Turret" && GS.CurrentAddition == "Turret")) {
							Part.gameObject.SetActive (true);
                            if (Part.name == PCPlaneModel || Part.name == PCEngineModel) {
                                foreach (Material PlaneMat in Part.GetComponent<MeshRenderer>().materials) {
                                    if (PlaneMat.name == "PlaneColor1 (Instance)") {
										switch(PCPaint){
                                            case "Basic Paint": PlaneMat.color = new Color32(200, 200, 200, 255); break;
                                            case "Present Colors": PlaneMat.color = new Color32(0, 125, 0, 255); break;
                                            case "Monochrome": PlaneMat.color = new Color32(100, 100, 100, 255); break;
                                            case "Night": PlaneMat.color = new Color32(0, 100, 200, 255); break;
                                            case "War Paint": PlaneMat.color = new Color32(100, 175, 0, 255); break;
                                            case "Toy Paint": PlaneMat.color = new Color32(0, 100, 200, 255); break;
                                            case "Girly": PlaneMat.color = new Color32(200, 0, 200, 255); break;
                                            case "Black and White": PlaneMat.color = new Color32(225, 225, 225, 255); break;
                                            case "Royal": PlaneMat.color = new Color32(0, 75, 255, 255); break;
                                            case "Aggressive": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Desert": PlaneMat.color = new Color32(255, 240, 220, 255); break;
                                            case "Rich": PlaneMat.color = new Color32(255, 255, 255, 255); break;
										}
                                    } else if (PlaneMat.name == "PlaneColor2 (Instance)") {
										switch(PCPaint){
                                            case "Basic Paint": PlaneMat.color = new Color32(200, 100, 100, 255); break;
                                            case "Present Colors": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Monochrome": PlaneMat.color = new Color32(55, 55, 55, 255); break;
                                            case "Night": PlaneMat.color = new Color32(5, 5, 55, 255); break;
                                            case "War Paint": PlaneMat.color = new Color32(55, 75, 55, 255); break;
                                            case "Toy Paint": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Girly": PlaneMat.color = new Color32(100, 0, 100, 255); break;
                                            case "Black and White": PlaneMat.color = new Color32(5, 5, 5, 255); break;
                                            case "Royal": PlaneMat.color = new Color32(255, 255, 0, 255); break;
                                            case "Aggressive": PlaneMat.color = new Color32(5, 5, 5, 255); break;
                                            case "Desert": PlaneMat.color = new Color32(155, 115, 85, 255); break;
                                            case "Rich": PlaneMat.color = new Color32(255, 190, 0, 255); break;
                                        }
                                    }
                                }
                            } else if (Part.name == "Turret") {
                                foreach (Material PlaneMat in Part.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().materials){
                                    if (PlaneMat.name == "PlaneColor1 (Instance)") {
										switch(PCPaint){
                                            case "Basic Paint": PlaneMat.color = new Color32(200, 200, 200, 255); break;
                                            case "Present Colors": PlaneMat.color = new Color32(0, 125, 0, 255); break;
                                            case "Monochrome": PlaneMat.color = new Color32(100, 100, 100, 255); break;
                                            case "Night": PlaneMat.color = new Color32(0, 100, 200, 255); break;
                                            case "War Paint": PlaneMat.color = new Color32(100, 175, 0, 255); break;
                                            case "Toy Paint": PlaneMat.color = new Color32(0, 100, 200, 255); break;
                                            case "Girly": PlaneMat.color = new Color32(200, 0, 200, 255); break;
                                            case "Black and White": PlaneMat.color = new Color32(225, 225, 225, 255); break;
                                            case "Royal": PlaneMat.color = new Color32(0, 75, 255, 255); break;
                                            case "Aggressive": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Desert": PlaneMat.color = new Color32(255, 240, 220, 255); break;
                                            case "Rich": PlaneMat.color = new Color32(255, 255, 255, 255); break;
										}
                                    } else if (PlaneMat.name == "PlaneColor2 (Instance)") {
										switch(PCPaint){
                                            case "Basic Paint": PlaneMat.color = new Color32(200, 100, 100, 255); break;
                                            case "Present Colors": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Monochrome": PlaneMat.color = new Color32(55, 55, 55, 255); break;
                                            case "Night": PlaneMat.color = new Color32(5, 5, 55, 255); break;
                                            case "War Paint": PlaneMat.color = new Color32(55, 75, 55, 255); break;
                                            case "Toy Paint": PlaneMat.color = new Color32(200, 0, 0, 255); break;
                                            case "Girly": PlaneMat.color = new Color32(100, 0, 100, 255); break;
                                            case "Black and White": PlaneMat.color = new Color32(5, 5, 5, 255); break;
                                            case "Royal": PlaneMat.color = new Color32(255, 255, 0, 255); break;
                                            case "Aggressive": PlaneMat.color = new Color32(5, 5, 5, 255); break;
                                            case "Desert": PlaneMat.color = new Color32(155, 115, 85, 255); break;
                                            case "Rich": PlaneMat.color = new Color32(255, 190, 0, 255); break;
                                        }
                                    }
                                }
                            }
						} else {
							Part.gameObject.SetActive (false);
						}
					}
				} else {
					SelectedPlane.gameObject.SetActive(false);
				}
			}

		}
		// Campaign Menu

		// Loading
		if(WindowToLoad != ""){
			if (TimeUntilLoad > 0f) {
				Loading = 1f;
				TimeUntilLoad -= 0.01f * (Time.fixedDeltaTime * 100f);
			} else {
				GS.LoadLevel(WindowToLoad);
			}
		}
		// Loading
		
	}

	void PlayMusic(string MusicName){
		foreach(Transform Music in Musics.transform){
			if (Music.name == MusicName) {
				Music.GetComponent<AudioSource> ().Play ();
				CurrentMusic = MusicName;
			} else {
				Music.GetComponent<AudioSource> ().Stop ();
			}
		}
	}

    void SettingMessage(){

        if (GS.HasDied == true) {
            if (GS.Parachutes > 0) {
                MessageText.text = GS.SetText(
                "You have crashed. All of the score and mooney you have earned in this round, have been lost, and you also have to repeat this round. Fortunetaly, you still have " + GS.Parachutes + " parachutes left!",
                "Rozbiłeś się. Twój wynik oraz piniądze zdobyte w tej rundzie, zostały utracone, i musisz powtórzyć tą rundę. Na szczęscie, jeszcze masz " + GS.Parachutes + " spadochron/y/ów!");
            } else {
                MessageText.text = GS.SetText(
                "You have crashed. All of the score and mooney you have earned in this round, have been lost, and you also have to repeat this round. You also don't have any parachutes left!\n\nIF YOU CRASH AGAIN, IT'S GAME OVER",
                "Rozbiłeś się. Twój wynik oraz piniądze zdobyte w tej rundzie, zostały utracone, i musisz powtórzyć tą rundę. Warto wspomnieć, że nie masz już ani jednego spadochronu!\n\nJEŚLI ZNOWU SIĘ ROZBIJESZ, TO KONIEC GRY");
            }
        } else if (GS.Level == 2) {
            MessageText.text = GS.SetText(
                "So, you've beaten the first level. Easy, wasn't it? Well, it's gonna get harder from here. In the second level, there will be enemy planes patroling the map. If you get too close to one of them, they will start shooting at you, so you better prepare yourself for a little bit of dogfighting! Oh, and BTW, this is the menu, where you're sent after every level. You can customize your plane, read new messages, and save your progress in here!",
                "Widocznie udało ci się zaliczyć pierwszy poziom. Łatwizna, no nie? No cóż, z tąd już będzie tylko pod górkę. W drugim poziomie, pojawią się wrogie samoloty. Jeżeli podlecisz zbyt blisko jednego z nich, zaczną do ciebie strzelać, tak więc przygotuj się na walkę! Aha, i jeszcze jedno, to jest menu do którego będziesz wysyłany po zaliczeniu każdego z poziomów. Tutaj, możesz edytować swój samolot, czytać nowe wiadomości, oraz zapisywać swój postęp!");
        } else if (GS.Level == 3) {
            MessageText.text = GS.SetText(
                "If you're reading this, then that means you've survived! Congratulations! Just keep in mind, that you don't have to eliminate all of the foes, but each foe gives you some mooney and score!",
                "Jeśli to czytasz, to znaczy że przeżyłeś! Gratulacje! Miej tylko na uwadze, że nie musisz eliminować wszystkich przeciwników, aczkolwiek każdy przeciwnik daje ci trochę piniędzy i punktów!");
        } else if (GS.Level == 4) {
            MessageText.text = GS.SetText(
                "More houses! Now, you'll have to deliver presents to two houses, instead of one. Every four levels, an additional house will be spawned.",
                "Zaraz będzie więcej domów! Teraz, zamiast dostarczyć prezent do jednego domu, będziesz musiał dostarczyć dwa prezenty do dwóch domów. Co cztery poziomy, dodatkowy dom będzie się pojawiał.");
        } else if (GS.Level == 5) {
            MessageText.text = GS.SetText(
                "The enemy is not only growing, but is also evolving! Now, aside from bigger amount of foes, there are new types of enemies to deal with. These new types are: Messerschmitt K4, and AA Gun. Messerschmitt K4, is pretty much the same as your regular Messerschmitt, except it has twice as much health, and has M2 Browning guns, instead of Vickers. AA Gun on the other hand, is immobile, but is constantly aiming and shooting at you, so be careful!",
                "Wróg nie tylko się powiększa, ale także ewouluje! Teraz, oprócz większej ilości nieprzyjaciół, są nowe rodzaje nieprzyjaciół. Tymi nowymi rodzajami są: Messerschmitt K4, oraz Broń Przeciwlotnicza. Messerschmitt K4, jest prawie taki sam jak zwykły Messerschmitt, z taką różnicą: że ma dwukrotnie więcej zdrowia, i ma broń M2 Browning, zamiast Vickers. Broń Przeciwlotnicza, jest nieruchoma, aczkolwiek cały czas w ciebie celuje i strzela, tak więc bądź ostrożny!");
        } else if (GS.Level == 10) {
            MessageText.text = GS.SetText(
                "Those german scientis have came up with new ways to eliminate you! They have created: Messerschmitt 110, and Balloons! Messerschmitt 110 is yet again pretty much the same as the previous models, the difference is that it has Cannons instead of Vickers, and that it has a backgunner (if you get behind this plane, it's backgunner will start shooting straight at you). Balloons are, well, AA Guns, except they are suspended in air. These two, are surely dangerous, so watch out!",
                "Ci niemieccy naukowcy wynaleźli nowe sposoby na twoją eliminację! Stworzyli: Messerschmitt 110, oraz balony! Messerschmit 110, jest taki sam jak inne messerschmitty, różnica jest taka, że ma Cannon'y zamiast Vickers, i że ma tylniego strzelca (jeśli znajdziesz się za tym samolotem, jego strzelec tylni zacznie strzelać prosto na ciebie). Czym są balony? To poprostu bronie przeciwlotnicze, tylko że wiszące w powietrzu. Te dwa wynalazki, są z pewnością niebezpieczne, tak więc uważaj na siebie!");
        } else if (GS.Level == 10) {
            MessageText.text = GS.SetText(
                "Having troubles with dogfighting? No? Very good, because they've created a new type of plane: Messerschmitt Me 262! This bad boy is as tuff as a Messerschmitt K4, is twice as fast as regular Messerschmitt, and has a powerful Jet Gun! With these fellas, the dogfighting is sure to become more interesting!",
                "Masz problemy z wrogimi samolotami? Nie? To bardzo dobrze, bo na scenę wchodzi nowy rodzaj samolotów: Messerschmitt Me 262! Ten zły chłopiec jest tak silny jak Messerschmitt K4, jest podwójnie szybszy od zwykłego Messerschmitt'a, i ma potężną broń Jet Gun! Z tymi delikwentami, walka stanie się bardzo interesująca!");
        } else {
            MessageText.text = "";
        }

    }

	void CampaignCustomization(){

		if (currentCustomizationOption == "") {
			WhichPage = 1;
			CurrentCustomizationOptionText.text = GS.SetText ("", "");
			OptionInfoName.text = "";
			OptionInfoDescription.text = "";
			CustomizationButtons.SetActive (true);
			CustomizationButtons2.SetActive (false);
			foreach (Transform Button in CustomizationButtons.transform.GetChild(0)) {
				if (Button.gameObject.name == "1") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Plane Model:\n" + GS.CurrentPlaneModel, "Model Samolotu:\n" + GS.CurrentPlaneModel);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Plane Model";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "2") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Engine Model:\n" + GS.CurrentEngineModel, "Model Silnika:\n" + GS.CurrentEngineModel);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Engine Model";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "3") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Gun Type:\n" + GS.CurrentGunType, "Typ Broni:\n" + GS.CurrentGunType);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Gun Type";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "4") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Present Cannon Type:\n" + GS.CurrentPresentCannonType, "Typ Wyrzutni Prezentów:\n" + GS.CurrentPresentCannonType);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Present Cannon Type";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "5") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Special Type:\n" + GS.CurrentSpecialType, "Typ Ekwipunku Specjalnego:\n" + GS.CurrentSpecialType);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Special Type";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "6") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Addition:\n" + GS.CurrentAddition, "Dodatek:\n" + GS.CurrentAddition);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Addition";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				} else if (Button.gameObject.name == "7") {
					Button.gameObject.GetComponent<Text> ().text = GS.SetText ("Paint:\n" + GS.CurrentPaint, "Farba:\n" + GS.CurrentPaint);
					if (Button.gameObject.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
						currentCustomizationOption = "Paint";
						Button.gameObject.GetComponent<ButtonScript> ().IsSelected = false;
						Click.Play ();
					}
				}
			}
		} else {
			CustomizationButtons.SetActive (false);
			CustomizationButtons2.SetActive (true);
			bool CanNext = false;
			bool CanPrev = false;
			foreach(Transform Window in CustomizationButtons2.transform){
				if (Window.name == currentCustomizationOption + WhichPage || Window.name == "PreviousPage" || Window.name == "NextPage") {
					Window.gameObject.SetActive (true);
				} else {
					if(Window.name == currentCustomizationOption + (WhichPage + 1)){
						CanNext = true;
					} else if(Window.name == currentCustomizationOption + (WhichPage - 1)){
						CanPrev = true;
					}
					Window.gameObject.SetActive (false);
				}
			}
			// Change page
			if (CanNext == true) {
				CC2Next.GetComponent<Text> ().text = GS.SetText ("Next\n Page", "Następna\n Strona");
				CC2Next.GetComponent<ButtonScript> ().IsActive = true;
				if (CC2Next.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
					WhichPage += 1;
				}
			} else {
				CC2Next.GetComponent<Text> ().text = GS.SetText ("", "");
				CC2Next.GetComponent<ButtonScript> ().IsActive = false;
			}
			if (CanPrev == true) {
				CC2Prev.GetComponent<Text> ().text = GS.SetText ("Previous\n Page", "Poprzednia\n Strona");
				CC2Prev.GetComponent<ButtonScript> ().IsActive = true;
				if (CC2Prev.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
					WhichPage -= 1;
				}
			} else {
				CC2Prev.GetComponent<Text> ().text = GS.SetText ("", "");
				CC2Prev.GetComponent<ButtonScript> ().IsActive = false;
			}
			// Change page

			// Set textes
			string[] CCOT = new string[]{"", ""};
			switch(currentCustomizationOption){
				case "Plane Model": CCOT = new string[]{ "Plane Model", "Model Samolotu" }; break;
				case "Engine Model": CCOT = new string[]{ "Engine Model", "Model Silnika" }; break;
				case "Gun Type": CCOT = new string[]{ "Gun Type", "Typ Broni" }; break;
				case "Present Cannon Type": CCOT = new string[]{ "Present Cannon Type", "Typ Wyrzutni Prezentów" }; break;
				case "Special Type": CCOT = new string[]{ "Special Type", "Typ Ekwipunku Specjalnego" }; break;
				case "Addition": CCOT = new string[]{ "Addition", "Dodatek" }; break;
				case "Paint": CCOT = new string[]{ "Paint", "Farba" }; break;
			}
			// Set textes

		}

	}

}
