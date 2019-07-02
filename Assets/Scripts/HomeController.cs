using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour {

	private Neighbor homeOwner;
	private Transform neighborTransform;

	public void SetHome(Neighbor owner) {
		homeOwner = owner;
		owner.SetHome(this);

		neighborTransform = transform.GetChild(1).transform;
	}

	public Neighbor GetHomeOwner() {
		return homeOwner;
	}

	public Vector3 GetNeighborPosition() {
		return neighborTransform.position;
	}
}
