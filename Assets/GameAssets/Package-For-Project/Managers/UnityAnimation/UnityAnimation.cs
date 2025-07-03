using UnityEngine;

namespace ProjectTools
{
    [DefaultExecutionOrder (-100)]
    public class UnityAnimation : MonoBehaviour
    {
        public delegate void TrackEntryEventDelegate (string animationName, string eventName);
        public event TrackEntryEventDelegate Event;
        private void OnEvent (string eventName)
        {
            string animCurrent = anim.GetCurrentAnimatorClipInfo (0) [0].clip.name;
            Event?.Invoke (animCurrent, eventName);
        }

        public delegate void TrackEntryAnimationDelegate (string animationName);
        public event TrackEntryAnimationDelegate Complete;
        private void OnComplete (string animationName)
        {
            Complete?.Invoke (animationName);
        }

        public event TrackEntryAnimationDelegate Start;
        private void OnStart (string animationName)
        {
            Start?.Invoke (animationName);
        }

        public AnimatorStateInfo SetAnimation (int layer, string animationName, float mixDuration = .2f)
        {
            anim.Play (animationName, layer);
            anim.CrossFade (animationName, mixDuration);
            return anim.GetCurrentAnimatorStateInfo (layer);
        }

        public void SetBool (string id, bool value)
        {
            anim.SetBool (id, value);
        }

        public void SetFloat (string id, float value)
        {
            anim.SetFloat (id, value);
        }

        public void SetTrigger (string id)
        {
            anim.SetTrigger (id);
        }

        public bool IsAnimationCurrent (int layer, string animationName)
        {
            return anim.GetCurrentAnimatorStateInfo (layer).IsName (animationName);
        }

        public void SetSpeed (float value)
        {
            anim.speed = value;
        }

        [HideInInspector] public Animator anim;
        private void Awake ()
        {
            anim = GetComponent<Animator> ();
            var controller = anim.runtimeAnimatorController;
            var clips = controller.animationClips;

            for (int i = 0; i < clips.Length; i++)
            {
                var clip = clips [i];

                if (!HasEvent (clip, "OnComplete"))
                {
                    AnimationEvent complete = new AnimationEvent ();
                    complete.time = clip.length;
                    complete.functionName = "OnComplete";
                    complete.stringParameter = clips [i].name;
                    clip.AddEvent (complete);
                }

                if (!HasEvent (clip, "OnStart"))
                {
                    AnimationEvent start = new AnimationEvent ();
                    start.time = 0;
                    start.functionName = "OnStart";
                    start.stringParameter = clip.name;
                    clip.AddEvent (start);
                }
            }
        }

        private bool HasEvent (AnimationClip clip, string functionName)
        {
            var events = clip.events;
            for (int i = 0; i < events.Length; i++)
            {
                if (events [i].functionName == functionName)
                    return true;
            }

            return false;
        }
    }
}