using System.Activities;

namespace Testautomation
{
    public sealed class RPA_Close : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            RPATestautomation testautomation = new RPATestautomation();
            testautomation.CloseRPA();
        }
    }
}
