# Async/Await

async/await is a feature that lets us write asynchronous code as if it were normal
synchronous code. With asynchronous programming, when we perform an opera-
tion that would normally make the CPU wait (usually for data to arrive from some
device—for example, reading a file), instead of waiting, we just do something else.

The await
keyword tells the compiler that the code needs to be suspended at this point and
resumed when whatever async operation you are waiting for completes.

---

# async void


because there is no Task, the caller of the method has no way of knowing when the method
finished running (all the ways we talked about—await, Wait, IsCompleted, and even
ContinueWith—require a Task object).

There is also no way to report the exception to the caller (like in the success case,
there’s no access to the Task.Exception property or any other way to get to the excep-
tion because there is no Task), but unlike the success case, this is a real problem

So if this feature is so problematic, why do we have async void methods to begin
with? The reason for async void is event handlers. By convention, just like in our exam-
ple, event handlers always have a void return type, so if async methods didn’t support
void, we couldn’t use async/await in event handlers.

---

# Task

Task does multiple things: it represents
an ongoing asynchronous operation, lets us schedule code to run when an asynchro-
nous operation ends (we’ll talk about these two in this chapter), and lets us create and
compose asynchronous operations.

A Task represents an
event that may happen in the future, while Task<T> represents a value that may be avail-
able in the future. Those events and values may or may not be the results of something
we will describe using the English word task. In computer science, those concepts are
often called future, promise, or deferred value, but in this book, we’ll refer to them using
the .NET/C# term Task.

## Are we there yet?

In the “Are we there yet” model, you are responsible for asking the Task whether
it has completed yet, usually in a loop that does other things between those checks
(this is called polling), which is done by reading the IsCompleted property. Note that
IsCompleted is true even if the task has errored out or was canceled.

Task also has a Status property we can use. The task has completed if Status is Ran-
ToCompletion, Canceled, or Faulted. Using the IsCompleted property is better than
using the Status property because checking one condition as opposed to three is more
concise and less error-prone (we will talk about canceled and faulted tasks later in this
book).

You should not check IsCompleted or Status in a loop unless you are doing other
work between the checks. 

## Wake me up when we get there

In the “Wake me up when we get there” model, you pass a callback method to the task,
and it will call you when it’s complete (or errored out or canceled). This is done by
passing the callback to the ContinueWith method.

## sync option

The Wait method and Result property will block
the current thread until the task is complete and will throw an exception if the task
is canceled or errored out, making it behave like synchronous code. Note that using
the Wait method or the Result property to wait for a task to complete is inefficient
and negates the advantages of using asynchronous programming in the first place.

---

# ValueTask

```csharp

public async Task<int> GetValue(string request)
{
    if (_cache.TryGetValue(request, out var fromCache))
    {
        return fromCache;
    }
    
    int newValue = await GetValueFromServer(request);
        
    return newValue;
}

```

The GetValue method first checks whether the requested value is in the cache. If so,
it will return the value before the first time it uses await. As we’ve seen in this chapter,
the code before the first await runs non-asynchronously, so if the value is in the cache,
it will be returned immediately, making the Task<int> returned by the method just a
very complicated wrapper for an int.

Allocating the entire Task<int> object when it’s not required is obviously wasteful,
and it would have been better if we could return an int if the value could be returned
immediately and only return the full Task when we need to perform an asynchronous
operation. This is what ValueTask<T> is. ValueTask<T> is a struct that contains the
value directly if the value is available immediately and a reference to a Task<T> other-
wise. The nongeneric ValueTask is the same, except it only contains a flag saying the
operation has completed and not the value.

ValueTask and ValueTask<T> are slightly less efficient than Task and Task<T> if
there is an asynchronous operation, but much more efficient if the result was available
immediately. It is recommended to return a ValueTask in methods that usually return
a value without performing an asynchronous operation, especially if those methods are
called often.