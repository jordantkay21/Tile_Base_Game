using System;
using UnityEngine;
using Sirenix.OdinInspector;

[DefaultExecutionOrder(-100)]
public class ResourceManager : MonoBehaviour
{
    public enum ResourceType
    {
        Wood,
        Stone,
        Coin
    }

    public static ResourceManager Instance;

    [SerializeField] int wood;
    [SerializeField] int stone;
    [SerializeField] int coin;

    public event Action<int, int, int> OnResourceUpdated;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        OnResourceUpdated?.Invoke(wood, stone, coin);
    }

    [Button("Add Resource")]
    public void AddResource(ResourceType resource, int amount)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                wood += amount;
                break;
            case ResourceType.Stone:
                stone += amount;
                break;
            case ResourceType.Coin:
                coin += amount;
                break;
            default:
                Debug.LogWarning("Invalid Resource Type");
                return;
        }
        OnResourceUpdated?.Invoke(wood, stone, coin);
    }

    [Button("Spend Resource")]
    public bool SpendResource(ResourceType resource, int amount)
    {
        switch (resource)
        {
            case ResourceType.Wood:
                if (wood >= amount)
                {
                    wood -= amount;
                    OnResourceUpdated?.Invoke(wood, stone, coin);
                    return true;
                }
                break;
            case ResourceType.Stone:
                if (stone >= amount)
                {
                    stone -= amount;
                    OnResourceUpdated?.Invoke(wood, stone, coin);
                    return true;
                }
                break;
            case ResourceType.Coin:
                if (coin >= amount)
                {
                    coin -= amount;
                    OnResourceUpdated?.Invoke(wood, stone, coin);
                    return true;
                }
                break;
            default:
                Debug.LogWarning("Invalid resource type");
                return false;
        }
        return false;
    }
    public (int wood, int stone, int coin) GetResources()
    {
        return (wood, stone, coin);
    }
}
