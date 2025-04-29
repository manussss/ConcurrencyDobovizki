using System.Diagnostics;

var stopWatch = new Stopwatch();

stopWatch.Start();
Processo1();
Processo2();
Processo3();
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
