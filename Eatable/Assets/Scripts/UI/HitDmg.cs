using UnityEngine;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/HitDmg")]
    internal class HitDmg : Hit {
		private void Start () {
            gameObject.SetActive (false);
            EventBus.Subscribe (EventBus.EventName.GetDamage, Show);
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.GetDamage, Show);
        }
    }
}
