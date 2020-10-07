namespace UniModules.UniRoutines.unigame.routines.Assets.UniGame.Routines.Runtime.Extension
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UnityEngine;

    public class WaitForSecondRoutine : CustomYieldInstruction, IPoolable
    {
        private float awaitEndTime;
            
        public override bool keepWaiting => Time.time < awaitEndTime;

        public WaitForSecondRoutine Initialize(float seconds)
        {
            awaitEndTime = Time.time + seconds;
            return this;
        }

        public void Release()
        {
            awaitEndTime = 0;
        }
    }
}