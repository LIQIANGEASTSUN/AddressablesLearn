using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IenumeratorTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Func());
    }

    private IEnumerator Func()
    {
        yield return Func1();
        Debug.LogError("11111");
        yield return Func2();
        Debug.LogError("22222");

        yield return Func3();
        Debug.LogError("33333");

        yield return Func4();
        Debug.LogError("44444");

    }

    private IEnumerator Func1()
    {
        Debug.LogError("Func1 Start");
        yield return new WaitForSeconds(1);
        Debug.LogError("Func1 End");
    }

    private IEnumerator Func2()
    {
        Debug.LogError("Func2 Start");
        yield return new WaitForSeconds(2);
        Debug.LogError("Func2 End");
        yield break;
    }

    private IEnumerator Func3()
    {
        Debug.LogError("Func3 Start");
        yield return new WaitForSeconds(3);
        Debug.LogError("Func3 End");
    }

    private IEnumerator Func4()
    {
        Debug.LogError("Func4 Start");
        yield return new WaitForSeconds(4);
        Debug.LogError("Func4 End");
    }

}
