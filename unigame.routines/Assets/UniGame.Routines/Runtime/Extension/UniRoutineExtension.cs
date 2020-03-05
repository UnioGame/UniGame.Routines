namespace UniGreenModules.UniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extension;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;

    public static class UniRoutineExtension {

        public static IDisposableItem RunWithSubRoutines(this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update)
        {
            return ExecuteRoutine(enumerator,routineType);   
        }
        
        public static IDisposableItem ExecuteRoutine(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false)
        {
            return Execute(enumerator,routineType,moveNextImmediately).AsDisposable();
        }
	
        public static RoutineHandler Execute(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false)
        {
            return UniRoutineManager.RunUniRoutine(enumerator,routineType,moveNextImmediately);
        }

        
        public static bool Cancel(this RoutineHandler handler)
        {
            return UniRoutineManager.TryToStopRoutine(handler);
        }
        
        public static bool IsActive(this RoutineHandler handler)
        {
            return UniRoutineManager.IsRoutineActive(handler);
        }
    
        public static IDisposableItem AsDisposable(this RoutineHandler handler)
        {
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => UniRoutineManager.TryToStopRoutine(handler));
            return disposable;
        }

        public static RoutineHandler AddTo(this RoutineHandler handler,ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => handler.Cancel());
            return handler;
        }
    
        public static IDisposableItem AddTo(this RoutineHandler handler,ICollection<IDisposable> collection)
        {
            var disposable = handler.AsDisposable();
            collection.Add(disposable);
            return disposable;
        }

        
    }
}
