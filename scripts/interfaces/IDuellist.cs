using Godot;
using System.Linq;

public interface IDuellist
{
	DuellistData GetDuelData();

	void OnDuelStarted();
	void OnDuelFinished(bool victory);
	
	public bool CanFight() {
		return GetDuelData().Party.Any(m => m.CurrentLife > 0);
	}
	
	public Monster GetFirstValidMonster() {
		// Todo: manage the first valid monster
		return GetDuelData().Party[0];
	}
	
	public Monster GetActiveMonster() {
		// Todo: manage active monsters
		return GetDuelData().Party[0];
	}
}
