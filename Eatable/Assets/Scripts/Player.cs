namespace Eatable {
	internal class Player {
		internal int Hp { get; private set; }
		internal int Score { get; private set; }
		internal void GetDamage (object sender, object arguments) {
			Hp -= Configurations.HP_Damage;

			if (Hp < Configurations.HP_Min)
				Hp = Configurations.HP_Min;

			EventBus.Call (EventBus.EventName.GetDamage, this, Configurations.HP_Damage);

			if (Hp == Configurations.HP_Min)
				EventBus.Call (EventBus.EventName.GameOver, this, null);
			else
				EventBus.Call (EventBus.EventName.NextQuestion, this, null);
		}
		internal void GetScore (object sender, object arguments) {
			Score += Configurations.Score_Increase;
			EventBus.Call (EventBus.EventName.GetScore, this, Configurations.Score_Increase);
			EventBus.Call (EventBus.EventName.NextQuestion, this, null);
		}
		internal void Reset (object sender, object arguments) {
			Hp = Configurations.HP_Max;
			Score = Configurations.Score_Default;
		}
		internal Player () {
			EventBus.Subscribe (EventBus.EventName.NewGame, Reset);
			EventBus.Subscribe (EventBus.EventName.RightAnswer, GetScore);
			EventBus.Subscribe (EventBus.EventName.WrongAnswer, GetDamage);
			EventBus.Subscribe (EventBus.EventName.TimeLeft, GetDamage);
		}
		internal void OnDestroy () {
			EventBus.Unsubscribe (EventBus.EventName.NewGame, Reset);
			EventBus.Unsubscribe (EventBus.EventName.RightAnswer, GetScore);
			EventBus.Unsubscribe (EventBus.EventName.WrongAnswer, GetDamage);
			EventBus.Unsubscribe (EventBus.EventName.TimeLeft, GetDamage);
		}
	}
}
