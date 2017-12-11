using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float maxSpeed;
	public float walkSpeed;
	public float angle_turn;
	public float x;
	public float y;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void FixedUpdate()
	{
		float moveVertical = Input.GetAxis("Vertical");
		float moveHorizontal = Input.GetAxis("Horizontal");


		//TU JEST KOD OD MYSZY
		//Kierunek w którym patrzy gracz
		//var MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//Quaternion rot = Quaternion.LookRotation(transform.position - MousePosition, Vector3.forward);
		//transform.rotation = rot;
		//transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);
		//GetComponent<Rigidbody2D>().angularVelocity = 0;



		//RUCH
		GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Lerp(0, Input.GetAxis("Horizontal")* maxSpeed, 0.8f), Mathf.Lerp(0, Input.GetAxis("Vertical")* maxSpeed, 0.8f));


		//Obroty

		x = Input.GetAxis("RightStickHorizontal") * -0.1f;
		y = Input.GetAxis("RightStickVertical") * -0.1f;
		float angle = Mathf.Atan2 (x, y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));

	}
		
}
