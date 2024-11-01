using GameJam.Managers;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScoreQuest", menuName = "ScriptableObjects/Quests/ScoreQuest")]
public class ScoreQuest : QuestScript
{
    [Inject] ScoreManager scoreManager;
    public override void CheckIsQuestComplete()
    {
        if(scoreManager.score >= RandomValue[Type])
        {
            QuestComplete();
        }
    }
    private void QuestComplete()
    {
        scoreManager.Money += RandomReward[Type];
    }
}
