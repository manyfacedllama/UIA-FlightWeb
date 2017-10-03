using System.Collections.Generic;

public interface ISettingsStore
{
    string GetVersion();

    Dictionary<string, string> FindAll();
}
