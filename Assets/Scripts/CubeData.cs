using System.Linq;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CubeData))]
public class CubeData : MonoBehaviour
{
	[SerializeField] private float _divideChance = 100;
	[SerializeField] private float _scaleMultiplier = 0.5f;
	[SerializeField] private float _maxStartImpulse = 20;
	[SerializeField] private float _divideChanceDivisor = 2;

	[SerializeField] private float[] _positionOffsetRange = new float[2] { -0.5f, 0.5f };
	[SerializeField] private float[] _startTorqueRange = new float[2] { -150, 150 };

	private WaitForFixedUpdate _waitForUpdate = new WaitForFixedUpdate();
	private Rigidbody _myRigidbody;

	public void Init(GameObject parent, CubeData cubeDataOfParent)
	{
		_myRigidbody = GetComponent<Rigidbody>();

		_divideChance = (cubeDataOfParent.GetDivideChance() / _divideChanceDivisor);
		transform.localScale *= _scaleMultiplier;
		SetNewPosition(_myRigidbody);
		SetRandomTorque(_myRigidbody);
		SetStartImpulse(_myRigidbody, parent);
	}

	public float GetDivideChance()
	{
		return _divideChance;
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

	private void SetStartImpulse(Rigidbody cube, GameObject parent)
	{
		StartCoroutine(SetImpulse(cube, parent));
	}

	private IEnumerator SetImpulse(Rigidbody cube, GameObject parent)
	{
		yield return _waitForUpdate;

		float startImpulse = _maxStartImpulse - (_maxStartImpulse * parent.transform.lossyScale.x);

		cube.AddExplosionForce(startImpulse, parent.transform.position, 10, 0.1f, ForceMode.Impulse);

		Destroy(parent);
	}
}
