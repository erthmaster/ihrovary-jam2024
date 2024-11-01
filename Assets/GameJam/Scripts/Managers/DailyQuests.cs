using GameJam.Managers;
using System;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class DailyQuests : MonoBehaviour
{
    [Inject] private DiContainer _container;

    [SerializeField] private QuestScript[] Quests;

    public QuestScript Quest_1;
    public QuestScript Quest_2;
    public QuestScript Quest_3;

    private int beforeSignDay;

    public ScoreQuest _scoreQ;
    private void Start()
    {
        Injection();
        //if(beforeSignDay != DateTime.Now.Day)//start to update quests
        //{
        //    beforeSignDay = DateTime.Now.Day;
        //    RandomizeQuests();
        //}

        RandomizeQuests();//temp
    }
    private void Injection()
    {
        foreach (QuestScript script in Quests) { 
            _container.Inject(script);
        }
    }
    private void RandomizeQuests()
    {
        Quest_1 = null;
        Quest_2 = null;
        Quest_3 = null;
        while (Quest_3 == null)
        { 
            int quest = UnityEngine.Random.Range(0, Quests.Length);
            if (Quest_1 == null)
            {
                Quest_1 = Quests[quest];
                continue;
            }
            if (Quest_2 == null)
            {
                //if (Quests[quest] == Quest_1) continue;
                Quest_2 = Quests[quest];
                continue;
            }
            if (Quest_3 == null)
            {
                //if (Quests[quest] == Quest_1) continue;
                //if (Quests[quest] == Quest_2) continue;

                Quest_3 = Quests[quest];
            }
        }
        Quest_1.GenerateRandoms();
        Quest_2.GenerateRandoms();
        Quest_3.GenerateRandoms();
    }
    public void CheckQuestsComplete()
    {
        Quest_1.CheckIsQuestComplete();
        Quest_2.CheckIsQuestComplete();
        Quest_3.CheckIsQuestComplete();
    }
}