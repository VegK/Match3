using System;
using UnityEngine;

public class ElementController : MonoBehaviour
{
	#region Properties
	#region Public
	public ElementType Type;
	public NameAnimation AnimationNames;

	/// <summary>
	/// Элемент должен быть уничтожен.
	/// </summary>
	public bool MustByDestroyed { get; set; }
	#endregion
	#region Private
	private Vector3 _defaultLocalScale;
	private Animation _animation;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Запущена любая анимация?
	/// </summary>
	/// <returns></returns>
	public bool AnimationIsPlay()
	{
		var res = false;
		if (_animation != null)
			res = _animation.isPlaying;
		return res;
	}
	/// <summary>
	/// Остановить все анимации элемента с возвратом элемента в первоночальное состояние.
	/// </summary>
	public void StopAnimations()
	{
		if (_animation == null)
			return;
		_animation.Stop();
		transform.localScale = _defaultLocalScale;
    }
	/// <summary>
	/// Запустить анимацию выбранного элемента.
	/// </summary>
	public void AnimationSelected()
	{
		if (_animation != null)
			_animation.Play(AnimationNames.Selected);
    }
	/// <summary>
	/// Запустить анимацию движения элемента вниз.
	/// </summary>
	public void AnimationUp()
	{
		StopAnimations();
		PlayAnimationMove(AnimationDirection.Up);
	}
	/// <summary>
	/// Запустить анимацию движения элемента вправо.
	/// </summary>
	public void AnimationRight()
	{
		StopAnimations();
		PlayAnimationMove(AnimationDirection.Right);
	}
	/// <summary>
	/// Запустить анимацию движения элемента вниз.
	/// </summary>
	public void AnimationDown()
	{
		StopAnimations();
		PlayAnimationMove(AnimationDirection.Down);
    }
	/// <summary>
	/// Запустить анимацию движения элемента влево.
	/// </summary>
	public void AnimationLeft()
	{
		StopAnimations();
		PlayAnimationMove(AnimationDirection.Left);
	}
	#endregion
	#region Private
	private void Start()
	{
		_animation = GetComponent<Animation>();
		_defaultLocalScale = transform.localScale;
    }

	private void PlayAnimationMove(AnimationDirection direction)
	{
		if (_animation == null)
			return;

		var str = string.Empty;
		switch (direction)
		{
			case AnimationDirection.Up:
				str = AnimationNames.MoveUp;
				break;
			case AnimationDirection.Right:
				str = AnimationNames.MoveRight;
				break;
			case AnimationDirection.Down:
				str = AnimationNames.MoveDown;
				break;
			case AnimationDirection.Left:
				str = AnimationNames.MoveLeft;
				break;
		}
		if (!string.IsNullOrEmpty(str))
			_animation.Play(str);
	}
	#endregion
	#endregion

	[Serializable]
	public class NameAnimation
	{
		public string Selected = "Selected";
		[Header("Перемещения")]
		public string MoveUp = "MoveUp";
		public string MoveRight = "MoveRight";
		public string MoveDown = "MoveDown";
		public string MoveLeft = "MoveLeft";
	}
	/// <summary>
	/// Направление анимации.
	/// </summary>
	public enum AnimationDirection
	{
		Up,
		Right,
		Down,
		Left
	}
}