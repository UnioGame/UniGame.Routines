using System;

namespace UniModules.UniRoutine.Runtime.Interfaces
{
    using System.Collections;

    public interface IUniRoutine
    {
        bool CancelRoutine(int id);

        bool IsActive(int id);

        bool AddFinally(int id, Action action);
        
        IUniRoutineTask AddRoutine(IEnumerator enumerator, bool moveNextImmediately = true, Action finalAction = null);
        
        void Update();
    }
}