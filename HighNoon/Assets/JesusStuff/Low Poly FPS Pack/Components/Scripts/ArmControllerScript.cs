using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text.RegularExpressions;

public class ArmControllerScript : MonoBehaviour
{
    string parentName;
	Animator anim;
    public float shotgunDamage = 85;
    public float revolverDamage = 35;
    public float rifleDamage = 65;
    float riflePool;
    float revolverPool;
    float shotgunPool;
	bool isReloading;
	bool outOfAmmo;
    bool triggerPulled = false;
	bool isShooting;
	bool isAimShooting;
	bool isAiming;
	bool isDrawing;
	bool isRunning;
	bool isJumping;
	bool isMeleeAttacking;

    #region ForGUIreference
    public Text loadedAmmo;
    public Text ammoRes;
    public GameObject parentObject;
    #endregion

    public int playerNum;

    //Random number generated to choose 
    //attack animation for melee
    int randomAttackAnim;
	
	//Used for fire rate
	float lastFired;
	
	//Ammo left
	public int currentAmmo;

	[System.Serializable]
	public class meleeSettings
	{  
		[Header("Melee Weapons")]
		//If the current weapon is a melee weapon
		public bool isMeleeWeapon;
	}
	public meleeSettings MeleeSettings;
	
	[System.Serializable]
	public class shootSettings
	{  
		[Header("Ammo")]
		//Total ammo
		public int ammo;
		
		[Header("Fire Rate & Bullet Settings")]
		public bool automaticFire;
		public float fireRate;
		
		[Space(10)]
		
		//How far the raycast will reach
		public float bulletDistance = 500.0f;
		//How much force will be applied to rigidbodies 
		//by the bullet raycast
		public float bulletForce = 500.0f;
		
		[Header("Shotgun Settings")]
		public bool useShotgunSpread;
		//How big the pellet spread area will be
		public float spreadSize = 2.0f;    
		//How many pellets to shoot
		public int pellets = 30;
		
		[Header("Projectile Weapon Settings")]
		
		//If the current weapon is a projectile weapon (rpg, bazooka, etc)
		public bool projectileWeapon;
			
		//The projectile spawned when shooting
		public Transform projectile;
		//The static projectile on the weapon
		//This will be hidden when shooting
		public Transform currentProjectile;
		
		//How long after shooting the reload will start
		public float reloadTime;	
	}
	public shootSettings ShootSettings;
	
	[System.Serializable]
	public class reloadSettings
	{  
		[Header("Reload Settings")]
		public bool casingOnReload;
		public float casingDelay;
		
		[Header("Bullet In Mag")]
		public bool hasBulletInMag;
		public Transform[] bulletInMag;
		public float enableBulletTimer = 1.0f;

		[Header("Bullet Or Shell Insert")]
		//If the weapon uses a bullet/shell insert style reload
		//Used for the bolt action sniper and pump shotgun for example
		public bool usesInsert;
		
	}
	public reloadSettings ReloadSettings;
	
	[System.Serializable]
	public class impactTags
	{  
		[Header("Impact Tags")]
		//Default tags for bullet impacts
		public string metalImpactStaticTag = "Metal (Static)";
		public string metalImpactTag = "Metal";
		public string woodImpactStaticTag = "Wood (Static)";
		public string woodImpactTag = "Wood";
		public string concreteImpactStaticTag = "Concrete (Static)";
		public string concreteImpactTag = "Concrete";
		public string dirtImpactStaticTag = "Dirt (Static)";
		public string dirtImpactTag = "Dirt";
	}
	public impactTags ImpactTags;
	
	//All Components
	[System.Serializable]
	public class components
	{  
		[Header("Muzzleflash Holders")]
		public bool useMuzzleflash = false;
		public GameObject sideMuzzle;
		public GameObject topMuzzle;
		public GameObject frontMuzzle;
		//Array of muzzleflash sprites
		public Sprite[] muzzleflashSideSprites;
		
