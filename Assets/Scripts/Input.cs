using UnityEngine;

public class Input : MonoBehaviour
{

	private Ray _ray;
	private RaycastHit _hit;
	private Camera _camera;
	private Spawner _spawner;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		_spawner = GetComponent<Spawner>();
	}

	void Update()
	{
		if (UnityEngine.Input.GetMouseButtonDown(0))
		{
			_ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

			if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
			{
				GameObject hitedObject = _hit.rigidbody.gameObject;
				_spawner.TrySpawnNewCubes(hitedObject);
			}
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.DrawRay(_ray.origin, _ray.direction * 100);
	}
}
