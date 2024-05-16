using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]

public class StartColorSetter : MonoBehaviour
{
	private MeshRenderer _mesh;

	private void Start()
	{
		_mesh = GetComponent<MeshRenderer>();

		float r = Random.Range(0f, 1f);
		float g = Random.Range(0f, 1f);
		float b = Random.Range(0f, 1f);

		_mesh.material.color = new Color(r, g, b);
	}
}
