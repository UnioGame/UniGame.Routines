using System.Collections;
using UniGreenModules.UniRoutine.Runtime;
using UniGreenModules.UniRoutine.Runtime.Extension;
using UnityEngine;

public class RoutineChainTest : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        LongOp1().Execute();
    }

    private IEnumerator LongOp1()
    {
        var counter = 0;
        while (counter < 100) {
            yield return LongOp2();
            yield return LongOp3();
        }
    }
    
    private IEnumerator LongOp2()
    {
        yield return null;
    }
    
    private IEnumerator LongOp3()
    {
        yield return this.WaitForSecond(0.1f);
    }
    
}
