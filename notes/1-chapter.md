# DeadLock

A classic deadlock: a very common multithreading problem where one thread is holding
resource A while waiting for B and a second thread is holding B while waiting for A,
resulting in both threads waiting forever.

---

# Multithreading and asynchronous

Multithreading and asynchronous programming are two techniques commonly
used for this task.

Multithreading allows a computer to appear as if it is executing several tasks at
once, even when the number of tasks exceeds the number of CPU cores. In con-
trast, asynchronous programming focuses on optimizing CPU usage during oper-
ations that would typically make it wait, which ensures the CPU remains active and
productive.

---

# Thread

Each thread represents a sequence of operations
that can happen in parallel with other similar or different sequences.

Inside the CPU, there’s a timer that signals when the CPU should switch to
the next thread, and with every switch, the CPU needs to store whatever it was doing
and load the other thread’s status (this is called a **context switch**).

The operations that can cause the thread to become blocked are called blocking oper-
ations. All file and network access operations are blocking, as is anything else that com-
municates with anything outside the CPU and memory; moreover, all operations that
wait for another thread can block.

In our software, every thread, even if not running,
consumes some memory, so while it’s possible to create a large number of threads, each
of them executing a blocking operation (so they are blocked most of the time and not
consuming CPU time), this is wasteful of memory. It will slow the program down as we
increase the number of threads because we must manage all the threads. At some point,
we will either spend so much time managing threads that no useful work will get done
or we will just run out of memory and crash.

---

