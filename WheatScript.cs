using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatScript : MonoBehaviour
{
	public VirScript player;

	private void OnCollisionEnter(Collision col) {
		if (col.collider.name == "Sickle") {
			if (SickleScript.wheatCount < 5) {
				player.printToHUD("Picked up a piece of wheat.");
				SickleScript.wheatCount++;
				Destroy(this.gameObject);
			} else {
				player.printToHUD("You're carrying too much wheat, go deposit it at the silo.");
			}
		}
	}
	// Start is called before the first frame update
	void Start() {}

	// Update is called once per frame
	void Update() {}
}
