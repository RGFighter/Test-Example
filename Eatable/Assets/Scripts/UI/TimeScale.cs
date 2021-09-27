using UnityEngine;
using UnityEngine.UI;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/TimeScale")]
    internal class TimeScale : MonoBehaviour {
        private RectTransform rectTransform;
        private RectTransform.Axis axis = RectTransform.Axis.Horizontal;
        private float maxWidth;
        [SerializeField]
        internal Image image;

		private void Awake () {
            rectTransform = image.rectTransform;
            maxWidth = rectTransform.rect.width;
        }
		private void Start () {
            EventBus.Subscribe (EventBus.EventName.NextQuestion, Reset);
            EventBus.Subscribe (EventBus.EventName.TimeChange, Refresh);
        }
		internal void Reset (object sender, object arguments) {
            rectTransform.SetSizeWithCurrentAnchors (axis, maxWidth);
        }
        internal void Refresh (object sender, object arguments) {
            if (arguments is float time) {
                rectTransform.SetSizeWithCurrentAnchors (axis, maxWidth * time / Configurations.TimeToChoose);
            }
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NextQuestion, Reset);
            EventBus.Unsubscribe (EventBus.EventName.TimeChange, Refresh);
        }
    }
}
