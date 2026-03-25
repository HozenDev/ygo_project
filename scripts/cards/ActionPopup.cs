using Godot;
using System;

public partial class ActionPopup : Control
{
    private const float _offsetY = 10.0f;
    private const float _offsetX = 10.0f;
    private Vector2 _offset;

    public void SetPosition(Vector2 newPosition) {
	Position = newPosition;
    }

    public void SetOffset(Vector2 offset) {
	_offset = offset;
	Position += offset;
    }

    public void Center() {
	Position = new Vector2((Size.X / 2), (Size.Y / 2));
    }

    public void AlignTop(float parentHeight) {
	Position = new Vector2(Position.X, - parentHeight - _offsetY + _offset.Y);
    }

    public void AlignBottom(float parentHeight) {
	Position = new Vector2(Position.X, parentHeight + _offsetY + _offset.X);
    }

    public void AlignLeft(float parentWidth) {
	Position = new Vector2(- parentWidth - _offsetX + _offset.X, Position.Y);
    }
    
    public override void _Input(InputEvent @event)
    {
	if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
	{
	    if (!GetGlobalRect().HasPoint(mouseEvent.GlobalPosition))
	    {
		QueueFree();
		// GetViewport().SetInputAsHandled();
	    }
	}
    }
}
