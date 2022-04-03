using System.Collections;
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

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartLevel);
        FillLevels();
    }

    void FillLevels()
    {
        LevelSelect.options = Levels.Select(level => new Dropdown.OptionData(level.Name)).ToList();
    }

    void StartLevel()
    {
        var selectedLevel = Levels[LevelSelect.value];
        BoardGame.StartingBoard = selectedLevel.Load();
        SceneManager.LoadScene("Assets/Test/Scenes/GamePlay.unity");
    }
}
