# Socket communication

We used socket communication in the load testing client and server. Because this isn’t a
book about networking, I won’t go into details, but here’s a very short explanation of the
networking calls we used.

On the server, we first must use Bind to take control of a network port, and then we call
Listen to signal we are waiting for connections from clients. Accept will actually wait
for the next connection. When a client connects to the server, Accept will return a new
socket representing the new connection. AcceptAsync is the asynchronous version of
Accept that, instead of waiting, returns a Task<Socket> that will complete when a cli-
ent connects.

On the client, we then call ConnectAsync to connect to the server. We use IPAddress
.Loopback as the server address, that is, a special address that always contacts the cur-
rent computer. It is better known as localhost in most networking tools.
Send sends data, and it returns after the data is handed over to the network stack inside
the sending computer (not after the data is sent and not after the data is received by the
other side; you can’t know when those happen). Send returns the number of bytes that
were actually accepted by the network stack on a modern computer, which will almost
always be the entire buffer you are trying to send. SendAsync is the asynchronous ver-
sion. It returns immediately and returns a Task<int> that will complete when Send
would have returned.

ReceiveAsync reads data into an array we give it and returns a Task<int> with the
number of bytes received. If that Task’s result is 0, it means no more data is available,
and we assume the server closed the connection.
And finally, Shutdown shuts down the connection gracefully, including sending a mes-
sage to the other side notifying it that the connection is now closed. It also clears all the
resources held by the connection.

---

# async / await

By making your code asynchronous, you add new edge cases and failure modes that just don’t exist in nonasynchronous single-threaded code. 

```csharp
private void button1_Click(object sender, EventArgs args)
{
    var source = GetRandomSource();
    label1.Text = source.Name;
    label2.Text = source.GetValue();
}
```

This code is pretty straightforward, and except for failures in the source itself, there’s
basically nothing that can go wrong. But if GetValue takes a long time to run, it will
make the UI unresponsive. We can solve this problem by making GetValue and this
method async. The changes to this method are minimal, and our UI will no longer
become unresponsive:

```csharp
private async void button1_Click(object sender, EventArgs args)
{
    var source = GetRandomSource();
    label1.Text = source.Name;
    label2.Text = await source.GetValue();
}
```

This may look like an easy fix, but we introduced a bug. Now that the UI is not frozen
while GetValue is running, the user can click the button again, and if we are unlucky
with timing, it’s easy to encounter a situation where the code displays that the source
value is from the first click, while the source name is from the second, showing the user
incorrect information. 

---

## When to use async/await

- If your code needs to manage a large number of connections simultaneously, use
async/await whenever you can.
- If you are using an async-only API, use async/await whenever you use that API.
- If you are writing a native UI application, and you have a problem with the UI
freezing, use async/await in the specific long-running code that makes the UI
freeze.
- If your code creates a thread per request/item, and a significant part of the run
time is I/O (for example, network or file access), consider using async/await.
- If you add code to a codebase that already uses async/await extensively, use
async/await where it makes sense to you.
- If you add code to a codebase that does not use async/await, avoid async/await
in the code as much as possible. If you decide to use async/await in the new
code, consider refactoring at least the code that calls the new code to also use
async/await.
- If you write code that only does one thing simultaneously, don’t use async/await.
- And in all other cases, absolutely and without a doubt, consider the trade-offs
and make your own judgement.