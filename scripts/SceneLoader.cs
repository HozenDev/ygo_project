using Godot;
using System;

public partial class SceneLoader : Node
{
	public static SceneLoader Instance { get; private set; }

	private string _targetScenePath;
	private bool _isLoading = false;
	private LoadingScreen _loadingScreenInstance;
	
	private Action<Node> _onSceneInstantiated;
	public static readonly string _worldScene = "res://scenes/game.tscn";
	public static readonly string _combatScene = "res://scenes/combat_scene.tscn";

	[Export] public PackedScene LoadingScreenScene;

	public override void _Ready()
	{
		Instance = this;
		LoadingScreenScene = GD.Load<PackedScene>("res://scenes/loading_screen.tscn") as PackedScene;
	}

	public void LoadScene(string path, Action<Node> setupAction = null)
	{
		_targetScenePath = path;
		_isLoading = true;
		_onSceneInstantiated = setupAction;

		// add the loading screen to the UI
		_loadingScreenInstance = LoadingScreenScene.Instantiate<LoadingScreen>();
		GetTree().Root.AddChild(_loadingScreenInstance);

		// start the background load request
		ResourceLoader.LoadThreadedRequest(path);
	}

	public override void _Process(double delta)
	{
		if (!_isLoading) return;

		// 3. Poll the loading status
		var progress = new Godot.Collections.Array();
		var status = ResourceLoader.LoadThreadedGetStatus(_targetScenePath, progress);

		switch (status)
		{
			case ResourceLoader.ThreadLoadStatus.InProgress:
				// Update the progress bar (progress[0] is a float 0.0-1.0)
				_loadingScreenInstance.UpdateProgress((double)progress[0]);
				break;

			case ResourceLoader.ThreadLoadStatus.Loaded:
				// 4. Finish loading
				_isLoading = false;
				var packedScene = (PackedScene)ResourceLoader.LoadThreadedGet(_targetScenePath);
				var newScene = packedScene.Instantiate();
				_onSceneInstantiated?.Invoke(newScene);
				
				var root = GetTree().Root;
				var currentScene = GetTree().CurrentScene;
				
				// Change scene and cleanup loading screen
				root.AddChild(newScene);
				GetTree().CurrentScene = newScene;
				currentScene.QueueFree();

				_loadingScreenInstance.QueueFree();
				break;

			case ResourceLoader.ThreadLoadStatus.Failed:
				GD.PrintErr("Failed to load scene: " + _targetScenePath);
				_isLoading = false;
				break;
		}
	}
}
