using System.Collections;
using UnityEngine;

public class CellController : MonoBehaviour
{
	#region Properties
	#region Public
	[HideInInspector]
	public int X;
	[HideInInspector]
	public int Y;

	public ElementController Element { get; set; }
	/// <summary>
	/// Событие срабатывающее при выборе ячейки.
	/// </summary>
	public event ChangeHandler OnSelection;
	#endregion
	#region Private

	#endregion
	#endregion

	#region Methods
	#region Public
	public void OnMouseDown()
	{
		if (OnSelection != null)
			OnSelection(this);
	}
	#endregion
	#region Private

	#endregion
	#endregion

	public delegate void ChangeHandler(CellController element);
}