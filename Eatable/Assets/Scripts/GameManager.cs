using UnityEngine;
using System.Collections;

namespace Eatable {
    [AddComponentMenu ("Eatable/GameManager")]
    internal class GameManager : MonoBehaviour {
        private Coroutine coroutine;
        internal EatableFactory EatableFactory { get; private set; }
        internal UneatableFactory UneatableFactory { get; private set; }
        internal Card Card { get; private set; }
        internal Player Player { get; private set; }

        [SerializeField]
        internal Transform cardsPlace;
        [SerializeField]
        internal CardVisual cardsPrefab;

        private void Awake () {
            EatableFactory = new EatableFactory ();
            UneatableFactory = new UneatableFactory ();
            EventBus.Initialize ();
            EventBus.Subscribe (EventBus.EventName.NewGame, NewGame);
            EventBus.Subscribe (EventBus.EventName.NextQuestion, NextQuestion);
            EventBus.Subscribe (EventBus.EventName.SwipeRight, CheckAnswer);
            EventBus.Subscribe (EventBus.EventName.SwipeLeft, CheckAnswer);
            EventBus.Subscribe (EventBus.EventName.RightAnswer, StopTime);
            EventBus.Subscribe (EventBus.EventName.WrongAnswer, StopTime);
            Player = new Player ();
        }
        private IEnumerator WaitForStart () {
            yield return new WaitForSecondsRealtime (Configurations.TimeWaitForStart);
            EventBus.Call (EventBus.EventName.NewGame, this, null);
        }
        void Start () {
            StartCoroutine (WaitForStart ());
        }

        private void CreateCard () {
            CardFactory cardFactory = Random.Range (0, 2) switch {
                0 => UneatableFactory,
                1 => EatableFactory
            };

            Card = new Card (cardFactory);
            CardVisual cardVisual = Instantiate (cardsPrefab, cardsPlace, false);
            cardVisual.transform.position = new Vector3 (cardVisual.transform.position.x, cardVisual.transform.position.y - 2.5f, cardVisual.transform.position.z);
            cardVisual.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
            cardVisual.Initialize (Card);
        }
        private IEnumerator Time () {
            yield return new WaitForSecondsRealtime (Configurations.TimeForCardAnimation);
            EventBus.Call (EventBus.EventName.TimeStart, this, null);
            float time = Configurations.TimeToChoose;

            while (time > 0) {
                yield return new WaitForSecondsRealtime (Configurations.TimeStep);
                time -= Configurations.TimeStep;

                if (time > 0)
                    EventBus.Call (EventBus.EventName.TimeChange, this, time);
                else
                    EventBus.Call (EventBus.EventName.TimeLeft, this, 0f);
            }
        }
        private void CreateQuestion () {
            CreateCard ();
            coroutine = StartCoroutine (Time ());
        }
        private void NextQuestion (object sender, object arguments) {
            CreateQuestion ();
        }
        private void NewGame (object sender, object arguments) {
            EventBus.Call (EventBus.EventName.NextQuestion, this, null);
        }
        private void CheckAnswer (object sender, object arguments) {
			if (arguments is bool answer)
				if (answer == Card.Data.Eatable)
					EventBus.Call (EventBus.EventName.RightAnswer, this, null);
				else
					EventBus.Call (EventBus.EventName.WrongAnswer, this, null);
		}
        private void StopTime (object sender, object arguments) {
            if (coroutine != null) {
                StopCoroutine (coroutine);
                coroutine = null;
            }
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NewGame, NewGame);
            EventBus.Unsubscribe (EventBus.EventName.NextQuestion, NextQuestion);
            EventBus.Unsubscribe (EventBus.EventName.SwipeRight, CheckAnswer);
            EventBus.Unsubscribe (EventBus.EventName.SwipeLeft, CheckAnswer);
            EventBus.Unsubscribe (EventBus.EventName.RightAnswer, StopTime);
            EventBus.Unsubscribe (EventBus.EventName.WrongAnswer, StopTime);
            Player.OnDestroy ();
        }
    }
}
