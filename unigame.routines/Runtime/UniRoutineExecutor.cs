namespace UniModules.UniRoutine.Runtime
{
    using System.Collections;
    using UniGame.Core.Runtime.Interfaces;

    public class UniRoutineExecutor : IContextExecutor<IEnumerator>
    {

        public IDisposableItem Execute(IEnumerator data)
        {
            var disposable = data.RunWithSubRoutines();
            return disposable;
        }

    }
}
