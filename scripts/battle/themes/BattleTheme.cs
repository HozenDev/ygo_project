using Godot;
using System;

[GlobalClass]
public partial class BattleTheme : Resource 
{
	[Export] public Texture2D BackgroundTexture;
	[Export] public AudioStream BattleMusic;
}
