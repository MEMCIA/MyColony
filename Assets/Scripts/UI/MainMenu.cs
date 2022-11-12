using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // TODO this causes this component to keep all BoardObject in memory when displayed
    public List<BoardObject> Levels;
    public Button StartButton;
    public Dropdown LevelSelect;
    public Dropdown ModeSelect;
    public Toggle AIToggle;

    public List<BoardGame.Mode> _modes;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartLevel);
        FillLevels();
        FillModes();
    }

    void FillLevels()
    {
        LevelSelect.options = Levels.Select(level => new Dropdown.OptionData(level.Name)).ToList();
    }

    void FillModes()
    {
        _modes.Add(BoardGame.Mode.EasyAI);
        _modes.Add(BoardGame.Mode.StandardAI);
        _modes.Add(BoardGame.Mode.Humans);

        var values = Enum.GetValues(typeof(BoardGame.Mode));
        ModeSelect.options = _modes.Select(mode => new Dropdown.OptionData(mode.ToString())).ToList();

        ModeSelect.value = 1;
    }

    void StartLevel()
    {
        var selectedLevel = Levels[LevelSelect.value];
        BoardGame.StartingBoard = selectedLevel.Load();
        BoardGame.CurrentMode = _modes[ModeSelect.value];
        SceneManager.LoadScene("Assets/Scenes/GamePlay.unity");
    }
}
