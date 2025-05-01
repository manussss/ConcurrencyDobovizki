# Canceling background tasks

The .NET library provides a
standard mechanism for signaling that a background operation should end, which
is called CancellationToken. The CancellationToken class is used consistently for
(almost) all cancelable operations in the .NET library itself and in most third-party
libraries.

It’s important to remember that at its core, CancellationToken is just a bool variable
(wrapped in a thread-safe, future-proof, abuse-resistant class); there’s nothing magic
about it, and it doesn’t know by itself how to cancel anything. If our previous program
did something time consuming in the loop instead of Console.WriteLine (for exam-
ple, a calculation that takes 1 full minute), the thread cancellation will be delayed until
that long calculation completes.

The CancellationToken.Register method is used to register the callback we want the
CancellationToken to call when it is canceled.

---

