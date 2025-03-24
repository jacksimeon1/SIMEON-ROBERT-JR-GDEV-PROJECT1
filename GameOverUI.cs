using Godot;
public partial class GameOverUI : Control
{
    public override void _Ready()
    {
        GetNode<Button>("MainMenuButton").Pressed += OnMainMenuPressed;
        GetNode<Button>("QuitButton").Pressed += OnQuitPressed;
    }

    private void OnMainMenuPressed()
    {
        GetTree().ChangeSceneToFile("res://Scenes/StartScreen.tscn"); // Change to your start screen scene
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
