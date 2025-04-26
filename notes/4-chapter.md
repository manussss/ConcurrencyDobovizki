# Threads

Chapter 1 discussed how a system can run multiple pieces of code simultaneously—
much more than the number of CPU cores—by quickly switching between them.
This functionality is made possible by a hardware timer inside the CPU. Each time
the timer ticks, the operating system can pause the currently running code and
switch to another piece of code. If the switching is fast enough, it creates the illusion
that all threads are running simultaneously

The Thread class also contains a method that will wait until the thread completes its
work, called Join. Join is the standard computer science term for waiting for a thread.
I’ve found conflicting stories about the origin of this term, all of them using metaphors
that I don’t want to repeat here because they don’t work that well. We’ll just have to
accept that in this context, join means wait.

Creating and destroying threads is relatively resource intensive, and if you create
a lot of threads where each thread does just a little bit of work, your app might spend
more time managing threads than doing actual useful work.

Sometimes, one thread must wait for another thread to do something. If we are waiting
for a thread we created to complete its work and terminate, we can use Thread.Join
(as discussed earlier in this chapter). But if the thread we are waiting for needs to con-
tinue running after notifying us, we can’t use Thread.Join, and we need some mech-
anism for one thread to send a signal to another thread and for that other thread to
wait until such a signal arrives. This mechanism is called ManualResetEventSlim and is
a newer, faster, and simpler implementation of ManualResetEvent. 

---

## Creating a thread

``` csharp
public void RunInBackground()
{
    var newThread = new Thread(CodeToRunInBackgroundThread);
    newThread.IsBackground = true;
    newThread.Start();
}
private void CodeToRunInBackgroundThread()
{
    Console.WriteLine("Do stuff");
}
```

---

## When to use Thread.Start

Use Thread.Start for
 Long-running code.
 If you need to change the thread properties such as language and locale infor-
mation, background status, COM apartment, etc. (We’ll talk about all the thread
settings near the end of this chapter.)
Do not use Thread.Start for
 Asynchronous code
 Short tasks

---

# The thread pool

The thread pool is the solution for the thread creation and destruction overhead we
talked about. With the thread pool, the system keeps a small number of threads waiting
in the background, and whenever you have something to run, you can use one of those
pre-existing threads to run it. The system automatically manages those threads and cre-
ates new ones when needed (between a minimum and maximum number of threads
you control).

``` csharp
public void RunInBackground()
{
    ThreadPool.QueueUserWorkItem(RunInPool);
}

private void RunInPool(object? state)
{
    Console.WriteLine("Do stuff");
}
```

---

## When to use ThreadPool.QueueUesrWorkItem
Use ThreadPool.QueueUesrWorkItem for
 Short-running tasks
Do not use ThreadPool.QueueUesrWorkItem
 For long-running tasks
 When you need to change the thread properties
 With Task-based asynchronous operations
 With async/await

---

# Task.Run

The Task.Run method runs code on the thread pool, just like ThreadPool.QueueUserWorkItem,
but it has a nicer interface that works well with async/await.

``` csharp
public void RunInBackground()
{
    Task.Run(RunInPool);
}

private void RunInPool()
{
    Console.WriteLine("Do stuff");
}
```

Note that when you use Task.Run without waiting for it, the compiler will generate
a warning, but adding an await is almost never the right thing to do. If you await Task
.Run, you are telling your compiler to wait for the task to complete before moving to the
next line of code, essentially making it run sequentially, which defeats the purpose of
using Task.Run. If you await Task.Run, you’re taking on the overhead of managing dif-
ferent tasks without getting any benefits; it’s more efficient to just run the code without
Task.Run. The exception to this rule is the UI thread.

``` csharp
Task.Run(MethodToRunInBackground); // Might generate a warning
_ = Task.Run(MethodToRunInBackground); //No warning
```

---

## When to use Task.Run
Use Task.Run for
 Code that uses async-await
 Short running tasks
Do not use Task.Run for
 Non-asynchronous long running tasks

---

# Locks and mutexes

What we are left with is synchronizing access to the shared state—whenever a thread
needs to access the shared state, it “locks” it, and when it is finished with the data, it
“releases” the lock. If another thread tries to lock the data while it is already locked, it
must wait until the data is released by the current user.
In computer science, this is called a mutex (short for mutual exclusion). In C#, we have
the Mutex class that represents the operating system’s mutex implementation and the
lock statement that uses an internal .NET implementation. The lock statement is eas-
ier to use and faster (because it doesn’t require a system call), so we will prefer to use it.

---

## Why use lock with an object

In .NET 8 and earlier, the best practice is to use an object of type Object (that can also
be used with the keyword object) because we’re not going to use this object for anything
else, and an object of type Object has the lowest overhead of all reference type objects.
In .NET 9 and later, it’s better to use an object of type System.Threading.Lock. Using
a lock statement with the new Lock class is clearer (because it’s obviously a lock) and
may be faster in newer versions.
Using the lock statement with an Object is still supported, safe, and correct in .NET 9
and later. In this book, all the examples will use an Object and not a Lock for backward
compatibility.

---

# Deadlocks

A deadlock is the situation where a thread or multiple threads are stuck waiting for
something that will never happen.

---

