using UnityEngine;

namespace Eatable {
    [AddComponentMenu ("Eatable/Fx")]
    internal class Fx : MonoBehaviour {
        [SerializeField]
        internal Animator animator1;
        [SerializeField]
        internal Animator animator2;

        private void Start () {
			EventBus.Subscribe (EventBus.EventName.NextQuestion, Show);
		}
        internal void Show (object sender, object arguments) {
            animator1.SetTrigger ("Show");
            animator2.SetTrigger ("Show");
        }
        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NextQuestion, Show);
        }
    }
}
