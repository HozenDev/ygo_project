using Godot;
using System;

public partial class MonsterCardSlot : CardSlot<MonsterCard, MonsterData>
{
	public PackedScene MonsterCardScene = GD.Load<PackedScene>("uid://c1qrdkfq4jgoj") as PackedScene;
	private const int CARD_OFFSET = 30;
	
	public override void SetCard(MonsterCard card) {
		base.SetCard(card);
		UpdateSprite(card.GetData().BattleSprite);
		AlignVertically();
	}
	
	public void SetBattleSprite() {
		
	}
	
	public void AlignVertically() {
		try {
			var sprite = GetSprite();
			sprite.Offset = new Vector2(0, -sprite.Texture.GetSize().Y / 2);
			sprite.Offset += new Vector2(0, CARD_OFFSET);
		}
		catch (Exception e) {
			GD.PushError("Sprite center error: ", e);
		}
	}
}
