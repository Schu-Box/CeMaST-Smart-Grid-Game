using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour {

	public bool book1;

	private GameManager gameManager;

	private void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	private void OnMouseUpAsButton() {
		if(!GameManager.IsPointerOverGameObject()) {
			gameManager.OpenBook(book1);
		}
	}
}
