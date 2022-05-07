using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VideoPoker {
	//-//////////////////////////////////////////////////////////////////////
	/// 
	/// The main game manager
	/// 
	public class GameManager : MonoBehaviour {
		[SerializeField] private UIManager uiManager;

		public int currentBalance = 100;
		public int currentBet = 0;

		[SerializeField] private Button dealButton = null;
		[SerializeField] private Button drawButton = null;
		[SerializeField] private Button betButton = null;
		[SerializeField] private Button betMaxButton = null;
		[SerializeField] private Card[] cards;

		[SerializeField] private Deck deckStyle;
		[SerializeField] private List<Sprite> currentDeck;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake() {
			if (uiManager == null) {
				uiManager = FindObjectOfType<UIManager>();
			}
			NewGame();
		}

		private void NewGame() {
			NewDeck();
			dealButton.gameObject.SetActive(true);
			dealButton.interactable = false;
			drawButton.gameObject.SetActive(false);
			betButton.interactable = true;
			betMaxButton.interactable = true;

			ClearHolds();
			currentBet = 0;
			uiManager.RefreshBetText();

			SetCardsInteractable(false);
		}
		private void GameOver() {
			Debug.Log("Game Over");
			dealButton.interactable = false;
			drawButton.interactable = false;
			betButton.interactable = false;
			betMaxButton.interactable = false;
			SetCardsInteractable(false);
		}
		private void NewDeck() {
			currentDeck = new List<Sprite>(deckStyle.cards);
		}
		private void ClearHolds() {
			foreach (var card in cards) {
				card.SetHold(false);
			}
		}
		private void SetCardsInteractable(bool value) {
			foreach (var card in cards) {
				card.GetComponent<Button>().interactable = value;
			}
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start() {
			dealButton.onClick.AddListener(OnDealButtonPressed);
			drawButton.onClick.AddListener(OnDrawButtonPressed);
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Update() {
		}

		public Sprite DrawCard() {
			int randomCardIndex = Random.Range(0, currentDeck.Count);
			Sprite card = currentDeck[randomCardIndex];
			currentDeck.RemoveAt(randomCardIndex);
			return card;
		}

		public void IncrementBet() {
			if (currentBet < 5 && currentBalance >= 1) {
				currentBalance--;
				currentBet++;
			}
			if (currentBet > 0) {
				dealButton.interactable = true;
			}
		}

		internal void MaxBet() {
			for (int i = 1; i <= 5 && currentBalance >= 1; i++) {
				currentBalance--;
				currentBet++;
			}
			if (currentBet > 0) {
				dealButton.interactable = true;
			}
		}

		private void OnDealButtonPressed() {
			dealButton.gameObject.SetActive(false);
			drawButton.gameObject.SetActive(true);
			betButton.interactable = false;
			betMaxButton.interactable = false;
			foreach (var card in cards) {
				if (!card.hold) {
					card.cardImage.sprite = DrawCard();
				}
			}

			SetCardsInteractable(true);
		}
		private void OnDrawButtonPressed() {
			foreach (var card in cards) {
				if (!card.hold) {
					card.cardImage.sprite = DrawCard();
				}
			}

			//Round Finished
			EvaluateHand();

			if (currentBalance > 0) {
				NewGame();
			} else {
				GameOver();
			}
		}

		private void EvaluateHand() {

		}
	}
}