using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class OnClickAction : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private GameObject _poolFolder;

	[SerializeField] private float _explosionForceMultiplier = 20f;
	[SerializeField] private float _explosionRadius = 2f;
	[SerializeField] private float _scaleMultiplier = 0.5f;
	[SerializeField] private float _divideChanceDivisor = 2f;

	[SerializeField] private float[] _newCubesAmountRange = new float[2] { 0.2f, 1 };
	[SerializeField] private float[] _positionOffsetRange = new float[2] { -0.5f, 0.5f };
	[SerializeField] private float[] _startTorqueRange = new float[2] { -300, 300 };

	private Ray _ray;
	private RaycastHit _hit;
	private float _divideChancePercent = 100f;

	public void SetSeparateChance(float separateChance)
	{
		_divideChancePercent = separateChance;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(_ray, out _hit, Mathf.Infinity) && _hit.transform == transform)
			{
				if (_divideChancePercent >= Random.Range(0f, 100f))
				{
					int newCubesAmount = (int)Random.Range(_newCubesAmountRange.Min(), _newCubesAmountRange.Max());

					SpawnNewCubes(newCubesAmount);
				}
				else 
					Explode();

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
			Rigidbody rb = clone.GetComponent<Rigidbody>();

			float offsetX = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
			float offsetY = Random.Range(0, _positionOffsetRange.Max());
			float offsetZ = Random.Range(_positionOffsetRange.Min(), _positionOffsetRange.Max());
			Vector3 spawnPosition = transform.position + new Vector3(offsetX, offsetY, offsetZ);

			Vector3 startTorque = new Vector3(Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
											Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()),
											Random.Range(_startTorqueRange.Min(), _startTorqueRange.Max()));

			Vector3 explosionForce = (spawnPosition - transform.position) * _explosionForceMultiplier;

			clone.transform.localScale = transform.localScale * _scaleMultiplier;
			clone.transform.position = spawnPosition;
			clone.transform.rotation = Quaternion.Euler(startTorque);


			rb.AddTorque(startTorque, ForceMode.Impulse);
			rb.AddForce(explosionForce, ForceMode.Impulse);
			clone.GetComponent<OnClickAction>().SetSeparateChance(_divideChancePercent / _divideChanceDivisor);
		}
	}

	private void Explode()
	{
		foreach (Transform child in _poolFolder.transform)
		{
			float explosionForce = _explosionForceMultiplier - (_explosionForceMultiplier * transform.lossyScale.x);
			float explosionRadius = _explosionRadius - (_explosionRadius * transform.lossyScale.x);

			child.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier: 0.5f, ForceMode.Impulse);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(_ray.origin, _ray.direction * 100);
	}
}
