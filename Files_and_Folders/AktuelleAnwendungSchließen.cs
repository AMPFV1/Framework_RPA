using System.Activities;
using System.Diagnostics;

namespace Files_and_Folders
{
    public sealed class AktuelleAnwendungSchließen : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            var prozess = Process.GetProcessesByName("ACLWin")[0];
            if(prozess != null)
            {
                Process.GetProcessesByName("ACLWin")[0].Kill();
            }
        }
    }
}
