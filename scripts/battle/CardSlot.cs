using Godot;
using System;

public partial class CardSlot<T, D>: Area2D 
	where T: Card<D> 
	where D: CardData
{
	[Export] private Sprite2D _sprite;
	
	[Export] private CollisionPolygon2D _collision;
	private float _collisionPrecision = 2.0f;
	
	private T _card;
	
	public bool IsOccupied => _card != null;
	
	public T GetCard() => _card;
	public Sprite2D GetSprite() => _sprite;
	
	// ---------- Card methods ---------- //	
	
	public virtual void SetCard(T card) {
		if (IsOccupied) {
			throw new Exception("The slot is already taken.");
		}

		_card = card;
		
		// Transfert de la carte de la main vers le slot
		if (card.GetParent() != null)
			card.GetParent().RemoveChild(card);
			
		AddChild(card);
		
		// Reset des transfos pour être au centre du slot
		card.Position = Vector2.Zero;
		card.Scale = Vector2.One;
	}
	
	public virtual void DropCard() {
		if (!IsOccupied) {
			throw new Exception("No card to drop");
		}
		
		_card.Destroy();
		_card = null;
	}
	
	// ---------- Signal methods ---------- //	
	
	public override void _Ready() {
		
		UpdateCollision();
	}
	
	// ---------- UI ---------- //	
	
	public bool IsFlipped() => _sprite.FlipH;
	public void Flip() {
		_sprite.FlipH = true;
	}
	
	public void UpdateOffset(Vector2 newOffset) {
		_sprite.Offset = newOffset;
	}
	
	public void UpdateSprite(Texture2D texture) {
		_sprite.Texture = texture;
	}
		
	// ---------- Signal methods ---------- //	
		
	public virtual void OnMouseEntered() {
		if (IsOccupied) {
			_card.OnMouseEntered();
		}
	}
	
	public virtual void OnMouseExited()
	{
		if (IsOccupied) {
			_card.OnMouseExited();
		}
	}
	
	// ---------- Collision ---------- //
	
	public void UpdateCollision()
	{
		if (_sprite.Texture == null || _collision == null) return;

		// Create a bitmap
		var bitmap = new Bitmap();
		bitmap.CreateFromImageAlpha(_sprite.Texture.GetImage());

		// Convert to have opaque pixels
		var rect = new Rect2I(Vector2I.Zero, (Vector2I)_sprite.Texture.GetSize());
		var polygons = bitmap.OpaqueToPolygons(rect, _collisionPrecision);
		
		if (polygons.Count > 0)
		{
			Vector2[] points = polygons[0];
			Vector2 size = _sprite.Texture.GetSize();
			Vector2 scale = _sprite.Scale;

			// Apply Transformations
			for (int i = 0; i < points.Length; i++)
			{
				// Handle Flips
				if (_sprite.FlipH) points[i].X = size.X - points[i].X;
				if (_sprite.FlipV) points[i].Y = size.Y - points[i].Y;

				// Handle Centering & Custom Offset
				// If Centered is true, (0,0) is at -size/2
				// If false, (0,0) is at (0,0)
				Vector2 finalOffset = _sprite.Offset;
				if (_sprite.Centered)
				{
					finalOffset -= size / 2;
				}
				
				points[i] += finalOffset;
				points[i] *= scale;
			}

			_collision.Polygon = points;
		}
	}
}
