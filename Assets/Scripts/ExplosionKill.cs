/// <summary>
/// Samounistenje sprajta eksplozije nakon nekog vremena. Vreme zavisi od duzine animacije eksplozije.
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionKill : MonoBehaviour 
{
	float time_to_live = 0.9f;
	AudioSource aso;

	public GameObject expl;


	void Start () 
	{
		// samounisti se, osim u slucaju da si originalna eksplozija
		if( this.tag != "ProtoBulletExplosion" )
		{
			aso = gameObject.GetComponent<AudioSource>();
			aso.Play();	
			Destroy(this.gameObject, time_to_live);

			GameObject go;
			go = Instantiate (expl, new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity);    // napravi ga na mestu gde je spawn objekat
		}
	}
}
