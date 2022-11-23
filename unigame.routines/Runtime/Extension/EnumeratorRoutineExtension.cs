namespace UniModules.UniRoutine.Runtime.Extension
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using global::UniGame.Runtime.ObjectPool;
    using global::UniGame.Runtime.ObjectPool.Extensions;
    using global::UniGame.Core.Runtime;
    using UniRoutines.unigame.routines.Assets.UniGame.Routines.Runtime.Extension;
    using UnityEngine;
    using UnityEngine.Profiling;

    public static partial class RoutineExtension
    {

        public static IEnumerator WaitUntil(this IEnumerator enumerator, ICompletionStatus status) {

            if(status == null)yield break;
            
            while (status.IsComplete == false) {
                yield return null;
            }

            yield return enumerator;
        }

        public static IEnumerator WaitUntil(this IEnumerator enumerator,  Func<bool> completeFunc)
        {

            if (completeFunc == null) yield break;
            while (completeFunc() == false)
            {
                yield return null;
            }

            yield return enumerator;
        }

        public static IEnumerator DoDelayed(this IEnumerator enumerator)
        {
            yield return null;
            yield return enumerator;
        }

        public static IEnumerator WaitForSeconds(this IEnumerator enumerator,float delay)
        {
            var time = Time.time + delay;
            while (time < Time.time)
            {
                yield return null;
            }

            yield return enumerator;
        }
        
        public static IEnumerator WaitForSecondUnscaled(this IEnumerator enumerator,float delay)
        {
            var time = Time.unscaledTime;
            var endOfAwait = time + delay;
            while (time < endOfAwait)
            {
                yield return null;
                time = Time.unscaledTime;
            }

            yield return enumerator;
        }

        public static IEnumerator WaitWhile(this IEnumerator enumerator, Func<bool> completeFunc)
        {

            while (completeFunc?.Invoke() == true)
            {
                yield return null;
            }

            yield return enumerator;
        }

        public static IEnumerator ExecuteWhile(this IEnumerator enumerator, Func<IEnumerator> sequence, Func<bool> condition)
        {
            while (condition())
            {
                yield return sequence();
                yield return null;
            }

            yield return enumerator;
        }

        /// <summary>
        /// execute target action when condition is true, repeat until awaiter is true
        /// </summary>
        /// <param name="enumerator"></param>
        /// <param name="action">target action</param>
        /// <param name="condition">action condition</param>
        /// <param name="awaiter">awaiter</param>
        /// <returns>progress awaiter</returns>
        public static IEnumerator ExecuteWhen(this IEnumerator enumerator, Action action, Func<bool> condition,
            Func<bool> awaiter)
        {
                        
            while (awaiter())
            {
                if (condition())
                {
                    action();
                }
                yield return null;
            }

            yield return enumerator;

        }
        
        
        /// <summary>
        /// repeat target action until condition is true
        /// </summary>
        /// <returns>progress enumerator</returns>
        public static IEnumerator ExecuteWhile(this IEnumerator enumerator, Action action, Func<bool> condition)
        {

            while (condition())
            {
                action();
                yield return null;
            }

            yield return enumerator;
            
        }
        
        public static IEnumerator RoutineWaitUntil(this IEnumerator enumerator, AsyncOperation operation) {

            while (operation!=null && operation.isDone == false) {
                yield return null;
            }

            yield return enumerator;
        }
        
    }
}
