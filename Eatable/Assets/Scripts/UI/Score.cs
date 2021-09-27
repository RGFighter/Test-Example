using UnityEngine;
using TMPro;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/Score")]
    internal class Score : MonoBehaviour {
        [SerializeField]
        internal TMP_Text text;

		private void Start () {
            EventBus.Subscribe (EventBus.EventName.NewGame, Reset);
            EventBus.Subscribe (EventBus.EventName.GetScore, Refresh);
        }
		internal void Reset (object sender, object arguments) {
            text.text = Configurations.Score_Default.ToString ();
        }
        internal void Refresh (object sender, object arguments) {
            if (sender is Player player)
                text.text = player.Score.ToString ();
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NewGame, Reset);
            EventBus.Unsubscribe (EventBus.EventName.GetScore, Refresh);
        }
    }
}
