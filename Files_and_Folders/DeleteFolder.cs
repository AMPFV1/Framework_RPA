using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Files_and_Folders
{
    public sealed class DeleteFolder : CodeActivity
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> FullFolderName { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override void Execute(CodeActivityContext context)
        {
            Directory.Delete(context.GetValue(this.FullFolderName),true);
        }
    }
}
