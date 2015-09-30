using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class FieldController
{
	#region Properties
	#region Public

	#endregion
	#region Private
	private CellController _firstSelected;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	/// <summary>
	/// Создать случайный элемент в ячейке.
	/// </summary>
	/// <param name="cell">Ячейка в которой будет элемент.</param>
	/// <returns>Созданный элемент.</returns>
	private ElementController CreateElement(CellController cell)
	{
		var rnd = UnityEngine.Random.Range(0, PrefabElements.Length);
		var element = PrefabElements[rnd];
		var res = Instantiate(element);

		cell.Element = res;

		var pos = Vector3.zero;
		pos.z = -1;
		res.transform.position = pos;
		res.transform.SetParent(cell.transform, false);
		return res;
	}
	/// <summary>
	/// Уничтожить элемент в ячейке.
	/// </summary>
	/// <param name="cell">Ячейка.</param>
	private void DestroyElement(CellController cell)
	{
		DestroyImmediate(cell.Element.gameObject);
		cell.Element = null;
		DestroyElementsCount++;
    }
	/// <summary>
	/// Поменять местами элементы в ячейках.
	/// </summary>
	/// <param name="cell1">Ячейка.</param>
	/// <param name="cell2">Ячейка.</param>
	private void SwapElements(CellController cell1, CellController cell2)
	{
		if (cell1.Element != null)
			cell1.Element.transform.SetParent(cell2.transform, false);
		if (cell2.Element != null)
			cell2.Element.transform.SetParent(cell1.transform, false);

		var element = cell1.Element;
		cell1.Element = cell2.Element;
		cell2.Element = element;
	}
	/// <summary>
	/// Уничтожить элементы с пометкой "Должен быть уничтожен".
	/// </summary>
	/// <param name="cell">Ячейка для проверки.</param>
	private void DestroyElementMustByDestroyed(CellController cell)
	{
		if (cell != null && cell.Element.MustByDestroyed)
			DestroyElement(cell);

		#region Горизонталь.
		// Вправо.
		for (int x = cell.X + 1; x < _cells.GetLength(0); x++)
		{
			var c = _cells[x, cell.Y];
			if (c != null && c.Element.MustByDestroyed)
				DestroyElement(c);
			else
				break;
		}
		// Влево.
		for (int x = cell.X - 1; x >= 0; x--)
		{
			var c = _cells[x, cell.Y];
			if (c != null && c.Element.MustByDestroyed)
				DestroyElement(c);
			else
				break;
		}
		#endregion
		#region Вертикаль.
		// Вверх.
		for (int y = cell.Y + 1; y < _cells.GetLength(1); y++)
		{
			var c = _cells[cell.X, y];
			if (c != null && c.Element.MustByDestroyed)
				DestroyElement(c);
			else
				break;
		}
		// Вниз.
		for (int y = cell.Y - 1; y >= 0; y--)
		{
			var c = _cells[cell.X, y];
			if (c != null && c.Element.MustByDestroyed)
				DestroyElement(c);
			else
				break;
		}
		#endregion
	}
	/// <summary>
	/// Проверить элемент на уничтожение.
	/// </summary>
	/// <param name="cell">Ячейка для проверки.</param>
	/// <returns>Список ячеек для уничтожения.</returns>
	private List<CellController> CheckElementDestroyed(CellController cell)
	{
		var res = new List<CellController>();
		var element = cell.Element;

		#region Горизонталь.
		var destroy = false;

		if (cell.X - 1 >= 0 && cell.X + 1 < _cells.GetLength(0) &&
			_cells[cell.X - 1, cell.Y] != null &&
			_cells[cell.X + 1, cell.Y] != null &&
			_cells[cell.X - 1, cell.Y].Element.Type == element.Type &&
			_cells[cell.X + 1, cell.Y].Element.Type == element.Type)
		{
			destroy = true;
		}
		else if (cell.X - 2 >= 0 &&
			_cells[cell.X - 1, cell.Y] != null &&
			_cells[cell.X - 2, cell.Y] != null &&
			_cells[cell.X - 1, cell.Y].Element.Type == element.Type &&
			_cells[cell.X - 2, cell.Y].Element.Type == element.Type)
		{
			destroy = true;
		}
		else if (cell.X + 2 < _cells.GetLength(0) &&
			_cells[cell.X + 1, cell.Y] != null &&
			_cells[cell.X + 2, cell.Y] != null &&
			_cells[cell.X + 1, cell.Y].Element.Type == element.Type &&
			_cells[cell.X + 2, cell.Y].Element.Type == element.Type)
		{
			destroy = true;
		}

		if (destroy)
		{
			// Вправо.
			for (int x = cell.X + 1; x < _cells.GetLength(0); x++)
			{
				var c = _cells[x, cell.Y];
				if (c != null && c.Element.Type == element.Type)
					res.Add(c);
				else
					break;
			}
			// Влево.
			for (int x = cell.X - 1; x >= 0; x--)
			{
				var c = _cells[x, cell.Y];
				if (c != null && c.Element.Type == element.Type)
					res.Add(c);
				else
					break;
			}
		}
		#endregion
		#region Вертикаль.
		destroy = false;

		if (cell.Y - 1 >= 0 && cell.Y + 1 < _cells.GetLength(1) &&
			_cells[cell.X, cell.Y - 1] != null &&
			_cells[cell.X, cell.Y + 1] != null &&
			_cells[cell.X, cell.Y - 1].Element.Type == element.Type &&
			_cells[cell.X, cell.Y + 1].Element.Type == element.Type)
		{
			destroy = true;
		}
		else if (cell.Y - 2 >= 0 &&
			_cells[cell.X, cell.Y - 1] != null &&
			_cells[cell.X, cell.Y - 2] != null &&
			_cells[cell.X, cell.Y - 1].Element.Type == element.Type &&
			_cells[cell.X, cell.Y - 2].Element.Type == element.Type)
		{
			destroy = true;
		}
		else if (cell.Y + 2 < _cells.GetLength(1) &&
			_cells[cell.X, cell.Y + 1] != null &&
			_cells[cell.X, cell.Y + 2] != null &&
			_cells[cell.X, cell.Y + 1].Element.Type == element.Type &&
			_cells[cell.X, cell.Y + 2].Element.Type == element.Type)
		{
			destroy = true;
		}

		if (destroy)
		{
			// Вверх.
			for (int y = cell.Y + 1; y < _cells.GetLength(1); y++)
			{
				var c = _cells[cell.X, y];
				if (c != null && c.Element.Type == element.Type)
					res.Add(c);
				else
					break;
			}
			// Вниз.
			for (int y = cell.Y - 1; y >= 0; y--)
			{
				var c = _cells[cell.X, y];
				if (c != null && c.Element.Type == element.Type)
					res.Add(c);
				else
					break;
			}
		}
		#endregion

		if (res.Count > 0)
			res.Add(cell);
		return res;
	}
	/// <summary>
	/// Выбрать элемент.
	/// </summary>
	/// <param name="cell">Ячейка.</param>
	private void SelectionElement(CellController selected)
	{
		if (_firstSelected == selected)
		{
			_firstSelected = null;
			return;
		}

		if (_firstSelected == null)
			_firstSelected = selected;
		else
		{
			var swap = false;
			if (selected.X <= _firstSelected.X + 1 &&
				selected.X >= _firstSelected.X - 1 &&
				selected.Y == _firstSelected.Y ||
				selected.Y <= _firstSelected.Y + 1 &&
				selected.Y >= _firstSelected.Y - 1 &&
				selected.X == _firstSelected.X)
				swap = true;
			if (swap)
			{
				SwapElements(selected, _firstSelected);

				var list = CheckElementDestroyed(selected);
				list.AddRange(CheckElementDestroyed(_firstSelected));
				if (list.Count > 0)
				{
					foreach (CellController c in list)
						DestroyElement(c);
					LowerElements();
					CreateElements();

					FullCheckField();
                }
				else
					SwapElements(_firstSelected, selected);
			}
			_firstSelected = null;
        }
    }
	#endregion
	#endregion
}