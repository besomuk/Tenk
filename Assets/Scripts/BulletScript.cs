using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour 
{
	public GameObject explosion;
	public float time_to_live = 5;

	Rigidbody2D _rigidbody; 

	// Use this for initialization
	void Start () 
	{
		if ( this.tag != "ProtoBullet")
			Destroy (this.gameObject, time_to_live); // osiguranje da ce se metak samo unistiti posle nekog vremena, na primer 
		    	                                     // ako se zaglavi izmedju nekih objekata ili ako iz ko zna kog razloga prodje kroz objekat "Granica"

		_rigidbody = GetComponent<Rigidbody2D> ();        // uzmi rigidbody komponentu

		if (_rigidbody == null)
		{
			Debug.LogError ("Rigidbody2D nedostaje");
			gameObject.AddComponent<Rigidbody2D> ();
		}						
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		// svuda oko arene su definisani prazni game objekti sa tagom "Granica".
		// ako metak udari u njih - nestaje
		// objekat je definisan tagom i Collider-om koji je isTrigger, osim u slucaju ako djule moze da se odbija o objekat,
		// tada nije potrebna ama bas nikakvo podesavanje. Dovoljna je sama fizika Collidera
		if (other.gameObject.tag == "Granica")
		{
			Destroy (this.gameObject);
		}

		// ako metak udari u objekat sa tagom prepreka, nesto se desava
		//if (other.gameObject.tag == "PreprekaOdbitak")
		//{
		//}

		if (other.gameObject.tag == "PreprekaKill")
		{
			Destroy (this.gameObject);
		}

	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			Destroy (this.gameObject);
		}		


	}

	void OnDestroy() 
	{
		GameObject exp;

		// proveri da li je objekat null, bez obzira sto sam ga rucno dodao bulletu. Kada se igra zatvara, unistavaju se svi objekti, a poziva se
		// i ova metoda, koja u tom slucaju baca exception.
		if (explosion != null) 
		{
			//aso.Play();
			exp = Instantiate (explosion, new Vector2 (this.transform.position.x, this.transform.position.y), Quaternion.identity);
			exp.tag = "BulletExplosion";
		}

		if( this.tag != "ProtoBullet" )
		{
			TankScript.currentNumberOfBullets--; // skenjaj jedno djule
		}
	}
}
