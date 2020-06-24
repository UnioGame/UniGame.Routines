namespace UniGreenModules.UniRoutine.Runtime
{
    using System.Collections;
    using System.Runtime.CompilerServices;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class UniRoutineManager
    {
        private static UniRoutinObject[] _routineObjects = new UniRoutinObject[2];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRoutineActive(RoutineHandle handler)
        {
            if(Application.isPlaying == false)
                return false;
            //get routine
            var scope   = GetRoutineObject(handler.Scope);
            var routine = scope.GetRoutine(handler.Type);
            return routine.IsActive(handler.Id);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryToStopRoutine(RoutineHandle handler)
        {
            if(Application.isPlaying == false)
                return true;
            //get routine
            var scope   = GetRoutineObject(handler.Scope);
            var routine = scope.GetRoutine(handler.Type);
            //add enumerator to routines
            return routine.CancelRoutine(handler.Id);
        }
        
        /// <summary>
        /// start uniroutine interator
        /// </summary>
        /// <param name="enumerator">target enumerator</param>
        /// <param name="routineType">routine type</param>
        /// <param name="scope">routine execution scope</param>
        /// <param name="moveNextImmediately"></param>
        /// <returns>cancelation handle</returns>
        public static RoutineHandle RunUniRoutine(
            IEnumerator enumerator,
            RoutineType routineType,
            RoutineScope scope,
            bool moveNextImmediately = true)
        {
            if(Application.isPlaying == false)
                return new RoutineHandle();
            
            var routineObject = GetRoutineObject(scope);
            //get routine
            var routine = routineObject.GetRoutine(routineType);
            //add enumerator to routines
            var routineTask = routine.AddRoutine(enumerator, moveNextImmediately);
            if (routineTask == null)
                return new RoutineHandle(0,routineType,scope);

            var routineValue = new RoutineHandle(routineTask.Id,routineType,scope);
            return routineValue;
        }

        private static UniRoutinObject GetRoutineObject(RoutineScope scope)
        {
            var index = (int) scope;
            var scopeObject = _routineObjects[index];
            if (scopeObject && scopeObject.gameObject)
                return scopeObject;

            scopeObject = CreateRoutineScopeObject(scope);
            _routineObjects[index] = scopeObject;
            return scopeObject;
        }

        private static UniRoutinObject CreateRoutineScopeObject(RoutineScope routineScope)
        {
            //create routine object and mark as immortal
            var gameObject        = new GameObject("UniRoutineManager");
            var routineGameObject = gameObject.AddComponent<UniRoutinObject>();
            if (routineScope == RoutineScope.Global) {
                UnityEngine.Object.DontDestroyOnLoad(gameObject); 
            }
            return routineGameObject;
        }

    }
}