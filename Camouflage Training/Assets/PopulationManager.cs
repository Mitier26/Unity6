using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject personPrefab;

    public int populationSize = 10;

    List<GameObject> population = new List<GameObject>();
    public static float elapsed = 0;
    int trialTime = 10;
    int generation = 1;     // 세대

    GUIStyle guiStyle = new GUIStyle();

    void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 200, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }

    void Start()
    {
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            go.GetComponent<DNA>().r = Random.Range(0f, 1f);
            go.GetComponent<DNA>().g = Random.Range(0f, 1f);
            go.GetComponent<DNA>().b = Random.Range(0f, 1f);
            go.GetComponent<DNA>().s = Random.Range(0.1f, 0.5f);
            population.Add(go);
        }
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > trialTime)
        {
            BreedNewPopulaation();
            elapsed = 0;
        }
    }

    void BreedNewPopulaation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();
        // List<GameObject> sortedList = population.OrderBydescending(o => o.GetComponent<DNA>().timeToDie).ToList();

        population.Clear();

        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f), 0);
        GameObject offsprint = Instantiate(personPrefab, pos, Quaternion.identity);
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();

        if (Random.Range(0, 10) < 5)
        {
            offsprint.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offsprint.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offsprint.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            offsprint.GetComponent<DNA>().s = Random.Range(0, 10) < 5 ? dna1.s : dna2.s;
        }
        else
        {
            offsprint.GetComponent<DNA>().r = Random.Range(0f, 1f);
            offsprint.GetComponent<DNA>().g = Random.Range(0f, 1f);
            offsprint.GetComponent<DNA>().b = Random.Range(0f, 1f);
            offsprint.GetComponent<DNA>().s = Random.Range(0.1f, 0.5f);
        }
        
        return offsprint;
    }
}
