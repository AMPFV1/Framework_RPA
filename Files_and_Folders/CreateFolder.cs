using System.Activities;
using System.IO;

namespace Files_and_Folders
{
    public sealed class CreateFolder : CodeActivity
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> FolderName { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override void Execute(CodeActivityContext context)
        {
            Directory.CreateDirectory(context.GetValue(this.FolderName));
        }
    }
}
