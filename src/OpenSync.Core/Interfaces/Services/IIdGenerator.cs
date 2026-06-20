namespace OpenSync.Core.Interfaces.Services;

public interface IIdGenerator
{
    string NewId(string prefix);
}
