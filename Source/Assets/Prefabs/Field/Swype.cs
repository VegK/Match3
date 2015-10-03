using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

public class Swype : MonoBehaviour
{
	#region Properties
	#region Public
	public float Sensitivity = 20f;
	public SwypeEvent OnSwypeEvent;
	#endregion
	#region Private
	private Vector3 _startMousePosition;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
			_startMousePosition = Input.mousePosition;
		else if (Input.GetMouseButtonUp(0))
		{
			var diff = Input.mousePosition - _startMousePosition;
			SwypeDirection? direction = null;

			if (diff.y > diff.x && diff.y > Sensitivity)
			{
				direction = SwypeDirection.Top;
			}
			else if (diff.x > -diff.y && diff.x > Sensitivity)
			{
				direction = SwypeDirection.Right;
			}
			else if (diff.y < diff.x && diff.y < -Sensitivity)
			{
				direction = SwypeDirection.Bottom;
			}
			else if (diff.x < -diff.y && diff.x < -Sensitivity)
			{
				direction = SwypeDirection.Left;
			}

			if (direction.HasValue)
				OnSwypeEvent.Invoke(direction.Value);
		}
	}
	#endregion
	#endregion

	[Serializable]
	public class SwypeEvent : UnityEvent<SwypeDirection> { }

	/// <summary>
	/// Направление свайпа.
	/// </summary>
	public enum SwypeDirection
	{
		/// <summary>
		/// Вверх.
		/// </summary>
		Top,
		/// <summary>
		/// Вправо.
		/// </summary>
		Right,
		/// <summary>
		/// Вниз.
		/// </summary>
		Bottom,
		/// <summary>
		/// Влево.
		/// </summary>
		Left
	}
}