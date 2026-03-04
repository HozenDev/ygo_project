using Godot;

public partial class LoadingScreen : Control
{
	[Export] private ProgressBar _progressBar;

	public void UpdateProgress(double progress)
	{
		// progress is a value from 0.0 to 1.0
		_progressBar.Value = progress * 100;
	}
}
