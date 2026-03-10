using Godot;
using System;

public enum SpecialCardState {
	None,
	InDeck,
	InHand,
	Setted,
	Activated
}

public partial class SpecialCard : Card<SpecialCardData>
{	
	// Card State
	private SpecialCardState _state;
	
	// ------------ Main Actions -------------- //
	
	public void Activate() {
		if (GetData() == null) return; // No card to be activated
		
		// todo: handle activation effect
		_state = SpecialCardState.Activated;
				
		// Always make the card visible for info
		SetVisibility(true);
	}
	
	public override void Destroy() {
		_state = SpecialCardState.None;
		SetData(null);
	} 
	
	public void Set() {
		_state = SpecialCardState.Setted;
	}
	
	// ------------ Informations -------------- //
	
	public SpecialCardState GetState() => _state;
}
