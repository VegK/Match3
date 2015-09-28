using UnityEngine;
using System.Collections;

#if (UNITY_EDITOR)
[ExecuteInEditMode]
public class FieldEditMode : MonoBehaviour
{
	#region Properties
	#region Public

	#endregion
	#region Private
	private FieldController _fieldController;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void OnEnable()
	{
		_fieldController = GetComponent<FieldController>();
    }
	private void Update()
	{
		if (_fieldController != null)
			_fieldController.RestorePositionCells();
    }
	#endregion
	#endregion
}
#endif