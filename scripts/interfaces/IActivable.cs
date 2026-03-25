using Godot;
using System;

public interface IActivable
{
    public PackedScene ActionPopupScene { get; set; }
    public ActionPopup Popup { get; set; }

    public virtual bool IsActivated() => Popup != null;

    public bool CanActivate();
    public void Activate();
    public void SetPopupPosition();

    public virtual void ShowActivationPopup(Node parent) {
	// Close the old one if exists
	if (GodotObject.IsInstanceValid(Popup))
	{
	    HideActivationPopup();
	}

	// Instantiate popup
	Popup = ActionPopupScene.Instantiate<ActionPopup>();
	parent.AddChild(Popup);
	SetPopupPosition();

	// Set the button
	Button btn = Popup.GetNode<Button>("BtnActivate");
	btn.Pressed += () => {
	    Activate();
	    HideActivationPopup();
	};
    }
    
    public virtual void HideActivationPopup() {
	if (IsActivated()) {
	    Popup.QueueFree();
	    Popup = null;
	}
    }
}
