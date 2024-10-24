using Onnywrite.Common;
using Onnywrite.Primitives;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _deltaPrefab;
    private Pool<Text> _deltaPool;
    private int _score = 0;

    public int Score => _score;

    private void Awake()
    {
        _deltaPool = new(() => 
        Instantiate(_deltaPrefab, _scoreText.transform.position, Quaternion.identity, transform), 5);
    }

    public void AddScore(int scores)
    {
        _score += scores;
        _scoreText.color = scores < 0 ? Color.red : Color.white;
        StartCoroutine(SummonDeltaCoroutine(scores));
        UpdateText();
    }
    
    public void Increment() => AddScore(1);

    private void UpdateText()
    {
        _scoreText.text = _score.ToString();
    }

    private IEnumerator SummonDeltaCoroutine(int scores)
    {
        var pos = MathGeek.RandomVec3(-1f,1f, 1f, 1f, 0f, 0f);
        var text = _deltaPool.Take();
        text.color = scores < 0 ? Color.red : Color.green;
        text.text = (scores < 0 ? "" : "+") + scores.ToString();
        text.transform.position += pos;
        yield return new WaitForSeconds(1f);
        text.transform.position = Vector3.zero;
        _deltaPool.Return(text);
    }
}