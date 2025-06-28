using System;

public static class FuncExt
{
    public static T RunExt<T>(this Func<T> action)
    {
        if (action == null)
        {
            return default;
        }

        return action();
    }

    public static TRet RunExt<T1, TRet>(this Func<T1, TRet> action, T1 arg1)
    {
        if (action == null)
        {
            return default;
        }

        return action(arg1);
    }

    public static TRet RunExt<T1, T2, TRet>(this Func<T1, T2, TRet> action, T1 arg1, T2 arg2)
    {
        if (action == null)
        {
            return default;
        }

        return action(arg1, arg2);
    }

    public static TRet RunExt<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action == null)
        {
            return default;
        }

        return action(arg1, arg2, arg3);
    }
}