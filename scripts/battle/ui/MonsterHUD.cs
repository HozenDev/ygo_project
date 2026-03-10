using Godot;
using System;

public partial class MonsterHUD : MarginContainer
{
	[Export] private Label _nameLabel;
	[Export] private Label _levelLabel;
	[Export] private TextureProgressBar _hpBar;
	[Export] private Label _hpText;
	[Export] private NinePatchRect _cardTexture;
	
	public void UpdateUI(MonsterCard monster)
	{
		_nameLabel.Text = monster.Nickname;
		_levelLabel.Text = $"Lv. {monster.Level}";
		
		// Life bar update
		_hpBar.MaxValue = monster.MaxLife;
		Tween tween = CreateTween();
		tween.TweenProperty(_hpBar, "value", monster.CurrentLife, 0.5f)
			 .SetTrans(Tween.TransitionType.Circ)
			 .SetEase(Tween.EaseType.Out);
		_hpText.Text = $"{monster.CurrentLife}/{monster.MaxLife}";
		UpdateBarColor(monster.CurrentLife, monster.MaxLife);
		
		// card update
		UpdateCardTexture(monster.GetData().CardCoordinates);
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
