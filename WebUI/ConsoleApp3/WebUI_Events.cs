using WebUI4CSharp_net48;
using System;
using System.IO;

namespace WebUIExample
{
    public static class WebUIEvents
    {
        public static void HandleDropdownSelection(ref webui_event_t e)
        {
            WebUIEvent lEvent = new WebUIEvent(e);

            string number1 = lEvent.GetString();
            string number2 = lEvent.GetStringAt(new UIntPtr(1));

            Console.WriteLine($"Selected numbers: Number 1 = {number1}, Number 2 = {number2}");

            // Process the selected values further if needed
        }
    }
}
