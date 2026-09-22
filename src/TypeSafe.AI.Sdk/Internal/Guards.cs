// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

internal static class Guards
{
    public static int NonNegativeInteger(string name, int value)
    {
        if (value < 0)
        {
            throw new TypeSafeException($"`{name}` must be a non-negative integer, got {value}.");
        }

        return value;
    }

    public static int PositiveMs(string name, int value)
    {
        if (value <= 0)
        {
            throw new TypeSafeException($"`{name}` must be a positive number of milliseconds, got {value}.");
        }

        return value;
    }

    public static int NonNegativeMs(string name, int value)
    {
        if (value < 0)
        {
            throw new TypeSafeException($"`{name}` must be a non-negative number of milliseconds, got {value}.");
        }

        return value;
    }

    public static double Fraction(string name, double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || value < 0 || value > 1)
        {
            throw new TypeSafeException($"`{name}` must be between 0 and 1, got {value}.");
        }

        return value;
    }

    public static ISet<int> StatusSet(string name, ISet<int> statuses)
    {
        foreach (var status in statuses)
        {
            if (status < 100 || status > 999)
            {
                throw new TypeSafeException($"`{name}` must contain HTTP status codes, got {status}.");
            }
        }

        return statuses;
    }
}
