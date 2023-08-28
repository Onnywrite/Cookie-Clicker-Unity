using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            UpdateText();
        }
    }

    private void Start()
    {
        Score = 0;
    }

    private void UpdateText()
    {
        _scoreText.text = Score.ToString();
    }
}
