using UniModules.UniGame.Core.Runtime.Interfaces;

namespace UniModules.UniRoutine.Runtime
{
    using System.Collections;
    using System.Collections.Generic;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;

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