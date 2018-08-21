using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//public class TankScript : MonoBehaviour 
public class TankScript : NetworkBehaviour
{
	public GameObject turret;        // uzmi kupolu
	public GameObject bullet;        // uzmi djule, od njega ce se praviti ostala djulad
	public GameObject bulletSpawn;   // uzmi prazan GO radi lakseg definisanja pozicije na kojoj se stvara granata.

	Transform _transform;       // uzmi transform od samog sebe
	Rigidbody2D _rigidbody;     // hocemo i rigidbody

	public float tankSpeed = 25;
	public float tankRotationSpeed = 2;
	public float tankBulletSpeed = 200;

	private float initialTankSpeed;   // podesena brzina tenka.
	                                  // koristi se za regulisanje brzine tenka u pesku i 
	                                  // za vracanje te brzine na 'default' vrednost ako nije u pesku
	                                  // ili u travi

	public int maxNumberOfBullets = 3;             // maksimalan broj djuladi koji moze da bude na ekranu u jednom trenutku
	public static int currentNumberOfBullets = 0;  // trenutni broj djuladi

	AudioSource audio_source_1;
	AudioSource audio_source_2;
	public AudioClip fire;
	public AudioClip engine;

	void Start () 
	{
		print ("Start()");

		_transform = GetComponent<Transform> ();          // uzmi transform komponentu
		_rigidbody = GetComponent<Rigidbody2D> ();        // uzmi rigidbody komponentu

		audio_source_1 = gameObject.AddComponent<AudioSource>();
		audio_source_2 = gameObject.AddComponent<AudioSource>();
		audio_source_1.clip = fire;
		audio_source_2.clip = engine;
		audio_source_1.playOnAwake = false;
		audio_source_2.playOnAwake = true;
		audio_source_2.loop = true;
		audio_source_2.Play();


		if (_rigidbody==null)
			Debug.LogError("Rigidbody2D nedostaje");


		initialTankSpeed = tankSpeed;
		//turret.transform.position = _transform.position;  // centriraj kupolu na sasiju
		//turret.transform.parent = _transform.transform;   // napravi od kupule dete sasije

		//Debug.LogError("start");
	}

	// Update is called once per frame
	void Update () 
	{
		if( !isLocalPlayer )
		{
			return;
		}
		// obradi napred nazad
		if (Input.GetKey (KeyCode.W))
		{
			_rigidbody.velocity = transform.up * tankSpeed;
			audio_source_2.pitch = 1.2f;
		} 
		else if (Input.GetKey (KeyCode.S))
		{
			_rigidbody.velocity = -transform.up * tankSpeed;
			audio_source_2.pitch = 1.2f;
		}
		else
		{
			_rigidbody.velocity = new Vector2(0,0);
			audio_source_2.pitch = 1.0f;
		}


		// obradi levo desno 
		if (Input.GetKey (KeyCode.D))
		{
			_transform.Rotate (Vector3.forward * -tankRotationSpeed);
			audio_source_2.pitch = 1.1f;
		}
		else if (Input.GetKey (KeyCode.A))
		{
			_transform.Rotate (Vector3.forward * tankRotationSpeed);
			audio_source_2.pitch = 1.1f;
		}
			

		// rotiraj kupolu ka misu ( ovo sam preuzeo od pitajboga koga )
		Vector3 mouse_pos;
		Transform target;
		Vector3 object_pos;
		float angle;			
		target = turret.transform;
		mouse_pos = Input.mousePosition;
		//mouse_pos.z = 5.23f; //The distance between the camera and object
		object_pos = Camera.main.WorldToScreenPoint(target.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;                   // to je ono sto mi treba i to je ono sto cu koristiti i kasnije, kada budem pravio djule
		turret.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle - 90));

		// obradi levi klik misa
		if (Input.GetMouseButtonDown (0))
		{
			SpawnBullet ( angle ); // posalji metku ugao kupole, to je ugao pod kojim ce se ispaljivati
		}
	}

	void SpawnBullet ( float angle )
	{
		if ( currentNumberOfBullets < maxNumberOfBullets )  // proveri da li smo dostigli maksimalan broj djuladi
		{
			audio_source_1.Play ();

			GameObject new_bullet; // definisi djule

			new_bullet = Instantiate (bullet, new Vector2 (bulletSpawn.transform.position.x, bulletSpawn.transform.position.y), Quaternion.identity);    // napravi ga na mestu gde je spawn objekat
			new_bullet.GetComponent<Rigidbody2D> ().rotation = angle - 90;                                                                               // malo ga zarotiraj, da napred bude pravo napred
			new_bullet.GetComponent<Rigidbody2D> ().AddForce ( turret.transform.up * tankBulletSpeed , ForceMode2D.Force );		                         // ispucaj ga napred 
			new_bullet.tag = "Bullet";

			//new_bullet.gameObject.AddComponent<BulletScript> (); // dodaj skriptu za samounistenje djuletu

			currentNumberOfBullets++; // uvecaj broj djuladi za jedan
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
			
		if( other.gameObject.tag == "Surface_Sand" )
		{
			print("U pesku sam");
			tankSpeed = initialTankSpeed / 2;
		}

	}

	void OnTriggerExit2D(Collider2D other)
	{		
		if( other.gameObject.tag == "Surface_Sand" )
		{
			print("U pesku sam nisam");
			tankSpeed = initialTankSpeed;
		}

	}

}