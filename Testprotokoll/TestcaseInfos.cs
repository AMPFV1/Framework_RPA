using System.Activities;

namespace Testautomation
{
    public sealed class TestcaseInfos : CodeActivity<string>
    {
        // Aktivitätseingabeargument vom Typ "string" definieren
        public InArgument<string> PathXMLFile { get; set; }
        public InArgument<string> Value { get; set; }

        // Wenn durch die Aktivität ein Wert zurückgegeben wird, erfolgt eine Ableitung von CodeActivity<TResult>
        // und der Wert von der Ausführmethode zurückgegeben.
        protected override string Execute(CodeActivityContext context)
        {
            RPATestautomation funktionen = new RPATestautomation();
            return funktionen.GetValueFromTestcaseXML(context.GetValue(this.PathXMLFile), context.GetValue(this.Value));
        }
    }
}
