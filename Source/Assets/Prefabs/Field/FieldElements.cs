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
	/// <summary>
	/// Отслеживание свайпа.
	/// </summary>
	/// <param name="direction">Направление свайпа.</param>
	public void SwypeElement(Swype.SwypeDirection direction)
	{
		if (_firstSelected == null)
			return;

		var x = _firstSelected.X;
		var y = _firstSelected.Y;
		CellController secondSelected = null;
		switch (direction)
		{
			case Swype.SwypeDirection.Top:
				if (y < Height - 1)
					secondSelected = _cells[x, y + 1];
				break;
			case Swype.SwypeDirection.Right:
				if (x < Width - 1)
					secondSelected = _cells[x + 1, y];
				break;
			case Swype.SwypeDirection.Bottom:
				if (y > 0)
					secondSelected = _cells[x, y - 1];
				break;
			case Swype.SwypeDirection.Left:
				if (x > 0)
					secondSelected = _cells[x - 1, y];
				break;
		}

		StartCoroutine(	SelectionElement(secondSelected));
    }
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
	private IEnumerator SelectionElement(CellController selected)
	{
		if (_fixedField > 0)
			yield break;

		if (_firstSelected == selected || selected == null)
		{
			_firstSelected.Element.StopAnimations();
			_firstSelected = null;
			yield break;
		}

		if (_firstSelected == null)
		{
			_firstSelected = selected;
			_firstSelected.Element.AnimationSelected();
        }
		else
		{
			_firstSelected.Element.StopAnimations();
			selected.Element.StopAnimations();

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
				yield return StartCoroutine(PlaySwapAnimation(selected, _firstSelected));
				SwapElements(selected, _firstSelected);

				var dest = (CheckElementDestroyed(selected).Count > 0);
				if (!dest)
					dest = (CheckElementDestroyed(_firstSelected).Count > 0);
				if (dest)
				{
					StartCoroutine(FullCheckField());
				}
				else
				{
					yield return StartCoroutine(PlaySwapAnimation(selected, _firstSelected));
					SwapElements(_firstSelected, selected);
				}
			}

			_firstSelected = null;
		}
	}
	/// <summary>
	/// Запустить анимацию смены местами элементов в ячейках.
	/// </summary>
	/// <param name="cell1">Ячейка.</param>
	/// <param name="cell2">Ячейка.</param>
	private IEnumerator PlaySwapAnimation(CellController cell1, CellController cell2)
	{
		_fixedField++;

        if (cell1.Y == cell2.Y)
		{
			if (cell1.X > cell2.X)
			{
				cell1.Element.AnimationLeft();
				cell2.Element.AnimationRight();
			}
			else
			{
				cell1.Element.AnimationRight();
				cell2.Element.AnimationLeft();
			}
		}
		else
		{
			if (cell1.Y > cell2.Y)
			{
				cell1.Element.AnimationDown();
				cell2.Element.AnimationUp();
			}
			else
			{
				cell1.Element.AnimationUp();
				cell2.Element.AnimationDown();
			}
		}

		// Ждём окончания анимаций.
		while (cell1.Element.AnimationIsPlay() && cell2.Element.AnimationIsPlay())
			yield return null;

		cell1.Element.transform.localPosition = new Vector3(0, 0, -1);
		cell2.Element.transform.localPosition = new Vector3(0, 0, -1);

		_fixedField--;
    }
	/// <summary>
	/// Опустить элемент если под ним пустая ячейка.
	/// </summary>
	/// <param name="cell">Ячейка с элементом.</param>
	private void LowerElement(CellController cell)
	{
		var x = cell.X;
		for (int y = cell.Y - 1; y >= 0; y--)
		{
			var cellY = _cells[x, y];
			if (cellY == null)
				continue;
			if (cellY.Element == null)
				StartCoroutine(LowerColumn(cell, cellY));
			break;
		}
	}
	/// <summary>
	/// Опустить элементы в столбце включая текущий и выше.
	/// </summary>
	/// <param name="cell">Ячейка содержащая элемент для опускания.</param>
	/// <param name="toCell">Куда опускать.</param>
	private IEnumerator LowerColumn(CellController cell, CellController toCell)
	{
		var x = cell.X;

		_fixedField++;
		while (_fixedColumn[x] > 0)
			yield return null;

		var prevCell = toCell;
        for (int y = cell.Y; y < _cells.GetLength(1); y++)
		{
			var cellY = _cells[x, y];
			if (cellY == null)
				continue;
			if (cellY.Element == null)
				break;

			StartCoroutine(LowerColumnElement(cellY, prevCell));
			prevCell = cellY;
		}
		_fixedField--;
	}
	/// <summary>
	/// Опустить элемент до дна.
	/// </summary>
	/// <param name="cell">Элемент.</param>
	/// <param name="toCell">Свободная ячейка под элементом.</param>
	private IEnumerator LowerColumnElement(CellController cell, CellController toCell)
	{
		_fixedField++;
		_fixedColumn[cell.X]++;

		cell.Element.AnimationLower();
		while (cell.Element.AnimationIsPlay())
			yield return null;

		_fixedField--;
		_fixedColumn[cell.X]--;

		cell.Element.transform.localPosition = new Vector3(0, 0, -1);
		SwapElements(toCell, cell);

		LowerElement(toCell);
	}
	#endregion
	#endregion
}