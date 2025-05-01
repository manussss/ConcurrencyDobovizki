# await-threading behavior

Basically, the rules for where the code after await runs are
- In UI apps (WinForms and WPF), if you are using await in a UI thread and don’t
use ConfigureAwait (we will talk about it later in this chapter), the code after the
await will run in the same thread.
- In ASP.NET classic (not ASP.NET Core), if you are using await in a thread that is
processing a web request, and you don’t use ConfigureAwait, the code after the
await will run in the same thread.
- In all other cases, the code after the await will run in the thread pool.

---

# Breaking away—ConfigureAwait(false)

- If the task has already completed, the code continues to run in the same thread.
ConfigureAwait(false) has no effect in this case.
- If there is a SynchronizationContext set for the current thread, and ConfigureAwait(false)
is not used, the code after the await will use the SynchronizationContext
to run.
- In all other cases, the code will run using the thread pool.
- you should always use ConfigureAwait(false) in library code and not application code.

I’ll mention how
we can solve our deadlock without using ConfigureAwait. This deadlock, in its gen-
eral form—UI waiting for something that is waiting for UI—has existed ever since we
started making UI applications; it predates async/await, it predates .NET and C#, it
even predates asynchronous IO in the Windows operating system. And so, unsurpris-
ingly, there is a standard solution for this problem already built into WinForms (and
WPF and all other UI frameworks I know about). This solution is to let the UI han-
dle events (or pump messages, in Windows API terminology) while we are waiting for
the background task to complete. 