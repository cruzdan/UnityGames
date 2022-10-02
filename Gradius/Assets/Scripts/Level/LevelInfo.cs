using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField] private GameObject enemyInfoPrefab;
    [SerializeField] private GameObject phaseInfoPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private BackgroundMovement background;
    [SerializeField] private EnemyGenerator enemyGenerator;
    private GameObject info;
    private float progress;
    private int phase = 0;

    //phase 1 variables
    [SerializeField] float timeToGenerateEnemy = 0.25f;
    float timer = 0f;
    private int totalEnemies8 = 100;
    private int actualEnemies = 0;

    public void SetActualEnemies(int enemies) { actualEnemies = enemies; }
    public void SetPhase(int value) { phase = value; }
    public void SetTimer(float value) { timer = value; }
    // Start is called before the first frame update
    void Start()
    {
        progress = SquaresResolution.TotalSquaresX / 14f;

        for (int i = 1; i < 6; i++)
        {
            float m;
            if (i % 2 == 0)
            {
                m = 0f;
            }
            else
            {
                m = 1f;
            }
            GenerateEnemyInfo(i * 4, m, false, 0);
        }

        for (int i = 1; i < 5; i++)
        {
            int m;
            if (i % 2 == 0)
            {
                m = 1;
            }
            else
            {
                m = -1;
            }
            GenerateEnemyInfo(i * 2 + 20, m, false, 1);
        }

        for (int i = 1; i < 4; i++)
        {
            float m;
            if (i % 2 == 0)
            {
                m = 0f;
            }
            else
            {
                m = 1f;
            }
            GenerateEnemyInfo(i * 3f + 28f, m, false, 0);
        }

        for (int i = 1; i < 4; i++)
        {
            int m;
            if (i % 2 == 0)
            {
                m = 1;
            }
            else
            {
                m = -1;
            }
            GenerateEnemyInfo(i * 2f + 37f, m, false, 1);
        }
        GenerateEnemyInfo(45f, 0, false, 2);
        GenerateEnemyInfo(47f, 0, true, 2);
        GenerateEnemyInfo(49f, 0, false, 2);

        for (int i = 1; i < 4; i++)
        {
            int m;
            if (i % 2 == 0)
            {
                m = 1;
            }
            else
            {
                m = -1;
            }
            GenerateEnemyInfo(i * 2 + 50, m, false, 1);
        }
        GenerateEnemyInfo(57f, 1f, false, 3);
        GenerateEnemyInfo(58f, 1f, false, 3);
        GenerateEnemyInfo(59f, 1f, false, 3);
        GenerateEnemyInfo(59f, 0, true, 4);
        GenerateEnemyInfo(61f, 0, true, 4);
        GenerateEnemyInfo(65f, 1f, false, 1);
        GenerateEnemyInfo(67f, -1f, false, 1);
        GenerateEnemyInfo(68f, 1f, true, 5);
        GenerateEnemyInfo(68f, -1f, true, 5);
        GenerateEnemyInfo(70f, 0, false, 2);
        GenerateEnemyInfo(72f, 0, true, 2);
        GenerateEnemyInfo(74f, -1f, false, 3);
        GenerateEnemyInfo(75f, -1f, false, 3);
        GenerateEnemyInfo(76f, -1f, false, 6);
        GenerateEnemyInfo(78f, 0, false, 2);
        GenerateEnemyInfo(79f, 0, false, 4);
        GenerateEnemyInfo(80f, 0, true, 4);
        GenerateEnemyInfo(82f, 1f, false, 5);
        GenerateEnemyInfo(82f, -1f, false, 5);
        GenerateEnemyInfo(84f, 0, false, 2);
        GenerateEnemyInfo(86f, 0, false, 2);
        GenerateEnemyInfo(88f, 0, false, 2);
        GenerateEnemyInfo(90f, 0, false, 2);
        GenerateEnemyInfo(92f, 0, false, 2);
        GenerateEnemyInfo(97f, 1f, false, 5);
        GenerateEnemyInfo(99f, -1f, false, 5);
        GenerateEnemyInfo(101f, 0, true, 4);
        GenerateEnemyInfo(103, -1f, false, 1);
        GenerateEnemyInfo(105, 1f, false, 1);
        GenerateEnemyInfo(107, -1f, false, 1);
		GenerateEnemyInfo(109f, 0, false, 2);
		GenerateEnemyInfo(111f, 0, false, 2);
		GenerateEnemyInfo(113f, -1f, false, 3);
		GenerateEnemyInfo(115f, -1f, false, 3);
		GenerateEnemyInfo(117f, -1f, false, 6);
		GenerateEnemyInfo(121f, 1f, false, 1);
		GenerateEnemyInfo(123f, -1f, false, 1);
		GenerateEnemyInfo(125f, 0, false, 2);
		GenerateEnemyInfo(127f, 0, false, 2);
		GenerateEnemyInfo(131f, 0, false, 4);
		GenerateEnemyInfo(133f, 1f, false, 1);
		GenerateEnemyInfo(135f, 0, false, 2);
		GenerateEnemyInfo(137f, 0, true, 2);
		GenerateEnemyInfo(139f, 1f, true, 5);
		GenerateEnemyInfo(141f, 1f, false, 1);
		GenerateEnemyInfo(144f, -1f, false, 5);
		GenerateEnemyInfo(146f, 1f, false, 3);
		GenerateEnemyInfo(147f, 1f, false, 3);
		GenerateEnemyInfo(150f, 1f, false, 6);
		GenerateEnemyInfo(151f, 0, false, 2);
		GenerateEnemyInfo(153f, 0, true, 2);
		GenerateEnemyInfo(155f, 1f, false, 1);
		GenerateEnemyInfo(156f, -1f, false, 5);
        GenerateEnemyInfo(158f, 1f, false, 5);
        GenerateEnemyInfo(159f, -1f, false, 3);
        GenerateEnemyInfo(160f, -1f, true, 3);
        GenerateEnemyInfo(161f, -1f, false, 6);
        GenerateEnemyInfo(163f, 1f, false, 5);
        GenerateEnemyInfo(163f, -1f, false, 5);
        GenerateEnemyInfo(165f, 1f, true, 5);
        GenerateEnemyInfo(167f, 0, false, 4);
        GenerateEnemyInfo(169f, 1f, false, 5);
        GenerateEnemyInfo(171f, 0, true, 4);
        GenerateEnemyInfo(173f, 0, false, 4);

        GeneratePhaseInfo(192f, 1);
    }

    private void Update()
    {
        switch (phase)
        {
            case 1:
                if (timer <= 0f)
                {
                    timer = timeToGenerateEnemy;
                    enemyGenerator.GenerateEnemies8();
                    actualEnemies += 2;
                    if (actualEnemies >= totalEnemies8)
                    {
                        phase = 2;
                        background.SetPause(false);
                        timer = 0f;
                        actualEnemies = 0;
                    }
                }
                else
                {
                    timer -= Time.deltaTime;
                }
                break;
            case 2:
                if (background.IsOnEnd())
                {
                    phase = 3;
                    background.ResetBackground();
                    background.SetPause(true);
                    enemyGenerator.GenerateEnemy9();
                    //just 1 boss
                    actualEnemies = 1;
                }
                break;
            case 3:
                if(actualEnemies < 1)
                {
                    phase = 0;
                    background.SetPause(false);
                }
                break;
        }
    }

    void GenerateEnemyInfo(float newProgress, float posY, bool upgrade, int type)
    {
        
        float position = SquaresResolution.TotalSquaresX / 2f + newProgress * progress;
        info = Instantiate(enemyInfoPrefab) as GameObject;
        info.transform.position = new Vector2(position, posY);
        info.transform.SetParent(parent);
        EnemyData en = info.GetComponent<EnemyData>();
        en.SetType(type);
        en.SetUpgrade(upgrade);
    }

    void GeneratePhaseInfo(float newProgress, int phaseNumber)
    {
        float position = SquaresResolution.TotalSquaresX / 2f + newProgress * progress;
        info = Instantiate(phaseInfoPrefab) as GameObject;
        info.transform.position = new Vector2(position, 0f);
        info.transform.SetParent(parent);
        TriggerPhaseData pd = info.GetComponent<TriggerPhaseData>();
        pd.SetPhase(phaseNumber);
    }

    public void CheckPhaseData(Collider2D collision)
    {
        switch (collision.GetComponent<TriggerPhaseData>().GetPhase())
        {
            case 1:
                background.SetPause(true);
                phase = 1;
                break;
        }
    }
}
