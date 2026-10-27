using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x02001305 RID: 4869
public class FlexibleCursor : CursorControllerAbs
{
	// Token: 0x17001271 RID: 4721
	// (get) Token: 0x060099F9 RID: 39417 RVA: 0x003A5E93 File Offset: 0x003A4093
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x060099FA RID: 39418 RVA: 0x003A7018 File Offset: 0x003A5218
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.AwakeBase();
		this.speedMultiplier = this.speed;
		this.cursor = base.GetComponentInChildren<UISprite>();
		if (FlexibleCursor.defaultMouseCursor == null)
		{
			FlexibleCursor.emptyCursor = new Texture2D(32, 32, TextureFormat.ARGB32, false);
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					FlexibleCursor.emptyCursor.SetPixel(i, j, new Color(0f, 0f, 0f, 0.01f));
				}
			}
			FlexibleCursor.emptyCursor.Apply();
			FlexibleCursor.defaultMouseCursor = Resources.Load<Texture2D>(FlexibleCursor.defaultMouseCursorResource);
			FlexibleCursor.defaultControllerCursor = Resources.Load<Texture2D>(FlexibleCursor.defaultControllerCursorResource);
			FlexibleCursor.mapCursor = Resources.Load<Texture2D>(FlexibleCursor.mapCursorResource);
		}
		UISprite[] componentsInChildren = base.GetComponentsInChildren<UISprite>(true);
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].gameObject.SetActive(false);
		}
		PlatformManager.NativePlatform.Input.OnLastInputStyleChanged += this.OnLastInputStyleChanged;
		FlexibleCursor.SetCursor(FlexibleCursor.currentCursorType);
	}

	// Token: 0x060099FB RID: 39419 RVA: 0x003A7122 File Offset: 0x003A5322
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		FlexibleCursor.SetCursor(FlexibleCursor.currentCursorType);
	}

	// Token: 0x060099FC RID: 39420 RVA: 0x003A611B File Offset: 0x003A431B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060099FD RID: 39421 RVA: 0x003A712E File Offset: 0x003A532E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		IPlatform nativePlatform = PlatformManager.NativePlatform;
		if (((nativePlatform != null) ? nativePlatform.Input : null) != null)
		{
			PlatformManager.NativePlatform.Input.OnLastInputStyleChanged -= this.OnLastInputStyleChanged;
		}
		base.DestroyBase();
	}

	// Token: 0x060099FE RID: 39422 RVA: 0x003A7164 File Offset: 0x003A5364
	public override void UpdateMoveSpeed()
	{
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 1000f * CursorControllerAbs.regularSpeed;
	}

	// Token: 0x060099FF RID: 39423 RVA: 0x003A718C File Offset: 0x003A538C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (Application.isPlaying)
		{
			IPlatform nativePlatform = PlatformManager.NativePlatform;
			if (((nativePlatform != null) ? nativePlatform.Input : null) != null && PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard && this.SoftcursorAllowed)
			{
				this.HandleControllerInput();
			}
			this.LastFrameTime = Time.realtimeSinceStartup;
		}
	}

	// Token: 0x06009A00 RID: 39424 RVA: 0x003A71E0 File Offset: 0x003A53E0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleControllerInput()
	{
		if (this.guiActions == null)
		{
			return;
		}
		Vector2 vector = new Vector2(this.guiActions.Right.RawValue - this.guiActions.Left.RawValue, this.guiActions.Up.RawValue - this.guiActions.Down.RawValue);
		float magnitude = vector.magnitude;
		Vector3 vector2 = this.GetScreenPosition();
		Vector3 vector3 = vector2;
		if (this.bHasHoverTarget && (this.HoverTarget == null || !this.HoverTarget.ColliderEnabled || !this.HoverTarget.IsActiveInHierarchy))
		{
			this.HoverTarget = null;
		}
		float num = (this.bHasHoverTarget ? CursorControllerAbs.hoverSpeed : 1f);
		this.currentAcceleration = Mathf.Clamp(this.currentAcceleration + magnitude * Time.unscaledDeltaTime, 0f, Mathf.Min(magnitude, num));
		this.speedMultiplier = Mathf.MoveTowards(this.speedMultiplier, this.bHasHoverTarget ? CursorControllerAbs.hoverSpeed : 1f, Time.unscaledDeltaTime * (this.bHasHoverTarget ? 10f : 1f));
		float num2 = Time.unscaledDeltaTime * this.speed * this.speedMultiplier * this.accelerationCurve.Evaluate(this.currentAcceleration);
		vector.x *= num2;
		vector.y *= num2;
		vector3.x += vector.x;
		vector3.y += vector.y;
		if (CursorControllerAbs.bSnapCursor)
		{
			if (vector2 == vector3)
			{
				if (!this.snapped)
				{
					vector3 = this.SnapOs(vector3);
					this.snapped = true;
				}
			}
			else
			{
				this.snapped = false;
			}
		}
		vector3 = this.ConstrainCursorOs(vector3);
		this.SetScreenPosition(vector3.x, vector3.y);
	}

	// Token: 0x06009A01 RID: 39425 RVA: 0x003A73B0 File Offset: 0x003A55B0
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 SnapOs(Vector3 _newPos)
	{
		if (this.hoverTarget == null || !this.hoverTarget.IsActiveInHierarchy)
		{
			return _newPos;
		}
		if (this.cursorWorldBounds.extents.x > this.hoverTarget.ColliderBounds.extents.x - this.OffsetSnapBounds)
		{
			return this.uiCamera.cachedCamera.WorldToScreenPoint(this.hoverTarget.ColliderBounds.center);
		}
		Vector3 vector = this.hoverTarget.ColliderBounds.ClosestPoint(this.uiCamera.cachedCamera.ScreenToWorldPoint(_newPos));
		Vector3 vector2 = Vector3.right * this.cursorWorldBounds.extents.x;
		Vector3 vector3 = vector - vector2;
		Vector3 vector4 = vector + vector2;
		if (!this.hoverTarget.ColliderBounds.Contains(vector3))
		{
			vector = this.hoverTarget.ColliderBounds.ClosestPoint(vector3) + vector2;
		}
		else if (!this.hoverTarget.ColliderBounds.Contains(vector4))
		{
			vector = this.hoverTarget.ColliderBounds.ClosestPoint(vector4) - vector2;
		}
		vector.y = this.hoverTarget.ColliderBounds.center.y;
		return this.uiCamera.cachedCamera.WorldToScreenPoint(vector);
	}

	// Token: 0x06009A02 RID: 39426 RVA: 0x003A7518 File Offset: 0x003A5718
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainCursorOs(Vector3 _newPos)
	{
		Vector3 vector = this.ConstrainToBounds(_newPos);
		vector.x = Mathf.Clamp(vector.x, 5f, (float)(Screen.width - 5));
		vector.y = Mathf.Clamp(vector.y, 5f, (float)(Screen.height - 5));
		return vector;
	}

	// Token: 0x06009A03 RID: 39427 RVA: 0x003A756C File Offset: 0x003A576C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainToBounds(Vector3 _newPosition)
	{
		Vector3 vector = _newPosition;
		vector.z = this.currentBounds.center.z;
		return this.currentBounds.ClosestPoint(vector);
	}

	// Token: 0x06009A04 RID: 39428 RVA: 0x003A75A0 File Offset: 0x003A57A0
	public override Vector2 GetScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x06009A05 RID: 39429 RVA: 0x003A75A0 File Offset: 0x003A57A0
	public override Vector2 GetLocalScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x06009A06 RID: 39430 RVA: 0x003A75A7 File Offset: 0x003A57A7
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		MouseLib.SetCursorPosition((int)(_newPosition.x + 0.5f), (int)(_newPosition.y + 0.5f));
	}

	// Token: 0x06009A07 RID: 39431 RVA: 0x003A680C File Offset: 0x003A4A0C
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x06009A08 RID: 39432 RVA: 0x003A75C8 File Offset: 0x003A57C8
	public override void ResetToCenter()
	{
		this.SetScreenPosition((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	// Token: 0x06009A09 RID: 39433 RVA: 0x000882A0 File Offset: 0x000864A0
	public override void SetNavigationTarget(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0A RID: 39434 RVA: 0x000882A0 File Offset: 0x000864A0
	public override void SetNavigationTargetLater(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0B RID: 39435 RVA: 0x000882A0 File Offset: 0x000864A0
	public override void ResetNavigationTarget()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0C RID: 39436 RVA: 0x000882A0 File Offset: 0x000864A0
	public override void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0D RID: 39437 RVA: 0x000882A0 File Offset: 0x000864A0
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0E RID: 39438 RVA: 0x000882A0 File Offset: 0x000864A0
	public override void RefreshSelection()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A0F RID: 39439 RVA: 0x003A75E0 File Offset: 0x003A57E0
	public override void SetCursorHidden(bool _hidden)
	{
		GameManager.Instance.SetCursorEnabledOverride(_hidden, false);
	}

	// Token: 0x06009A10 RID: 39440 RVA: 0x003A75EE File Offset: 0x003A57EE
	public override bool GetCursorHidden()
	{
		return GameManager.Instance.GetCursorEnabledOverride();
	}

	// Token: 0x06009A11 RID: 39441 RVA: 0x000882A0 File Offset: 0x000864A0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009A12 RID: 39442 RVA: 0x003A75FA File Offset: 0x003A57FA
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator ApplyCursorChangeLater()
	{
		while (!Cursor.visible)
		{
			yield return null;
		}
		Cursor.SetCursor(FlexibleCursor.currentCursorTexture, FlexibleCursor.currentCursorHotspot, CursorMode.Auto);
		FlexibleCursor.cursorUpdateCo = null;
		yield break;
	}

	// Token: 0x06009A13 RID: 39443 RVA: 0x003A7602 File Offset: 0x003A5802
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SetCursorTexture(Texture2D _tex, Vector2 _hotspot)
	{
		if (_tex != FlexibleCursor.currentCursorTexture)
		{
			FlexibleCursor.currentCursorTexture = _tex;
			FlexibleCursor.currentCursorHotspot = _hotspot;
			if (FlexibleCursor.cursorUpdateCo == null)
			{
				FlexibleCursor.cursorUpdateCo = ThreadManager.StartCoroutine(FlexibleCursor.ApplyCursorChangeLater());
			}
		}
	}

	// Token: 0x06009A14 RID: 39444 RVA: 0x003A7634 File Offset: 0x003A5834
	public new static void SetCursor(CursorControllerAbs.ECursorType _cursorType)
	{
		FlexibleCursor.currentCursorType = _cursorType;
		switch (_cursorType)
		{
		case CursorControllerAbs.ECursorType.None:
			FlexibleCursor.SetCursorTexture(FlexibleCursor.emptyCursor, FlexibleCursor.mapCursorCenter);
			return;
		case CursorControllerAbs.ECursorType.Default:
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle == PlayerInputManager.InputStyle.Keyboard)
			{
				FlexibleCursor.SetCursorTexture(FlexibleCursor.defaultMouseCursor, FlexibleCursor.defaultMouseCursorCenter);
				return;
			}
			FlexibleCursor.SetCursorTexture(FlexibleCursor.defaultControllerCursor, FlexibleCursor.defaultControllerCursorCenter);
			return;
		case CursorControllerAbs.ECursorType.Map:
			FlexibleCursor.SetCursorTexture(FlexibleCursor.mapCursor, FlexibleCursor.mapCursorCenter);
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x040073BD RID: 29629
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x040073BE RID: 29630
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 1000f;

	// Token: 0x040073BF RID: 29631
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorResource = "@:Textures/UI/cursor01.tga";

	// Token: 0x040073C0 RID: 29632
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultMouseCursorCenter = Vector2.zero;

	// Token: 0x040073C1 RID: 29633
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorResource = "@:Textures/UI/soft_cursor";

	// Token: 0x040073C2 RID: 29634
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultControllerCursorCenter = new Vector2(16f, 16f);

	// Token: 0x040073C3 RID: 29635
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorResource = "@:Textures/UI/map_cursor.tga";

	// Token: 0x040073C4 RID: 29636
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 mapCursorCenter = new Vector2(16f, 16f);

	// Token: 0x040073C5 RID: 29637
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D emptyCursor;

	// Token: 0x040073C6 RID: 29638
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultControllerCursor;

	// Token: 0x040073C7 RID: 29639
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultMouseCursor;

	// Token: 0x040073C8 RID: 29640
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D mapCursor;

	// Token: 0x040073C9 RID: 29641
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D currentCursorTexture;

	// Token: 0x040073CA RID: 29642
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 currentCursorHotspot;

	// Token: 0x040073CB RID: 29643
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;

	// Token: 0x040073CC RID: 29644
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Coroutine cursorUpdateCo;

	// Token: 0x040073CD RID: 29645
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x040073CE RID: 29646
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x040073CF RID: 29647
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x040073D0 RID: 29648
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x040073D1 RID: 29649
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x040073D2 RID: 29650
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x040073D3 RID: 29651
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x040073D4 RID: 29652
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int CONTROLLER_CURSOR_MOVEMENT_LIMIT = 5;
}
