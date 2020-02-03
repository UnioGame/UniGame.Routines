namespace UniGreenModules.UniRoutine.Runtime {
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    public class UniRoutineRootObject : MonoBehaviour, IDisposable
    {
        [SerializeField]
        private List<IUniRoutine> routines = new List<IUniRoutine>();

        public void Dispose()
        {
            routines.Clear();
        }
        
        public void AddLateRoutine(IUniRoutine routine)
        {
            routines.Add(routine);
        }
        
        private void LateUpdate()
        {
            for (var i = 0; i < routines.Count; i++) {
                var routine = routines[i];
                routine.Update();
            }
        }
    }

}
