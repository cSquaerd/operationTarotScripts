using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatScript : MonoBehaviour
{
	public VirScript player;
	public MessorScript messor;
	public Mesh harvestedMesh;
	public float harvestedBump = 0.125f;
	private MeshFilter wheatMesh;
	private static Vector3 harvestedScale = new Vector3(0.5f, 0.5f, 0.5f);

	private void OnCollisionEnter(Collision col) {
		if (col.collider.name == "Sickle") {
			if (SickleScript.wheatCount < SickleScript.carryLimit) {
				player.printToHUD(
					"[You harvested a piece of wheat.\nYou can harvest "
					+ (SickleScript.carryLimit - SickleScript.wheatCount - 1)
					+ " more.]"
				);
				SickleScript.wheatCount++;
				if (harvestedMesh != null) {
					wheatMesh.mesh = harvestedMesh;
					Transform wheatTransform = this.gameObject.GetComponent<Transform>();
					wheatTransform.localScale = harvestedScale;
					wheatTransform.Translate(new Vector3(0.0f, harvestedBump, 0.0f));
					this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
				} else Destroy(this.gameObject);
			} else {
				player.printToHUD("[You're carrying too much wheat, go deposit it at the shed.]");
			}
			messor.resetDillyDally();
		}
	}
	// Start is called before the first frame update
	void Start() {
		wheatMesh = this.gameObject.GetComponent<MeshFilter>();
	}

	// Update is called once per frame
	void Update() {}
}
