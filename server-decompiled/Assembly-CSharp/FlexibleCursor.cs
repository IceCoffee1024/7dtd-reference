using System;
using System.Collections;
using Platform;
using UnityEngine;

// Token: 0x020012B9 RID: 4793
public class FlexibleCursor : CursorControllerAbs
{
	// Token: 0x170011E7 RID: 4583
	// (get) Token: 0x060097FA RID: 38906 RVA: 0x00399C9B File Offset: 0x00397E9B
	public bool SoftcursorAllowed
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return this.guiActions != null && this.guiActions.Enabled && LocalPlayerUI.AnyModalWindowOpen();
		}
	}

	// Token: 0x060097FB RID: 38907 RVA: 0x0039AE14 File Offset: 0x00399014
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

	// Token: 0x060097FC RID: 38908 RVA: 0x0039AF1E File Offset: 0x0039911E
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnLastInputStyleChanged(PlayerInputManager.InputStyle _style)
	{
		FlexibleCursor.SetCursor(FlexibleCursor.currentCursorType);
	}

	// Token: 0x060097FD RID: 38909 RVA: 0x00399F23 File Offset: 0x00398123
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.uiCamera = base.GetComponentInParent<UICamera>();
		this.UpdateMoveSpeed();
		base.InitCursorBounds();
	}

	// Token: 0x060097FE RID: 38910 RVA: 0x0039AF2A File Offset: 0x0039912A
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

	// Token: 0x060097FF RID: 38911 RVA: 0x0039AF60 File Offset: 0x00399160
	public override void UpdateMoveSpeed()
	{
		CursorControllerAbs.regularSpeed = GamePrefs.GetFloat(EnumGamePrefs.OptionsInterfaceSensitivity);
		this.speed = 500f + 1000f * CursorControllerAbs.regularSpeed;
	}

	// Token: 0x06009800 RID: 38912 RVA: 0x0039AF88 File Offset: 0x00399188
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

	// Token: 0x06009801 RID: 38913 RVA: 0x0039AFDC File Offset: 0x003991DC
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

	// Token: 0x06009802 RID: 38914 RVA: 0x0039B1AC File Offset: 0x003993AC
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

	// Token: 0x06009803 RID: 38915 RVA: 0x0039B314 File Offset: 0x00399514
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainCursorOs(Vector3 _newPos)
	{
		Vector3 vector = this.ConstrainToBounds(_newPos);
		vector.x = Mathf.Clamp(vector.x, 5f, (float)(Screen.width - 5));
		vector.y = Mathf.Clamp(vector.y, 5f, (float)(Screen.height - 5));
		return vector;
	}

	// Token: 0x06009804 RID: 38916 RVA: 0x0039B368 File Offset: 0x00399568
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 ConstrainToBounds(Vector3 _newPosition)
	{
		Vector3 vector = _newPosition;
		vector.z = this.currentBounds.center.z;
		return this.currentBounds.ClosestPoint(vector);
	}

	// Token: 0x06009805 RID: 38917 RVA: 0x0039B39C File Offset: 0x0039959C
	public override Vector2 GetScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x06009806 RID: 38918 RVA: 0x0039B39C File Offset: 0x0039959C
	public override Vector2 GetLocalScreenPosition()
	{
		return MouseLib.GetLocalMousePosition();
	}

	// Token: 0x06009807 RID: 38919 RVA: 0x0039B3A3 File Offset: 0x003995A3
	public override void SetScreenPosition(Vector2 _newPosition)
	{
		MouseLib.SetCursorPosition((int)(_newPosition.x + 0.5f), (int)(_newPosition.y + 0.5f));
	}

	// Token: 0x06009808 RID: 38920 RVA: 0x0039A614 File Offset: 0x00398814
	public override void SetScreenPosition(float _x, float _y)
	{
		this.SetScreenPosition(new Vector2(_x, _y));
	}

	// Token: 0x06009809 RID: 38921 RVA: 0x0039B3C4 File Offset: 0x003995C4
	public override void ResetToCenter()
	{
		this.SetScreenPosition((float)(Screen.width / 2), (float)(Screen.height / 2));
	}

	// Token: 0x0600980A RID: 38922 RVA: 0x000876FD File Offset: 0x000858FD
	public override void SetNavigationTarget(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600980B RID: 38923 RVA: 0x000876FD File Offset: 0x000858FD
	public override void SetNavigationTargetLater(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600980C RID: 38924 RVA: 0x000876FD File Offset: 0x000858FD
	public override void ResetNavigationTarget()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600980D RID: 38925 RVA: 0x000876FD File Offset: 0x000858FD
	public override void SetNavigationLockView(XUiView _view, XUiView _viewToSelect = null)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600980E RID: 38926 RVA: 0x000876FD File Offset: 0x000858FD
	public override bool IsWithinNavigationLockView(XUiView _view)
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600980F RID: 38927 RVA: 0x000876FD File Offset: 0x000858FD
	public override void RefreshSelection()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009810 RID: 38928 RVA: 0x0039B3DC File Offset: 0x003995DC
	public override void SetCursorHidden(bool _hidden)
	{
		GameManager.Instance.SetCursorEnabledOverride(_hidden, false);
	}

	// Token: 0x06009811 RID: 38929 RVA: 0x0039B3EA File Offset: 0x003995EA
	public override bool GetCursorHidden()
	{
		return GameManager.Instance.GetCursorEnabledOverride();
	}

	// Token: 0x06009812 RID: 38930 RVA: 0x000876FD File Offset: 0x000858FD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnVirtualCursorVisibleChanged()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06009813 RID: 38931 RVA: 0x0039B3F6 File Offset: 0x003995F6
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

	// Token: 0x06009814 RID: 38932 RVA: 0x0039B3FE File Offset: 0x003995FE
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

	// Token: 0x06009815 RID: 38933 RVA: 0x0039B430 File Offset: 0x00399630
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

	// Token: 0x0400722A RID: 29226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float BaseSpeed = 500f;

	// Token: 0x0400722B RID: 29227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float SpeedModRange = 1000f;

	// Token: 0x0400722C RID: 29228
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultMouseCursorResource = "@:Textures/UI/cursor01.tga";

	// Token: 0x0400722D RID: 29229
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultMouseCursorCenter = Vector2.zero;

	// Token: 0x0400722E RID: 29230
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string defaultControllerCursorResource = "@:Textures/UI/soft_cursor";

	// Token: 0x0400722F RID: 29231
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 defaultControllerCursorCenter = new Vector2(16f, 16f);

	// Token: 0x04007230 RID: 29232
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly string mapCursorResource = "@:Textures/UI/map_cursor.tga";

	// Token: 0x04007231 RID: 29233
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly Vector2 mapCursorCenter = new Vector2(16f, 16f);

	// Token: 0x04007232 RID: 29234
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D emptyCursor;

	// Token: 0x04007233 RID: 29235
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultControllerCursor;

	// Token: 0x04007234 RID: 29236
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D defaultMouseCursor;

	// Token: 0x04007235 RID: 29237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D mapCursor;

	// Token: 0x04007236 RID: 29238
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Texture2D currentCursorTexture;

	// Token: 0x04007237 RID: 29239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Vector2 currentCursorHotspot;

	// Token: 0x04007238 RID: 29240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static CursorControllerAbs.ECursorType currentCursorType = CursorControllerAbs.ECursorType.Default;

	// Token: 0x04007239 RID: 29241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static Coroutine cursorUpdateCo;

	// Token: 0x0400723A RID: 29242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speed;

	// Token: 0x0400723B RID: 29243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedMultiplier = 1f;

	// Token: 0x0400723C RID: 29244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float LastFrameTime;

	// Token: 0x0400723D RID: 29245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool snapped;

	// Token: 0x0400723E RID: 29246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float currentAcceleration;

	// Token: 0x0400723F RID: 29247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float OffsetSnapBounds = 0.1f;

	// Token: 0x04007240 RID: 29248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public PlayerInputManager.InputStyle m_lastInputStyle = PlayerInputManager.InputStyle.Count;

	// Token: 0x04007241 RID: 29249
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int CONTROLLER_CURSOR_MOVEMENT_LIMIT = 5;
}
