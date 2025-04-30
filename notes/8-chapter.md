# Processing a sequence of items in the background

There are three primary reasons for using multithreading in everyday applications.
The first, and the most common, is when an application server needs to manage
requests from multiple clients simultaneously. Typically, this is handled by the
framework we use, such as ASP.NET, and it is beyond our control. The other two rea-
sons for using multithreading are to finish processing sooner by performing parts
of the processing in parallel and to push some tasks to be run later in the back-
ground. 

---

# Performance tests

Note that if you run the program under a debugger, you will get significantly worst
results because the debugger monitors thread creation and destruction. When I ran the
program under a debugger, it took 14 seconds instead of just 1.2—be careful with your
performance tests!

---

# Processing items in parallel with the thread pool

Creating an arbitrarily large number of threads inside our server process seems bad.
Fortunately for us, the thread pool was designed exactly for this situation. 

---

# The Parallel class

In all the samples so far, we wrote a loop that created threads or added items to the
thread pool. We then collected the Thread or Task objects so we could wait until they
were all completed. This is tedious and exactly the kind of boilerplate code we don’t like
to write. Luckily, the .NET library has the Parallel class that can do all of this for us.

The Parallel class has four static methods:
- Invoke—Takes an array of delegates and executes all of them, potentially in par-
allel. This method returns after all the delegates finish running.
- For—Acts like a for loop, but iterations happen in parallel. It will return after all
the iterations finish running.
- ForEach—Acts like a foreach loop, but iterations happen in parallel. It will
return after all the iterations finish running.
- ForEachAsync—Similar to ForEach, but the loop body is an async method. It will
return immediately and return a Task that will complete when all the iterations
finish running.

---

# The work queue pattern and BlockingCollection

If we no longer care about the time it takes to send the messages, it’s better to just
use one thread, or a small, fixed number of threads, that will send everything. This
is called the work queue pattern and is implemented by creating a queue where every
thread can add items to the queue, and there is a dedicated set of threads that pro-
cesses all the items. 

BlockingCollection<T> can be used in multiple ways. For example, it can be
used as a thread-safe List<T>. But the interesting scenario is when we use BlockingCollection<T>
as a work queue.

---

