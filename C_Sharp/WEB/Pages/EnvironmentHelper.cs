using System;

public static class EnvironmentHelper
{
    public static string GetEnvironmentVariable(string name)
    {
        return name + "; " + System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
    }
}
