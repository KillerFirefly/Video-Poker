using UnityEngine;
using UnityEngine.UI;

namespace VideoPoker {
	//-//////////////////////////////////////////////////////////////////////
	///
	/// Manages UI including button events and updates to text fields
	/// 
	public class UIManager : MonoBehaviour {
		[SerializeField] private GameManager gameManager = null;
		[SerializeField] private Text currentBalanceText = null;
		[SerializeField] private Text currentBetText = null;
		[SerializeField] private Text winningText = null;
		[SerializeField] private Button betButton = null;
		[SerializeField] private Button maxBetButton = null;

		void Start() {
			betButton.onClick.AddListener(OnBetButtonPressed);
			maxBetButton.onClick.AddListener(OnMaxBetButtonPressed);
			RefreshBalanceText();
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		/// Event that triggers when bet button is pressed
		/// 
		private void OnBetButtonPressed() {
			gameManager.IncrementBet();
			RefreshBalanceText();
			RefreshBetText();
		}
		private void OnMaxBetButtonPressed() {
			gameManager.MaxBet();
			RefreshBalanceText();
			RefreshBetText();
		}
		public void RefreshBetText() {
			currentBetText.text = $"Bet: {gameManager.currentBet}";
		}
		public void RefreshBalanceText() {
			currentBalanceText.text = $"Balance: {gameManager.currentBalance} Credits";
		}

		/// <summary>Message to be displayed with a win.</summary>
		/// <param name="type">Name of hand/win</param>
		/// <param name="credits">The amount that was won</param>
		public void PrintWinMessage(string type, string credits) {
			if (string.IsNullOrEmpty(type)) {
				winningText.text = "";
			} else {
				winningText.text = $"{type}! You won {credits} credits.";
			}
		}
	}
}