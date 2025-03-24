using Godot;
using System;

public partial class StartScreen : Control
{
    public override void _Ready()
    {
        // Get button references
        TextureButton playButton = GetNode<TextureButton>("PlayButton");
        TextureButton leaderboardButton = GetNode<TextureButton>("LeaderboardButton");

        // Connect button signals
        playButton.Pressed += OnPlayPressed;
        leaderboardButton.Pressed += OnLeaderboardPressed;
    }

    private void OnPlayPressed()
    {
        GD.Print("Play button clicked! Loading Game...");
        GetTree().ChangeSceneToFile("res://Scenes/Game/Game.tscn"); // Change to your actual game scene path
    }

    private void OnLeaderboardPressed()
    {
        GD.Print("Leaderboard button clicked! Loading Leaderboard...");
        GetTree().ChangeSceneToFile("res://scenes/Leaderboard.tscn"); // Change to your actual leaderboard scene path
    }
}