using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _highScoreText;
    [SerializeField] TMP_Text _multiplierText;
    [SerializeField] FloatScoreText _floatingTextPrefab;
    [SerializeField] Canvas _floatingScoreCanvas;
    
    int _score;
    int _highScore;
    float _scoreMultiplierExpiration;
    int _killMultiplier;

    void Start()
    {
        Zombie.Died += Zombie_Died;
        _highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreText.SetText("High Score: " + _highScore);
    }

    void OnDestroy()
    {
        Zombie.Died -= Zombie_Died;
    }

    void Zombie_Died(Zombie zombie)
    {
        UpdateKillMultiplier();

        _score += _killMultiplier;

        if (_score > _highScore)
        {
            _highScore = _score;
            _highScoreText.SetText("High Score: " + _highScore);
            PlayerPrefs.SetInt("HighScore", _highScore);
        }
        _scoreText.SetText(_score.ToString());

        var floatingText = Instantiate(_floatingTextPrefab, 
            new Vector3(zombie.transform.position.x, _floatingScoreCanvas.transform.position.y, zombie.transform.position.z),
            _floatingScoreCanvas.transform.rotation, 
            _floatingScoreCanvas.transform);

        floatingText.SetScoreValue(_killMultiplier);
    }
    void UpdateKillMultiplier()
    {
        if (Time.time <= _scoreMultiplierExpiration)
        {
            _killMultiplier++;
        }
        else
        {
            _killMultiplier = 1;
        }
        _scoreMultiplierExpiration = Time.time + 1f;

        _multiplierText.SetText("x " + _killMultiplier);

        if (_killMultiplier < 3)
            _multiplierText.color = Color.white;
        else if (_killMultiplier < 10)
            _multiplierText.color = Color.grey;
        else if (_killMultiplier < 20)
            _multiplierText.color = Color.yellow;
        else if (_killMultiplier < 30)
            _multiplierText.color = Color.red;
    }
}
