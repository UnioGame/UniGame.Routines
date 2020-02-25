# Unity Routine System

Unity Routine System package

## How To Install

### Unity Package Installation

Add to your project manifiest by path [%UnityProject%]/Packages/manifiest.json these lines:

```json
{
  "scopedRegistries": [
    {
      "name": "Unity",
      "url": "https://packages.unity.com",
      "scopes": [
        "com.unity"
      ]
    },
    {
      "name": "UniGame",
      "url": "http://packages.unigame.pro:4873/",
      "scopes": [
        "com.unigame"
      ]
    }
  ],
}
```
Open window Package Manager in Unity and install UniGame Routine System Package

## How to Use

For use routines just implement your method with IEnumerator result as your alway do wiith regular Unity coroutins

### Run routine

```csharp
public class RoutineChainTest : MonoBehaviour
{
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
    
    //just skip 1 routine update
    private IEnumerator LongOp2()
    {
        yield return null;
    }
    
    //wait 0.1 seconds before result
    private IEnumerator LongOp3()
    {
        yield return this.WaitForSecond(0.1f);
    }
}

```


### Stop Routine

## Routine Extensions
