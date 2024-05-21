using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
	[SerializeField] private PoolHolder _poolFolder;

	[SerializeField] private float _scaleMultiplier = 0.5f;
	[SerializeField] private float _maxStartImpulse = 20;
	[SerializeField] private float _divideChanceDivisor = 2;

	[SerializeField] private float[] _newCubesAmountRange = new float[2] { 2, 10 };
	[SerializeField] private float[] _positionOffsetRange = new float[2] { -0.5f, 0.5f };
	[SerializeField] private float[] _startTorqueRange = new float[2] { -150, 150 };

	public event UnityAction<GameObject> OnSpawn;

	public void TrySpawnNewCubes(GameObject cube)
	{
		cube.TryGetComponent(out CubeData cubeData);

		if (cubeData.GetDivideChance() >= Random.Range(0f, 100f))
		{
			GameObject clone;
			int newCubesAmount = (int)Random.Range(_newCubesAmountRange.Min(), _newCubesAmountRange.Max());

			for (int i = 0; i < newCubesAmount; i++)
			{
				clone = Instantiate(cube, cube.transform.position, Quaternion.identity, _poolFolder.transform);
				clone.TryGetComponent(out Rigidbody rigidbodyOfClone);
				clone.TryGetComponent(out CubeData cubeDataOfClone);

				cubeDataOfClone.SetDivideChance(cubeData.GetDivideChance() / _divideChanceDivisor);
				clone.transform.localScale *= _scaleMultiplier;
				SetNewPosition(rigidbodyOfClone);
				SetRandomTorque(rigidbodyOfClone);
			}
		}

		OnSpawn?.Invoke(cube);
	}

	private void SetRandomTorque(Rigidbody cube)
	{
		var startTorque = new Vector3(Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()));

		cube.transform.rotation = Quaternion.Euler(startTorque);
		cube.AddTorque(startTorque, ForceMode.Impulse);
	}

	private void SetNewPosition(Rigidbody cube)
	{
		float offsetX = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
		float offsetY = Random.Range(0, _positionOffsetRange.Max());
		float offsetZ = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
		Vector3 spawnPosition = cube.transform.position + new Vector3(offsetX, offsetY, offsetZ);

		cube.transform.position = spawnPosition;
	}
}
