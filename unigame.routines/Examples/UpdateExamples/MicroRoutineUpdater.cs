using System.Collections;
using UnityEngine;

namespace UniModules.UniRoutine.Examples.UpdateExamples
{
    using System;
    using System.Collections.Generic;
    using UniRx;

    public class MicroRoutineUpdater : MonoBehaviour
    {
        public int updateCount;
        public int routineCount;
        public List<IDisposable> Disposables = new List<IDisposable>();
        // Start is called before the first frame update
        private void Start()
        {
            for (var i = 0; i < routineCount; i++) {
                MainThreadDispatcher.StartUpdateMicroCoroutine(OnUpdate(i));
            }
        }

        private void OnDisable()
        {
            Disposables.ForEach(x => x.Dispose());
            Disposables.Clear();
        }

        private IEnumerator OnUpdate(int number)
        {
            var counter = 0;
            while (true) {
                counter++;
                if (counter > updateCount) {
                    Debug.Log($"UniRx MicroRoutine: COUNTER #{number}  FINISHED");
                    yield break;
                }

                yield return null;
            }
        }
    }
}
