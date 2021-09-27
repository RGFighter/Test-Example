using UnityEngine;
using UnityEngine.UI;

namespace Eatable.UI {
    [AddComponentMenu ("Eatable/UI/Heart")]
    [RequireComponent (typeof (Image))]
    internal class Heart : MonoBehaviour {
        private Image image;
		private void Awake () {
            image = GetComponent<Image> ();
        }
		internal void Available (bool state) {
            image.color = state ? new Color32 (255, 153, 213, 255) : new Color32 (100, 75, 100, 255);
        }
    }
}
