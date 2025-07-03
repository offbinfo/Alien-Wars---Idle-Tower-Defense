using UnityEngine;
using UnityEngine.Events;

namespace ProjectTools
{
    [RequireComponent (typeof (UnityAnimation))]
    public class UnityAnimationTrigger : MonoBehaviour
    {
        public UnityEvent<string, string> onEvent;
        public UnityEvent<string> onStart, onComplete;

        UnityAnimation unityAnimation;

        private void Awake ()
        {
            unityAnimation = GetComponent<UnityAnimation> ();
            unityAnimation.Complete += UnityAnimation_Complete;
            unityAnimation.Start += UnityAnimation_Start;
            unityAnimation.Event += UnityAnimation_Event;
        }

        private void UnityAnimation_Event (string animationName, string eventName)
        {
            onEvent.Invoke (animationName, eventName);
        }

        private void UnityAnimation_Start (string animationName)
        {
            onStart.Invoke (animationName);
        }

        private void UnityAnimation_Complete (string animationName)
        {
            onComplete.Invoke (animationName);
        }
    }
}