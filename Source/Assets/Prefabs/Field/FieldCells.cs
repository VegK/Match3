using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class FieldController
{
	#region Properties
	#region Public

	#endregion
	#region Private
	private CellController[,] _cells;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Восстановить игровые объекты ячеек.
	/// </summary>
	public void RestoreGameObjectCells()
	{
		if (_cells == null)
			return;

		for (int x = 0; x < _cells.GetLength(0); x++)
		{
			for (int y = 0; y < _cells.GetLength(1); y++)
			{
				var cell = _cells[x, y];
				if (cell != null)
				{
					var tr = cell.gameObject.transform;
					var trPref = PrefabCell.transform;
					tr.localPosition = new Vector2(x, y);
					tr.localRotation = trPref.localRotation;
					tr.localScale = trPref.localScale;
				}
			}
		}
	}
	/// <summary>
	/// Восстановить связь с клетками.
	/// </summary>
	public void BindCells()
	{
		if (_cells == null)
			_cells = new CellController[Width, Height];

		var objs = gameObject.GetComponentsInChildren<CellController>();
		foreach (CellController obj in objs)
		{
			var x = obj.X;
			var y = obj.Y;
			var addObj = false;

			if (x < _cells.GetLength(0) && y < _cells.GetLength(1))
				addObj = true;
			if (addObj)
				if (_cells[x, y] == null || _cells[x, y] == obj)
					addObj = true;

			if (addObj)
			{
				_cells[x, y] = obj;
				obj.OnSelection += new CellController.ChangeHandler(c => StartCoroutine(SelectionElement(c)));
			}
			else
				DestroyImmediate(obj.gameObject);
		}
	}
	#endregion
	#region Private
	/// <summary>
	/// Создать ячейку.
	/// </summary>
	/// <param name="x">Позиция по X.</param>
	/// <param name="y">Позиция по Y.</param>
	/// <returns>Ячейка.</returns>
	private CellController CreateCell(int x, int y)
	{
		var obj = Instantiate(PrefabCell);
		obj.name = x + ":" + y;
		obj.transform.position = new Vector2(x, y);
		obj.transform.SetParent(transform);
		obj.X = x;
		obj.Y = y;
		return obj;
	}
	#endregion
	#endregion
}