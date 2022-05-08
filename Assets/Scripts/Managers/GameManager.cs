using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Text.RegularExpressions;

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
		[SerializeField] private Card[] hand;

		[SerializeField] private Deck deckStyle;
		[SerializeField] private List<Sprite> currentDeck;

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
			uiManager.RefreshBalanceText();

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

		/// <summary>Refresh the current play deck from the deck style.</summary>
		private void NewDeck() {
			currentDeck = new List<Sprite>(deckStyle.cards);
		}

		/// <summary>Unhold all held cards.</summary>
		private void ClearHolds() {
			foreach (var card in hand) {
				card.SetHold(false);
			}
		}
		private void SetCardsInteractable(bool value) {
			foreach (var card in hand) {
				card.button.interactable = value;
			}
		}

		void Start() {
			dealButton.onClick.AddListener(OnDealButtonPressed);
			drawButton.onClick.AddListener(OnDrawButtonPressed);
		}

		/// <summary>Draw a random card from the deck.</summary>
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
			foreach (var card in hand) {
				if (!card.hold) {
					card.cardImage.sprite = DrawCard();
				}
			}

			SetCardsInteractable(true);
		}
		private void OnDrawButtonPressed() {
			foreach (var card in hand) {
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
			//Get the name from the card sprite
			List<string> cards = new List<string>();
			foreach (var card in hand) {
				//Regex cardName = new Regex("[^_]*$");
				//Regex cardDigit = new Regex("\d{2}");
				string cardName = card.cardImage.sprite.name;
				cards.Add(cardName.Substring(cardName.Length - 3));
			}

			int prize = 0;
			string winType = "";

			if (RoyalFlush(cards)) { prize = 800; winType = "Royal Flush!!"; } 
			else if (StraightFlush(cards)) { prize = 50; winType = "Straight Flush!"; }
			else if (FourOfAKind(cards)) { prize = 25; winType = "Four of a kind";}
			else if (FullHouse(cards)) { prize = 9; winType = "Full House";}
			else if (Flush(cards)) { prize = 6; winType = "Flush";}
			else if (Straight(cards)) { prize = 4; winType = "Straight";}
			else if (ThreeOfAKind(cards)) { prize = 3; winType = "Three of a kind";}
			else if (TwoPair(cards)) { prize = 2; winType = "Two Pair";}
			else if (JacksOrBetter(cards)) { prize = 1; winType = "Jacks or Better"; }

			//Print win message
			int winAmount = currentBet * prize;
			uiManager.PrintWinMessage(winType, winAmount.ToString());

			currentBalance += winAmount;
		}

		private bool RoyalFlush(List<string> cards) {
			if (Flush(cards)) {
				List<int> digits = ExtractDigits(cards);

				digits.Sort();
				if (digits[0] == 1 && digits[1] == 10 && digits[2] == 11 && digits[3] == 12 && digits[4] == 13) {
					return true;
				}
			}
			
			return false;
		}

		private bool StraightFlush(List<string> cards) {
			return Straight(cards) && Flush(cards);
		}

		private bool FourOfAKind(List<string> cards) {
			bool four = false;

			List<int> digits = ExtractDigits(cards);

			for (int first = 0; first < digits.Count; first++) {
				for (int second = 0; second < digits.Count; second++) {
					for (int third = 0; third < digits.Count; third++) {
						for (int fourth = 0; fourth < digits.Count; fourth++) {
							//Make sure we're not comparing a card with itself
							if (first != second && first != third && first != fourth && second != third && second != fourth && third != fourth) {
								if (digits[first] == digits[second] && digits[second] == digits[third] && digits[third] == digits[fourth]) {
									four = true;
								}
							}
						}
					}
				}
			}

			return four;
		}

		private bool FullHouse(List<string> cards) {
			
			
			return false;
		}

		private bool Flush(List<string> cards) {
			bool flush = false;

			List<char> suits = ExtractSuits(cards);

			if (suits[0] == suits[1] && suits[1] == suits[2] && suits[2] == suits[3] && suits[3] == suits[4]) {
				flush = true;
			}

			return flush;
		}

		private bool Straight(List<string> cards) {
			bool straight = false;

			List<int> digits = ExtractDigits(cards);

			digits.Sort();
			//Check incrementing by one
			for (int i = 1; i < digits.Count; i++) {
				if (digits[i] == digits[i-1] + 1) {
					straight = true;
				} else {
					straight = false;
					break;
				}
			}
			//if (digits[1] == digits[0] + 1 && digits[2] == digits[1] + 1 && digits[3] == digits[2] + 1 && digits[4] == digits[3] + 1) {
			//	straight = true;
			//}

			return straight;
		}

		private bool ThreeOfAKind(List<string> cards) {
			bool triple = false;

			List<int> digits = ExtractDigits(cards);

			for (int first = 0; first < digits.Count; first++) {
				for (int second = 0; second < digits.Count; second++) {
					for (int third = 0; third < digits.Count; third++) {
						//Make sure we're not comparing a card with itself
						if (first != second && first != third && second != third) {
							if (digits[first] == digits[second] && digits[second] == digits[third]) {
								triple = true;
							}
						}
					}
				}
			}

			return triple;
		}

		private bool TwoPair(List<string> cards) {
			int pairCount = 0;

			List<int> digits = ExtractDigits(cards);

			//Find pairs
			for (int first = 0; first < digits.Count; first++) {
				for (int second = 0; second < digits.Count; second++) {
					if (first != second) {
						if (digits[first] == digits[second]) {
							pairCount++;
							//This counts each pair twice.
						}
					}
				}
			}

			//Divide our doubled pair count.
			pairCount /= 2;
			
			return pairCount == 2;
		}

		private bool JacksOrBetter(List<string> cards) {
			//if contains pair of 11 or higher?
			//Should this include aces?
			int jacks = 0;
			int queens = 0;
			int kings = 0;
			foreach (var card in cards) {
				int digit = int.Parse(card.Substring(card.Length - 2));
				if (digit == 11) jacks++;
				else if (digit == 12) queens++;
				else if (digit == 13) kings++;
			}

			if (jacks == 2 || queens == 2 || kings == 2) {
				return true;
			} else {
				return false;
			}
		}


		/// <summary>Remove the suite from the name of cards.</summary>
		/// <returns>Int List of parsed digits.</returns>
		private static List<int> ExtractDigits(List<string> cards) {
			//Load digits list from card names
			List<int> digits = new List<int>();
			foreach (var card in cards) {
				int digit = int.Parse(card.Substring(card.Length - 2));
				digits.Add(digit);
			}

			return digits;
		}

		private List<char> ExtractSuits(List<string> cards) {
			List<char> suits = new List<char>();
			foreach (var card in cards) {
				char suit = card.ToCharArray()[0];
				suits.Add(suit);
			}
			return suits;
		}
	}
}