using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WitchController : MonoBehaviour {


	public float maxHR = 100;
	public float curHR = 0;
	public float addHRPoint;
	public float minusHRPoint;
	public float decreaseSpeedHR = 2;
	public float decreaseSpeedLR = 1;
	public GameObject hrBar;
	public GameObject hrText;
	public GameObject lrText;
	public GameObject hpText;
	public SpriteRenderer angryEmo;
	public SpriteRenderer loveEmo;
	private float countEmo;

	public ParticleSystem healEffect;
	public ParticleSystem debuffEffect;

	public bool isFreeze;
	public bool isRitual;

	public static WitchController instance;
	public GameObject bat_prefer, bird_prefer, cat_prefer, candle_prefer, hat_prefer, hook_prefer,
		pot_prefer, star_prefer, wand_prefer;

	private GameObject[] allElement;
	public GameObject[] prefer;
	public int[] preferNumber;

	private GameObject prefer1, prefer2, prefer3;

	private float count;

	public int HP;
	public int ATK;
	public int INT;
	public int WIS;
	public int AGI;
	public TextMesh ATKNumber;
	public TextMesh HPNumber;
	public TextMesh INTNumber;
	public TextMesh WISNumber;
	public TextMesh AGINumber;
	public TextMesh curHp_maxHp;

	public GameObject cooldownBar;
	public float cooldown;
	public float maxCooldown;

	void Awake(){
		instance = this;
		allElement = new GameObject[] {
			bat_prefer,
			bird_prefer,
			cat_prefer,
			candle_prefer,
			hat_prefer,
			hook_prefer,
			pot_prefer,
			star_prefer,
			wand_prefer,
		};
		preferNumber = new int[3];

//		ATKNumber.text = ""+ATK;
//		HPNumber.text = ""+HP;
//		INTNumber.text = ""+INT;
//		WISNumber.text = "" + WIS;
//		AGINumber.text = "" + AGI;

	}

	// Use this for initialization
	void Start () {
		isRitual = false;
		isFreeze = false;
		addHRPoint = 10;
		minusHRPoint = 10;
		count = 0f;
		curHR = maxHR/2;
		prefer = new GameObject[3];
		assignPrefer ();
//		spawnPrefer ();

		spawnEmo ();
		loveEmo.enabled = false;
		angryEmo.enabled = false;

		lrText.SetActive (false);
		hpText.SetActive (false);
		hrText.SetActive (true);
		curHp_maxHp.text = "";
		hrBar.GetComponent<Image> ().color = new Color(1,167f/255,167f/255,1);
		Debug.Log (this.transform.position);
	}

	void countTime (){
		count += 1 * Time.deltaTime;
	}

	void checkRitualPhase(){
		if (curHR == maxHR && hrText.active == true) {
//			isRitual = true;
			this.transform.position = new Vector3(0,2,this.transform.position.z);
			GameController.instance.addWizard ();;
		}
	}

	void checkGameover(){
		if (curHR == 0) {
			GameController.instance.isGameOver = true;
			GameController.instance.restart ();
		}
	}

	void OnMouseDown(){
		instance = this;
	}

	void assignPrefer(){
		int[] checkDuplicate = new int[3];
		for(int i = 0; i < prefer.Length; i++){
			int rand = Random.Range(0, allElement.Length);
			if(i == 0){
				prefer[i] = allElement[rand];
				preferNumber [i] = rand;
				checkDuplicate[i] = rand;
			} else if(i != 0 && rand != checkDuplicate[0] && rand != checkDuplicate[1]) {
				prefer[i] = allElement[rand];
				preferNumber [i] = rand;
				checkDuplicate [i] = rand;
			} else {
				i--;
			}
		}
	}

	void spawnPrefer(){
		prefer1 = Instantiate(prefer[0]) as GameObject;
		prefer1.transform.parent = this.transform;

		prefer2 = Instantiate(prefer[1]) as GameObject;
		prefer2.transform.parent = this.transform;

		prefer3 = Instantiate(prefer[2]) as GameObject;
		prefer3.transform.parent = this.transform;

	}

	void Moveprefer(){
		prefer1.transform.position= new Vector3 (this.transform.position.x + 0.8f, this.transform.position.y + 1f , 0);
		prefer2.transform.position= new Vector3 (this.transform.position.x + 0.8f, this.transform.position.y , 0);
		prefer3.transform.position= new Vector3 (this.transform.position.x + 0.8f, this.transform.position.y - 1f, 0);
	}

	void spawnEmo(){
		angryEmo.transform.position= new Vector3 (this.transform.position.x - 0.75f, this.transform.position.y + 1.25f, 0);
		loveEmo.transform.position= new Vector3 (this.transform.position.x - 0.75f, this.transform.position.y + 1.0f, 0);
	}

	void showHealEffect(){
		ParticleSystem newHealElementEffect = Instantiate (healEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
			, Quaternion.identity) as ParticleSystem;
		Destroy (newHealElementEffect.gameObject, 2);
	}

	public void showDebuffEffect(){
		ParticleSystem newDebuffElementEffect = Instantiate (debuffEffect, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)
			, Quaternion.identity) as ParticleSystem;
		Destroy (newDebuffElementEffect.gameObject, 2);
	}

	void addHR(float hr){
		curHR += hr;
		if (curHR > maxHR) {
			curHR = maxHR;
		}
		loveEmo.enabled = true;
		angryEmo.enabled = false;
		countEmo = 0;
		showHealEffect ();
	}

	void addHR(){
		curHR += addHRPoint;
		if (curHR > maxHR) {
			curHR = maxHR;
		}
		loveEmo.enabled = true;
		angryEmo.enabled = false;
		countEmo = 0;
		showHealEffect ();
	}

	void checkElement(GameObject element){
		int count_elem = 0;
		string element_name = element.name.Substring(0, element.name.Length - "_element(Clone)".Length);
		foreach (GameObject i in prefer) {
			string i_name = i.name.Substring(0, i.name.Length - "_prefer".Length);
			if (element_name == "talk") {
				count_elem = 2;
			}
			if(i_name == element_name){
				count_elem++;
			} 
		}
		if (count_elem == 2) {
			addHR (15f);
		}
			
		/*if (count_elem == 1) {
			addHR ();
		}
		else if(count_elem == 0) {
			minusHR ();
		}*/

		ATK += element.GetComponent<ElementController> ().ATK;
		HP += element.GetComponent<ElementController> ().HP;
		INT += element.GetComponent<ElementController> ().INT;
		WIS += element.GetComponent<ElementController> ().WIS;
		AGI += element.GetComponent<ElementController> ().AGI;
		if (ATK <= 0) { ATK = 0; }
		if (HP <= 0) { HP = 0; }
		if (INT <= 0) { INT = 0; }
		if (WIS <= 0) { WIS = 0; }
		if (AGI <= 0) { AGI = 0; }
	}

	void minusHR(){
		curHR -= minusHRPoint;
		if (curHR < 0) {
			curHR = 0;
		}
		angryEmo.enabled = true;
		loveEmo.enabled = false;
		countEmo = 0;
		showDebuffEffect ();
	}
		
	private void deceaseHR(){
		float decreaseSpeed = 1;
		if (isFreeze) {
			decreaseSpeed = 0;
		} else if (lrText.active) {
			decreaseSpeed = decreaseSpeedLR;
		} else if (hrText.active) {
			decreaseSpeed = decreaseSpeedHR;
		}

		if (curHR > 0) {
			curHR -= Time.deltaTime * decreaseSpeed;
		}
		if (curHR < 0) {
			curHR = 0;
		}
	}

	void showPrefer(){
		if (count > 3f) {
			count = 0;
		}
	}

	void OnTriggerEnter(Collider coll) {
		checkElement (coll.gameObject);
		Destroy (coll.gameObject);
	}

	void upDateStatus(){
		ATKNumber.text = ""+ATK;
		HPNumber.text = ""+HP;
		INTNumber.text = ""+INT;
		WISNumber.text = ""+WIS;
		AGINumber.text = ""+AGI;
	}

	// Update is called once per frame
	void Update () {
		countTime ();

//		showPrefer ();

		if (instance == this) {
			if (Input.GetKeyDown (KeyCode.H)) { addHR (); }
			if (Input.GetKeyDown (KeyCode.L)) { minusHR (); }
		}

		checkRitualPhase ();
//		checkGameover ();
		deceaseHR ();

		if (curHR< 0) {
			curHR = 0;
		}
		if (curHR == 0){
			cooldown = 0;
		}
		hrBar.transform.localScale = new Vector3 (curHR / maxHR, hrBar.transform.localScale.y, hrBar.transform.localScale.z);
//		Moveprefer ();

		spawnEmo ();
		countEmo += Time.deltaTime;
		if (countEmo >= 1.5) {
			loveEmo.enabled = false;
			angryEmo.enabled = false;
		}

		if (isRitual) {
			curHp_maxHp.text = curHR+" /"+maxHR;
		}

		upDateStatus ();
//		changeToHpBar ();

		increaseCooldown ();
		cooldownBar.transform.localScale = new Vector3 (cooldown / maxCooldown, cooldownBar.transform.localScale.y, cooldownBar.transform.localScale.z);
	}

	public void changeToHpBar(){
		lrText.SetActive (false);
		hrText.SetActive (false);

		hpText.SetActive (true);

		maxHR = HP;
		curHR = HP;

		curHp_maxHp.text = curHR+" /"+maxHR;
		hrBar.GetComponent<Image> ().color = new Color(72f/255,179f/255,0,1);
	}

	private void increaseCooldown(){
		float increaseSpeed = 1+AGI;
		if (isRitual) {
			if (cooldown < maxCooldown) {
				cooldown += Time.deltaTime * increaseSpeed;
				cooldownBar.GetComponent<Image> ().color = new Color(0,181f/255,1,1);
			}
			if (cooldown > maxCooldown) {
				cooldown = maxCooldown;
				cooldownBar.GetComponent<Image> ().color = new Color(0,1,181f/255,1);
			}	
		}
	}
}
