using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour {

    // We declare a new data structure called MyDelegate.
    // It's a delegate type.
    // This new type which will be pointing to a function.
    // That function takes a string argument.
    private delegate void MyDelegate(string theString);

    // We delcare a new variable of the type we just declared.
    MyDelegate theDelegate;

    void Start()
    {
        // We assign a method name to the delegate variable.
        theDelegate = Method1;

        // We call a method, passing it the delegate variable.
        MethodWithCallBack("Try the ", theDelegate);

        // We change the method assigned to the delegate variable.
        theDelegate = Method2;

        // We run the same method again.
        MethodWithCallBack("Once more, try the ", theDelegate);
    }

    void MethodWithCallBack(string text, MyDelegate d)
    {
        // We get the string from the original calling method in Start.
        Debug.Log(text + "callback method.");

        // We also get the delegate variable.
        // That variable holds a method name with the MyDelegate signature,
        // so we send it another string.
        d("This string can be sent to any method, ");
    }

    void Method1(string a)
    {
        // We get the string from the callback.
        Debug.Log(a + "and only Method1 knows about it.");
    }

    void Method2(string a)
    {
        // We get the string from the callback here as well.
        Debug.Log(a + "and it's the same idea in Method2.");
    }

    // Prints:
    // Try the callback method.
    // This string can be sent to any method, and only Method1 knows about it.
    // Once more, try the callback method.
    // This string can be sent to any method, and it's the same idea in Method2.
}
