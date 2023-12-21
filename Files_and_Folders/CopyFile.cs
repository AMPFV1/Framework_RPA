using System.Activities;
using System.IO;

namespace Files_and_Folders
{
    public sealed class CopyFile : CodeActivity
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> SourcePath { get; set; }
        public InArgument<string> TargetPath { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override void Execute(CodeActivityContext context)
        {
            // Laufzeitwert des Texteingabearguments abrufen
            File.Copy(context.GetValue(this.SourcePath), context.GetValue(this.TargetPath), true);
        }
    }
}
