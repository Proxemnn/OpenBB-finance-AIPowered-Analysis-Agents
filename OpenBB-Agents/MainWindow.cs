using Services;

namespace Window
{
	public partial class MainForm : Form
	{
		private readonly ClickService clickService;

		public MainForm(ClickService clickService)
		{
			this.clickService = clickService;
			InitializeComponent();
		}

		~MainForm()
		{
			this.clickService.Dispose();
		}

		private void milissecondsSelector_ValueChanged(object sender, EventArgs e)
		{
			this.clickService.UpdateInterval((int)milissecondsSelector.Value);
		}
	}
}