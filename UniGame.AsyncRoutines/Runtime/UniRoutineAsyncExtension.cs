using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniModules.UniRoutine.Runtime;

public static class UniRoutineAsyncExtension
{
    public static IEnumerator<RoutineHandle> GetEnumerator(this RoutineHandle handle)
    {
        while (handle.IsActive())
        {
            yield return handle;
        }
    }

    public static IEnumerator GetAwaiter(this RoutineHandle handle)
    {
        while (handle.IsActive())
        {
            yield return null;
        }
    }

    public static async UniTask ToUniTask(
        this RoutineHandle handle,
        PlayerLoopTiming timing = PlayerLoopTiming.Update,
        CancellationToken cancellationToken = default)
    {
        await UniTask.WaitWhile(() => handle.IsActive(), timing, cancellationToken);
    }
    
}
