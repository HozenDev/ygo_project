using Godot;
using System.Collections.Generic;

public enum ThemeType { CITY, FOREST, DUNGEON, BOSS }

public static class ThemeRegistry
{
	// Le dictionnaire qui lie l'Enum au chemin du fichier Resource
	private static readonly Dictionary<ThemeType, string> _paths = new()
	{
		{ ThemeType.CITY, "res://resources/battle_themes/city.tres" }
	};

	public static BattleTheme Get(ThemeType type)
	{
		if (_paths.TryGetValue(type, out string path))
		{
			return GD.Load<BattleTheme>(path);
		}
		
		GD.PrintErr($"Theme {type} not found! Load default theme.");
		return GD.Load<BattleTheme>("res://resources/battle_themes/city.tres");
	}
}
