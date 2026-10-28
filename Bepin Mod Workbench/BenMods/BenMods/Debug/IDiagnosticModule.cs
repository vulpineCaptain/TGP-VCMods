namespace BenMods.Debugging
{
    public interface IDiagnosticModule
    {
        string Name { get; }
        string GetDisplayText();
    }
}