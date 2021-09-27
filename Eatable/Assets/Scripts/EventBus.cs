using System;
using System.Collections.Generic;

namespace Eatable {
	internal readonly struct EventBus {
		private static Dictionary<EventName, EventHandler> events;
		internal delegate void EventHandler (object sender, object arguments);
		internal enum EventName { NewGame, NextQuestion, SwipeLeft, SwipeRight, RightAnswer, WrongAnswer, TimeStart, TimeLeft, GetDamage, GetScore, TimeChange, GameOver }

		internal static void Subscribe (EventName eventName, EventHandler handler) {
			events [eventName] += handler;
		}
		internal static void Unsubscribe (EventName eventName, EventHandler handler) {
			events [eventName] -= handler;
		}
		internal static void Call (EventName eventName, object sender, object arguments) {
			EventHandler handler = events [eventName];

			if (handler != null)
				handler (sender, arguments);
		}

		internal static void Initialize () {
			events = new Dictionary<EventName, EventHandler> ();

			foreach (EventName value in Enum.GetValues (typeof (EventName))) {
				events.Add (value, null);
			}
		}
	}
}
