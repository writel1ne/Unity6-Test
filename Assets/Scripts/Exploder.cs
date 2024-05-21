using System.Collections;
using UnityEngine;

public class Exploder : MonoBehaviour
{
	[SerializeField] private float _maxExplosionForce = 20f;
	[SerializeField] private float _maxExplosionRadius = 15f;

	private Spawner _spawner;

	private void Start()
	{
		_spawner = GetComponent<Spawner>();
		_spawner.OnSpawn += ExecuteExplode;
	}

	private void OnEnable()
	{
		_spawner.OnSpawn += ExecuteExplode;
	}

	private void OnDisable()
	{
		_spawner.OnSpawn -= ExecuteExplode;
	}

	public void ExecuteExplode(GameObject hitedCube)
	{
		StartCoroutine(ExplodeFrom(hitedCube));
	}

	IEnumerator ExplodeFrom(GameObject hitedCube)
	{
		yield return new WaitForFixedUpdate();

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
