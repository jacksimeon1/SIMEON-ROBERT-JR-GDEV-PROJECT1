using Godot;
using System;
using System.Collections.Generic;
[Export] private VBoxContainer _scoreContainer;


public partial class Leaderboard : Control
{
    private const string SCORE_FILE_PATH = "user://scores.json"; // Path to saved scores

    private VBoxContainer _scoreContainer;

    public override void _Ready()
    {
        _scoreContainer = GetNode<VBoxContainer>("scoreContainer"); // Ensure the name matches in the scene
        LoadScores();

        TextureButton backButton = GetNode<TextureButton>("BackButton");
        backButton.Pressed += OnBackPressed;
    }

    private void LoadScores()
    {
        _scoreContainer.CallDeferred("queue_free"); // Ensures previous children are cleared

        // Read the scores file
        if (!FileAccess.FileExists(SCORE_FILE_PATH))
        {
            GD.Print("No scores file found. Creating a new one.");
            DisplayNoScoresMessage();
            return;
        }

        string json = FileAccess.GetFileAsString(SCORE_FILE_PATH);
        var parsed = Json.ParseString(json);

        if (parsed is not Array scoresArray)
        {
            GD.PrintErr("Failed to parse scores JSON.");
            DisplayNoScoresMessage();
            return;
        }

        List<int> scores = new List<int>();
        foreach (var item in scoresArray)
        {
            if (item is int score)
                scores.Add(score);
        }

        if (scores.Count == 0)
        {
            DisplayNoScoresMessage();
            return;
        }

        // Sort and display scores
        scores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order

        for (int i = 0; i < scores.Count; i++)
        {
            Label scoreLabel = new Label();
            scoreLabel.Text = $"{i + 1}. {scores[i]}";
            scoreLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1)); // White text
            _scoreContainer.AddChild(scoreLabel);
        }
    }

    private void DisplayNoScoresMessage()
    {
        Label noScoresLabel = new Label();
        noScoresLabel.Text = "No scores available.";
        noScoresLabel.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red text
        _scoreContainer.AddChild(noScoresLabel);
    }

    private void OnBackPressed()
    {
        GD.Print("Going back to Start Screen...");
        GetTree().ChangeSceneToFile("res://control/StartScreen.tscn"); // Fix path case sensitivity
    }
}
