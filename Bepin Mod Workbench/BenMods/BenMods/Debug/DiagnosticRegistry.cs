using System.Collections.Generic;
using System.Text;

namespace BenMods.Debugging
{
    public static class DiagnosticRegistry
    {
        private static readonly List<IDiagnosticModule> modules =
            new List<IDiagnosticModule>();

        public static void Register(IDiagnosticModule module)
        {
            if (module != null && !modules.Contains(module))
            {
                modules.Add(module);
            }
        }

        public static string GetDisplayText()
        {
            StringBuilder output = new StringBuilder();

            foreach (IDiagnosticModule module in modules)
            {
                output.Append("\n\n--- ");
                output.Append(module.Name);
                output.Append(" ---\n");
                output.Append(module.GetDisplayText());
            }

            return output.ToString();
        }
    }
}