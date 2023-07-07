namespace UniGame.Routines.Runtime.Extension
{
    using System;
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using UniModules.UniRoutine.Runtime.Extension;
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

        public static async UniTask WaitForEndAsync(this Animator animator, int stateHash, int layer = 0)
        {
            while (animator == null || animator.GetCurrentAnimatorStateInfo(layer).shortNameHash != stateHash)
                await UniTask.Yield();

            await UniTask.Delay(TimeSpan.FromSeconds(animator.GetCurrentAnimatorStateInfo(layer).length));
        }
        
        public static IEnumerator WaitStateEnd(this Animator animator, int stateHash, int layer = 0)
        {
            if (animator == null || !animator.HasState(layer, stateHash)) {
                var nextState = animator.GetNextAnimatorStateInfo(layer);
                yield break;
            }
            
            animator.SetTrigger(stateHash);

            yield return animator.WaitForEnd(stateHash, layer);
        }
        
        public static async UniTask WaitStateEndAsync(this Animator animator, int stateHash, int layer = 0)
        {
            if (animator == null || !animator.HasState(layer, stateHash)) {
                var nextState = animator.GetNextAnimatorStateInfo(layer);
                return;
            }
            
            animator.SetTrigger(stateHash);

            await animator.WaitForEndAsync(stateHash, layer);
        }
        
        
    }
}