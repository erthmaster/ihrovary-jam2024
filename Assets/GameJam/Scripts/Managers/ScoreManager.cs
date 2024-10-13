using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace GameJam.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public int score;
        private int visScore = 0;

        private bool active;

        [SerializeField] private float _speed;

        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _scoreTextOnGameOver;

        public void AddScore(int _score)
        {
            score += _score;
        }
        public void RemoveScore(int _score)
        {
            score -= _score;
        }
        private void Update()
        {
            if(visScore != score)
                StartCoroutine(textAnim());
            if (_scoreTextOnGameOver.gameObject.activeInHierarchy)
            {
                _scoreTextOnGameOver.text = $"Score: \n{score}";//temp, soon in playermanager!
            }
        }
        IEnumerator textAnim()
        {
            active = true;
            if (visScore < score)
                visScore++;
            else if (visScore > score)
                visScore--;
            else
            {
                _scoreText.text = "Score: " + visScore.ToString();
                active = false;
                yield break;
            }

            _scoreText.text = "Score: " + visScore.ToString();
            yield return new WaitForSeconds(_speed);
            StartCoroutine(textAnim());
        }
    }
}
