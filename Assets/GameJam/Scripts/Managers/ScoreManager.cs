using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameJam.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int score;
        private int maxScore;
        private int visScore = 0;

        public int money;

        [SerializeField] private float _speed;

        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _scoreTextOnGameOver;

        [SerializeField] private TMP_Text _menuScoreText;

        [SerializeField] private ParticleSystem _moneyParticle;
        [SerializeField] private TMP_Text _moneyText;

        [SerializeField] private TMP_Text _moneyMenuText;

        [Inject] Items _items;

        private async void Start()
        {
            await UnityServices.InitializeAsync();
            var client = CloudSaveService.Instance.Data;
            var query = await client.LoadAsync(new HashSet<string> { "max_score" });
            maxScore = Convert.ToInt32(query["max_score"]);

            SetZeroScore();
        }
        public void SetZeroScore()
        {
            score = 0;
        }
        public void AddScore(int _score)
        {
            score += _score;
        }
        public void RemoveScore(int _score)
        {
            score -= _score;
        }
        public async void ShowEndScore()
        {
            _scoreTextOnGameOver.text = $"Score: \n{score}";
            if(score > maxScore)
            {
                maxScore = score;
                _menuScoreText.text = $"Score \n{maxScore}";

                var data = new Dictionary<string, object> { { "max_score", maxScore } };
                await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            }
        }
        private void Update()
        {
            if(visScore != score)
                StartCoroutine(textAnim());
        }
        public void AddMoney()
        {
            if (_items._isDoubleGold)
                money += 2;
            else
                money++;
            _moneyText.text = money.ToString();
            _moneyParticle.Play();
        }
        IEnumerator textAnim()
        {
            if (visScore < score)
                visScore++;
            else if (visScore > score)
                visScore--;
            else
            {
                _scoreText.text = visScore.ToString();

                yield break;
            }

            _scoreText.text = visScore.ToString();
            yield return new WaitForSeconds(_speed);
            StartCoroutine(textAnim());
        }
    }
}
