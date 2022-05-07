using UnityEngine;

namespace VideoPoker
{
	//-//////////////////////////////////////////////////////////////////////
	/// 
	/// The main game manager
	/// 
	public class GameManager : MonoBehaviour {

		private static int currentBalance = 100;
		public static int CurrentBalance { get => currentBalance;}
		
		private static int currentBet = 0;
		public static int CurrentBet { get => currentBet;}

		[SerializeField] private Deck startingDeck;
		[SerializeField] private Deck currentDeck;

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Awake()
		{
			currentDeck = startingDeck;
		}

		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Start()
		{
		}
		
		//-//////////////////////////////////////////////////////////////////////
		/// 
		void Update()
		{
		}

		//private void Deal() {
		//	foreach (var card in cards) {
		//		int randomCardIndex = Random.Range(0, currentDeck.cards.Count);
		//		currentDeck.cards.RemoveAt(randomCardIndex);
		//		card.GetComponentInChildren<Image>().
		//	}
		//}
		public Sprite DrawCard() {
			int randomCardIndex = Random.Range(0, currentDeck.cards.Count);
			Sprite card = currentDeck.cards[randomCardIndex];
			currentDeck.cards.RemoveAt(randomCardIndex);
			return card;
		}

		public static void IncrementBet() {
			if (CurrentBet < 5 && currentBalance >= 1) {
				currentBalance--;
				currentBet++;
			}
		}
	}
}