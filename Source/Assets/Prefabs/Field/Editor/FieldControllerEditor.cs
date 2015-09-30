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
		var field = target as FieldController;

		if (GUILayout.Button("Восстановить связи с ячейками"))
			field.BindCells();

		base.OnInspectorGUI();

		if (GUILayout.Button("Создать новое поле"))
			field.CreateField();

		GUI.enabled = field.CheckParams();
		if (GUILayout.Button("Заполнить пустые элементы поля"))
			field.FillField();
		if (GUILayout.Button("Изменить размер поля"))
			field.ResizeField(false);
		GUI.enabled = true;

		// TODO: test
		GUILayout.Space(15);
		if (GUILayout.Button("Уничтожить элементы"))
		{
			field.CheckElementsDestroyed();
			field.DestroyElements();
		}
		if (GUILayout.Button("Опустить элементы"))
			field.LowerElements();
		if (GUILayout.Button("Создать новые элементы"))
			field.CreateElements();
	}
	#endregion
	#region Private

	#endregion
	#endregion
}