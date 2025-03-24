using Godot;
using System;

public partial class Gem : Area2D
{
    [Export] float _speed = 110.0f;
    [Signal] public delegate void OnScoredEventHandler();
    [Signal] public delegate void OnGemOffScreenEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Position += new Vector2(0, _speed * (float)delta);
        CheckHitBottom();
	}

    private void CheckHitBottom()
    {
        if (Position.Y > GetViewportRect().End.Y)
        {
            GD.Print("Hit Bottom");
            EmitSignal(SignalName.OnGemOffScreen);
            SetProcess(false);
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print("Scored!");
        EmitSignal(SignalName.OnScored);
        QueueFree();
    }


}
