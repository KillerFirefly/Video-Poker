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
		[SerializeField] private Button dealButton = null;

		[SerializeField] private Image[] cards;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake() {
			if (gameManager == null) {
				gameManager = FindObjectOfType<GameManager>();
			}
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start() {
			betButton.onClick.AddListener(OnBetButtonPressed);
			RefreshBalanceText();

			dealButton.onClick.AddListener(OnDealButtonPressed);
		}

		//-//////////////////////////////////////////////////////////////////////
		///
		/// Event that triggers when bet button is pressed
		/// 
		private void OnBetButtonPressed() {
			GameManager.IncrementBet();
			RefreshBalanceText();
			RefreshBetText();
		}
		private void RefreshBetText() {
			currentBetText.text = $"Bet: {GameManager.CurrentBet}";
		}
		private void RefreshBalanceText() {
			currentBalanceText.text = $"Balance: {GameManager.CurrentBalance} Credits";
		}

		private void OnDealButtonPressed() {
			Debug.Log("Deal");
			foreach (var card in cards) {
				card.sprite = gameManager.DrawCard();
			}
		}
	}
}