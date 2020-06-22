namespace UniGreenModules.UniRoutine.Runtime {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Interfaces;
    using UniCore.Runtime.Attributes;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Utils;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UnityEngine;

    public class UniRoutinObject : MonoBehaviour, 
        IDisposable,
        ILifeTimeContext
    {
        [ReadOnlyValue]
        [SerializeField]
        public RoutineScope scope;
        
        [SerializeField]
        private List<IUniRoutine> _lateUpdateRoutines = new List<IUniRoutine>();
        
        [SerializeField]
        private UniRoutine[] _routines = new UniRoutine[EnumValue<RoutineType>.Values.Count];
        
        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        public ILifeTime LifeTime => _lifeTime;
      
        #region public methods
        
        public void Dispose()
        {
            _lateUpdateRoutines.Clear();
            StopAllCoroutines();
            _lifeTime.Terminate();
        }

        public IUniRoutineTask AddRoutine(RoutineType type, IEnumerator enumerator, bool moveNextImmediately = true)
        {
            var routine = GetRoutine(type);
            return routine.AddRoutine(enumerator, moveNextImmediately);
        }

        public IUniRoutine GetRoutine(RoutineType routineType)
        {
            var index   = (int) routineType;
            var routine = _routines[index];
            if (routine != null) return routine;
            routine = CreateRoutine(routineType);
            _routines[index] = routine;
            return routine;
        }
        
        #endregion
        
        private void AddLateRoutine(IUniRoutine routine)
        {
            _lateUpdateRoutines.Add(routine);
        }
        
        private void LateUpdate()
        {
            for (var i = 0; i < _lateUpdateRoutines.Count; i++) {
                var routine = _lateUpdateRoutines[i];
                routine.Update();
            }
        }

        private void OnDestroy() => Dispose();
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private UniRoutine CreateRoutine(RoutineType routineType)
        {
            //create uni routine
            var routine = new UniRoutine();
            //run coroutine for target update type
            ExecuteRoutine(routine, routineType);
            
            return routine;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExecuteRoutine(
            IUniRoutine routine, 
            RoutineType routineType)
        {
            if (routineType == RoutineType.LateUpdate) {
                AddLateRoutine(routine);
                return;
            }

            StartCoroutine(ExecuteOnUpdate(routine, routineType));
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator ExecuteOnUpdate(IUniRoutine routine, RoutineType routineType)
        {
            var awaiter = GetRoutineAwaiter(routineType);
            while (true) {
                routine.Update();
                //wait time before next update
                yield return awaiter;
            }
        }
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static YieldInstruction GetRoutineAwaiter(RoutineType routineType)
        {
            switch (routineType) {
                case RoutineType.Update:
                    return null;
                case RoutineType.EndOfFrame:
                    return new WaitForEndOfFrame();
                case RoutineType.FixedUpdate:
                    return new WaitForFixedUpdate();
                case RoutineType.LateUpdate:
                    return null;
            }

            return null;
        }
    }

}
