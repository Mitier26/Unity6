using System;
using System.Collections;
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

    public GameObject endExplosion;
    public bool battleEnding;
    public float timeToExplosionEnd;
    public float waitToEndLevel;

    public Transform theBoss;

    public int scoreValue = 5000;
    
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

        if (!battleEnding)
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
    }

    public void HurtBoss()
    {
        currentHealth--;
        UIManager.instance.bossSlider.value = currentHealth;
        
        if (currentHealth <= 0 && !battleEnding)
        {
            // Destroy(gameObject);
            // UIManager.instance.bossSlider.gameObject.SetActive(false);

            battleEnding = true;

            StartCoroutine(EndBattleCo());
        }
    }

    public IEnumerator EndBattleCo()
    {
        UIManager.instance.bossSlider.gameObject.SetActive(false);
        Instantiate(endExplosion, theBoss.position, theBoss.rotation);
        bossAnim.enabled = false;
        GameManager.instance.AddScore(scoreValue);

        yield return new WaitForSeconds(timeToExplosionEnd);
        
        theBoss.gameObject.SetActive(false);

        yield return new WaitForSeconds(waitToEndLevel);

        StartCoroutine(GameManager.instance.EndLevelCo());

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
