using System;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager instance;

    public string bossName;
    public int currentHealth = 100;

    // public BattleShot[] shotsToFire;

    public BattlePhase[] phases;
    public int currentPhase;
    public Animator bossAnim;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.bossName.text = bossName;
        UIManager.instance.bossSlider.maxValue = currentHealth;
        UIManager.instance.bossSlider.value = currentHealth;
        UIManager.instance.bossSlider.gameObject.SetActive(true);
        
        MusicController.instance.PlayBoos();
    }

    private void Update()
    {
        

        if (currentHealth <= phases[currentPhase].healthToEndPhase)
        {
            phases[currentPhase].removeAtPhaseEnd.SetActive(false);
            Instantiate(phases[currentPhase].addAtPhaseEnd, phases[currentPhase].newSpawnPoint.position, phases[currentPhase].newSpawnPoint.rotation);
            currentPhase++;
            
            bossAnim.SetInteger("Phase",2);
        }
        else
        {
            for (int i = 0; i < phases[currentPhase].phaseShots.Length; i++)
            {
                phases[currentPhase].phaseShots[i].shotCounter -= Time.deltaTime;
            
                if (phases[currentPhase].phaseShots[i].shotCounter <= 0)
                {
                    phases[currentPhase].phaseShots[i].shotCounter = phases[currentPhase].phaseShots[i].timeBetweenShots;
                    Instantiate(phases[currentPhase].phaseShots[i].theShot, phases[currentPhase].phaseShots[i].firePoint.position, phases[currentPhase].phaseShots[i].firePoint.rotation);
                }
            }
        }
    }

    public void HurtBoss()
    {
        currentHealth--;
        UIManager.instance.bossSlider.value = currentHealth;
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            UIManager.instance.bossSlider.gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class BattleShot
{
    public GameObject theShot;
    public float timeBetweenShots;
    public float shotCounter;
    public Transform firePoint;
}

[Serializable]
public class BattlePhase
{
    public BattleShot[] phaseShots;
    public int healthToEndPhase;
    public GameObject removeAtPhaseEnd;
    public GameObject addAtPhaseEnd;
    public Transform newSpawnPoint;
}
