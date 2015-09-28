using UnityEngine;
using System.Collections;
using System;

public class FieldController : MonoBehaviour
{
	#region Properties
	#region Public
	public CellController PrefabCell;
	public GameObject PrefabElement;

	public int Width = 5;
	public int Height = 5;
	#endregion
	#region Private
	private CellController[,] _cells;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Создать новое поле.
	/// </summary>
	public void CreateField()
	{
		if (_cells == null)
			_cells = new CellController[Width, Height];

		ClearField();

        for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var obj = Instantiate(PrefabCell);
				obj.name = x + ":" + y;
				obj.transform.position = new Vector2(x, y);
				obj.transform.SetParent(transform);
				obj.X = x;
				obj.Y = y;
				_cells[x, y] = obj;
			}
		}
	}
	/// <summary>
	/// Восстановить положения ячеек.
	/// </summary>
	public void RestorePositionCells()
	{
		if (_cells == null)
		{
			_cells = new CellController[Width, Height];
			BindCells();
		}

		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var cell = _cells[x, y];
				if (cell != null)
					cell.gameObject.transform.position = new Vector2(x, y);
			}
		}
	}
	#endregion
	#region Private
	/// <summary>
	/// Восстановить связь с клетками.
	/// </summary>
	private void BindCells()
	{
		var objs = gameObject.GetComponentsInChildren<CellController>();
		foreach (CellController obj in objs)
		{
			var x = obj.X;
			var y = obj.Y;

			if (_cells[x, y] == null)
				_cells[x, y] = obj;
			else
				DestroyImmediate(obj.gameObject);
		}
	}
	/// <summary>
	/// Уничтожить ячейки и очистить массив ячеек.
	/// </summary>
	private void ClearField()
	{
		var objs = gameObject.GetComponentsInChildren<CellController>();
		foreach (CellController obj in objs)
			DestroyImmediate(obj.gameObject);

		if (_cells != null)
			Array.Clear(_cells, 0, _cells.Length);
	}
	#endregion
	#endregion
}