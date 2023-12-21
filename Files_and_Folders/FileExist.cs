using System.Activities;
using System.IO;

namespace Files_and_Folders
{
    public sealed class FileExist : CodeActivity<bool>
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> FullFileName { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override bool Execute(CodeActivityContext context)
        {
            if (File.Exists(context.GetValue(this.FullFileName))){
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
