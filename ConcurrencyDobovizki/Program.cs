var myEvent = new ManualResetEventSlim(false);
var threadWeWaitFor = new Thread(()=>
{
Console.WriteLine("Doing something");
Thread.Sleep(5000);
Console.WriteLine("Finished");
myEvent.Set();
});
var waitingThread = new Thread(()=>
{
Console.WriteLine("Waiting for other thread to do something");
myEvent.Wait();
Console.WriteLine("Other thread finished, we can continue");
});
threadWeWaitFor.Start();
waitingThread.Start();

Console.ReadKey();