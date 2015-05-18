using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CubeMove : MonoBehaviour {

	public GameObject pfab;
	public CardboardHead head;
	public float initialSpeed;
	public float maxSpeed;
	public float initialAcceleration;
//	public float accelerationDecay;
//	public float speedFocusDecay;
//	public string scoreServer;
	public Text highScoreBoard;
	public Text scoreBoard;
	public Text scoreFlash;
	
	private float score;
	private int highScore;
	private float speed;
	private float acceleration;
	private float pi;

//	private List<GameObject> cubes;
	private bool CULLING;

	// Use this for initialization
	void Start () {
		pi = (float)System.Math.PI;
		speed = initialSpeed;
		acceleration = initialAcceleration;
		score = 0;
		highScore = PlayerPrefs.GetInt("highscore", 0);
		DisplayHighScore();
//		cubes = new List<GameObject>();
		CULLING = false;
	}

	void SaveHighscore() {
		PlayerPrefs.SetInt("highscore", highScore);
	}

	void DisplayHighScore() {
		highScoreBoard.text = "Highscore: " + highScore.ToString();
	}

	void UpdateHighScore () {
		highScore = (int)score;
		DisplayHighScore();
		SaveHighscore();
	}

	void UpdateScore (float _score) {
		score = _score;
		scoreBoard.text = "Score: "+((int)score).ToString();
	}

	IEnumerator Flash(float _score) {
		scoreFlash.text = ((int)_score).ToString();
		yield return new WaitForSeconds(1.5f);
		scoreFlash.text = "";
	}

	void OnTriggerEnter(Collider col){
		// reset speed
		if(col.gameObject.name == "RoteCube(Clone)"){
			ResetSpeed();
		}
		// score stuff
		StartCoroutine(Flash(score));
		if (score > highScore) {
			UpdateHighScore();
		}
		UpdateScore(0.0f);
	}
	
	float Sin (float x) {
		return (float)System.Math.Sin( (double)x );
	}
	
	float Cos (float x) {
		return (float)System.Math.Cos( (double)x );
	}

	void ResetSpeed () {
		speed = initialSpeed;
		acceleration = initialAcceleration;
	}

	float NextGaussian () {
		float u, v, S;
		do {
			u = 2.0f * Random.value - 1.0f;
			v = 2.0f * Random.value - 1.0f;
			S = u * u + v * v;
		} while (S >= 1.0f);
		
		float fac = (float)System.Math.Sqrt(-2.0f * (float)System.Math.Log(S) / S);
		return u * fac;
	}

	void AddPrefab() {
		// make cube in front of camera
		Quaternion q = Quaternion.FromToRotation(Vector3.up, head.Gaze.direction);
		float phi = 2.0f * pi * Random.value;
		// "focus" variables will narrow the angular spread of boxes being generated
		// in front of the camera. The faster the player, the narrower the spread. Also
		// use Gaussian number generator to give a natural focussing
		float speedFocus = (3.0f + maxSpeed - speed)/(3.0f + maxSpeed);
		float gaussianFocus = NextGaussian() * (20.0f * pi / 180.0f);
		float theta = speedFocus * 0.5f * pi * gaussianFocus;
		Vector3 v = new Vector3(
			Cos(phi)*Sin(theta),
			Cos(theta),
			Sin(phi)*Sin(theta)
		);
		float dist = 35.0f + 2.0f * speed;
		Vector3 perturb = 3.0f * Random.insideUnitSphere;
		Vector3 posn = dist * (q * v) + perturb;
		GameObject go = (GameObject)Instantiate(
			pfab,
			head.transform.position + posn,
			new Quaternion()
		);
//		cubes.Add (go);
	}

	void SafeDestroy(GameObject go) {
		if (go != null) {
			Destroy(go);
		}
	}

//	IEnumerator KillFarCubes() {
//		int upTo = 0;
//		List<GameObject> toKill = new List<GameObject>();
//		for ( int i=0; i < cubes.Count; i++ ) {
//			GameObject cube = cubes[i];
//			if (cube != null) {
//				if (Vector3.Distance(cube.transform.position, transform.position) < 40.0f) {
//					break;
//				} else {
//					upTo = i;
//					toKill.Add (cube);
//				}
//			}
//			yield return null;
//		}
//		for (int j=0; j < upTo; j++) {
//			cubes.RemoveAt(0);
//		}
//		yield return null;
//		toKill.ForEach(SafeDestroy);
//		CULLING = false;
//	}
	
	// Update is called once per frame
	void Update () {
		// lazy remove cubes
//		if (CULLING == false && Random.value > 0.95) {
//			CULLING = true;
//			KillFarCubes();
//		}
		// update score
		UpdateScore(score + Time.deltaTime);
		// accelerate
		if (speed < maxSpeed) {
			speed += Time.deltaTime * acceleration;
		}
		// cubes
		transform.position = transform.position + speed * Camera.main.transform.forward;
		for (int i=0; i<2; i++) {
			AddPrefab();
		}
	}
}
