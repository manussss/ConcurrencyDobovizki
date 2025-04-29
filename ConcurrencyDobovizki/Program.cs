using System.Diagnostics;

var action1 = new Action(Processo1);
var action2 = new Action(Processo2);
var action3 = new Action(Processo3);

var stopWatch = new Stopwatch();

stopWatch.Start();

//reduced from 3017ms to 1014ms
Parallel.Invoke(action1, action2, action3);

stopWatch.Stop();

Console.WriteLine($"Tempo de processamento: {stopWatch.ElapsedMilliseconds}ms");

void Processo1()
{
    Console.WriteLine($"Processo 1 finalizado. Thread: {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
}

void Processo2()
{
    Console.WriteLine($"Processo 2 finalizado. Thread: {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
}

void Processo3()
{
    Console.WriteLine($"Processo 3 finalizado. Thread: {Thread.CurrentThread.ManagedThreadId}");
    Thread.Sleep(1000);
}
