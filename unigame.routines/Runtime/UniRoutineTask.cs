namespace UniModules.UniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class UniRoutineTask : IUniRoutineTask
    {
        private readonly Stack<IEnumerator> awaiters = new Stack<IEnumerator>();
        private readonly LifeTimeDefinition lifeTimeDefinition = new LifeTimeDefinition();
        public readonly ILifeTime lifeTime;

        public bool isComplete = true;
        public int IdValue;
        
        private IEnumerator rootEnumerator;
        private IEnumerator current;
        private RoutineState state = RoutineState.Complete;
        
        public UniRoutineTask()
        {
            lifeTime = lifeTimeDefinition.LifeTime;
        }

        public int Id => IdValue;
        
        public ILifeTime LifeTime => lifeTime;

        public bool IsCompleted => isComplete;
        
        public IEnumerator Current => current;

        object IEnumerator.Current => Current;

        public void Initialize(
            int id,
            IEnumerator enumerator,
            bool moveNextImmediately = false)
        {
            Release();

            lifeTimeDefinition.Release();
            
            IdValue = id;
            rootEnumerator   = enumerator;
            current = enumerator;

            SetTaskState(RoutineState.Active);

            if (moveNextImmediately) MoveNext();
        }

        /// <summary>
        /// iterate all enumerator steps with inner iterators
        /// </summary>
        /// <returns>is iteration completed</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (isComplete) return false;
            
            if (state == RoutineState.Paused)
                return true;

            var moveNext = MoveNextInner();
            if (!moveNext) {
                Complete();
            }

            return moveNext;
        }

        public void Pause()
        {
            if (isComplete) return;
            SetTaskState(RoutineState.Paused);
        }

        public void Unpause()
        {
            if (isComplete) return;
            SetTaskState(RoutineState.Active);
        }

        public void Complete()
        {
            if (isComplete) return;
            SetTaskState(RoutineState.Complete);
        }
        
        public void Release()
        {
            IdValue = 0;
            rootEnumerator = null;
            current = null;
            SetTaskState(RoutineState.Complete);
            awaiters.Clear();
            lifeTimeDefinition.Terminate();
        }
        
        public void Reset()
        {
            rootEnumerator?.Reset();
            current = rootEnumerator;
            awaiters.Clear();
            SetTaskState(RoutineState.None);
        }

        public void Dispose()
        {
            Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool MoveNextInner()
        {
            //if current already null - stop execution
            if (current == null)
            {
                Dispose();
                return false;
            }

            //cacl nect execution step
            var moveNext = current.MoveNext();

            //if current enumerator motion finished try get next one from stack
            if (!moveNext)
            {
                if (awaiters.Count == 0){
                    return false;
                }
                current = awaiters.Pop();
                return true;
            }

            while (moveNext && current?.Current is IEnumerator awaiter)
            {
                //add new inner enumerator to stack
                awaiters.Push(current);
                current = awaiter;
                //for new root enumerator calculate first step
                moveNext = current.MoveNext();
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetTaskState(RoutineState activeState)
        {
            state = activeState;
            isComplete = false;
            
            switch (state) {
                case RoutineState.Complete:
                    isComplete = true;
                    break;
            }
        }
        
    }

}