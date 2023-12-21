using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Testautomation
{
    public sealed class SetError : CodeActivity
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> PathXMLFile { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RPATestautomation protokoll = new RPATestautomation();
            protokoll.SetError(context.GetValue(this.PathXMLFile));
        }
    }
}
