using System;

public static class EnvironmentHelper
{
    public static string GetEnvironmentVariable(string name)
    {
        return System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}
