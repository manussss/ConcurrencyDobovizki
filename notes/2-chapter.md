# Yield return

What is yield return? It basically lets you write functions that generate a sequence
of values you can use in foreach loops directly without using a collection such as a list
or an array. Each value can be used without waiting for the entire sequence to be gener-
ated.

``` csharp
private IEnumerable<int> NoYieldDemo()
{
    var result = new List<int>();
    result.Add(1);
    result.Add(2);

    return result;
}

public void UseNoYieldDemo()
{
    foreach(var current in NoYieldDemo())
    {
        Console.WriteLine($"Got {current}");
    }
}
```

``` csharp
private IEnumerable<int> YieldDemo()
{
    yield return 1;
    yield return 2;
}

public void UseYieldDemo()
{
    foreach(var current in YieldDemo())
    {
        Console.WriteLine($"Got {current}");
    }
}
```

The code looks very similar, and the results are the same. So what is the big difference? In
the first example, all the values were generated first and then used, while in the second
example, each value was generated just when it was needed

---

