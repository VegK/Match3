using UnityEngine;
using System.Collections;

public partial class FieldController
{
	#region Properties
	#region Public

	#endregion
	#region Private
	private ElementController[] _elements;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	/// <summary>
	/// Метод подготовки элементов.
	/// </summary>
	private void ElementsStart()
	{
		_elements = new ElementController[] { PrefabElement1, PrefabElement2, PrefabElement3, PrefabElement4, PrefabElement5, PrefabElement6, PrefabElement7 };
	}
	/// <summary>
	/// Создать случайный элемент.
	/// </summary>
	/// <returns></returns>
	private ElementController CreateElement()
	{
		var rnd = UnityEngine.Random.Range(0, _elements.Length);
		var element = _elements[rnd];
		var res = Instantiate(element);

		return res;
	}
	#endregion
	#endregion
}