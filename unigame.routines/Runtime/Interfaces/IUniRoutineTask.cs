using UniGame.Core.Runtime;

namespace UniModules.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.DataFlow.Interfaces;
    using global::UniGame.Core.Runtime.ObjectPool;
    using global::UniGame.Core.Runtime;

    public interface IUniRoutineTask : 
        IEnumerator<IEnumerator>, 
        IPoolable,
        ILifeTimeContext
    {
        int Id { get; }
        bool IsCompleted { get; }
        void Pause();
        void Unpause();
        void Complete();
    }
}