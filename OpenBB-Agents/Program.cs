using Services;

namespace Window
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			var ClickingThread = new ClickService(20);
			GlobalListenerService.InstallHook(ClickingThread.Switch);

			MainForm mainForm = new MainForm(ClickingThread);
			mainForm.Show();

			Application.Run();
		}
	}
}