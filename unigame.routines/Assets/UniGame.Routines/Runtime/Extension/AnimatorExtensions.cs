namespace UniGame.Routines.Runtime.Extension
{
    using System.Collections;
    using UniGreenModules.UniRoutine.Runtime.Extension;
    using UnityEngine;

    public static class AnimatorExtensions
    {
        public static IEnumerator WaitForEnd(this Animator animator, int stateHash, int layer = 0)
        {
            while (animator == null || animator.GetCurrentAnimatorStateInfo(layer).shortNameHash != stateHash) {
                yield return null;
            }

            yield return animator.WaitForSeconds(animator.GetCurrentAnimatorStateInfo(layer).length);
        }

        public static IEnumerator WaitStateEnd(this Animator animator, int stateHash, int layer = 0)
        {
            if (animator == null || !animator.HasState(layer, stateHash))
                yield break;
            
            animator.SetTrigger(stateHash);

            yield return animator.WaitForEnd(stateHash, layer);
        }
    }
}