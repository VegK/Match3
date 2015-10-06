using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
	#region Properties
	#region Public

	#endregion
	#region Private
	private ParticleSystem _particleSystem;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Start()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (_particleSystem != null && !_particleSystem.IsAlive())
			Destroy(gameObject);
    }
	#endregion
	#endregion
}