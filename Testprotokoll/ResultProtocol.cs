using System.Activities;

namespace Testautomation
{
    public sealed class ResultProtocol : CodeActivity
    {
        // Aktivitätseingabeargument
        public InArgument<string> PathXMLFile { get; set; }
        public InArgument<string> Text { get; set; }
        public InArgument<string> Soll { get; set; }
        public InArgument<string> Ist { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RPATestautomation protokoll = new RPATestautomation();
            protokoll.WriteResult(
                context.GetValue(this.PathXMLFile),
                context.GetValue(this.Text),
                context.GetValue(this.Soll),
                context.GetValue(this.Ist)
                );
        }
    }
}
