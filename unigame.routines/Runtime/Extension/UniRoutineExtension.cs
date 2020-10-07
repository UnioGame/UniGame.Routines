namespace UniModules.UniRoutine.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Extension;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

    public static class UniRoutineExtension {

        public static IDisposableItem RunWithSubRoutines(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update)
        {
            return ExecuteRoutine(enumerator,routineType);   
        }

        public static IDisposableItem ExecuteRoutine(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false,
            RoutineScope scope = RoutineScope.Global)
        {
            return Execute(enumerator,routineType,moveNextImmediately,scope).AsDisposable();
        }
	
        public static RoutineHandle Execute(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false,
            RoutineScope scope = RoutineScope.Global)
        {
            return UniRoutineManager.RunUniRoutine(enumerator,routineType,scope,moveNextImmediately);
        }

        public static bool Cancel(this RoutineHandle handler)
        {
            return UniRoutineManager.TryToStopRoutine(handler);
        }
        
        public static bool IsActive(this RoutineHandle handler)
        {
            return UniRoutineManager.IsRoutineActive(handler);
        }
    
        public static IDisposableItem AsDisposable(this RoutineHandle handler)
        {
            var disposable = ClassPool.Spawn<DisposableAction>();
            disposable.Initialize(() => UniRoutineManager.TryToStopRoutine(handler));
            return disposable;
        }

        public static RoutineHandle AddTo(this RoutineHandle handler,ILifeTime lifeTime)
        {
            lifeTime.AddCleanUpAction(() => handler.Cancel());
            return handler;
        }
    
        public static IDisposableItem AddTo(this RoutineHandle handler,ICollection<IDisposable> collection)
        {
            var disposable = handler.AsDisposable();
            collection.Add(disposable);
            return disposable;
        }

        
    }
}
