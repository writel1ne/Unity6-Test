using UnityEngine;

public class CubeData : MonoBehaviour
{
    [SerializeField] private float _divideChance = 100;

    public void SetDivideChance(float chance)
    {
        _divideChance = chance;
    }

    public float GetDivideChance()
    {
        return _divideChance;
	}
}
