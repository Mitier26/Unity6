using System.Collections.Generic;
using UnityEngine;

public class TestCannonManager : MonoBehaviour
{
    [SerializeField] private List<TestCannonRotator> cannons;
    [SerializeField] private Material player1Material;
    [SerializeField] private Material player2Material;

    public void InitCannons(int numberOfCannons)
    {
        for (int i = 0; i < cannons.Count; i++)
        {
            var cannon = cannons[i];
            var playerType = (i % 2 == 0) ? TestCannonRotator.PlayerType.Player1 : TestCannonRotator.PlayerType.Player2;
            Material mat = playerType == TestCannonRotator.PlayerType.Player1 ? player1Material : player2Material;
            string tagName = playerType.ToString();
            string layerName = playerType.ToString();

            cannon.Init(playerType, mat, tagName, layerName);
            cannon.StartRotation();
        }
    }
}