		[Header("Light Front")]
		public bool useLightFlash = false;
		public Light lightFlash;
		
		[Header("Particle System")]
		public bool playSmoke = false;
		public ParticleSystem smokeParticles;
		public bool playSparks = false;
		public ParticleSystem sparkParticles;
		public bool playTracers = false;
		public ParticleSystem bulletTracerParticles;

		[Header("Melee Components")]
		public GameObject weaponTrail;
	}
	public components Components;
	
	//All weapon types
	[System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
		public Transform casingPrefab;
		
		[Header("Metal")]
		[Header("Bullet Impacts & Tags")]
		public Transform metalImpactStaticPrefab;
		public Transform metalImpactPrefab;
		[Header("Wood")]
		public Transform woodImpactStaticPrefab;
		public Transform woodImpactPrefab;
		[Header("Concrete")]
		public Transform concreteImpactStaticPrefab;
		public Transform concreteImpactPrefab;
		[Header("Dirt")]
		public Transform dirtImpactStaticPrefab;
		public Transform dirtImpactPrefab;
	}
	public prefabs Prefabs;
	
	[System.Serializable]
	public class spawnpoints
	{  
		[Header("Spawnpoints")]
		//Array holding casing spawn points 
		//(some weapons use more than one casing spawn)
		public Transform [] casingSpawnPoints;
		//Bullet raycast start point
		public Transform bulletSpawnPoint;
	}
	public spawnpoints Spawnpoints;
	
	[System.Serializable]
	public class audioClips
	{  
		[Header("Audio Clips")]
		
		//All audio clips
		public AudioClip shootSound;
		public AudioClip reloadSound;
        public AudioClip dryFire;
        public AudioClip leverAction;
	}
	public audioClips AudioClips;

	public bool noSwitch = false;
	
	void Awake ()
    {		
		//Set the animator component
		anim = GetComponent<Animator>();
        parentName = this.transform.parent.transform.parent.transform.parent.transform.parent.name;
        if(parentName == "Player1")
        {
            parentObject = GameObject.Find("Player1");
        }
        if (parentName == "Player2")
        {
            parentObject = GameObject.Find("Player2");
        }
        //Set the ammo count
        RefillAmmo ();
		
		//Hide muzzleflash and light at start, disable for projectile, grenade, melee weapons, grenade launcher and flamethrower
		if (!ShootSettings.projectileWeapon && !MeleeSettings.isMeleeWeapon)
        { 			
			Components.sideMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
			Components.topMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
			Components.frontMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
		}
		
		//Disable the light flash, disable for melee weapons and grenade
		if (!MeleeSettings.isMeleeWeapon)
        {
			Components.lightFlash.GetComponent<Light> ().enabled = false;
		}

		//Set the "shoot" sound clip for melee weapons
		if (MeleeSettings.isMeleeWeapon == true)
        {
			
			//Disable the weapon trail at start
			Components.weaponTrail.GetComponent<TrailRenderer>().enabled = false;
		}

        string numberOnly = Regex.Replace(parentName, "[^0-9]", "");
        playerNum = int.Parse(numberOnly);
    }
	
