// Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Entities/Enemy/Enemy Shield")]
public class EnemyShield : MonoBehaviour
{
	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "Enemy") {
			Enemy enemy = other.GetComponent<Enemy>();
			enemy.gainShield();
		}
	}
}
