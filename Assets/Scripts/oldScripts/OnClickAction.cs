using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class OnClickAction : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private PoolHolder _poolFolder;

	[SerializeField] private float _maxExplosionForce = 20f;
	[SerializeField] private float _maxExplosionRadius = 15f;
	[SerializeField] private float _scaleMultiplier = 0.5f;
	[SerializeField] private float _divideChanceDivisor = 2f;

	[SerializeField] private float[] _newCubesAmountRange = new float[2] { 2, 10 };
	[SerializeField] private float[] _positionOffsetRange = new float[2] { -0.5f, 0.5f };
	[SerializeField] private float[] _startTorqueRange = new float[2] { -150, 150 };

	private Ray _ray;
	private RaycastHit _hit;
	private float _divideChancePercent = 100f;

	public void SetDivideChance(float divideChance)
	{
		_divideChancePercent = divideChance;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetMouseButtonDown(0))
		{
			_ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (Physics.Raycast(_ray, out _hit, Mathf.Infinity) && _hit.transform == transform)
			{
				if (_divideChancePercent >= Random.Range(0f, 100f))
				{
					int newCubesAmount = (int)Random.Range(_newCubesAmountRange.Min(), _newCubesAmountRange.Max());

					SpawnNewCubes(newCubesAmount);
				}
				else
				{
					Explode();
				}

				Destroy(gameObject);
			}
		}
	}

	private void SpawnNewCubes(int amount)
	{
		GameObject clone;

		for (int i = 0; i < amount; i++)
		{
			clone = Instantiate(gameObject, _poolFolder.transform);

			clone.transform.localScale = transform.localScale * _scaleMultiplier;
			clone.TryGetComponent(out Rigidbody rigidbodyOfClone);

			SetNewPosition(clone);
			SetRandomTorque(rigidbodyOfClone);
			Explode(ref rigidbodyOfClone);

			clone.GetComponent<OnClickAction>().SetDivideChance(_divideChancePercent / _divideChanceDivisor);
		}
	}

	private void SetRandomTorque(Rigidbody cube)
	{
		var startTorque = new Vector3(Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
							Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()));

		cube.transform.rotation = Quaternion.Euler(startTorque);
		cube.AddTorque(startTorque, ForceMode.Impulse);
	}

	private void SetNewPosition(GameObject cube)
	{
		float offsetX = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
		float offsetY = Random.Range(0, _positionOffsetRange.Max());
		float offsetZ = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
		Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, offsetZ);

		cube.transform.position = spawnPosition;
	}

	private void Explode()
	{
		float explosionForce = _maxExplosionForce - (_maxExplosionForce * transform.lossyScale.x);
		float explosionRadius = _maxExplosionRadius - (_maxExplosionRadius * transform.lossyScale.x);

		var cubes = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (var cube in cubes)
		{
			cube.gameObject.TryGetComponent(out Rigidbody rigidbodyOfnearCubes);
			rigidbodyOfnearCubes?.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.01f, ForceMode.Impulse);
		}
	}
	private void Explode(ref Rigidbody cube)
	{
		float explosionForce = _maxExplosionForce - (_maxExplosionForce * transform.lossyScale.x);
		float explosionRadius = _maxExplosionRadius - (_maxExplosionRadius * transform.lossyScale.x);

		cube.AddExplosionForce(20, transform.position, 20, 0.05f, ForceMode.Impulse);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(_ray.origin, _ray.direction * 100);
	}
}
