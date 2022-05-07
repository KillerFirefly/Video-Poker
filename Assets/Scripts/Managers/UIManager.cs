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

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake() {
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
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
	}
}