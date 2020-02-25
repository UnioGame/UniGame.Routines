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

### Routine Execution

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
            counter++;
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

You can use two ways to execute Routine:

1. Execute routine with "ExecuteRoutine" to get IDisposable cancelation handler

```csharp
public static IDisposableItem ExecuteRoutine(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false)
```

2. Start routine with "Execute" to get RoutineHandler struct that allow get more info about routines status and control them

```csharp
public static RoutineHandler Execute(
            this IEnumerator enumerator, 
            RoutineType routineType = RoutineType.Update,
            bool moveNextImmediately = false)
```

### Routine Type

Routine update type can one from the following regular Unity life cycle

```csharp
  public enum RoutineType : byte
  {
      Update = 0,
      EndOfFrame = 1,
      FixedUpdate = 2,
      LateUpdate = 3,
  }
```

### Routine Cancelation

Any Routine can be canceled one extension method call with no depends on that Execution method you choose.

```csharp
var handler = Foo().Execute();
var disposable = Foo().ExecuteRoutine();

handler.Cancel();//can be called as many times as you wish
disposable.Cancel();//IMPORTANT! CALL Cancel for Disposable ONLY ONCE. It use ClassPool under the hood
```

## Routine Extensions

## Perfomance

UniRoutine vs UniRx.MicroCoroutines on 100K Updates methods

![](https://i.gyazo.com/4f97f199a23a429c81c532cebcb308f4.png)

![](https://i.gyazo.com/0982259e12e4e68d3283be8f85d0708c.png)
