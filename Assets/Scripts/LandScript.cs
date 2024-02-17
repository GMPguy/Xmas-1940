using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandScript : MonoBehaviour {

	public GameObject plains;

	public string SkyColor = "Blue1";
	public Skybox MainSkybox;

	public void spawnLand(string LandType, int Size, float Seed, int SkyPick){

		// Land
		switch(LandType){
			case "Plains":
				for(int x = 0; x < Size; x++) for(int y = 0; y < Size; y++) {
					GameObject NewLand = Instantiate(plains) as GameObject;
					NewLand.transform.SetParent(this.transform);
					NewLand.transform.localPosition = new Vector3(x*1000, 0, y*1000) - new Vector3(1000f, 0f, 1000f)*(Size/2f);

					float[] ChostLand = new float[]{ 
						Mathf.PerlinNoise( (NewLand.transform.position.x / 0.3333f) + Seed, (NewLand.transform.position.z / 0.3333f) + Seed),
						Mathf.PerlinNoise( (NewLand.transform.position.z / 0.3333f) + Seed, (NewLand.transform.position.x / 0.3333f) + Seed)
					};

					NewLand.transform.GetChild((int)(ChostLand[0]*2.9f)).gameObject.SetActive(true);
					GameObject Land = NewLand.transform.GetChild((int)(ChostLand[0]*2.9f)).gameObject;
					NewLand.transform.Rotate(Vector3.up * ((int)(ChostLand[1]*3.9f) * 90f));

					foreach(Material Mat in Land.GetComponent<MeshRenderer>().materials){
						if(Mat.name == "Building1 (Instance)" || Mat.name == "Building2 (Instance)" || Mat.name == "Building3 (Instance)" || Mat.name == "Bulding4 (Instance)" || Mat.name == "Bulding5 (Instance)"){
							Mat.color = new Color32 ((byte)Random.Range(25, 255), (byte)Random.Range(25, 255), (byte)Random.Range(25, 255), (byte)255);
						}
					}

				}
				break;
		}
		// Land

		// Lighting
		int ChooseSky = SkyPick;
		string[] SkyColors = new string[]{"Blue", "DarkBlue", "Violet", "Orange"};
		SkyColor = SkyColors[ChooseSky];

		Color32 Sky = new(0,0,0,0);
		float SkySize = 0.5f;
		Color32 Ambient = new(0,0,0,0);
		Color32 Fog = new(0,0,0,0);
		switch(SkyColor){
			case "Blue":
				Sky = new Color32(0, 64, 125, 255);
				Ambient = new Color32 (25, 64, 128, 255);
				Fog = new Color32 (72, 108, 179, 255);
				GameObject.Find ("MainLight").GetComponent<Light> ().color = new Color32 (0, 55, 75, 255);
				GameObject.Find ("MainLight").transform.eulerAngles = new Vector3 (45f, 0f, 0f);
				break;
			case "DarkBlue":
				Sky = new Color32(0, 125, 255, 255);
				SkySize = 0.2f;
				Ambient = new Color32 (0, 0, 55, 255);
				Fog = new Color32 (13, 32, 99, 255);
				GameObject.Find ("MainLight").GetComponent<Light> ().color = new Color32 (0, 125, 255, 255);
				GameObject.Find ("MainLight").transform.eulerAngles = new Vector3 (10f, 0f, 0f);
				break;
			case "Violet":
				Sky = new Color32(100, 0, 75, 255);
				Ambient = new Color32 (37, 35, 58, 255);
				Fog = new Color32 (77, 70, 116, 255);
				GameObject.Find ("MainLight").GetComponent<Light> ().color = new Color32 (75, 65, 116, 255);
				GameObject.Find ("MainLight").transform.eulerAngles = new Vector3 (10f, 0f, 0f);
				break;
			case "Orange":
				Sky = new Color32(255, 125, 0, 255);
				Ambient = new Color32 (100, 50, 50, 255);
				Fog = new Color32 (100, 50, 50, 255);
				GameObject.Find ("MainLight").GetComponent<Light> ().color = new Color32 (125, 75, 0, 255);
				GameObject.Find ("MainLight").transform.eulerAngles = new Vector3 (30f, 180f, 0f);
				break;
		}

		RenderSettings.ambientLight = Ambient;

		RenderSettings.skybox.SetColor("_SkyTint", Sky);
		RenderSettings.skybox.SetFloat("_AtmosphereThickness", SkySize);
		RenderSettings.skybox.SetColor("_GroundColor", Fog);
		RenderSettings.fogColor = GameObject.Find ("MainCamera").GetComponent<Camera> ().backgroundColor = Fog;
		// Lighting

	}

}
