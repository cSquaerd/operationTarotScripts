using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatScript : MonoBehaviour
{
	public VirScript player;
	public Mesh harvestedMesh;
	private MeshFilter wheatMesh;

	private void OnCollisionEnter(Collision col) {
		if (col.collider.name == "Sickle") {
			if (SickleScript.wheatCount < SickleScript.carryLimit) {
				player.printToHUD("Picked up a piece of wheat.");
				print("Picked up a piece of wheat.");
				SickleScript.wheatCount++;
				if (harvestedMesh != null) {
					wheatMesh.mesh = harvestedMesh;
					this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
				} else Destroy(this.gameObject);
			} else {
				player.printToHUD("You're carrying too much wheat, go deposit it at the silo.");
			}
		}
	}
	// Start is called before the first frame update
	void Start() {
		wheatMesh = this.gameObject.GetComponent<MeshFilter>();
	}

	// Update is called once per frame
	void Update() {}
}
