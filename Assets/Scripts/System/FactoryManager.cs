using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct entries
{
    public Text coin;
    public Text workTime;
    public Text produce;
    public Text claim;
    public Image selectedBox;
    public Text boxOrder;
    public List<Image> plan;
    public Transform boxes;
}
public class FactoryManager : MonoBehaviour
{
    public static FactoryManager Instance { get;  private set; }
    Vector2 genPos = new(-18, 3);

    public int coin, produce, claim;
    public List<Product> plan = new();
    public List<Box> boxes = new();
    public List<Product> products = new();

    float time, lastTime;
    public float interval;
    public bool started;
    public entries entries;
    public int maxProduce;

    void Start()
    {
        Instance = this;
        GameStart();
    }

    public void GameStart()
    {
        coin = 7200;
        produce = 0;
        claim = 0;
        lastTime = 8 * 60;

        interval = 8;

        maxProduce = UnityEngine.Random.Range(32, 64);

        plan.Clear();

        plan.Add(products[UnityEngine.Random.Range(0, products.Count)]);
        plan.Add(products[UnityEngine.Random.Range(0, products.Count)]);
        plan.Add(products[UnityEngine.Random.Range(0, products.Count)]);

        started = true;
    }

    void Update()
    {
        time += Time.deltaTime;
        lastTime -= Time.deltaTime;

        if (time > interval)
        {
            time = 0;

            var target = plan[UnityEngine.Random.Range(0, plan.Count)];
            Instantiate(target, genPos, Quaternion.identity);
        }

        for (int i = 0; i < plan.Count; i++)
        {
            entries.plan[i].sprite = plan[i].dig0;
        }

        int minutes = Mathf.FloorToInt(lastTime / 60f);
        int seconds = Mathf.FloorToInt(lastTime % 60f);

        string timeFormatted = string.Format("{0:00}:{1:00}", minutes, seconds);

        entries.coin.text = coin.ToString();
        entries.workTime.text = "업무 시간	:	" + timeFormatted + " 남음";
        entries.produce.text = "생산량		:	" + produce + "/" + maxProduce;
        entries.claim.text = "클레임		:	" + claim + "건";
    }

    public void PurchaseBox()
    {
        if (coin < 2400)
        {
            return;
        }

        coin -= 2400;

        foreach (Transform child in entries.boxes)
        {
            var spawned = Instantiate(child.gameObject, child.transform.position, Quaternion.identity);
            spawned.SetActive(true);
        }
    }
}
