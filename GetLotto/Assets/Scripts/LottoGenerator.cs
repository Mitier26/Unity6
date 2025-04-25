using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LottoGenerator : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GenerateLottoNumbers());
    }

    private void OnEnable()
    {
        StartCoroutine(GenerateLottoNumbers());
    }

    IEnumerator GenerateLottoNumbers()
    {
        // 위치 정보 가져오기 (가능한 경우)
        yield return StartCoroutine(GetLocationSeed(seedFromLocation =>
        {
            // 현재 시간 기반 시드
            long timeSeed = DateTime.Now.Ticks;

            // 최종 시드 = 시간 + 위치 (또는 시간만)
            long finalSeed = timeSeed + seedFromLocation;
            System.Random random = new System.Random(finalSeed.GetHashCode());

            // 5세트 생성
            for (int i = 0; i < 5; i++)
            {
                List<int> numbers = GenerateUniqueLottoSet(random);
                string setOutput = $"Set {i + 1}: {string.Join(", ", numbers)}";
                Debug.Log(setOutput);
            }
        }));
    }

    List<int> GenerateUniqueLottoSet(System.Random random)
    {
        HashSet<int> lottoNumbers = new HashSet<int>();
        while (lottoNumbers.Count < 6)
        {
            int number = random.Next(1, 46); // 1 ~ 45
            lottoNumbers.Add(number);
        }

        // 정렬된 결과 반환
        List<int> result = new List<int>(lottoNumbers);
        result.Sort();
        return result;
    }

    IEnumerator GetLocationSeed(Action<long> callback)
    {
        if (!Input.location.isEnabledByUser)
        {
            callback(0); // 위치 정보 불가한 경우 fallback
            yield break;
        }

        Input.location.Start();
        int maxWait = 5;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            callback(0);
        }
        else
        {
            float lat = Input.location.lastData.latitude;
            float lon = Input.location.lastData.longitude;

            // 소수점 좌표를 시드로 활용 (간단하게 변환)
            long seedFromLocation = (long)(lat * 10000) + (long)(lon * 10000);
            callback(seedFromLocation);
        }

        Input.location.Stop();
    }
}
