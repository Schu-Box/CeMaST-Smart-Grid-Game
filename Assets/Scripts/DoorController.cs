using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorController : MonoBehaviour {

	public bool playerDoor = false;
	public Sprite doorClosedSprite;
	public Sprite doorOpenSprite;

	private GameManager gameManager;
	private bool interactable = false;
	private SpriteRenderer sr;
	private SpriteRenderer highlight;

	public Neighbor neighborOwner;

	void Awake() {
		gameManager = FindObjectOfType<GameManager> ();
		sr = GetComponent<SpriteRenderer>();
		highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();
		
		ResetDoor();
	}

	private void OnMouseUpAsButton() {
		if(interactable && !GameManager.IsPointerOverGameObject()) {
			highlight.enabled = false;

			if(playerDoor) {
				sr.sprite = doorOpenSprite;

				gameManager.InteractWithPlayerDoor();
			} else {
				//Interact with neighbor door
				sr.sprite = null;
				gameManager.InteractWithNeighbor(neighborOwner);
			}
		}
	}

	public void SetInteractable(bool inter) {
		ResetDoor();
		
		interactable = inter;

		if(interactable) {
			GetComponent<BoxCollider2D>().enabled = true;
		}
		
		highlight.enabled = inter;	
	}

	public void ResetDoor() {
		sr.sprite = doorClosedSprite;
		highlight.enabled = false;
	}
}
