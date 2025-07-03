using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class ScrollRectFixed : ScrollRect
    {
        [SerializeField] bool scrollOnEnable = false;
        [SerializeField] Vector2 start, end;
        [SerializeField] float delay = 0;

        int pointerID = -1;

        public override void OnInitializePotentialDrag (PointerEventData eventData)
        {
            StopMovement ();

            base.OnInitializePotentialDrag (eventData);

            OnEndDrag (eventData);
            pointerID = eventData.pointerId;
        }

        public override void OnDrag (PointerEventData eventData)
        {
            if (eventData.pointerId == pointerID)
            {
                base.OnDrag (eventData);
            }
        }

        public override void StopMovement ()
        {
            base.StopMovement ();
            StopAllCoroutines ();
        }

        public bool IsMoving ()
        {
            return velocity != Vector2.zero;
        }

        public void SetPosition (Vector2 end, float delay)
        {
            SetPosition (normalizedPosition, end, delay);
        }

        public void SetPosition (Vector2 start, Vector2 end, float delay)
        {
            StopMovement ();
            StartCoroutine (IESetPosition (start, end, delay));
        }

        IEnumerator IESetPosition (Vector2 start, Vector2 end, float delay)
        {
            yield return new WaitForEndOfFrame ();
            normalizedPosition = start;
            yield return new WaitForSecondsRealtime (delay);

            while (normalizedPosition != end)
            {
                normalizedPosition = Vector2.Lerp (normalizedPosition, end, Time.unscaledDeltaTime * 5f);
                yield return null;
            }
        }

        protected override void OnEnable ()
        {
            base.OnEnable ();

            if (scrollOnEnable)
            {
                SetPosition (start, end, delay);
            }
        }
    }
}
