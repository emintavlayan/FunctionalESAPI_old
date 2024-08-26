using WebUI4CSharp_net48;
using System;
using System.IO;

namespace WebUIExample
{
    class Program
    {
        static void Main()
        {
            // Read the HTML content from the file
            // Calculate the path to the index.html file two levels above the current directory
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\"));
            string htmlFilePath = Path.Combine(projectRoot, "index.html");

            // Read the HTML content from the file
            string my_html = File.ReadAllText(htmlFilePath);
            
            // Create the WebUI window
            WebUIWindow window = new WebUIWindow();

            // Bind the JavaScript function to C# functions
            window.Bind("handleDropdownSelection", WebUIEvents.HandleDropdownSelection);

            // Show the WebUI window with the HTML content
            window.Show(my_html);

            // Wait for the UI to close
            WebUI.Wait();

            // Clean up resources
            WebUI.Clean();
        }
    }
}
