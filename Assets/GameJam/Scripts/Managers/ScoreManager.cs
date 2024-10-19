using System.Collections;
using TMPro;
using UnityEngine;
namespace GameJam.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int score;
        private int visScore = 0;

        public int money;

        [SerializeField] private float _speed;

        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _scoreTextOnGameOver;

        [SerializeField] private ParticleSystem _moneyParticle;
        [SerializeField] private TMP_Text _moneyText;
        [SerializeField] private TMP_Text _moneyMenuText;

        private void Start()
        {
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
        public void ShowEndScore()
        {
            _scoreTextOnGameOver.text = $"Score: \n{score}";
        }
        private void Update()
        {
            if(visScore != score)
                StartCoroutine(textAnim());

        }
        public void textMoney()
        {
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
