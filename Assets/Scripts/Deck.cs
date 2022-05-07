using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoPoker {
    [CreateAssetMenu(fileName = "New Deck Style", menuName = "Deck Style")]
    public class Deck : ScriptableObject {
        public List<Sprite> cards;
    }
}
