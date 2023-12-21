using System.Activities;

namespace Testautomation
{
    public sealed class ProgressProtocol : CodeActivity
    {
        // Aktivitätseingabeargument
        public InArgument<string> PathXMLFile { get; set; }
        public InArgument<string> Text { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            RPATestautomation protokoll = new RPATestautomation();
            protokoll.WriteProgress(
                context.GetValue(this.PathXMLFile),
                context.GetValue(this.Text)
                );
        }
    }
}
