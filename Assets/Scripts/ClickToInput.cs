using UnityEngine;

public class ClickToInput : MonoBehaviour
{
	private Ray _ray;
	private Camera _camera;
	private Spawner _spawner;

	private void Start()
	{
		_camera = GetComponent<Camera>();
		_spawner = GetComponent<Spawner>();
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_ray = _camera.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(_ray, out RaycastHit _hit, Mathf.Infinity) && _hit.transform.gameObject.TryGetComponent(out CubeData cubeData))
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
