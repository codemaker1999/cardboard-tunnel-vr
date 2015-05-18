using UnityEngine;
using System.Collections;

public class AssignRandomColour : MonoBehaviour {

	public Material baseMaterial;

	// Use this for initialization
	void Start () {
		Material m = new Material(baseMaterial);
		m.SetColor(
			"_Color",
			new Color(
				0.1f + 0.5f*Random.value,
				0.1f + 0.5f*Random.value,
				0.1f + 0.5f*Random.value
			)
		);
		gameObject.GetComponent<Renderer>().material = m;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
