using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/GameOver")]
    internal class GameOver : MonoBehaviour {
        [SerializeField]
        internal TMP_Text score;
        [SerializeField]
        internal Button button;

		private void Start () {
            gameObject.SetActive (false);
            button.onClick.AddListener (() => { EventBus.Call (EventBus.EventName.NewGame, this, null); });
            EventBus.Subscribe (EventBus.EventName.NewGame, Hide);
            EventBus.Subscribe (EventBus.EventName.GameOver, Show);
        }
        internal void Hide (object sender, object arguments) {
            gameObject.SetActive (false);
        }
        internal void Show (object sender, object arguments) {
            if (sender is Player player) {
                gameObject.SetActive (true);
                score.text = $"счёт: {player.Score}";
            }
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NewGame, Hide);
            EventBus.Unsubscribe (EventBus.EventName.GameOver, Show);
        }
    }
}
