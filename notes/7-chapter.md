# Multithreading

When transitioning from single-thread to multithreaded programming, it’s import-
ant to recognize that multithreading introduces certain types of bugs that don’t
occur in single-threaded applications.

---

# Partial updates

Partial updates happen when one thread updates some data, and then, in the middle
of that update, another thread reads the data and sees a mix of the old and new values.

```csharp
private int _x;
private int _y;
private object _lock = new object();

public void SetXY(int newX, int newY)
{
    lock(_lock) //lock statement around writes
    {
        _x = newX;
        _y = newY;
    }
}
public (int x, int y) GetXY()
{
    lock(_lock) //lock statement around reads
    {
        return (_x,_y);
    }
}
```

---

# Deadlocks

A deadlock is a situation where a thread is stuck
waiting for a resource that will never become available because of something that the
same thread did. In the classic deadlock, one thread is holding resource A while wait-
ing for resource B at the same time that another thread is holding resource B while
waiting for resource A. At this point, both threads are stuck, each waiting for the other
one to complete. And that will never happen because the other one is also stuck

never call code that is not under your control while
holding a lock. When you need to call any code that is not under your control, you must
call it after releasing the locks.

---

# Race conditions

A race condition is a situation where the result of the code is dependent on uncontrolla-
ble timing events.

---

# Synchronization

Synchronization is the situation when operations happen sequentially and not in paral-
lel.

```csharp
public void GetCorrectValue()
{
    int theValue = 0;
    object theLock = new Object();
    var threads = new Thread[2];

    for(int i=0;i<2;++i)
    {
        threads[i] = new Thread(()=>
        {
            for(int j=0;j<5000000;++j)
            {
                lock(theLock)
                {
                    ++theValue;
                }
            }
        });
    threads[i].Start();
    }

    foreach(var current in threads)
    {
        current.Join();
    }

    Console.WriteLine(theValue);
}
```

This code creates two threads, and each increments a value five million times. To avoid
the partial updates problem we talked about at the beginning of this chapter, the code
uses a lock while incrementing the value. But there is still a problem with this code. We
use two threads so we can increase performance, but because of the locks around the
actual incrementing operations, the incrementing itself happens sequentially and not
in parallel.

to avoid synchronization, we need to hold locks for the
shortest time possible, preferably just for the time it takes to access the shared resource,
and not for the duration of the entire operation.

If our locks are too short, we risk race conditions, and if our locks are too long, we get synchronization.

There are situations where reducing the lock’s
duration to anything that doesn’t cause synchronization will cause race conditions. In
those cases, you should remember that synchronization may hurt performance, but
race conditions will produce incorrect and unexpected results. So prefer synchroniza-
tion to race conditions.

---

# Starvation

Starvation is the situation when one thread or a group of threads monopolizes access
to some resource, never letting another thread or group of threads do any work. For
example, the following program will create two threads, with each thread acquiring
a lock and writing a character in the console (the first thread is a minus sign and the
second thread is an x) in an infinite loop.