# Multithreading

Multithreading is about doing stuff that
may or may not require the CPU in the background while using the CPU to do
something else. 

---

# Task

Task.Run, runs our code in a thread pool thread

--

# async / await

await tries to run the code after it in a thread of the same type, so in most cases, you
don’t have to think about the possibility of a thread change. If await can’t use a thread
of the same type, it will use the thread pool instead. The specific rules are
- If you are using await on the UI thread of a WinForms, WPF, or UWP app, the
code after the await will run on the same thread.
- If you are using await while processing a request in an ASP.NET Classic applica-
tion (.NET framework 4.8 and earlier), the code after the await will run on the
same thread.
- If your code or a framework you are using changes the current thread’s
Synchronization­ Context or TaskFactory (we’ll talk about them later in the
book), then await will use those. This is how the frameworks in the previous
bullet points control the behavior of await; except for UI frameworks, this is
extremely rare.
- In all other cases, the code after await will run on the thread pool, examples:
    - If the code calling await is running in the thread pool, the code after the await will also run in the thread pool. However, it might run on a different thread.
    - This also applies to code processing a request in an ASP.NET Core application
(.NET Core or .NET 5.0 and later) because ASP.NET Core uses the thread pool for all processing.
    – If you use await in the main thread of a non-UI app, the code after the await will also run in the thread pool and not in the main thread. The system will
keep the main thread (and the entire application) alive until the Task you are
awaiting completes.
    – If you use await in a thread you created with the Thread class, inside the
method you passed to the Thread constructor, the thread will terminate, and
the code after the await will run on the thread pool.

If the operation you are awaiting has
already completed by the time await runs, in almost all cases, the code will just continue
normally without switching threads. The most common situation in which this happens
is if you are awaiting a method that doesn’t do anything asynchronous.

```csharp
// This method is not thread safe, keep reading for the correct version
private Dictionary<string,string> _cache = new();

public async Task<string> GetResult(string query)
{
    if(_cache.TryGetValue(query, out var cacheResult)
        return cacheResult;

    var http = new HttpClient();
    var result = await http.GetStringAsync(
    "https://example.com?"+query);
    _cache[query] = result;
    return result;
}
``` 

This method first checks whether the query string is in the cache; if the value is already
there, it returns it without doing asynchronous work. If not, the code performs an
asynchronous HTTP call to get the result from the server. After the code gets the result
from the server, it stores it in the cache.
The first time you call this method for a given query, it will return a Task that needs
awaiting, but on subsequent calls for the same query, it will return a Task that has already
completed because no asynchronous operation has happened. 
