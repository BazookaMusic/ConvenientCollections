using BenchmarkDotNet.Attributes;

namespace ConvenientCollections.Benchmarks;

public class LazySortedListBenchmark
{
    private LazySortedList<int, int> lazy = new LazySortedList<int, int>();

    private SortedList<int, int> eager = new SortedList<int, int>();

    [Params(1000, 5000, 10000)]
    public int N { get; set; }

    [IterationSetup]
    public void Setup()
    {
        lazy.Clear();
        eager.Clear();

        for (int i = 0; i < N; i++)
        {
            lazy.Add(i, i);
            eager.Add(i, i);
        }
    }

    [Benchmark]
    public bool SystemSortedListGet()
    {
        bool res = true;
        for (int i = 0;i < N; i++)
        {
            res ^= this.eager.TryGetValue(i, out _);
        }

        return res;
    }

    [Benchmark]
    public bool LazySortedListGet()
    {
        bool res = true;
        for (int i = 0; i < N; i++)
        {
            res ^= this.lazy.TryGetValue(i, out _);
        }

        return res;
    }

    [Benchmark]
    public void SystemSortedListAdd()
    {
        for (int i = N; i < 2 * N; i++)
        {
            this.eager.Add(i, i);
        }
    }

    [Benchmark]
    public void LazySortedListAdd()
    {
        for (int i = N; i < 2 * N; i++)
        {
            this.lazy.Add(i, i);
        }
    }

    [Benchmark]
    public void SystemSortedListRemove()
    {
        for (int i = 0; i < N; i++)
        {
            this.eager.Remove(i);
        }
    }

    [Benchmark]
    public void LazySortedListRemove()
    {
        for (int i = 0; i < N; i++)
        {
            this.lazy.Remove(i);
        }
    }

    [Benchmark]
    public void SystemSortedListAddGetMix()
    {
        for (int i = 0; i < N; i++)
        {
            this.eager.Add(N + i, i);
            this.eager.TryGetValue(i, out _);
        }
    }

    [Benchmark]
    public void LazySortedListAddGetMix()
    {
        for (int i = 0; i < N; i++)
        {
            this.lazy.Add(N + i, i);
            this.lazy.TryGetValue(i, out _);
        }
    }
}
