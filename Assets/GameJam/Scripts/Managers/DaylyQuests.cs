using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DaylyQuests : MonoBehaviour
{
    [SerializeField] private QuestScript[] Quests;

    public QuestScript Quest_1;
    public QuestScript Quest_2;
    public QuestScript Quest_3;

    private int beforeSignDay;
    private void Start()
    {
        if(beforeSignDay != DateTime.Now.Day)
        {
            beforeSignDay = DateTime.Now.Day;
            RandomizeQuests();
        }

        RandomizeQuests();//temp
    }
    private void RandomizeQuests()
    {
        //Quest_1 = Quests[Random.]
        Debug.Log(Quests[0]);
    }
}