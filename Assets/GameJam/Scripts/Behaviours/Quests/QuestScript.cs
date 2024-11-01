using UnityEngine;

public abstract class QuestScript : ScriptableObject
{
    public int[] RandomValue;
    public int[] RandomReward;

    public int Type;

    public bool IsQuestComplited;
    public void GenerateRandoms()
    {
        Type = Random.Range(0, RandomValue.Length);
    }
    public abstract void CheckIsQuestComplete();
}
