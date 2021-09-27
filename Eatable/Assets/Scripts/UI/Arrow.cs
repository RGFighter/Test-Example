using UnityEngine;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/Arrow")]
    [RequireComponent (typeof (Animator))]
    internal class Arrow : MonoBehaviour {
        private Animator animator;

        private void Awake () {
            animator = GetComponent<Animator> ();
        }
		private void Start () {
            gameObject.SetActive (false);
            EventBus.Subscribe (EventBus.EventName.TimeStart, Show);
            EventBus.Subscribe (EventBus.EventName.RightAnswer, Hide);
            EventBus.Subscribe (EventBus.EventName.WrongAnswer, Hide);
            EventBus.Subscribe (EventBus.EventName.TimeLeft, Hide);
        }
        internal void Show (object sender, object arguments) {
            gameObject.SetActive (true);
            animator.SetTrigger ("Show");
        }
        internal void Hide (object sender, object arguments) {
            animator.SetTrigger ("Hide");
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.TimeStart, Show);
            EventBus.Unsubscribe (EventBus.EventName.RightAnswer, Hide);
            EventBus.Unsubscribe (EventBus.EventName.WrongAnswer, Hide);
            EventBus.Unsubscribe (EventBus.EventName.TimeLeft, Hide);
        }
    }
}
