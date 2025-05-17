using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    public GameObject personPrefab;    // 개체 프리팹
    public int populationSize = 10;    // 한 세대 당 개체 수

    List<GameObject> population = new List<GameObject>();  // 현재 세대 개체들

    public static float elapsed = 0;   // 현재 시간 (전체 생존 시간 측정용)
    int trialTime = 10;                // 1세대당 평가 시간
    int generation = 1;                // 현재 세대 번호

    GUIStyle guiStyle = new GUIStyle();  // UI 스타일 설정

    void OnGUI()
    {
        // 화면 좌상단에 텍스트 출력 (세대 정보 및 경과 시간)
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 200, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 65, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }

    void Start()
    {
        // 첫 세대 개체 무작위 생성
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f), 0);
            GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);

            // 무작위 유전자 값 설정
            go.GetComponent<DNA>().r = Random.Range(0f, 1f);
            go.GetComponent<DNA>().g = Random.Range(0f, 1f);
            go.GetComponent<DNA>().b = Random.Range(0f, 1f);
            go.GetComponent<DNA>().s = Random.Range(0.1f, 0.5f);

            population.Add(go);
        }
    }

    void Update()
    {
        // 매 프레임마다 시간 증가
        elapsed += Time.deltaTime;

        // trialTime(10초) 초과 시 다음 세대로 진화
        if (elapsed > trialTime)
        {
            BreedNewPopulaation(); // 새로운 세대 생성
            elapsed = 0;
        }
    }

    void BreedNewPopulaation()
    {
        List<GameObject> newPopulation = new List<GameObject>();

        // 오래 살아남은 순으로 정렬
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeToDie).ToList();

        // 이전 세대 삭제 준비
        population.Clear();

        // 상위 절반에서 두 개씩 페어로 번식
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        // 이전 세대 제거
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }

        generation++;  // 세대 증가
    }

    // 자식 생성
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9f, 9f), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);

        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();

        // 50% 확률로 부모 유전자 혼합 / 나머지 50%는 완전 랜덤
        if (Random.Range(0, 10) < 5)
        {
            offspring.GetComponent<DNA>().r = Random.Range(0, 10) < 5 ? dna1.r : dna2.r;
            offspring.GetComponent<DNA>().g = Random.Range(0, 10) < 5 ? dna1.g : dna2.g;
            offspring.GetComponent<DNA>().b = Random.Range(0, 10) < 5 ? dna1.b : dna2.b;
            offspring.GetComponent<DNA>().s = Random.Range(0, 10) < 5 ? dna1.s : dna2.s;
        }
        else
        {
            offspring.GetComponent<DNA>().r = Random.Range(0f, 1f);
            offspring.GetComponent<DNA>().g = Random.Range(0f, 1f);
            offspring.GetComponent<DNA>().b = Random.Range(0f, 1f);
            offspring.GetComponent<DNA>().s = Random.Range(0.1f, 0.5f);
        }

        return offspring;
    }
}
