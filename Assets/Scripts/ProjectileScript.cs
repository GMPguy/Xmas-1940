using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

	public string TypeofGun = "";
	public GameObject WhoShot;
	public Transform GunFirePos;
	Vector3 StartPos;
	public GameObject PreviousPos;
	float Damage = 1f;
	float Speed = 1f;
	float Range = 100f;
	public float PreviusSpeed = 0f;
	public bool Hit = false;
	GameObject HomingMissileTarget;
	public float PresentRange = 0f;
    float LaserLifetime = 10f;

	public Color PresentColor1;
	public Color PresentColor2;

	// References
	public GameObject Effect;
	public GameObject Special;
	public GameObject ChostBullet;
	public GameObject ChostGunFire;

	public GameObject Vickers, Present, M2Browning, Cannon, Flammable, MuggerMissiles, Flak, HomingMissile, Laser, BlueLaser, Brick, Paintball, Rocket;
	public GameObject BasicGunFire, PresentCannon, BrowningGunFire, CannonGunFire, FlammableGunFire, FlakGunFire, HomingMissileGunFire, JetGunFire, LaserGunFire, BlueLaserGunFire, PaintballGunFire;

    // References

    // Use this for initialization
    void Start () {

		PreviousPos.transform.parent = null;
		PreviousPos.transform.position = this.transform.position - (this.transform.forward * 1f);
		PreviousPos.transform.LookAt(this.transform.position);
		StartPos = this.transform.position;

		float[] BulletVars = new float[]{0f, 0f, 0f};
		switch(TypeofGun){
		case "Vickers":
			ChostBullet = Vickers; ChostGunFire = BasicGunFire;
			BulletVars = new float[]{Random.Range(1f, 5f), 750f, 400f};
			break;
		case "M2 Browning":
			ChostBullet = M2Browning; ChostGunFire = BrowningGunFire;
			BulletVars = new float[]{Random.Range(1f, 5f), 1000f, 600f};
			break;
		case "M3 Browning":
			ChostBullet = M2Browning; ChostGunFire = BrowningGunFire;
			BulletVars = new float[]{Random.Range(3f, 7f), 1000f, 800f};
			break;
		case "Cannon":
			ChostBullet = Cannon; ChostGunFire = CannonGunFire;
			BulletVars = new float[]{Random.Range(10f, 15f), 500f, 300f};
			break;
		case "Flammable":
			ChostBullet = Flammable; ChostGunFire = FlammableGunFire;
			BulletVars = new float[]{Random.Range(1f, 2f), 750f, 400f};
			break;
		case "Mugger Missiles":
			ChostBullet = MuggerMissiles; ChostGunFire = FlammableGunFire;
			BulletVars = new float[]{Random.Range(1f, 2f), 750f, 400f};
			break;
		case "Flak":
			ChostBullet = Flak; ChostGunFire = FlakGunFire;
			BulletVars = new float[]{Random.Range(10f, 25f), 1000f, Random.Range(500f, 750f)};
			break;
		case "Jet Gun":
			ChostBullet = M2Browning; ChostGunFire = JetGunFire;
			BulletVars = new float[]{Random.Range(10f, 15f), 2000f, 1000f};
			break;
        case "Paintball":
			ChostBullet = Paintball; ChostGunFire = PaintballGunFire;
			BulletVars = new float[]{Random.Range(1f,2f), 500f, 600f};
			break;
        case "Rocket":
			ChostBullet = Rocket; ChostGunFire = HomingMissileGunFire;
			BulletVars = new float[]{Random.Range(50f, 100f), 500f, 600f};
			break;
        case "Laser":
            ChostBullet = Laser; ChostGunFire = LaserGunFire;
			BulletVars = new float[]{Random.Range(3f, 7f), PreviusSpeed, 500f};
			break;
        case "Blue Laser":
            ChostBullet = BlueLaser; ChostGunFire = BlueLaserGunFire;
			BulletVars = new float[]{Random.Range(6f, 10f), PreviusSpeed, 1000f};
			break;
        case "Present":
			ChostBullet = Present; ChostGunFire = PresentCannon;
			BulletVars = new float[]{0f, 200f, PresentRange};
			break;
		case "Homing Missile":
			ChostBullet = HomingMissile; ChostGunFire = HomingMissileGunFire;
			BulletVars = new float[]{Random.Range(50f, 100f), PreviusSpeed, 1000f};
			break;
        case "Brick":
			ChostBullet = Brick; ChostGunFire = CannonGunFire;
			BulletVars = new float[]{1000f, PreviusSpeed*2f, 5000f};
			break;
        }

		Damage = BulletVars[0];
		Speed = BulletVars[1];
		Range = BulletVars[2];

		ChostBullet.gameObject.SetActive (true);
		ChostGunFire.gameObject.SetActive (true);
		ChostGunFire.transform.parent = GunFirePos.transform;
		ChostGunFire.transform.position = GunFirePos.position;
		ChostGunFire.GetComponent<AudioSource>().Play();

        if (TypeofGun == "Paintball") {
			ParticleSystem.MainModule PC = Paintball.transform.GetChild(0).GetComponent<ParticleSystem>().main;
            PC.startColor = PresentColor1;
        }

    }
	
	// Update is called once per frame
	void FixedUpdate () {

		if(Speed < PreviusSpeed){
			Speed = PreviusSpeed;
		}

        this.transform.position += this.transform.forward * (Speed/90f);

		if(Vector3.Distance(this.transform.position, StartPos) > Range && Hit == false && TypeofGun != "Laser" && TypeofGun != "Blue Laser"){
			Hit = true;
			if(TypeofGun == "Flak" || TypeofGun == "Homing Missile"){
				GameObject Boom = Instantiate (Special) as GameObject;
				Boom.GetComponent<SpecialScript> ().TypeofSpecial = "Explosion";
				Boom.GetComponent<SpecialScript> ().ExplosionPower = 25f;
				Boom.GetComponent<SpecialScript> ().ExplosionRadius = 5f;
				Boom.transform.position = this.transform.position;
				if (WhoShot != null){
					if(WhoShot.tag == "Player"){
						Boom.GetComponent<SpecialScript> ().CausedByPlayer = true;
					}
				}
			}
		}

        // Laser Lifetime
        if (TypeofGun == "Laser" || TypeofGun == "Blue Laser") {
            if (LaserLifetime > 0f) {
                LaserLifetime -= 1f;
            } else {
                Hit = true;
            }
        }
        // Laser Lifetime

        // Raycast Detection
        if (TypeofGun == "Laser" || TypeofGun == "Blue Laser") {
            Ray CheckRaya = new(this.transform.position, this.transform.forward * 1f);
            if (Physics.Raycast(CheckRaya, out RaycastHit CheckRayaHit, Range)) {
                if (CheckRayaHit.collider != null){
                    BulletHit(CheckRayaHit.collider, CheckRayaHit.point);
                    print(CheckRayaHit.collider);
                }
            }
        } else {
            if (PreviousPos != null){
                if (PreviousPos.transform.position != this.transform.position){
                    PreviousPos.transform.LookAt(this.transform.position);
                    Ray CheckRay = new(PreviousPos.transform.position, PreviousPos.transform.forward * 1f);
                    if (Physics.Raycast(CheckRay, out RaycastHit CheckRayHit, Vector3.Distance(PreviousPos.transform.position, this.transform.position + this.transform.forward * 1f))){
                        if (CheckRayHit.collider != null){
                            BulletHit(CheckRayHit.collider, CheckRayHit.point);
                        }
                    }
                    PreviousPos.transform.position = this.transform.position - this.transform.forward * 1f;
                }
            }
        }
        // Raycast Detection

        // Projectiles special
        if (TypeofGun == "Present") {
            foreach (Material Mat in Present.transform.GetChild(0).GetComponent<MeshRenderer>().materials) {
                if (Mat.name == "Material1 (Instance)") {
                    Mat.color = PresentColor1;
                } else if (Mat.name == "Material2 (Instance)") {
                    Mat.color = PresentColor2;
                }
            }
        } else if (TypeofGun == "Homing Missile") {

            if (HomingMissileTarget != null) {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(new Vector3(HomingMissileTarget.transform.position.x, HomingMissileTarget.transform.position.y, HomingMissileTarget.transform.position.z) - this.transform.position), 0.1f);
            } else {
                if (WhoShot != null) {
                    if (WhoShot.GetComponent<PlayerScript>() != null) {
                        float Distance = 1000f;
                        GameObject ChoosenTarget = null;
                        foreach (GameObject Enemy in GameObject.FindGameObjectsWithTag("Foe")) {
                            if (Vector3.Distance(this.transform.position, Enemy.transform.position) < Distance) {
                                Distance = Vector3.Distance(this.transform.position, Enemy.transform.position);
                                ChoosenTarget = Enemy;
                            }
                        }
                        if (ChoosenTarget != null) {
                            HomingMissileTarget = ChoosenTarget;
                        }
                    }
                }
            }
            if (Speed < PreviusSpeed * 10f) {
                Speed += PreviusSpeed / 100f;
            } else {
                if (Hit == false) {
                    Hit = true;
                    GameObject Boom = Instantiate(Special) as GameObject;
                    Boom.GetComponent<SpecialScript>().TypeofSpecial = "Explosion";
                    Boom.GetComponent<SpecialScript>().ExplosionPower = 25f;
                    Boom.GetComponent<SpecialScript>().ExplosionRadius = 5f;
                    Boom.transform.position = this.transform.position;
                    if (WhoShot != null) {
                        if (WhoShot.tag == "Player") {
                            Boom.GetComponent<SpecialScript>().CausedByPlayer = true;
                        }
                    }
                }
            }
        } else if (TypeofGun == "Brick") {
            Brick.transform.Rotate(1f, 1f, 1f);
            this.transform.eulerAngles += new Vector3(0.1f, 0f, 0f);
        }
		// Projectiles special

		// Destroying projectile
		if(Hit == true){
			Destroy (PreviousPos);	
			switch(TypeofGun){
            case "Laser": case "Blue Laser":
                ChostBullet.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                if (ChostBullet.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f) {
                    Destroy(this.gameObject);
                }
				break;
            case "Brick": case "Paintball": case "Present":
                Destroy(this.gameObject);
				break;
			case "Homing Missile":
				ChostBullet.transform.GetChild (1).gameObject.SetActive (false);
				ChostBullet.transform.GetChild (0).GetComponent<ParticleSystem> ().Stop ();
				if (ChostBullet.transform.GetChild (0).GetComponent<ParticleSystem> ().particleCount <= 0f) {
					Destroy (this.gameObject);
				}
				break;
			default:
				ChostBullet.transform.GetChild (1).GetComponent<ParticleSystem> ().Stop ();
				ChostBullet.transform.GetChild (0).GetComponent<ParticleSystem> ().Stop ();
				if (ChostBullet.transform.GetChild (0).GetComponent<ParticleSystem> ().particleCount <= 0f) {
					Destroy (this.gameObject);
				}
				break;
			}
        }
		// Destroying projectile

		// Destroying gunfire
		if(ChostGunFire != null){
			if(ChostGunFire.transform.GetChild(0).GetComponent<ParticleSystem>() && ChostGunFire.transform.GetChild(0).GetComponent<ParticleSystem>().particleCount <= 0f)
				Destroy(ChostGunFire);
		}
		// Destroying gunfire
		
	}

    private void OnTriggerEnter(Collider col){
        BulletHit(col, this.transform.position);
    }

    void BulletHit(Collider Col, Vector3 HitPoint){

		if (Hit == false && Col.tag != "Cloud" && Col.tag != "Explosion") {

			if(TypeofGun == "Flammable"){
				int chance = Random.Range (0, 5);
				if(chance == 0){
					GameObject Boom = Instantiate (Special) as GameObject;
					Boom.GetComponent<SpecialScript> ().TypeofSpecial = "Explosion";
					Boom.GetComponent<SpecialScript> ().ExplosionPower = 25f;
					Boom.GetComponent<SpecialScript> ().ExplosionRadius = 5f;
					Boom.transform.position = HitPoint;
					if (WhoShot != null){
						if(WhoShot.tag == "Player"){
							Boom.GetComponent<SpecialScript> ().CausedByPlayer = true;
						}
					}
				}
			} else if (TypeofGun == "Flak" || TypeofGun == "Homing Missile" || TypeofGun == "Rocket"){
				Hit = true;
				GameObject Boom = Instantiate (Special) as GameObject;
				Boom.GetComponent<SpecialScript> ().TypeofSpecial = "Explosion";
				Boom.GetComponent<SpecialScript> ().ExplosionPower = 25f;
				Boom.GetComponent<SpecialScript> ().ExplosionRadius = 5f;
				Boom.transform.position = HitPoint;
				if (WhoShot != null){
					if(WhoShot.tag == "Player"){
						Boom.GetComponent<SpecialScript> ().CausedByPlayer = true;
					}
				}
			}

			bool FiredByEnemy = false;
			if(WhoShot != null){
				if(WhoShot.GetComponent<EnemyVesselScript>() != null){
					FiredByEnemy = true;
				}
			}

			if(Col.name == "Home"){
				if(TypeofGun == "Present"){
					Hit = true;
					Col.transform.parent.GetComponent<HomeScript> ().PresentGot ();
				}
			} else if(Col.transform.parent != null){
				if (Col.transform.parent.tag == "Terrain" && TypeofGun != "Paintball") {
					GameObject Efect = Instantiate (Effect) as GameObject;
					Efect.GetComponent<EffectScript> ().TypeofEffect = "BullethitGround";
					Efect.transform.position = new Vector3 (HitPoint.x, 0f, HitPoint.z);
					Hit = true;
				} else if (Col.transform.parent.parent != null) {
					if (Col.transform.parent.parent.name == "PlaneModels" && TypeofGun != "Present") {
						if (Col.transform.parent.parent.parent.gameObject != WhoShot) {
							GameObject Efect = Instantiate (Effect) as GameObject;
							Efect.GetComponent<EffectScript> ().TypeofEffect = "BullethitPlane";
							Efect.transform.position = Col.transform.position + new Vector3 (Random.Range (-2f, 2f), Random.Range (-2f, 2f), Random.Range (-2f, 2f));
							Col.transform.parent.parent.parent.GetComponent<PlayerScript> ().Hurt (Damage, 1, 1);
							Hit = true;
						}
					} else if (Col.transform.parent.parent.GetComponent<EnemyVesselScript>() != null && FiredByEnemy != true && TypeofGun != "Present") {
						if (Col.transform.parent.parent.gameObject != WhoShot) {
							GameObject Efect = Instantiate (Effect) as GameObject;
							Efect.GetComponent<EffectScript> ().TypeofEffect = "BullethitPlane";
							Efect.transform.position = Col.transform.position + new Vector3 (Random.Range (-2f, 2f), Random.Range (-2f, 2f), Random.Range (-2f, 2f));
							Col.transform.parent.parent.GetComponent<EnemyVesselScript> ().Health -= Damage;
                            if(TypeofGun == "Paintball"){
                                Col.transform.parent.parent.GetComponent<EnemyVesselScript>().Paintballed += Random.Range(0.5f, 1f);
							}
							Hit = true;
							if(WhoShot != null){
								if(WhoShot.GetComponent<PlayerScript>() != null){
									Col.transform.parent.parent.GetComponent<EnemyVesselScript> ().HitByAPlayer = 5f;
									if(TypeofGun == "Mugger Missiles"){
										WhoShot.GetComponent<PlayerScript> ().Health += (float)Random.Range (0, 5);
										WhoShot.GetComponent<PlayerScript> ().Fuel += (float)Random.Range (0, 5);
										WhoShot.GetComponent<PlayerScript> ().Ammo += Random.Range (0, 5);
									}
								}
							}
						}
					}
				}
			}

            if (TypeofGun == "Paintball") {
                GameObject Efect = Instantiate(Effect) as GameObject;
                Efect.GetComponent<EffectScript>().TypeofEffect = "PaintballHit";
				ParticleSystem.MainModule EC = Efect.GetComponent<EffectScript>().PaintballHit.GetComponent<ParticleSystem>().main;
                EC.startColor = new Color(PresentColor1.r, PresentColor1.g, PresentColor1.b, 0.25f);
                Efect.transform.position = HitPoint;
            }

		}

	}

}
