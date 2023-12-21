using System.Activities;
using System.IO;

namespace Files_and_Folders
{
    public sealed class FolderExist : CodeActivity<bool>
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> FullFolderName { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override bool Execute(CodeActivityContext context)
        {
            if (Directory.Exists(context.GetValue(this.FullFolderName)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
