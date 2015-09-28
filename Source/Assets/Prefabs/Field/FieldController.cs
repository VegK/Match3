using UnityEngine;
using System.Collections;
using System;

public class FieldController : MonoBehaviour
{
	#region Properties
	#region Public
	[Header("Prefabs")]
	public CellController PrefabCell;
	public GameObject PrefabElement;

	[Header("Параметры поля")]
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
	/// Проверить параметры поля.
	/// </summary>
	/// <returns>Возвращает true, если параметры в порядке.</returns>
	public bool CheckParams()
	{
		var res = true;
		res &= (_cells != null);
		return res;
	}
	/// <summary>
	/// Создать новое поле.
	/// </summary>
	public void CreateField()
	{
		ClearField();

		_cells = new CellController[Width, Height];

        for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var obj = CreateCell(x, y);
				_cells[x, y] = obj;
			}
		}
	}
	/// <summary>
	/// Заполнить пустые элементы поля ячейками.
	/// </summary>
	public void FillField()
	{
		for (int x = 0; x < Width; x++)
		{
			for (int y = 0; y < Height; y++)
			{
				var cell = _cells[x, y];
				if (cell == null)
				{
					var obj = CreateCell(x, y);
					_cells[x, y] = obj;
				}
			}
		}
	}
	/// <summary>
	/// Изменить размер поля.
	/// </summary>
	/// <param name="clear">Очистить поле.</param>
	public void ResizeField(bool clear)
	{
		if (clear)
			ClearField();

		// Уничтожаем лишние объекты.
		if (Width < _cells.GetLength(0))
		{
			for (int x = Width; x < _cells.GetLength(0); x++)
				for (int y = 0; y < _cells.GetLength(1); y++)
					if (_cells[x, y] != null)
						DestroyImmediate(_cells[x, y].gameObject);
		}

		if (Height < _cells.GetLength(1))
		{
			for (int y = Height; y < _cells.GetLength(1); y++)
				for (int x = 0; x < _cells.GetLength(0); x++)
					if (_cells[x, y] != null)
						DestroyImmediate(_cells[x, y].gameObject);
		}



		// Формируем новое поле.
		var newCells = new CellController[Width, Height];

		// Создаём новые ячейки, если новый размер поля больше старого.
		if (Width > _cells.GetLength(0))
		{
			for (int x = _cells.GetLength(0); x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (newCells[x, y] == null)
						newCells[x, y] = CreateCell(x, y);
		}

		if (Height > _cells.GetLength(1))
		{
			for (int y = _cells.GetLength(1); y < Height; y++)
				for (int x = 0; x < Width; x++)
					if (newCells[x, y] == null)
						newCells[x, y] = CreateCell(x, y);
		}

		// Переносим данные в новое поле.
		var minWidht = Width;
		if (_cells.GetLength(0) < Width)
			minWidht = _cells.GetLength(0);

		var minHeight = Height;
		if (_cells.GetLength(1) < Height)
			minHeight = _cells.GetLength(1);

		for (int x = 0; x < minWidht; x++)
			for (int y = 0; y < minHeight; y++)
				newCells[x, y] = _cells[x, y];
		_cells = newCells;
	}
	/// <summary>
	/// Восстановить положения ячеек.
	/// </summary>
	public void RestorePositionCells()
	{
		if (_cells == null)
			return;

		for (int x = 0; x < _cells.GetLength(0); x++)
		{
			for (int y = 0; y < _cells.GetLength(1); y++)
			{
				var cell = _cells[x, y];
				if (cell != null)
					cell.gameObject.transform.position = new Vector2(x, y);
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
				_cells[x, y] = obj;
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