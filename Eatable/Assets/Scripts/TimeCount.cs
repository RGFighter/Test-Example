using System;
using UnityEngine;
using TMPro;

namespace Eatable {
    [AddComponentMenu ("Eatable/TimeCount")]
    [RequireComponent (typeof (TMP_Text))]
    [RequireComponent (typeof (Animator))]
    internal class TimeCount : MonoBehaviour {
        private TMP_Text text;
        private int second;
        private int nextSecond;
        private Animator animator;

        private void Awake () {
            text = GetComponent<TMP_Text> ();
            animator = GetComponent<Animator> ();
        }
		private void Start () {
            gameObject.SetActive (false);
            EventBus.Subscribe (EventBus.EventName.NewGame, Hide);
            EventBus.Subscribe (EventBus.EventName.NextQuestion, Hide);
            EventBus.Subscribe (EventBus.EventName.TimeStart, Start);
            EventBus.Subscribe (EventBus.EventName.TimeChange, Refresh);
            EventBus.Subscribe (EventBus.EventName.TimeLeft, Refresh);
        }
		private void Reset () {
            Change (Configurations.TimeToChoose);
        }
		private void Change (int value) {
            second = value;
            nextSecond = second - 1;
            text.text = second.ToString ();
        }
        internal void Start (object sender, object arguments) {
            Reset ();
            gameObject.SetActive (true);
            animator.SetTrigger ("Tick");
        }
        internal void Hide (object sender, object arguments) {
            gameObject.SetActive (false);
        }
        internal void Refresh (object sender, object arguments) {
            if (arguments is float time) {
                if (Math.Ceiling (time) <= nextSecond) {
                    Change (nextSecond);
                    animator.SetTrigger ("Tick");
                }
            }
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NewGame, Hide);
            EventBus.Unsubscribe (EventBus.EventName.NextQuestion, Hide);
            EventBus.Unsubscribe (EventBus.EventName.TimeStart, Start);
            EventBus.Unsubscribe (EventBus.EventName.TimeChange, Refresh);
            EventBus.Unsubscribe (EventBus.EventName.TimeLeft, Refresh);
        }
    }
}
