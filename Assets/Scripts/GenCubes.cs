using UnityEngine;
using System.Collections;

public class GenCubes : MonoBehaviour {

	public GameObject pfab;
	public CardboardHead head;

	private float pi;

	// Use this for initialization
	void Start () {
		pi = (float)System.Math.PI;
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.name == "RoteCube(Clone)"){
			gameObject.SendMessage("ResetSpeed");
		}
	}

	float Sin (float x) {
		return (float)System.Math.Sin( (double)x );
	}

	float Cos (float x) {
		return (float)System.Math.Cos( (double)x );
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion q = Quaternion.FromToRotation(Vector3.up, head.Gaze.direction);
		float phi = 2.0f * pi * Random.value;
		float theta = 0.35f * pi * Random.value;
		Vector3 v = new Vector3(
			Cos(phi)*Sin(theta),
			Cos(theta),
			Sin(phi)*Sin(theta)
		);
		Vector3 posn = 30.0f * (q * v) + 3.0f * Random.insideUnitSphere;
		Instantiate(
			pfab,
			head.transform.position + posn,
			new Quaternion()
		);
	}
}
