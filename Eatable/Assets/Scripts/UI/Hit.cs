using UnityEngine;
using TMPro;

namespace Eatable.UI {
    [RequireComponent (typeof (Animator))]
    internal abstract class Hit : MonoBehaviour {
        private protected Animator animator;
        [SerializeField]
        internal TMP_Text text;
        [SerializeField]
        internal string prefix = "";

        private void Awake () {
            animator = GetComponent<Animator> ();
        }
        internal void Show (object sender, object arguments) {
            if (arguments is int value) {
                gameObject.SetActive (true);
                text.text = $"{prefix}{value}";
                animator.SetTrigger ("Show");
            }
        }
    }
}
