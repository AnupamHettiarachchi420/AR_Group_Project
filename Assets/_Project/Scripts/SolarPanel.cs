using System.Resources;
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] float energyPerSecond = 1f;

    void Update()
    {
        ResourceManager.Instance.AddEnergy(energyPerSecond * Time.deltaTime);
    }
}