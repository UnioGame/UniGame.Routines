namespace UniModules.UniGame.DoTweenRoutines.Runtime
{
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;

    public static class DOTweenRoutineExtensions
    {
        
        public static async UniTask<Tween> WaitForCompletionTweenAsync(this Tween t)
        {
            while (t.active && !t.IsComplete())
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            return t;
        }

        public static async UniTask<Tween> WaitForRewindTweenAsync(this Tween t)
        {
            while (t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0.0)) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }

        public static async UniTask<Tween> WaitForKillTweenAsync(this Tween t)
        {
            while (t.active) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }
        public static async UniTask<Tween> WaitForElapsedLoopTweenAsync(this Tween t, int elapsedLoops)
        {
            while (t.active && t.CompletedLoops() < elapsedLoops) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }

        public static async UniTask<Tween> WaitForPositionTweenAsync(this Tween t, float position)
        {
            while (t.active && t.position * (t.CompletedLoops() + 1) < position) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }
        
        public static async UniTask<Tween> WaitForStartTweenAsync(this Tween t)
        {
            while (t.active && !t.playedOnce) {
                await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
            }
            return t;
        }

        public static IEnumerator WaitForCompletionTween(this Tween t)
        {
            while (t.active && !t.IsComplete()) {
                yield return null;
            }
        }
        
        public static IEnumerator WaitForRewindTween(this Tween t)
        {
            while (t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0.0)) {
                yield return null;
            }
        }
        
        public static IEnumerator WaitForKillTween(this Tween t)
        {
            while (t.active) {
                yield return null;
            }
        }
        
        public static IEnumerator WaitForElapsedLoopTween(this Tween t, int elapsedLoops)
        {
            while (t.active && t.CompletedLoops() < elapsedLoops) {
                yield return null;
            }
        }

        public static IEnumerator WaitForPositionTween(this Tween t, float position)
        {
            while (t.active && t.position * (t.CompletedLoops() + 1) < position) {
                yield return null;
            }
        }
        
        public static IEnumerator WaitForStartTween(this Tween t)
        {
            while (t.active && !t.playedOnce) {
                yield return null;
            }
        }
    }
}