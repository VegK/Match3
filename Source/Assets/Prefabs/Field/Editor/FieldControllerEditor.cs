using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldController))]
public class FieldControllerEditor : Editor
{
	#region Properties
	#region Public

	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var field = target as FieldController;

		if (GUILayout.Button("Создать новое поле"))
			field.CreateField();
	}
	#endregion
	#region Private

	#endregion
	#endregion
}