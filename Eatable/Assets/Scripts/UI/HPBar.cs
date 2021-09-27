using UnityEngine;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/HPBar")]
    internal class HPBar : MonoBehaviour {
        [SerializeField]
        internal Transform place;
        [SerializeField]
        internal Heart prefab;
        [Space]
        [SerializeField]
        internal Heart [] hearts;

		private void Awake () {
            hearts = new Heart [Configurations.HP_Max];

            for (int i = 0; i < Configurations.HP_Max; i++) {
                Heart heart = Instantiate (prefab, place, false);
                heart.transform.localScale = Vector3.one;
                hearts [i] = heart;
            }
        }
		private void Start () {
            EventBus.Subscribe (EventBus.EventName.NewGame, Restore);
            EventBus.Subscribe (EventBus.EventName.GetDamage, GetDamage);
        }
		internal void Restore (object sender, object arguments) {
            for (int i = 0; i < hearts.Length; i++) {
                hearts [i].Available (true);
            }
        }
        internal void GetDamage (object sender, object arguments) {
            if (sender is Player player)
                for (int i = player.Hp; i < hearts.Length; i++) {
                    hearts [i].Available (false);
                }
        }

        private void OnDestroy () {
            EventBus.Unsubscribe (EventBus.EventName.NewGame, Restore);
            EventBus.Unsubscribe (EventBus.EventName.GetDamage, GetDamage);
        }
    }
}
