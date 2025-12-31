using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Start the application with a custom ApplicationContext
        // This prevents a main window from ever being created or shown automatically.
        Application.Run(new CSharpNewYearCountdown.NewYearAppContext());
    }
}
