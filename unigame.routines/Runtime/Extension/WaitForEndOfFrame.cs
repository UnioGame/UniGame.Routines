namespace UniModules.UniRoutines.unigame.routines.Assets.UniGame.Routines.Runtime.Extension
{
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;
    using UnityEngine;

    public class WaitForEndOfFrame : CustomYieldInstruction, IPoolable
    {
        private int _awaitEndFrame;

        public override bool keepWaiting => Time.frameCount < _awaitEndFrame;

        public WaitForEndOfFrame Initialize()
        {
            _awaitEndFrame = Time.frameCount + 1;
            return this;
        }

        public void Release()
        {
            _awaitEndFrame = 0;
        }
    }
}