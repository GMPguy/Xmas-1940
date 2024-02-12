using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButtonScript : MonoBehaviour {

	public int Index;
	GameScript GS;
	Text MainT;
	MenuScript MS;

	public string WhichOption = "";
	public string Name = "";
	public bool Owned = false;
	public int Price = 0;
	public string EnglishDesc = "";
	public string PolishDesc = "";

	public string DisplA;
	public string DisplB;
	public string DisplC;
	public string DisplD;
	public string DisplE;

	// Use this for initialization
	void Start () {

		GS = GameObject.Find("GameScript").GetComponent<GameScript>();
		MainT = this.GetComponent<Text>();
		MS = GameObject.Find ("MainMenu").GetComponent<MenuScript> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		Sett ();

		if (GameObject.Find ("MainMenu").GetComponent<MenuScript> ().currentCustomizationOption == WhichOption) {

			// Set text
			string CurrentItem = "";
			if (WhichOption == "Plane Model") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentPlaneModel;
			} else if (WhichOption == "Engine Model") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentEngineModel;
			} else if (WhichOption == "Gun Type") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentGunType;
			} else if (WhichOption == "Present Cannon Type") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentPresentCannonType;
			} else if (WhichOption == "Special Type") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentSpecialType;
			} else if (WhichOption == "Addition") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentAddition;
			} else if (WhichOption == "Paint") {
				CurrentItem = GameObject.Find ("GameScript").GetComponent<GameScript> ().CurrentPaint;
			}
			if (Owned == true) {
				if (Name == CurrentItem) MainT.text = GS.SetText(Name + "\nequipped", Name + "\nzamontowany");
				else MainT.text = GS.SetText(Name, Name);
			} else MainT.text = GS.SetText(Name + "\n" + Price + " mooney", Name + "\n" + Price + " piniądzy");
			// Set text

			// Buying or equiping
			if (this.GetComponent<ButtonScript> ().IsSelected && Input.GetMouseButtonDown (0)) {
				if (Owned == true) {
					switch(WhichOption){
						case "Plane Model": GS.CurrentPlaneModel = Name; break;
						case "Engine Model": GS.CurrentEngineModel = Name; break;
						case "Gun Type": GS.CurrentGunType = Name; break;
						case "Present Cannon Type": GS.CurrentPresentCannonType = Name; break;
						case "Special Type": GS.CurrentSpecialType = Name; break;
						case "Addition": GS.CurrentAddition = Name; break;
						case "Paint": GS.CurrentPaint = Name; break;
					}
				} else if (GameObject.Find ("GameScript").GetComponent<GameScript> ().Mooney >= Price) {
					GameObject.Find ("GameScript").GetComponent<GameScript> ().Mooney -= Price;

					string replace(string sauce){
						sauce = sauce.Remove(Index, 1); sauce = sauce.Insert(Index, "1"); return sauce;
					}

					switch(WhichOption){
						case "Plane Model": GS.OwnedPlaneModels = replace(GS.OwnedPlaneModels); break;
						case "Engine Model": GS.OwnedEngineModels = replace(GS.OwnedEngineModels); break;
						case "Gun Type": GS.OwnedGundTypes = replace(GS.OwnedGundTypes); break;
						case "Present Cannon Type": GS.OwnedPresentCannonTypes = replace(GS.OwnedPresentCannonTypes); break;
						case "Special Type": GS.OwnedSpecialTypes = replace(GS.OwnedSpecialTypes); break;
						case "Addition": GS.OwnedAdditions = replace(GS.OwnedAdditions); break;
						case "Paint": GS.OwnedPaints = replace(GS.OwnedPaints); break;
					}
				}
			}
			// Buying or equiping

			// Setting option info
			if (this.GetComponent<ButtonScript> ().IsSelected == true) {
				MS.EraseOptionInfo = 3f;
				MS.OptionInfoName.text = Name;

				switch(WhichOption){
					case "Plane Model":
						MS.OptionInfoDescription.text = GS.SetText("Max Health/Fuel: " + DisplA + "\nMax Speed: " + DisplB + "\nRotation Speed: " + DisplC + "\nAmount of Guns: " + DisplD + "\nClass: " + DisplE + "\n\n" + EnglishDesc, "Zdrowie/Paliwo: " + DisplA + "\nPrędkość: " + DisplB + "\nPrędkość Obrotowa: " + DisplC + "\nIlość Broni: " + DisplD + "\nKlasa: " + DisplE + "\n\n" + PolishDesc) ;
						break;
					case "Engine Model":
						MS.OptionInfoDescription.text = GS.SetText("Speed Multiplier: " + DisplA + "\n\n" + EnglishDesc, "Mnożnik Prędkości: " + DisplA + "\n\n" + PolishDesc);
						break;
					case "Gun Type":
						MS.OptionInfoDescription.text = GS.SetText("Initial Ammo: " + DisplA + "\nCooldown: " + DisplB + "\nRange: " + DisplC + "\nSpeed: " + DisplD + "\n\n" + EnglishDesc, "Amunicja: " + DisplA + "\nPrędkość Strzelania: " + DisplB + "\nDystans: " + DisplC + "\nPrędkość: " + DisplD + "\n\n" + PolishDesc);
						break;
					case "Present Cannon Type":
						MS.OptionInfoDescription.text = GS.SetText("Range: " + DisplA + "\nCooldown: " + DisplB + "\n\n" + EnglishDesc, "Dystans: " + DisplA + "\nPrędkość Strzelania: " + DisplB + "\n\n" + PolishDesc);
						break;
					case "Special Type":
						MS.OptionInfoDescription.text = GS.SetText("Required Ammo: " + DisplA + "\nCooldown: " + DisplB + "\n\n" + EnglishDesc, "Wymagana Amunicja: " + DisplA + "\nPrędkość Używania: " + DisplB + "\n\n" + PolishDesc);
						break;
					case "Addition":
						MS.OptionInfoDescription.text = GS.SetText(EnglishDesc, PolishDesc);
						break;
					case "Paint":
						MS.OptionInfoDescription.text = GS.SetText("First Color: " + DisplA + "\nSecond Color: " + DisplB + "\n\n" + EnglishDesc, "Pierwszy Kolor: " + DisplA + "\nDrugi Kolor: " + DisplB + "\n\n" + PolishDesc);
						break;
				}
			}
			// Setting option info

		}
		
	}

	public void Sett(){

		string Sample = "";
		switch(WhichOption){
			case "Plane Model": Sample = GS.OwnedPlaneModels; break;
			case "Engine Model": Sample = GS.OwnedEngineModels; break;
			case "Gun Type": Sample = GS.OwnedGundTypes; break;
			case "Present Cannon Type": Sample = GS.OwnedPresentCannonTypes; break;
			case "Special Type": Sample = GS.OwnedSpecialTypes; break;
			case "Addition": Sample = GS.OwnedAdditions; break;
			case "Paint": Sample = GS.OwnedPaints; break;
		}

		if(Sample.Substring(Index, 1) == "1") Owned = true;
		else Owned = false;

	}

}
