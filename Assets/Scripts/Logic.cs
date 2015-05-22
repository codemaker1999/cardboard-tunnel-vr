using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Logic : MonoBehaviour {
	public CardboardHead head;
	public float initialVelocity;
	public float acceleration;
	public float tunnelRadius;
	public int countdown;
	public int numNodes;
	public float maxAngle;
	public float totalAngle;
	public float dt;

	public Text flashText;
	public Text scoreText;
	public Text highscoreText;
	public GameObject prefab;

	private float score;
	private float highscore;
	private float velocity;
	private float[] omegasX; // angular frequency
	private float[] omegasY;
	private float[] thetasX;
	private float[] thetasY;
	private float phiX;
	private float phiY;
	private Quaternion tunnelPtr;
	private Vector3 tunnelPos;
	private Vector3 tunnelFwd;
	private bool makeTunnel = false;
	private float time = 0;
	private int counter = 0;

	// Use this for initialization
	IEnumerator Start () {
		// score stuff
		highscore = PlayerPrefs.GetFloat("highscore",0.0f);
		score = 0.0f;
		highscoreText.text = "Highscore: "+((int)highscore).ToString();
		scoreText.text = "Score: 0";
		// setup
		velocity = initialVelocity;
		phiX = 0;
		phiY = 0;
		// decompose thetas
		omegasX = new float[numNodes];
		omegasY = new float[numNodes];
		thetasX = new float[numNodes];
		thetasY = new float[numNodes];
		float leftoverX = totalAngle;
		float leftoverY = totalAngle;
		float totalX = 0;
		float totalY = 0;
		float rX;
		float rY;
		for (int i=0; i<numNodes-1; i++) {
			do { 
				rX = leftoverX * Random.value;
			} while (rX > maxAngle);
			do { 
				rY = leftoverY * Random.value;
			} while (rY > maxAngle);

			thetasX[i] = rX;
			thetasY[i] = rY;
			totalX += rX;
			totalY += rY;
			leftoverX = totalAngle - totalX;
			leftoverY = totalAngle - totalY;
			omegasX[i] = (float)System.Math.Sqrt(9.81/(double)(3.0f*Random.value));
   			omegasY[i] = (float)System.Math.Sqrt(9.81/(double)(3.0f*Random.value));
		}
		thetasX[numNodes-1] = totalAngle - totalX;
		thetasY[numNodes-1] = totalAngle - totalY;
		// countdown to start (do preparation here)
		int n = 0;
		float begin = Time.time;
		while (n<countdown) {
			if (Time.time - begin > 1.0f) {
				flashText.text = (countdown-n).ToString();
				begin = Time.time;
				n++;
			}
			yield return null;
		}
		flashText.text = "";
		// record head direction
		tunnelPtr = Quaternion.FromToRotation(Vector3.up, head.Gaze.direction);
		tunnelPos = transform.position;
		makeTunnel = true;
	}

	float Sin(float x) {
		return (float)System.Math.Sin ((double)x);
	}

	float Cos(float x) {
		return (float)System.Math.Cos ((double)x);
	}

	void UpdateTunnel() {
		// update values
		float totalX = 0.0f;
		float totalY = 0.0f;
		time += dt;
		for (int i=0; i<numNodes; i++) {
			float dx = thetasX[i] * Sin (omegasX[i] * time);;
			float dy = thetasY[i] * Sin (omegasY[i] * time);;
			totalX += dx;
			totalY += dy;
		}
		phiX = totalX;
		phiY = totalY;
		// create tunnel
		Vector3 v1 = new Vector3(Sin(phiX),Cos(phiX),0);
		Vector3 v2 = new Vector3(0,Cos(phiY),Sin(phiY));
		tunnelFwd = tunnelPtr * Vector3.Normalize(v1 + v2);
		tunnelPos = tunnelPos + (velocity * dt * tunnelFwd);
//		Vector3 n1 = Vector3.Normalize(new Vector3( (fwd.y-fwd.x)/(fwd.y-fwd.z), 0, 1));
//		Vector3 n2 = -1.0f * n1;
//		Vector3 n3 = Vector3.Normalize(Vector3.Cross(fwd, n1));
//		Vector3 n4 = -1.0f * n3;
//		Vector3 pt1 = tunnelPos + tunnelRadius * n1;
//		Vector3 pt2 = tunnelPos + tunnelRadius * n2;
//		Vector3 pt3 = tunnelPos + tunnelRadius * n3;
//		Vector3 pt4 = tunnelPos + tunnelRadius * n4;
//		Plane pl1 = new Plane(n1,pt1);
//		Plane pl2 = new Plane(n2,pt2);
//		Plane pl3 = new Plane(n3,pt3);
//		Plane pl4 = new Plane(n4,pt4);
//		Instantiate(prefabPlane,pt1,Quaternion.identity);
//		Instantiate(prefabPlane,pt2,Quaternion.identity);
//		Instantiate(prefabPlane,pt3,Quaternion.identity);
//		Instantiate(prefabPlane,pt4,Quaternion.identity);
	}

	IEnumerator Flash(string msg, float t) {
		flashText.text = msg;
		yield return new WaitForSeconds(t);
		flashText.text = "";
	}

	void OnCollisionEnter (Collision other) {
		// maybe update highscore
		if (score > highscore) {
			highscore = score;
			highscoreText.text = "Highscore: "+((int)highscore).ToString();
			PlayerPrefs.SetFloat("highscore", score);
		}
		// flash score
		StartCoroutine(Flash(score.ToString(), 1.5f));
		// reset
		score = 0;
		velocity = initialVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		while (makeTunnel && Vector3.Distance(transform.position, tunnelPos) < 40.0f) {
			UpdateTunnel();
			if (counter == 10) {
				Instantiate(prefab,tunnelPos,Quaternion.FromToRotation(Vector3.up,tunnelFwd));
				counter = 0;
			}
			++counter;
		}
		// velocity
		velocity += acceleration * Time.deltaTime;
		Vector3 fwd = head.Gaze.direction;
		transform.position = transform.position + (velocity * Time.deltaTime * fwd);
		// score
		score += Time.deltaTime;
		scoreText.text = "Score: "+((int)score).ToString();
	}
}
