using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomManager : IEngineComponent
{
    #region Interface
    public static RandomManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RandomManager();
            }

            return _instance;
        }
    }
    private static RandomManager _instance;

    public IEngineComponent Init()
    {
        _random = new System.Random();
        return this;
    }
    #endregion

    private System.Random _random;
    private int _seed;

    private void UpdateSeed()
    {
        _seed = _random.Next();
        _random = new System.Random(_seed);
    }

    public int GetCurrentSeed()
    {
        return _seed;
    }

    public void SetCurrentSeed(int seed)
    {
        _seed = seed;
        _random = new System.Random(_seed);
    }

    public int RandomRange(int min, int max)
    {
        int result = _random.Next(min, max);
        UpdateSeed();
        return result;
    }

    public long RandomRange(long min, long max)
    {
        long result = (long)(_random.NextDouble() * (max - min) + min);
        UpdateSeed();
        return result;
    }

    public float RandomRange(float min, float max)
    {
        float result = (float)(_random.NextDouble() * (max - min) + min);
        UpdateSeed();
        return result;
    }

    public double RandomRange(double min, double max)
    {
        double result = _random.NextDouble() * (max - min) + min;
        UpdateSeed();
        return result;
    }

    public T RandomInCollection<T>(ICollection<T> collection)
    {
        if (collection == null || collection.Count == 0)
        {           
            return default;
        }

        var index = _random.Next(0, collection.Count);
        IEnumerator<T> enumerator = collection.GetEnumerator();
        enumerator.MoveNext();

        while (index > 0)
        {
            enumerator.MoveNext();
            index--;
        }

        T result = enumerator.Current;
        UpdateSeed();
        return result;
    }

    public T[] RandomInCollection<T>(ICollection<T> collection, int amount, bool allowDuplicate)
    {
        if (collection == null || collection.Count == 0)
        {            
            return default;
        }

        if (amount == 1)
        {
            return new T[] { RandomInCollection(collection) };
        }

        if (allowDuplicate)
        {
            return _randomAmountWithDuplicate(collection, amount);
        }
        else
        {
            return _randomAmountWithoutDuplicate(collection, amount);
        }
    }

    private T[] _randomAmountWithDuplicate<T>(ICollection<T> collection, int amount)
    {
        var result = new T[amount];

        for (int i = 0; i < amount; i++)
        {
            result[i] = RandomInCollection(collection);
        }

        return result;
    }

    private T[] _randomAmountWithoutDuplicate<T>(ICollection<T> collection, int amount)
    {
        if (amount > collection.Count)
        {
            return default;
        }

        var tempList = new List<T>(collection);

        for (int i = tempList.Count - 1; i > 0; --i)
        {
            int j = RandomRange(0, tempList.Count);

            T temp = tempList[i];
            tempList[i] = tempList[j];
            tempList[j] = temp;
        }

        return tempList.GetRange(0, amount).ToArray();
    }
}