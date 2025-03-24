using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class Game : Node2D
{
    const double GEM_MARGIN = 50.0;
    private static readonly AudioStream EXPLODE_SOUND = GD.Load<AudioStream>("res://assets/explode.wav");

    [Export] private PackedScene _gemScene;
    [Export] private Timer _spawnTimer;
    [Export] private Label _scoreLabel;
    [Export] private AudioStreamPlayer _music;
    [Export] private AudioStreamPlayer2D _effects;

    private int _score = 0;
    private bool _isGameOver = false;
    private PackedScene _gameOverScene = GD.Load<PackedScene>("res://Scenes/GameOver.tscn");

    private const string SCORE_FILE_PATH = "user://scores.json"; // Save scores in a JSON file

    public override void _Ready()
    {
        _spawnTimer.Timeout += SpawnGem;
        SpawnGem();
    }

    private void SpawnGem()
    {
        Rect2 vpr = GetViewportRect();
        Gem gem = (Gem)_gemScene.Instantiate();
        AddChild(gem);

        float rX = (float)GD.RandRange(vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN);
        gem.Position = new Vector2(rX, -100);
        gem.OnScored += OnScored;
        gem.OnGemOffScreen += GameOver;
    }

    private void OnScored()
    {
        GD.Print("OnScored Received");
        _score += 1;
        _scoreLabel.Text = $"{_score:0000}";
        _effects.Play();
    }

    private void GameOver()
    {
        GD.Print("GameOver");

        foreach (Node node in GetChildren())
        {
            node.SetProcess(false);
        }
        _spawnTimer.Stop();
        _music.Stop();
        _effects.Stop();
        _effects.Stream = EXPLODE_SOUND;
        _effects.Play();

        SaveScore(_score); // Save the score before showing the game over screen
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        Control gameOverUI = (Control)_gameOverScene.Instantiate();
        AddChild(gameOverUI);

        Button mainMenuButton = gameOverUI.GetNode<Button>("MainMenuButton");
        Button quitButton = gameOverUI.GetNode<Button>("QuitButton");

        mainMenuButton.Pressed += OnMainMenuPressed;
        quitButton.Pressed += OnQuitPressed;

        _isGameOver = true;
    }

    private void OnMainMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://control/StartScreen.tscn");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }

    private void SaveScore(int score)
    {
        List<int> scores = LoadScores();
        scores.Add(score);
        scores = scores.OrderByDescending(s => s).Take(10).ToList(); // Keep only top 10 scores

        string json = Godot.Json.Stringify(scores);
        File.WriteAllText(SCORE_FILE_PATH, json);
        GD.Print("Scores saved successfully!");
    }

    private List<int> LoadScores()
    {
        if (!File.Exists(SCORE_FILE_PATH))
            return new List<int>();

        string json = File.ReadAllText(SCORE_FILE_PATH);
        return Godot.Json.Parse(json).As<List<int>>() ?? new List<int>();
    }
}
