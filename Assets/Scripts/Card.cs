using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

	public Image cardImage;
	public GameObject holdText;
	public bool hold = false;

	public void ToggleHold() {
		hold = !hold;
		holdText.SetActive(hold);
	}
	public void SetHold(bool value) {
		hold = value;
		holdText.SetActive(hold);
	}
}
