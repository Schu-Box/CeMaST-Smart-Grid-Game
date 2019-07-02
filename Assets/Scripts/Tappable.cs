using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tappable : MonoBehaviour {

	public string id;

	private Animator animator;

	private AudioManager audioManager;

	private void Awake() {
		animator = GetComponent<Animator>();

		audioManager = FindObjectOfType<AudioManager>();
	}

	private void OnMouseUpAsButton() {
		if(!GameManager.IsPointerOverGameObject() && animator.GetCurrentAnimatorClipInfo(0).Length == 0f) {
			audioManager.Play(id);

			animator.Play(id);
		}
	}
}
