using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Spawner))]

public class Exploder : MonoBehaviour
{
	[SerializeField] private float _maxExplosionForce = 20f;
	[SerializeField] private float _maxExplosionRadius = 15f;

	private Spawner _spawner;
	private WaitForFixedUpdate _waitForUpdate = new WaitForFixedUpdate();
	public void ExecuteExplode(GameObject hitedCube)
	{
		StartCoroutine(ExplodeFrom(hitedCube));
	}

	private void Start()
	{
		TryGetComponent(out Spawner _spawner);
		_spawner.OnSpawn += ExecuteExplode;
	}

	private void OnDisable()
	{
		_spawner.OnSpawn -= ExecuteExplode;
	}

	private IEnumerator ExplodeFrom(GameObject hitedCube)
	{
		yield return _waitForUpdate;

		float explosionForce = _maxExplosionForce - (_maxExplosionForce * hitedCube.transform.lossyScale.x);
		float explosionRadius = _maxExplosionRadius - (_maxExplosionRadius * hitedCube.transform.lossyScale.x);
		var overlapedCubes = Physics.OverlapSphere(hitedCube.transform.position, explosionRadius);

		foreach (var cube in overlapedCubes)
		{
			cube.TryGetComponent(out Rigidbody rigidbodyOfOverlapedCube);
			rigidbodyOfOverlapedCube?.AddExplosionForce(explosionForce, hitedCube.transform.position, explosionRadius, 0.01f, ForceMode.Impulse);
		}

		Destroy(hitedCube);
	}
}
