using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x020012F3 RID: 4851
public class FlexibleCursor : CursorControllerAbs
{
	// Token: 0x17001265 RID: 4709
	// (get) Token: 0x060099A1 RID: 39329 RVA: 0x003A3A8F File Offset: 0x003A1C8F
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x060099A2 RID: 39330 RVA: 0x003A4C14 File Offset: 0x003A2E14
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

	// Token: 0x060099A3 RID: 39331 RVA: 0x003A4D1E File Offset: 0x003A2F1E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		FlexibleCursor.SetCursor(FlexibleCursor.currentCursorType);
	}

	// Token: 0x060099A4 RID: 39332 RVA: 0x003A3D17 File Offset: 0x003A1F17
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060099A5 RID: 39333 RVA: 0x003A4D2A File Offset: 0x003A2F2A
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

	// Token: 0x060099A6 RID: 39334 RVA: 0x003A4D60 File Offset: 0x003A2F60
	public override void UpdateMoveSpeed()
	{
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 1000f * CursorControllerAbs.regularSpeed;
	}

	// Token: 0x060099A7 RID: 39335 RVA: 0x003A4D88 File Offset: 0x003A2F88
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

	// Token: 0x060099A8 RID: 39336 RVA: 0x003A4DDC File Offset: 0x003A2FDC
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

	// Token: 0x060099A9 RID: 39337 RVA: 0x003A4FAC File Offset: 0x003A31AC
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

	// Token: 0x060099AA RID: 39338 RVA: 0x003A5114 File Offset: 0x003A3314
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainCursorOs(Vector3 _newPos)
	{
		Vector3 vector = this.ConstrainToBounds(_newPos);
		vector.x = Mathf.Clamp(vector.x, 5f, (float)(Screen.width - 5));
		vector.y = Mathf.Clamp(vector.y, 5f, (float)(Screen.height - 5));
		return vector;
	}

	// Token: 0x060099AB RID: 39339 RVA: 0x003A5168 File Offset: 0x003A3368
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainToBounds(Vector3 _newPosition)
	{
		Vector3 vector = _newPosition;
		vector.z = this.currentBounds.center.z;
		return this.currentBounds.ClosestPoint(vector);
	}

	// Token: 0x060099AC RID: 39340 RVA: 0x003A519C File Offset: 0x003A339C
	public override Vector2 GetScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x060099AD RID: 39341 RVA: 0x003A519C File Offset: 0x003A339C
	public override Vector2 GetLocalScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x060099AE RID: 39342 RVA: 0x003A51A3 File Offset: 0x003A33A3
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		MouseLib.SetCursorPosition((int)(_newPosition.x + 0.5f), (int)(_newPosition.y + 0.5f));
	}

	// Token: 0x060099AF RID: 39343 RVA: 0x003A4408 File Offset: 0x003A2608
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x060099B0 RID: 39344 RVA: 0x003A51C4 File Offset: 0x003A33C4
	public override void ResetToCenter()
	{
		this.SetScreenPosition((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	// Token: 0x060099B1 RID: 39345 RVA: 0x0008806C File Offset: 0x0008626C
	public override void SetNavigationTarget(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B2 RID: 39346 RVA: 0x0008806C File Offset: 0x0008626C
	public override void SetNavigationTargetLater(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B3 RID: 39347 RVA: 0x0008806C File Offset: 0x0008626C
	public override void ResetNavigationTarget()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B4 RID: 39348 RVA: 0x0008806C File Offset: 0x0008626C
	public override void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B5 RID: 39349 RVA: 0x0008806C File Offset: 0x0008626C
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B6 RID: 39350 RVA: 0x0008806C File Offset: 0x0008626C
	public override void RefreshSelection()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099B7 RID: 39351 RVA: 0x003A51DC File Offset: 0x003A33DC
	public override void SetCursorHidden(bool _hidden)
	{
		GameManager.Instance.SetCursorEnabledOverride(_hidden, false);
	}

	// Token: 0x060099B8 RID: 39352 RVA: 0x003A51EA File Offset: 0x003A33EA
	public override bool GetCursorHidden()
	{
		return GameManager.Instance.GetCursorEnabledOverride();
	}

	// Token: 0x060099B9 RID: 39353 RVA: 0x0008806C File Offset: 0x0008626C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060099BA RID: 39354 RVA: 0x003A51F6 File Offset: 0x003A33F6
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

	// Token: 0x060099BB RID: 39355 RVA: 0x003A51FE File Offset: 0x003A33FE
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

	// Token: 0x060099BC RID: 39356 RVA: 0x003A5230 File Offset: 0x003A3430
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

	// Token: 0x0400737C RID: 29564
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x0400737D RID: 29565
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 1000f;

	// Token: 0x0400737E RID: 29566
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorResource = "@:Textures/UI/cursor01.tga";

	// Token: 0x0400737F RID: 29567
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultMouseCursorCenter = Vector2.zero;

	// Token: 0x04007380 RID: 29568
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorResource = "@:Textures/UI/soft_cursor";

	// Token: 0x04007381 RID: 29569
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultControllerCursorCenter = new Vector2(16f, 16f);

	// Token: 0x04007382 RID: 29570
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorResource = "@:Textures/UI/map_cursor.tga";

	// Token: 0x04007383 RID: 29571
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 mapCursorCenter = new Vector2(16f, 16f);

	// Token: 0x04007384 RID: 29572
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D emptyCursor;

	// Token: 0x04007385 RID: 29573
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultControllerCursor;

	// Token: 0x04007386 RID: 29574
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultMouseCursor;

	// Token: 0x04007387 RID: 29575
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D mapCursor;

	// Token: 0x04007388 RID: 29576
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D currentCursorTexture;

	// Token: 0x04007389 RID: 29577
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 currentCursorHotspot;

	// Token: 0x0400738A RID: 29578
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;

	// Token: 0x0400738B RID: 29579
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Coroutine cursorUpdateCo;

	// Token: 0x0400738C RID: 29580
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x0400738D RID: 29581
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x0400738E RID: 29582
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x0400738F RID: 29583
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x04007390 RID: 29584
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x04007391 RID: 29585
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x04007392 RID: 29586
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04007393 RID: 29587
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int CONTROLLER_CURSOR_MOVEMENT_LIMIT = 5;
}
