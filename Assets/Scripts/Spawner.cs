using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
	[SerializeField] private PoolHolder _poolFolder;

	[SerializeField] private float[] _newCubesAmountRange = new float[2] { 2, 10 };

	public event UnityAction<GameObject> OnSpawn;

	public void TrySpawnNewCubes(GameObject cube)
	{
		cube.TryGetComponent(out CubeData cubeData);

		if (cubeData?.GetDivideChance() >= Random.Range(0f, 100f))
		{
			GameObject clone;
			int newCubesAmount = (int)Random.Range(_newCubesAmountRange.Min(), _newCubesAmountRange.Max());

			for (int i = 0; i < newCubesAmount; i++)
			{
				clone = Instantiate(cube, cube.transform.position, Quaternion.identity, _poolFolder.transform);
				clone.TryGetComponent(out CubeData cubeDataOfClone);
				cubeDataOfClone.Init(cube, cubeData);
			}
		}
		else
		{
			OnSpawn?.Invoke(cube);
		}

	}
}
