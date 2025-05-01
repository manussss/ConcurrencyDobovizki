# Exceptions and asynchronous code

marking a method as async does not
make it asynchronous; it’s just a flag for the compiler to enable all the processing
required for supporting await. If you don’t use await, the only thing the compiler
does is wrap the return value in a Task object.

---

# await and AggregateException

Task uses AggreggateException because a Task can represent the result of multiple
operations running in parallel (for example, multiple asynchronous operations passed
to Task.WhenAll). Because more than one of those background operations can fail, we
need a way to store multiple exceptions.
In practice, this feature is almost never used. In fact, this feature is so rarely used that
if you use await, and the Task you are awaiting fails, the await operator will always throw
the first exception inside the AggregateException and not the AggregateException
itself. 

``` csharp
public async Task MethodThatThrowsException()
{
    await Task.Delay(100);
    throw new NotImplementedException();
}

public async Task MethodThatCallsOtherMethod()
{
    MethodThatTHrowsException();
}
```

Here we have two methods. The first method, MethodThatThrowsException, throws
an exception after an await, so the compiler will catch the exception and stash it in
the returned Task. The second method calls the first, but when I wrote it, I forgot the
await, so no one is looking at the retuned Task. The exception was caught in the first
method by the compiler-generated code but ignored by the second method because I
didn’t use await. And so the runtime thinks we handled the exception (because the
compiler generated code caught it), and the code continues to run while ignoring the
error.

If the method that throws the exception is in a library, and you have the “just my
code” feature of the debugger enabled, you won’t even see the exception in the debug-
ger. So if some code in your program seems to stop running with no indication of why,
there’s a good chance someone forgot an await somewhere.

