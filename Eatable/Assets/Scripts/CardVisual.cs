using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

namespace Eatable {
    [AddComponentMenu ("Eatable/CardVisual")]
    internal class CardVisual : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
        private bool active = true;
        private Transform baseTransform;
        private Vector3 basePosition;
        private Quaternion baseRotation;
        [SerializeField]
        internal Animator animator;
        [SerializeField]
        internal SpriteRenderer spriteRenderer;
        [SerializeField]
        internal TMP_Text text;

        private void Awake () {
            baseTransform = transform;
        }
        private void Start () {
			EventBus.Subscribe (EventBus.EventName.SwipeLeft, WaitAndFree);
			EventBus.Subscribe (EventBus.EventName.SwipeRight, WaitAndFree);
			EventBus.Subscribe (EventBus.EventName.TimeLeft, Free);
		}
        private IEnumerator LoadSprite (string name) {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite> (name);
			yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded) {
                spriteRenderer.sprite = handle.Result;
                Addressables.Release (handle);
			}
		}
        internal void Initialize (Card card) {
            basePosition = baseTransform.position;
            baseRotation = baseTransform.rotation;
            text.text = card.Data.Text;
            StartCoroutine (LoadSprite (card.Data.ImagePath));
            animator.SetTrigger ("Show");
        }
        void IBeginDragHandler.OnBeginDrag (PointerEventData eventData) {
        }
        void IDragHandler.OnDrag (PointerEventData eventData) {
            if (active) {
                if (baseTransform.position.x > -1.4f && baseTransform.position.x < 1.4f) {
                    baseTransform.position = new Vector3 (baseTransform.position.x + eventData.delta.x * Time.deltaTime * 2f, baseTransform.position.y);
                    baseTransform.rotation = Quaternion.Euler (0f, 0f, baseTransform.localRotation.eulerAngles.z - eventData.delta.x * Time.deltaTime * 7f);
                } else {
                    active = false;

                    if (baseTransform.position.x <= -1.4f) {
                        animator.SetTrigger ("SwipeLeft");
                        EventBus.Call (EventBus.EventName.SwipeLeft, this, false);
                    } else {
                        animator.SetTrigger ("SwipeRight");
                        EventBus.Call (EventBus.EventName.SwipeRight, this, true);
                    }
                }
            }
        }
        void IEndDragHandler.OnEndDrag (PointerEventData eventData) {
            if (active) {
                baseTransform.position = Vector3.Lerp (baseTransform.position, basePosition, 1f);
                baseTransform.rotation = Quaternion.Lerp (baseTransform.rotation, baseRotation, 1f);
            }
        }
        private IEnumerator Wait () {
            yield return new WaitForSecondsRealtime (0.3f);
            Destroy (gameObject);
        }
        internal void WaitAndFree (object sender, object arguments) {
            Unsubscribe ();
            StartCoroutine (Wait ());
        }
        internal void Free (object sender, object arguments) {
            Unsubscribe ();
            Destroy (gameObject);
        }
        private void Unsubscribe () {
            EventBus.Unsubscribe (EventBus.EventName.SwipeLeft, WaitAndFree);
            EventBus.Unsubscribe (EventBus.EventName.SwipeRight, WaitAndFree);
            EventBus.Unsubscribe (EventBus.EventName.TimeLeft, Free);
        }
    }
}