	void Update ()
    {
        shotgunDamage = 85;
        revolverDamage = 35;
        rifleDamage = 65;
        loadedAmmo.text = currentAmmo.ToString();
        if(this.gameObject.name == "arms@revolver_1")
        {
            revolverPool = parentObject.GetComponent<Player>().revolverAmmoPool;
            ammoRes.text = revolverPool.ToString();
        }
        if (this.gameObject.name == "arms@lever_action_rifle")
        {
            riflePool = parentObject.GetComponent<Player>().rifleAmmoPool;
            ammoRes.text = riflePool.ToString();
        }
        if (this.gameObject.name == "arms@sawn_off_shotgun")
        {
            shotgunPool = parentObject.GetComponent<Player>().shotgunAmmoPool;
            ammoRes.text = shotgunPool.ToString();
        }
        //Generate random number to choose which melee attack animation to play
        //If using a melee weapon
        if (MeleeSettings.isMeleeWeapon == true)
        {
			randomAttackAnim = Random.Range (1, 4);
		}

		//Check which animation 
		//is currently playing
		AnimationCheck ();
		
		//Left click (if automatic fire is false)
		if(parentName == "Player1")
        {
            Shooter1();
        }
        if (parentName == "Player2")
        {
            Shooter2();
        }

        //If out of ammo
        if (currentAmmo == 0)
        {
			outOfAmmo = true;
			//if ammo is higher than 0
		}
        else if (currentAmmo > 0)
        {
			outOfAmmo = false;
		}
	}
    void NewShoot()
    {
        if (Input.GetAxis("RT" + playerNum) == 1 && !ShootSettings.automaticFire && !isReloading && !isShooting && !isAimShooting && !isRunning && !isJumping && triggerPulled == false)
        {
            triggerPulled = true;
            if (currentAmmo == 0)
            {
                SoundManager.instance.Play(AudioClips.dryFire, "sfx");
            }
            else
            {
                //If shotgun shoot is true
                if (ShootSettings.useShotgunSpread == true)
                {
                    ShotgunShoot();
                }
                //If projectile weapon, grenade, melee weapons, grenade launcher and flamethrower is false
                if (!ShootSettings.projectileWeapon && !ShootSettings.useShotgunSpread && !MeleeSettings.isMeleeWeapon)
                {
                    Shoot();
                    //If projectile weapon is true
                }
                else if (ShootSettings.projectileWeapon == true)
                {
                    StartCoroutine(ProjectileShoot());
                }
            }
            #region MeleeShit
            //If melee weapon is used, play random attack animation on left click
            if (MeleeSettings.isMeleeWeapon == true)
            {
                //Play attack animation 1, if not currently attacking or drawing weapon
                if (randomAttackAnim == 1 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 1");
                    //Play weapon sound
                }
                //Play attack animation 2, if not currently attacking or drawing weapon
                if (randomAttackAnim == 2 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 2");
                    //Play weapon sound
                }
                //Play attack animation 3, if not currently attacking or drawing weapon
                if (randomAttackAnim == 3 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 3");
                    //Play weapon sound
                }
            }
            #endregion
        }
        if (Input.GetAxis("RT" + playerNum) == 0)
        {
            triggerPulled = false;
        }
        //R key to reload
        //Not used for projectile weapons, grenade or melee weapons
        if (Input.GetButtonDown("RB" + playerNum) && !isReloading && !MeleeSettings.isMeleeWeapon && currentAmmo != ShootSettings.ammo)
        {
            if (currentAmmo != 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@revolver_1" && revolverPool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@lever_action_rifle" && riflePool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@sawn_off_shotgun" && shotgunPool == 0)
            {
                return;
            }
            else
            {
                Reload();
            }
        }

        //Run when holding down left shift and moving
        if (Input.GetButton("LSBut" + playerNum) && Input.GetAxis("LSVert" + playerNum) > 0)
        {
            this.anim.SetFloat("Run", 0.2f);
        }
        else
        {
            //Stop running
            this.anim.SetFloat("Run", 0.0f);
        }
    }
    void Shooter1()
    {
        if (Input.GetAxis("RT1") == 1 && !ShootSettings.automaticFire && !isReloading && !isShooting && !isAimShooting && !isRunning && !isJumping && triggerPulled == false)
        {
            triggerPulled = true;
            if (currentAmmo == 0)
            {
                SoundManager.instance.Play(AudioClips.dryFire, "sfx");
            }
            else
            {
                //If shotgun shoot is true
                if (ShootSettings.useShotgunSpread == true)
                {
                    ShotgunShoot();
                }
                //If projectile weapon, grenade, melee weapons, grenade launcher and flamethrower is false
                if (!ShootSettings.projectileWeapon && !ShootSettings.useShotgunSpread && !MeleeSettings.isMeleeWeapon)
                {
                    Shoot();
                    //If projectile weapon is true
                }
                else if (ShootSettings.projectileWeapon == true)
                {
                    StartCoroutine(ProjectileShoot());
                }
            }
            #region MeleeShit
            //If melee weapon is used, play random attack animation on left click
            if (MeleeSettings.isMeleeWeapon == true)
            {
                //Play attack animation 1, if not currently attacking or drawing weapon
                if (randomAttackAnim == 1 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 1");
                    //Play weapon sound
                }
                //Play attack animation 2, if not currently attacking or drawing weapon
                if (randomAttackAnim == 2 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 2");
                    //Play weapon sound
                }
                //Play attack animation 3, if not currently attacking or drawing weapon
                if (randomAttackAnim == 3 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 3");
                    //Play weapon sound
                }
            }
            #endregion
        }
        if (Input.GetAxis("RT1") == 0)
        {
            triggerPulled = false;
        }
        //R key to reload
        //Not used for projectile weapons, grenade or melee weapons
        if (Input.GetButtonDown("RB1") && !isReloading && !MeleeSettings.isMeleeWeapon && currentAmmo != ShootSettings.ammo)
        {
            if (currentAmmo != 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@revolver_1" && revolverPool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@lever_action_rifle" && riflePool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@sawn_off_shotgun" && shotgunPool == 0)
            {
                return;
            }
            else
            {
                Reload();
            }
        }

        //Run when holding down left shift and moving
        if (Input.GetButton("LSBut1") && Input.GetAxis("LSVert1") > 0)
        {
            anim.SetFloat("Run", 0.2f);
        }
        else
        {
            //Stop running
            anim.SetFloat("Run", 0.0f);
        }
    }
    void Shooter2()
    {
        if (Input.GetAxis("RT2") == 1 && !ShootSettings.automaticFire && !isReloading && !isShooting && !isAimShooting && !isRunning && !isJumping && triggerPulled == false)
        {
            triggerPulled = true;
            if (currentAmmo == 0)
            {
                SoundManager.instance.Play(AudioClips.dryFire, "sfx");
            }
            else
            {
                //If shotgun shoot is true
                if (ShootSettings.useShotgunSpread == true)
                {
                    ShotgunShoot();
                }
                //If projectile weapon, grenade, melee weapons, grenade launcher and flamethrower is false
                if (!ShootSettings.projectileWeapon && !ShootSettings.useShotgunSpread && !MeleeSettings.isMeleeWeapon)
                {
                    Shoot();
                    //If projectile weapon is true
                }
                else if (ShootSettings.projectileWeapon == true)
                {
                    StartCoroutine(ProjectileShoot());
                }
            }
            #region MeleeShit
            //If melee weapon is used, play random attack animation on left click
            if (MeleeSettings.isMeleeWeapon == true)
            {
                //Play attack animation 1, if not currently attacking or drawing weapon
                if (randomAttackAnim == 1 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 1");
                    //Play weapon sound
                }
                //Play attack animation 2, if not currently attacking or drawing weapon
                if (randomAttackAnim == 2 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 2");
                    //Play weapon sound
                }
                //Play attack animation 3, if not currently attacking or drawing weapon
                if (randomAttackAnim == 3 && !isMeleeAttacking && !isDrawing)
                {
                    anim.SetTrigger("Attack 3");
                    //Play weapon sound
                }
            }
            #endregion
        }
        if (Input.GetAxis("RT2") == 0)
        {
            triggerPulled = false;
        }
        //R key to reload
        //Not used for projectile weapons, grenade or melee weapons
        if (Input.GetButtonDown("RB2") && !isReloading && !MeleeSettings.isMeleeWeapon)
        {
            if(currentAmmo != 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@revolver_1" && revolverPool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@lever_action_rifle" && riflePool == 0)
            {
                return;
            }
            if (this.gameObject.name == "arms@sawn_off_shotgun" && shotgunPool == 0)
            {
                return;
            }
            else
            {
                Reload();
            }            
        }

        //Run when holding down left shift and moving
        if (Input.GetButton("LSBut2") && Input.GetAxis("LSVert2") > 0)
        {
            anim.SetFloat("Run", 0.2f);
        }
        else
        {
            //Stop running
            anim.SetFloat("Run", 0.0f);
        }
    }

	//Muzzleflash
	IEnumerator MuzzleFlash () {
		
		//Show muzzleflash if useMuzzleFlash is true
		if (!ShootSettings.projectileWeapon && Components.useMuzzleflash == true) {
			//Show a random muzzleflash from the array
			Components.sideMuzzle.GetComponent<SpriteRenderer> ().sprite = Components.muzzleflashSideSprites 
				[Random.Range (0, Components.muzzleflashSideSprites.Length)];
			Components.topMuzzle.GetComponent<SpriteRenderer> ().sprite = Components.muzzleflashSideSprites 
				[Random.Range (0, Components.muzzleflashSideSprites.Length)];
			
			//Show the muzzleflashes
			Components.sideMuzzle.GetComponent<SpriteRenderer> ().enabled = true;
			Components.topMuzzle.GetComponent<SpriteRenderer> ().enabled = true;
			Components.frontMuzzle.GetComponent<SpriteRenderer> ().enabled = true;
		}
		
		//Enable the light flash if true
		if (Components.useLightFlash == true) {
			Components.lightFlash.GetComponent<Light> ().enabled = true;
		}
		
		//Play smoke particles if true
		if (Components.playSmoke == true) {
			Components.smokeParticles.Play ();
		}
		//Play spark particles if true
		if (Components.playSparks == true) {
			Components.sparkParticles.Play ();
		}
		//Play bullet tracer particles if true
		if (Components.playTracers == true) {
			Components.bulletTracerParticles.Play();
		}
		
		//Show the muzzleflash for 0.02 seconds
		yield return new WaitForSeconds (0.02f);
		
		if (!ShootSettings.projectileWeapon && Components.useMuzzleflash == true) {
			//Hide the muzzleflashes
			Components.sideMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
			Components.topMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
			Components.frontMuzzle.GetComponent<SpriteRenderer> ().enabled = false;
		}
		
		//Disable the light flash if true
		if (Components.useLightFlash == true) {
			Components.lightFlash.GetComponent<Light> ().enabled = false;
		}
	}
    
	//Projectile shoot
	IEnumerator ProjectileShoot () {
		
		//Play shoot animation
		if (!anim.GetBool ("isAiming"))
        {
			anim.Play ("Fire");
		}
        else
        {
			anim.SetTrigger("Shoot");
		}

		//Remove 1 bullet
		currentAmmo -= 1;

		//Play shoot sound
		
		StartCoroutine (MuzzleFlash ());
		
		//Spawn the projectile
		Instantiate (ShootSettings.projectile, Spawnpoints.bulletSpawnPoint.transform.position, Spawnpoints.bulletSpawnPoint.transform.rotation);
		
		//Hide the current projectile mesh
		ShootSettings.currentProjectile.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		
		yield return new WaitForSeconds (ShootSettings.reloadTime);
		
		//Play reload animation
		anim.Play ("Reload");

		//Play shoot sound
		
		
		//Show the current projectile mesh
		ShootSettings.currentProjectile.GetComponent
			<SkinnedMeshRenderer> ().enabled = true;
		
	}
	
	//Shotgun shoot
	void ShotgunShoot()
    {
		
		//Play shoot animation
		if (!anim.GetBool ("isAiming"))
        {
			anim.Play ("Fire");
		}
        else
        {
			anim.SetTrigger("Shoot");
		}
		
		//Remove 1 bullet
		currentAmmo -= 1;

        //Play shoot sound
        SoundManager.instance.Play(AudioClips.shootSound, "sfx");
		
		//Start casing instantiate
		if (!ReloadSettings.casingOnReload)
        {
			StartCoroutine (CasingDelay ());
		}
		
		//Show the muzzleflash
		StartCoroutine (MuzzleFlash ());
		
		//Send out shotgun raycast with set amount of pellets
		for (int i = 0; i < ShootSettings.pellets; ++i)
        {
			
			float randomRadius = Random.Range 
				(0, ShootSettings.spreadSize);        
			float randomAngle = Random.Range 
				(0, 2 * Mathf.PI);
			
			//Raycast direction
			Vector3 direction = new Vector3 (
				randomRadius * Mathf.Cos (randomAngle),
				randomRadius * Mathf.Sin (randomAngle),
				15);
			
			direction = transform.TransformDirection (direction.normalized);
			
			RaycastHit hit;        
			if (Physics.Raycast (Spawnpoints.bulletSpawnPoint.transform.position, direction, out hit, ShootSettings.bulletDistance))
            {
				//If a rigibody is hit, add bullet force to it
				if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(direction * ShootSettings.bulletForce);
                }
                if(hit.transform.CompareTag("Player")==true)
                {
                    hit.transform.GetComponent<Player>().TakeDamage(shotgunDamage);
                }
			}    
		}
	}
	IEnumerator LeverAction()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.instance.Play(AudioClips.leverAction, "sfx");
    }
	//Shoot
	void Shoot() {
		
		//Play shoot animation
		if (!anim.GetBool ("isAiming"))
        {
			anim.Play ("Fire");
		}
        else
        {
			anim.SetTrigger("Shoot");
		}
		
		//Remove 1 bullet
		currentAmmo -= 1;

        //Play shoot sound
        SoundManager.instance.Play(AudioClips.shootSound, "sfx");
        if(this.gameObject.name == "arms@lever_action_rifle")
        {
            StartCoroutine(LeverAction());
        }
		//Start casing instantiate
		if (!ReloadSettings.casingOnReload)
        {
			StartCoroutine (CasingDelay ());
		}
		
		//Show the muzzleflash
		StartCoroutine (MuzzleFlash ());
		
		//Raycast bullet
		RaycastHit hit;
		Ray ray = new Ray (transform.position, transform.forward);
		
		//Send out the raycast from the "bulletSpawnPoint" position
		if (Physics.Raycast (Spawnpoints.bulletSpawnPoint.transform.position, Spawnpoints.bulletSpawnPoint.transform.forward, out hit, ShootSettings.bulletDistance))
        {
			
			//If a rigibody is hit, add bullet force to it
			if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(ray.direction * ShootSettings.bulletForce);
            }
            if (hit.transform.CompareTag("Player") == true && this.gameObject.name == "arms@revolver_1")
            {
                hit.transform.GetComponent<Player>().TakeDamage(revolverDamage);
            }
            if (hit.transform.CompareTag("Player") == true && this.gameObject.name == "arms@lever_action_rifle")
            {
                hit.transform.GetComponent<Player>().TakeDamage(rifleDamage);
            }
        }
	}
	
	//Refill ammo
	void RefillAmmo ()
    {
		currentAmmo = ShootSettings.ammo;
	}
	
	//Reload
	void Reload ()
    {
		
		//Play reload animation
		anim.Play ("Reload");
        while(currentAmmo < ShootSettings.ammo)
        {
            if (this.gameObject.name == "arms@revolver_1")
            {
                if(parentObject.GetComponent<Player>().revolverAmmoPool == 0)
                {
                    break;
                }
                currentAmmo++;
                parentObject.GetComponent<Player>().revolverAmmoPool--;
            }
            if (this.gameObject.name == "arms@lever_action_rifle")
            {
                if (parentObject.GetComponent<Player>().rifleAmmoPool == 0)
                {
                    break;
                }
                currentAmmo++;
                parentObject.GetComponent<Player>().rifleAmmoPool--;
            }
            if (this.gameObject.name == "arms@sawn_off_shotgun")
            {
                if (parentObject.GetComponent<Player>().shotgunAmmoPool == 0)
                {
                    break;
                }
                currentAmmo++;
                parentObject.GetComponent<Player>().shotgunAmmoPool--;
            }
        }
        
        //Play reload sound
        SoundManager.instance.Play(AudioClips.reloadSound, "sfx");
		
		//Spawn casing on reload, only used on some weapons
		if (ReloadSettings.casingOnReload == true)
        {
			StartCoroutine(CasingDelay());
		}
		
		if (outOfAmmo == true && ReloadSettings.hasBulletInMag == true)
        {
			//Hide the bullet inside the mag if ammo is 0
			for (int i = 0; i < ReloadSettings.bulletInMag.Length; i++)
            {
				ReloadSettings.bulletInMag[i].GetComponent<MeshRenderer> ().enabled = false;
			}
			//Start the "show bullet" timer
			StartCoroutine (BulletInMagTimer ());
		}
	}
	
	IEnumerator BulletInMagTimer ()
    {
		//Wait for set amount of time 
		yield return new WaitForSeconds (ReloadSettings.enableBulletTimer);
		
		//Show the bullet inside the mag
		for (int i = 0; i < ReloadSettings.bulletInMag.Length; i++)
        {
			ReloadSettings.bulletInMag[i].GetComponent<MeshRenderer> ().enabled = true;
		}
	}
	
	IEnumerator CasingDelay ()
    {
		//Wait set amount of time for casing to spawn
		yield return new WaitForSeconds (ReloadSettings.casingDelay);
		//Spawn a casing at every casing spawnpoint
		for (int i = 0; i < Spawnpoints.casingSpawnPoints.Length; i++) {
			Instantiate (Prefabs.casingPrefab, 
			             Spawnpoints.casingSpawnPoints [i].transform.position, 
			             Spawnpoints.casingSpawnPoints [i].transform.rotation);
		}
	}
	
	//Check current animation playing
	void AnimationCheck ()
    {
		//Check if shooting
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Fire"))
        {
			isShooting = true;
		}
        else
        {
			isShooting = false;
		}
		//Check if shooting while aiming down sights
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Aim Fire"))
        {
			isAimShooting = true;
		}
        else
        {
			isAimShooting = false;
		}

		//Check if running
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Run"))
        {
			isRunning = true;
		}
        else
        {
			isRunning = false;
		}
		
		//Check if jumping
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Jump"))
        {
			isJumping = true;
		}
        else
        {
			isJumping = false;
		}

		//Check if drawing weapon
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Draw"))
        {
			isDrawing = true;
		}
        else
        {
			isDrawing = false;
		}

		//Check if finsihed reloading when using "insert" style reload
		//Used for bolt action sniper and pump shotgun for example
		if (ReloadSettings.usesInsert == true && anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
        {
			isReloading = false;
			//Used in the demo scnes
			noSwitch = false;
		}
        
		//Check if reloading
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload"))
        {
			// If reloading
			isReloading = true;
			//Refill ammo
			//RefillAmmo();
			//Used in the demo scenes
			noSwitch = true;
		}
        else
        {
			//If not using "insert" style reload
			if (!ReloadSettings.usesInsert)
            {
				//If not reloading
				isReloading = false;
				//Used in the demo scenes
				noSwitch = false;
			}
		}

		//Check if melee weapon animation is playing
		//To make sure melee animations cant be played at same time
		if (MeleeSettings.isMeleeWeapon == true) {
			//Check if any melee attack animation is playing
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack 1") || anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack 2") || anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack 3"))
            {
				//If attacking
				isMeleeAttacking = true;
				//Enable the weapon trail, only shown when attacking
				Components.weaponTrail.GetComponent<TrailRenderer>().enabled = true;
			}
            else
            {
				//If not attacking
				isMeleeAttacking = false;
				//Disable the weapon trail
				Components.weaponTrail.GetComponent<TrailRenderer>().enabled = false;
			}
		}
	}
}