using UnityEngine;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/HitScore")]
    internal class HitScore : Hit {
		private void Start () {
            gameObject.SetActive (false);
            EventBus.Subscribe (EventBus.EventName.GetScore, Show);
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.GetScore, Show);
        }
    }
}
