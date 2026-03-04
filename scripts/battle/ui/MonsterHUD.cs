using Godot;
using System;

public partial class MonsterHUD : MarginContainer
{
	[Export] private Label _nameLabel;
	[Export] private Label _levelLabel;
	[Export] private TextureProgressBar _hpBar;
	[Export] private Label _hpText;
	[Export] private NinePatchRect _cardTexture;
	
	public void UpdateUI(MonsterData monster)
	{
		_nameLabel.Text = monster.MonsterName;
		_levelLabel.Text = $"Lv. {monster.Level}";
		
		// Life bar update
		_hpBar.MaxValue = monster.MaxHp;
		_hpBar.Value = monster.CurrentHp;
		_hpText.Text = $"{monster.CurrentHp}/{monster.MaxHp}";
		UpdateBarColor(monster.CurrentHp, monster.MaxHp);
		
		// card update
		UpdateCardTexture(monster.CardCoordinates);
	}

	private void UpdateBarColor(int current, int max)
	{
		float ratio = (float)current / max;
		if (ratio < 0.2f) _hpBar.TintProgress = Colors.Red;
		else if (ratio < 0.5f) _hpBar.TintProgress = Colors.Yellow;
		else _hpBar.TintProgress = Colors.Green;
	}
	
	private void UpdateCardTexture(Vector2 cardPosition) {
		Rect2 regionRect = _cardTexture.RegionRect;
		regionRect.Position = cardPosition;
		_cardTexture.RegionRect = regionRect;
	}
}
