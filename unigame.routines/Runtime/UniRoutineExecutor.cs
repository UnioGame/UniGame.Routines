namespace UniModules.UniRoutine.Runtime
{
    using System.Collections;
    using global::UniGame.Core.Runtime;

    public class UniRoutineExecutor : IContextExecutor<IEnumerator>
    {

        public IDisposableItem Execute(IEnumerator data)
        {
            var disposable = data.RunWithSubRoutines();
            return disposable;
        }

    }
}
