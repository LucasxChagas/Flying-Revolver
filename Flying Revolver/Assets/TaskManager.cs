using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

    [Header("General Settings")]
    [SerializeField] Animator anim;
    [SerializeField] Color32 sucessTaskColor;

    [Header("Money Bag Task")]
    [SerializeField] GameObject moneyBagTask;
    [SerializeField] TMP_Text[] moneyBagTexts; // 0 - QTD || 1 - Text
    [SerializeField] int maxMoneyBags = 5;
    int moneyBagsCollected = 0;

    [Header("Kill Bandits Task")]
    [SerializeField] GameObject killBanditsTask;
    [SerializeField] TMP_Text[] killBanditsTexts; // 0 - QTD || 1 - Text
    [SerializeField] int maxBandits = 5;
    int banditsKilled = 0;

    [Header("Report To Sheriff Task")]
    [SerializeField] GameObject reportToSheriffTask;
    [SerializeField] TMP_Text reportToSheriffText;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public void UpdateTask(int taskNumber)
    {
        switch(taskNumber)
        {
            case 0: // Money Bag
                moneyBagsCollected++;
                if (moneyBagsCollected >= maxMoneyBags)
                {
                    moneyBagsCollected = maxMoneyBags;

                    foreach (var item in moneyBagTexts)
                    {
                        item.color = sucessTaskColor;
                    }
                }
                moneyBagTexts[0].text = $"- {moneyBagsCollected}/{maxMoneyBags}";

                if (moneyBagsCollected >= maxMoneyBags && banditsKilled >= maxBandits)
                    StartCoroutine(CallLastTask());
                break;

            case 1: // Bandit
                banditsKilled++;
                if (banditsKilled >= maxBandits)
                {
                    banditsKilled = maxBandits;

                    foreach (var item in killBanditsTexts)
                    {
                        item.color = sucessTaskColor;
                    }
                }
                killBanditsTexts[0].text = $"- {banditsKilled}/{maxBandits}";

                if (moneyBagsCollected >= maxMoneyBags && banditsKilled >= maxBandits)
                    StartCoroutine(CallLastTask());
                break;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            UpdateTask(0);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            UpdateTask(1);
        }
    }

    IEnumerator CallLastTask()
    {
        Debug.Log("Cu");
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("LastTask");
    }
}
