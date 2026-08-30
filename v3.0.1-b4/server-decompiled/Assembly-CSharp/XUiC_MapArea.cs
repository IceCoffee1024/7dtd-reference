using System;
using System.Collections.Generic;
using Audio;
using GUI_2;
using InControl;
using Platform;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F67 RID: 3943
[global::UnityEngine.Scripting.Preserve]
public class XUiC_MapArea : XUiController
{
	// Token: 0x06007C5A RID: 31834 RVA: 0x002F12EC File Offset: 0x002EF4EC
	public override void Init()
	{
		base.Init();
		if (this.mapTexture == null)
		{
			this.mapTexture = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
			this.mapTexture.name = "XUiC_MapArea.mapTexture";
		}
		NativeArray<Color32> rawTextureData = this.mapTexture.GetRawTextureData<Color32>();
		Color32 color = new Color32(0, 0, 0, byte.MaxValue);
		for (int i = 0; i < rawTextureData.Length; i += 4)
		{
			rawTextureData[i] = color;
		}
		XUiController childById = base.GetChildById("mapViewTexture");
		this.xuiTexture = (XUiV_Texture)childById.ViewComponent;
		this.transformSpritesParent = base.GetChildById("clippingPanel").ViewComponent.UiTransform;
		this.mapView = base.GetChildById("mapView");
		this.mapView.OnDrag += this.onMapDragged;
		this.mapView.OnScroll += this.onMapScrolled;
		this.mapView.OnPress += this.onMapPressedLeft;
		this.mapView.OnRightPress += this.onMapPressed;
		this.mapView.OnHover += this.onMapHover;
		this.zoomScale = 1f;
		this.targetZoomScale = 1f;
		this.xui.LoadData<GameObject>("Prefabs/MapSpriteEntity", delegate(GameObject o)
		{
			this.prefabMapSprite = o;
		});
		this.xui.LoadData<GameObject>("Prefabs/MapSpriteStartPoint", delegate(GameObject o)
		{
			this.prefabMapSpriteStartPoint = o;
		});
		this.xui.LoadData<GameObject>("Prefabs/MapSpritePrefab", delegate(GameObject o)
		{
			this.prefabMapSpritePrefab = o;
		});
		this.xui.LoadData<GameObject>("Prefabs/MapSpriteEntitySpawner", delegate(GameObject o)
		{
			this.prefabMapSpriteEntitySpawner = o;
		});
		this.initFOWChunkMaskColors();
		base.GetChildById("playerIcon").OnPress += this.onPlayerIconPressed;
		this.uiLblPlayerPos = (XUiV_Label)base.GetChildById("playerPos").ViewComponent;
		this.uiLblCursorPos = (XUiV_Label)base.GetChildById("cursorPos").ViewComponent;
		base.GetChildById("bedrollIcon").OnPress += this.onBedrollIconPressed;
		this.uiLblBedrollPos = (XUiV_Label)base.GetChildById("bedrollPos").ViewComponent;
		base.GetChildById("waypointIcon").OnPress += this.onWaypointIconPressed;
		this.uiLblMapMarkerDistance = (XUiV_Label)base.GetChildById("waypointDistance").ViewComponent;
		this.switchStaticMap = (XUiV_Button)base.GetChildById("switchStaticMap").ViewComponent;
		base.GetChildById("switchStaticMap").OnPress += delegate(XUiController _sender, int _args)
		{
			this.showStaticData = !this.showStaticData;
			this.switchStaticMap.Selected = this.showStaticData;
			this.cbxStaticMapType.ViewComponent.IsVisible = this.showStaticData;
		};
		this.cbxStaticMapType = base.GetChildByType<XUiC_ComboBoxEnum<XUiC_MapArea.EStaticMapOverlay>>();
		this.cbxStaticMapType.OnValueChanged += this.CbxStaticMapType_OnValueChanged;
		this.bShouldRedrawMap = true;
		this.initMap();
		this.kilometers = Localization.Get("xuiKilometers", false, null);
		this.crosshair = base.GetChildById("crosshair").ViewComponent as XUiV_Sprite;
		NavObjectManager.Instance.OnNavObjectRemoved += this.Instance_OnNavObjectRemoved;
		if (GameManager.Instance.IsEditMode() && !PrefabEditModeManager.Instance.IsActive())
		{
			this.showStaticData = true;
			this.switchStaticMap.Selected = true;
			this.cbxStaticMapType.Value = XUiC_MapArea.EStaticMapOverlay.Biomes;
		}
	}

	// Token: 0x06007C5B RID: 31835 RVA: 0x002F1653 File Offset: 0x002EF853
	[PublicizedFrom(EAccessModifier.Private)]
	public void Instance_OnNavObjectRemoved(NavObject newNavObject)
	{
		global::UnityEngine.Object.Destroy(this.keyToNavSprite[newNavObject.Key]);
		this.keyToNavObject.Remove(newNavObject.Key);
		this.keyToNavSprite.Remove(newNavObject.Key);
	}

	// Token: 0x06007C5C RID: 31836 RVA: 0x002F168D File Offset: 0x002EF88D
	[PublicizedFrom(EAccessModifier.Private)]
	public void initFOWChunkMaskColors()
	{
		this.xui.LoadData<Texture2D>("@:Textures/UI/fow_chunkMask.png", delegate(Texture2D o)
		{
			Color32[] pixels = o.GetPixels32();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					byte[] array = new byte[256];
					int num = 0;
					for (int k = i * 16; k < (i + 1) * 16; k++)
					{
						for (int l = j * 16; l < (j + 1) * 16; l++)
						{
							array[num++] = pixels[k * o.width + l].r;
						}
					}
					this.fowChunkMaskAlphas[i * 3 + j] = array;
				}
			}
			int num2 = 3;
			for (int m = 0; m < 4; m++)
			{
				byte[] array2 = new byte[256];
				int num3 = 0;
				for (int n = num2 * 16; n < (num2 + 1) * 16; n++)
				{
					for (int num4 = m * 16; num4 < (m + 1) * 16; num4++)
					{
						array2[num3++] = pixels[n * o.width + num4].r;
					}
				}
				this.fowChunkMaskAlphas[num2 * 3 + m] = array2;
			}
		});
	}

	// Token: 0x06007C5D RID: 31837 RVA: 0x002F16AC File Offset: 0x002EF8AC
	public override void OnOpen()
	{
		base.OnOpen();
		this.closeAllPopups();
		this.xui.playerUI.windowManager.Open("windowpaging", false);
		if (this.isOpen)
		{
			return;
		}
		this.isOpen = true;
		this.localPlayer = this.xui.playerUI.entityPlayer;
		this.bFowMaskEnabled = !GameManager.Instance.IsEditMode();
		this.switchStaticMap.IsVisible = SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && (GameManager.Instance.IsEditMode() || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled));
		this.cbxStaticMapType.ViewComponent.IsVisible = this.switchStaticMap.IsVisible && this.showStaticData;
		this.initExistingWaypoints(this.localPlayer.Waypoints);
		this.initMap();
		this.positionMap();
		this.PositionMapAt(this.localPlayer.GetPosition());
		this.xui.playerUI.GetComponentInParent<LocalPlayerCamera>().PreRender += this.OnPreRender;
		this.xui.CalloutWindow.ClearCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightStick, "igcoMapMoveNoHold", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonSouth, "igcoMapWaypoint", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.RightTrigger, "igcoMapZoomIn", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.LeftTrigger, "igcoMapZoomOut", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.AddCallout(UIUtils.ButtonIcon.FaceButtonEast, "igcoExit", XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.CalloutWindow.EnableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu, 0f);
		XUiC_WindowSelector childByType = this.xui.FindWindowGroupByName("windowpaging").GetChildByType<XUiC_WindowSelector>();
		if (childByType != null)
		{
			childByType.SetSelected("map");
		}
		this.crosshair.IsVisible = false;
		this.windowGroup.isEscClosable = false;
	}

	// Token: 0x06007C5E RID: 31838 RVA: 0x002F1896 File Offset: 0x002EFA96
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetMapCursor(bool _customCursor)
	{
		this.bMapCursorSet = _customCursor;
		if (_customCursor)
		{
			CursorControllerAbs.SetCursor(CursorControllerAbs.ECursorType.Map);
			return;
		}
		CursorControllerAbs.SetCursor(CursorControllerAbs.ECursorType.Default);
	}

	// Token: 0x06007C5F RID: 31839 RVA: 0x002F18B0 File Offset: 0x002EFAB0
	public override void OnClose()
	{
		base.OnClose();
		this.xui.playerUI.windowManager.Close("windowpaging");
		if (!this.isOpen)
		{
			return;
		}
		this.isOpen = false;
		this.bShouldRedrawMap = false;
		if (this.bMapCursorSet)
		{
			this.SetMapCursor(false);
			this.xui.ToolTipWindow.ToolTip = string.Empty;
		}
		this.xui.playerUI.GetComponentInParent<LocalPlayerCamera>().PreRender -= this.OnPreRender;
		this.xui.CalloutWindow.DisableCallouts(XUiC_GamepadCalloutWindow.CalloutType.Menu);
		this.xui.playerUI.CursorController.Locked = false;
		SoftCursor.SetCursor(CursorControllerAbs.ECursorType.Default);
		this.closestMouseOverNavObject = null;
		XUiC_Paging.IsOverPagingOverrideElement = false;
	}

	// Token: 0x06007C60 RID: 31840 RVA: 0x002F1973 File Offset: 0x002EFB73
	public override void OnCursorSelected(bool _isActualElement)
	{
		base.OnCursorSelected(_isActualElement);
		this.crosshair.IsVisible = PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard;
	}

	// Token: 0x06007C61 RID: 31841 RVA: 0x002F199C File Offset: 0x002EFB9C
	public override void OnCursorUnSelected()
	{
		base.OnCursorUnSelected();
		this.crosshair.IsVisible = false;
	}

	// Token: 0x06007C62 RID: 31842 RVA: 0x002F19B0 File Offset: 0x002EFBB0
	public override void Update(float _dt)
	{
		base.Update(_dt);
		if (!this.windowGroup.isShowing)
		{
			return;
		}
		if (!XUi.IsGameRunning() || this.xui.playerUI.entityPlayer == null)
		{
			return;
		}
		if (!this.bMapInitialized)
		{
			this.initMap();
		}
		if (this.xui.playerUI.playerInput.GUIActions.LastDeviceClass == InputDeviceClass.Controller && !this.xui.GetWindow("mapAreaSetWaypoint").IsVisible && this.bMouseOverMap)
		{
			Vector2 vector = -this.xui.playerUI.playerInput.GUIActions.Camera.Vector;
			if (vector.sqrMagnitude > 0f)
			{
				this.DragMap(vector * 500f * _dt);
			}
		}
		this.updateMapOverlay();
		if (this.bShouldRedrawMap)
		{
			this.updateFullMap();
			this.bShouldRedrawMap = false;
		}
		if (this.timeToRedrawMap > 0f)
		{
			this.timeToRedrawMap -= _dt;
			if (this.timeToRedrawMap <= 0f)
			{
				this.bShouldRedrawMap = true;
			}
		}
		if (this.localPlayer.ChunkObserver.mapDatabase.IsNetworkDataAvail())
		{
			this.timeToRedrawMap = 0.5f;
			this.localPlayer.ChunkObserver.mapDatabase.ResetNetworkDataAvail();
		}
		this.uiLblPlayerPos.Text = ValueDisplayFormatters.WorldPos(this.xui.playerUI.entityPlayer.GetPosition(), " ", false);
		Vector3 vector2 = this.screenPosToWorldPos(this.xui.playerUI.CursorController.GetScreenPosition(), false);
		this.uiLblCursorPos.Text = ValueDisplayFormatters.WorldPos(vector2, " ", false);
		this.uiLblCursorPos.IsVisible = this.bMouseOverMap;
		this.uiLblBedrollPos.Text = ((this.localPlayer.SpawnPoints.Count > 0) ? ValueDisplayFormatters.WorldPos(this.localPlayer.SpawnPoints[0].ToVector3(), " ", false) : string.Empty);
		string text = string.Empty;
		if (this.localPlayer.markerPosition != Vector3i.zero)
		{
			text = string.Format("{0} {1}", ((this.localPlayer.position - this.localPlayer.markerPosition.ToVector3()).magnitude / 1000f).ToCultureInvariantString("0.0"), this.kilometers);
		}
		this.uiLblMapMarkerDistance.Text = text;
		float num = 5f * this.xui.playerUI.playerInput.GUIActions.TriggerAxis.Value;
		if (num != 0f)
		{
			this.targetZoomScale = Utils.FastClamp(this.targetZoomScale + num * _dt, 0.7f, 6.15f);
		}
		this.zoomScale = Mathf.Lerp(this.zoomScale, this.targetZoomScale, 5f * _dt);
		this.positionMap();
		this.updateMapObjects();
		this.UpdateWaypointSelection();
		if (this.xui.playerUI.playerInput.GUIActions.Cancel.WasPressed || this.xui.playerUI.playerInput.PermanentActions.Cancel.WasPressed)
		{
			XUiV_Window window = this.xui.GetWindow("mapAreaSetWaypoint");
			if (window.IsVisible)
			{
				window.IsVisible = false;
				this.xui.GetWindow("mapAreaChooseWaypoint").IsVisible = false;
				this.xui.GetWindow("mapAreaEnterWaypointName").IsVisible = false;
				this.xui.playerUI.CursorController.SetNavigationTargetLater(base.GetChildById("MapView").ViewComponent);
				return;
			}
			this.xui.playerUI.windowManager.CloseAllOpenModalWindows(null, true);
		}
	}

	// Token: 0x06007C63 RID: 31843 RVA: 0x002F1D8C File Offset: 0x002EFF8C
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe void updateMapOverlay()
	{
		if (this.showStaticData != (this.staticWorldTexture != null))
		{
			this.bShouldRedrawMap = true;
			if (!this.showStaticData)
			{
				this.staticWorldTexture = null;
				return;
			}
			if (this.staticWorldTexture == null)
			{
				World world = GameManager.Instance.World;
				ChunkProviderGenerateWorldFromRaw chunkProviderGenerateWorldFromRaw = world.ChunkCache.ChunkProvider as ChunkProviderGenerateWorldFromRaw;
				if (chunkProviderGenerateWorldFromRaw == null)
				{
					return;
				}
				IBiomeProvider biomeProvider = chunkProviderGenerateWorldFromRaw.GetBiomeProvider();
				WorldDecoratorPOIFromImage poiFromImage = chunkProviderGenerateWorldFromRaw.poiFromImage;
				if (biomeProvider == null || poiFromImage == null)
				{
					return;
				}
				int splat3Width = poiFromImage.splat3Width;
				int splat3Height = poiFromImage.splat3Height;
				int num = splat3Width / 2;
				int num2 = splat3Height / 2;
				this.staticWorldWidth = splat3Width;
				this.staticWorldHeight = splat3Height;
				this.staticMapLeft = -this.staticWorldWidth / 2;
				this.staticMapRight = this.staticWorldWidth / 2 - 1;
				this.staticMapBottom = -this.staticWorldHeight / 2;
				this.staticMapTop = this.staticWorldHeight / 2 - 1;
				this.staticWorldTexture = new Color32[poiFromImage.m_Poi.DimX * poiFromImage.m_Poi.DimY];
				Color32 color = new Color32(0, 0, 0, byte.MaxValue);
				for (int i = 0; i < splat3Height; i++)
				{
					int num3 = i * splat3Width;
					ReadOnlySpan<ushort> readOnlySpan2;
					using (chunkProviderGenerateWorldFromRaw.heightData.GetReadOnlySpan(num3, splat3Width, out readOnlySpan2))
					{
						int j = 0;
						while (j < splat3Width)
						{
							color.r = 0;
							color.g = 0;
							color.b = 0;
							byte value = poiFromImage.m_Poi.colors.GetValue(j, i);
							if (value == 1)
							{
								color.r = (color.b = byte.MaxValue);
								goto IL_0444;
							}
							if (value == 2)
							{
								color.r = byte.MaxValue;
								goto IL_0444;
							}
							if (value == 3)
							{
								color.g = byte.MaxValue;
								goto IL_0444;
							}
							if (value == 4)
							{
								color.b = byte.MaxValue;
								goto IL_0444;
							}
							PoiMapElement poiForColor;
							if (value != 0 && (poiForColor = world.Biomes.getPoiForColor((uint)value)) != null && poiForColor.m_BlockValue.Block.blockMaterial.IsLiquid)
							{
								color.b = byte.MaxValue;
								goto IL_0444;
							}
							byte b = (byte)((float)(*readOnlySpan2[j]) * 0.0038910506f);
							color.r = (color.g = (color.b = b));
							if (this.cbxStaticMapType.Value == XUiC_MapArea.EStaticMapOverlay.Biomes)
							{
								BiomeDefinition biomeAt = biomeProvider.GetBiomeAt(j - num, i - num2);
								if (biomeAt != null)
								{
									color.r = (byte)Mathf.LerpUnclamped((float)color.r, (biomeAt.m_uiColor >> 16) & 255U, 0.25f);
									color.g = (byte)Mathf.LerpUnclamped((float)color.g, (biomeAt.m_uiColor >> 8) & 255U, 0.25f);
									color.b = (byte)Mathf.LerpUnclamped((float)color.b, biomeAt.m_uiColor & 255U, 0.25f);
									goto IL_0444;
								}
								goto IL_0444;
							}
							else
							{
								if (this.cbxStaticMapType.Value != XUiC_MapArea.EStaticMapOverlay.Radiation)
								{
									goto IL_0444;
								}
								float radiationAt = biomeProvider.GetRadiationAt(j - num, i - num2);
								if (radiationAt >= 0.5f)
								{
									if (radiationAt < 1.5f)
									{
										color.r = (byte)Mathf.LerpUnclamped((float)color.r, 0f, 0.25f);
										color.g = (byte)Mathf.LerpUnclamped((float)color.g, 255f, 0.25f);
										color.b = (byte)Mathf.LerpUnclamped((float)color.b, 0f, 0.25f);
										goto IL_0444;
									}
									if (radiationAt < 2.5f)
									{
										color.r = (byte)Mathf.LerpUnclamped((float)color.r, 0f, 0.25f);
										color.g = (byte)Mathf.LerpUnclamped((float)color.g, 0f, 0.25f);
										color.b = (byte)Mathf.LerpUnclamped((float)color.b, 255f, 0.25f);
										goto IL_0444;
									}
									color.r = (byte)Mathf.LerpUnclamped((float)color.r, 255f, 0.25f);
									color.g = (byte)Mathf.LerpUnclamped((float)color.g, 0f, 0.25f);
									color.b = (byte)Mathf.LerpUnclamped((float)color.b, 0f, 0.25f);
									goto IL_0444;
								}
							}
							IL_0456:
							j++;
							continue;
							IL_0444:
							this.staticWorldTexture[num3 + j] = color;
							goto IL_0456;
						}
					}
				}
			}
		}
	}

	// Token: 0x06007C64 RID: 31844 RVA: 0x002F2238 File Offset: 0x002F0438
	[PublicizedFrom(EAccessModifier.Private)]
	public void initExistingWaypoints(WaypointCollection _waypoints)
	{
		if (!this.existingWaypointsInitialized)
		{
			using (List<Waypoint>.Enumerator enumerator = _waypoints.Collection.list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Waypoint w = enumerator.Current;
					if (w.bIsAutoWaypoint || w.bUsingLocalizationId)
					{
						w.navObject.name = Localization.Get(w.name.Text, false, null);
					}
					else
					{
						GeneratedTextManager.GetDisplayText(w.name, delegate(string _filtered)
						{
							w.navObject.name = _filtered;
						}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
					}
				}
			}
			this.existingWaypointsInitialized = true;
		}
	}

	// Token: 0x06007C65 RID: 31845 RVA: 0x002F230C File Offset: 0x002F050C
	[PublicizedFrom(EAccessModifier.Private)]
	public void initMap()
	{
		if (this.xui.playerUI.entityPlayer == null)
		{
			return;
		}
		this.localPlayer = this.xui.playerUI.entityPlayer;
		this.bMapInitialized = true;
		this.xuiTexture.Texture = this.mapTexture;
		this.xuiTexture.Size = new Vector2i(712, 712);
	}

	// Token: 0x06007C66 RID: 31846 RVA: 0x002F237C File Offset: 0x002F057C
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateFullMap()
	{
		int num = (int)this.mapMiddlePosChunks.x - 1024;
		int num2 = (int)this.mapMiddlePosChunks.x + 1024;
		int num3 = (int)this.mapMiddlePosChunks.y - 1024;
		int num4 = (int)this.mapMiddlePosChunks.y + 1024;
		this.updateMapSection(num, num3, num2, num4, 0, 0, 2048, 2048);
		this.mapScrollTextureOffset.x = 0f;
		this.mapScrollTextureOffset.y = 0f;
		this.mapScrollTextureChunksOffsetX = 0;
		this.mapScrollTextureChunksOffsetZ = 0;
		this.positionMap();
		this.mapTexture.Apply();
		this.SendMapPositionToServer();
	}

	// Token: 0x06007C67 RID: 31847 RVA: 0x002F2430 File Offset: 0x002F0630
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMapForScroll(int deltaChunksX, int deltaChunksZ)
	{
		if (deltaChunksX != 0)
		{
			int num = Mathf.Abs(deltaChunksX);
			int num2 = this.mapScrollTextureChunksOffsetX * 16;
			int num3 = (this.mapScrollTextureChunksOffsetX + deltaChunksX) * 16;
			num3 = Utils.WrapInt(num3, 0, 2048);
			int num4;
			int num5;
			if (deltaChunksX > 0)
			{
				if (num2 == 2048)
				{
					num2 = 0;
				}
				num4 = (int)this.mapMiddlePosChunks.x + 1024;
				num5 = num4 - num * 16;
			}
			else
			{
				if (num2 == 0)
				{
					num2 = 2048;
				}
				int num6 = num2;
				num2 = num3;
				num3 = num6;
				num5 = (int)this.mapMiddlePosChunks.x - 1024;
				num4 = num5 + num * 16;
			}
			int num7 = (this.mapScrollTextureChunksOffsetZ + deltaChunksZ) * 16;
			num7 = Utils.WrapIndex(num7, 2048);
			int num8 = Utils.WrapIndex(num7 - 1, 2048);
			int num9 = (int)this.mapMiddlePosChunks.y - 1024;
			int num10 = num9 + 2048;
			this.updateMapSection(num5, num9, num4, num10, num2, num7, num3, num8);
		}
		if (deltaChunksZ != 0)
		{
			int num11 = Mathf.Abs(deltaChunksZ);
			int num7 = this.mapScrollTextureChunksOffsetZ * 16;
			int num8 = (this.mapScrollTextureChunksOffsetZ + deltaChunksZ) * 16;
			num8 = Utils.WrapInt(num8, 0, 2048);
			int num9;
			int num10;
			if (deltaChunksZ > 0)
			{
				if (num7 == 2048)
				{
					num7 = 0;
				}
				num10 = (int)this.mapMiddlePosChunks.y + 1024;
				num9 = num10 - num11 * 16;
			}
			else
			{
				if (num7 == 0)
				{
					num7 = 2048;
				}
				int num12 = num7;
				num7 = num8;
				num8 = num12;
				num9 = (int)this.mapMiddlePosChunks.y - 1024;
				num10 = num9 + num11 * 16;
			}
			int num2 = (this.mapScrollTextureChunksOffsetX + deltaChunksX) * 16;
			num2 = Utils.WrapIndex(num2, 2048);
			int num3 = Utils.WrapIndex(num2 - 1, 2048);
			int num5 = (int)this.mapMiddlePosChunks.x - 1024;
			int num4 = num5 + 2048;
			this.updateMapSection(num5, num9, num4, num10, num2, num7, num3, num8);
		}
		this.mapScrollTextureOffset.x = this.mapScrollTextureOffset.x + (float)(deltaChunksX * 16) / (float)this.mapTexture.width;
		this.mapScrollTextureOffset.y = this.mapScrollTextureOffset.y + (float)(deltaChunksZ * 16) / (float)this.mapTexture.width;
		this.mapScrollTextureOffset.x = Utils.WrapFloat(this.mapScrollTextureOffset.x, 0f, 1f);
		this.mapScrollTextureOffset.y = Utils.WrapFloat(this.mapScrollTextureOffset.y, 0f, 1f);
		this.mapScrollTextureChunksOffsetX += deltaChunksX;
		this.mapScrollTextureChunksOffsetZ += deltaChunksZ;
		this.mapScrollTextureChunksOffsetX = Utils.WrapIndex(this.mapScrollTextureChunksOffsetX, 128);
		this.mapScrollTextureChunksOffsetZ = Utils.WrapIndex(this.mapScrollTextureChunksOffsetZ, 128);
		this.positionMap();
		this.mapTexture.Apply();
		this.SendMapPositionToServer();
	}

	// Token: 0x06007C68 RID: 31848 RVA: 0x002F26F8 File Offset: 0x002F08F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMapSection(int mapStartX, int mapStartZ, int mapEndX, int mapEndZ, int drawnMapStartX, int drawnMapStartZ, int drawnMapEndX, int drawnMapEndZ)
	{
		IMapChunkDatabase mapDatabase = this.localPlayer.ChunkObserver.mapDatabase;
		bool flag = this.showStaticData && this.staticWorldTexture != null;
		NativeArray<Color32> rawTextureData = this.mapTexture.GetRawTextureData<Color32>();
		int i = mapStartZ;
		int num = drawnMapStartZ;
		while (i < mapEndZ)
		{
			int j = mapStartX;
			int num2 = drawnMapStartX;
			while (j < mapEndX)
			{
				int num3 = World.toChunkXZ(j);
				int num4 = World.toChunkXZ(i);
				if (flag)
				{
					int num5 = num3 << 4;
					int num6 = num4 << 4;
					for (int k = 0; k < 256; k++)
					{
						int num7 = k / 16;
						int num8 = k % 16;
						int num9 = (num + num7) * 2048;
						int num10 = num2 + num8;
						int num11 = num9 + num10;
						int num12 = num5 + num8;
						int num13 = num6 + num7;
						if (num12 < this.staticMapLeft || num12 > this.staticMapRight || num13 < this.staticMapBottom || num13 > this.staticMapTop)
						{
							rawTextureData[num11] = new Color32(0, 0, 0, 0);
						}
						else
						{
							int num14 = num12 - this.staticMapLeft + (num13 - this.staticMapBottom) * this.staticWorldWidth;
							Color32 color = this.staticWorldTexture[num14];
							if (color.a > 0)
							{
								rawTextureData[num11] = new Color32(color.r, color.g, color.b, byte.MaxValue);
							}
							else
							{
								rawTextureData[num11] = new Color32(0, 0, 0, 0);
							}
						}
					}
				}
				else
				{
					long num15 = WorldChunkCache.MakeChunkKey(num3, num4);
					ushort[] mapColors = mapDatabase.GetMapColors(num15);
					if (mapColors == null)
					{
						for (int l = 0; l < 256; l++)
						{
							int num16 = (num + l / 16) * 2048;
							int num17 = num2 + l % 16 + num16;
							rawTextureData[num17] = new Color32(0, 0, 0, 0);
						}
					}
					else
					{
						bool flag2 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3, num4 + 1));
						bool flag3 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3, num4 - 1));
						bool flag4 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 - 1, num4));
						bool flag5 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 + 1, num4));
						int num18 = 0;
						if (flag2 && flag3 && flag4 && flag5)
						{
							bool flag6 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 - 1, num4 + 1));
							bool flag7 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 + 1, num4 + 1));
							bool flag8 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 - 1, num4 - 1));
							bool flag9 = mapDatabase.Contains(WorldChunkCache.MakeChunkKey(num3 + 1, num4 - 1));
							if (!flag6)
							{
								num18 = 9;
							}
							else if (!flag7)
							{
								num18 = 10;
							}
							else if (!flag9)
							{
								num18 = 11;
							}
							else if (!flag8)
							{
								num18 = 12;
							}
							else
							{
								num18 = 4;
							}
						}
						else
						{
							if (flag3 && !flag2)
							{
								num18 += 6;
							}
							else if (flag3 && flag2)
							{
								num18 += 3;
							}
							if (flag5 && flag4)
							{
								num18++;
							}
							else if (flag4)
							{
								num18 += 2;
							}
						}
						byte[] array = this.fowChunkMaskAlphas[num18];
						if (!this.bFowMaskEnabled)
						{
							array = this.fowChunkMaskAlphas[4];
						}
						for (int m = 0; m < 256; m++)
						{
							int num19 = m / 16;
							int num20 = m % 16;
							int num21 = (num + num19) * 2048;
							int num22 = num2 + num20;
							int num23 = num21 + num22;
							int num24 = num19 * 16;
							byte b = array[num24 + num20];
							Color32 color2 = Utils.FromColor5To32(mapColors[m]);
							rawTextureData[num23] = new Color32(color2.r, color2.g, color2.b, (b < byte.MaxValue) ? b : byte.MaxValue);
						}
					}
				}
				j += 16;
				num2 = Utils.WrapIndex(num2 + 16, 2048);
			}
			i += 16;
			num = Utils.WrapIndex(num + 16, 2048);
		}
	}

	// Token: 0x06007C69 RID: 31849 RVA: 0x002F2AD0 File Offset: 0x002F0CD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SendMapPositionToServer()
	{
		if (GameManager.Instance.World.IsRemote() && !this.mapMiddlePosChunksToServer.Equals(this.mapMiddlePosChunks))
		{
			this.mapMiddlePosChunksToServer = this.mapMiddlePosChunks;
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageMapPosition>().Setup(this.localPlayer.entityId, new Vector2i(Utils.Fastfloor(this.mapMiddlePosChunks.x), Utils.Fastfloor(this.mapMiddlePosChunks.y))), false);
		}
	}

	// Token: 0x06007C6A RID: 31850 RVA: 0x002F2B54 File Offset: 0x002F0D54
	[PublicizedFrom(EAccessModifier.Private)]
	public void positionMap()
	{
		float num = (2048f - 336f * this.zoomScale) / 2f;
		this.mapScale = 336f * this.zoomScale / 2048f;
		float num2 = (num + (this.mapMiddlePosPixel.x - this.mapMiddlePosChunks.x)) / 2048f;
		float num3 = (num + (this.mapMiddlePosPixel.y - this.mapMiddlePosChunks.y)) / 2048f;
		this.mapPos = new Vector3(num2 + this.mapScrollTextureOffset.x, num3 + this.mapScrollTextureOffset.y, 0f);
		this.mapBGPos.x = (num + this.mapMiddlePosPixel.x) / 2048f;
		this.mapBGPos.y = (num + this.mapMiddlePosPixel.y) / 2048f;
	}

	// Token: 0x06007C6B RID: 31851 RVA: 0x002F2C40 File Offset: 0x002F0E40
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnPreRender(LocalPlayerCamera _localPlayerCamera)
	{
		Shader.SetGlobalVector("_MainMapPosAndScale", new Vector4(this.mapPos.x, this.mapPos.y, this.mapScale, this.mapScale));
		Shader.SetGlobalVector("_MainMapBGPosAndScale", new Vector4(this.mapBGPos.x, this.mapBGPos.y, this.mapScale, this.mapScale));
	}

	// Token: 0x06007C6C RID: 31852 RVA: 0x002F2CAF File Offset: 0x002F0EAF
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMapDragged(XUiController _sender, EDragType _dragType, Vector2 _mousePositionDelta)
	{
		if (UICamera.currentKey != KeyCode.Mouse0)
		{
			return;
		}
		if (this.xui.playerUI.playerInput.GUIActions.LastDeviceClass != InputDeviceClass.Controller)
		{
			this.DragMap(_mousePositionDelta);
		}
		this.closeAllPopups();
	}

	// Token: 0x06007C6D RID: 31853 RVA: 0x002F2CE8 File Offset: 0x002F0EE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void DragMap(Vector2 delta)
	{
		float pixelRatioFactor = this.xui.GetPixelRatioFactor();
		this.mapMiddlePosPixel -= delta * pixelRatioFactor * 0.47191012f * this.zoomScale;
		this.mapMiddlePosPixel = GameManager.Instance.World.ClampToValidWorldPosForMap(this.mapMiddlePosPixel);
		int num = 0;
		int num2 = 0;
		while (this.mapMiddlePosChunks.x - this.mapMiddlePosPixel.x >= 16f)
		{
			this.mapMiddlePosChunks.x = this.mapMiddlePosChunks.x - 16f;
			num--;
		}
		while (this.mapMiddlePosChunks.x - this.mapMiddlePosPixel.x <= -16f)
		{
			this.mapMiddlePosChunks.x = this.mapMiddlePosChunks.x + 16f;
			num++;
		}
		while (this.mapMiddlePosChunks.y - this.mapMiddlePosPixel.y >= 16f)
		{
			this.mapMiddlePosChunks.y = this.mapMiddlePosChunks.y - 16f;
			num2--;
		}
		while (this.mapMiddlePosChunks.y - this.mapMiddlePosPixel.y <= -16f)
		{
			this.mapMiddlePosChunks.y = this.mapMiddlePosChunks.y + 16f;
			num2++;
		}
		if (num != 0 || num2 != 0)
		{
			this.updateMapForScroll(num, num2);
		}
	}

	// Token: 0x06007C6E RID: 31854 RVA: 0x002F2E40 File Offset: 0x002F1040
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMapScrolled(XUiController _sender, float _delta)
	{
		float num = 6f;
		if (InputUtils.ShiftKeyPressed)
		{
			num = 5f * this.zoomScale;
		}
		float num2 = 0.7f;
		float num3 = 6.15f;
		this.targetZoomScale = Utils.FastClamp(this.zoomScale - _delta * num, num2, num3);
		if (_delta < 0f)
		{
			Manager.PlayInsidePlayerHead("map_zoom_in", -1, 0f, false, false);
		}
		else
		{
			Manager.PlayInsidePlayerHead("map_zoom_out", -1, 0f, false, false);
		}
		this.closeAllPopups();
	}

	// Token: 0x06007C6F RID: 31855 RVA: 0x002F2EC0 File Offset: 0x002F10C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMapObject(EnumMapObjectType _type, long _key, string _name, Vector3 _position, Vector3 _size, GameObject _prefab)
	{
		GameObject gameObject;
		if (!this.keyToMapSprite.TryGetValue(_key, out gameObject))
		{
			gameObject = this.transformSpritesParent.gameObject.AddChild(_prefab);
			gameObject.GetComponent<UISprite>().depth = 20;
			gameObject.name = _name;
			gameObject.GetComponent<UISprite>().depth = 1;
			this.keyToMapObject[_key] = new MapObject(_type, _position, _key, null, true);
			this.keyToMapSprite[_key] = gameObject;
		}
		if (gameObject)
		{
			float num = this.getSpriteZoomScaleFac() * 4.3f;
			UISprite component = gameObject.GetComponent<UISprite>();
			component.width = (int)(_size.x * num);
			component.height = (int)(_size.z * num);
			Transform transform = gameObject.transform;
			transform.localPosition = this.worldPosToScreenPos(_position);
			transform.localRotation = Quaternion.identity;
			this.mapObjectsOnMapAlive.Add(_key);
		}
	}

	// Token: 0x06007C70 RID: 31856 RVA: 0x002F2F99 File Offset: 0x002F1199
	[PublicizedFrom(EAccessModifier.Private)]
	public float getSpriteZoomScaleFac()
	{
		return Utils.FastClamp(1f / (this.zoomScale * 2f), 0.02f, 20f);
	}

	// Token: 0x06007C71 RID: 31857 RVA: 0x002F2FBC File Offset: 0x002F11BC
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateMapObjectList(List<MapObject> _mapObjectList, bool _bConsiderInOnMouseOverCursor = false)
	{
		bool flag = false;
		for (int i = 0; i < _mapObjectList.Count; i++)
		{
			MapObject mapObject = _mapObjectList[i];
			mapObject.RefreshData();
			long num = ((long)mapObject.type << 32) | mapObject.key;
			if (mapObject.IsMapIconEnabled())
			{
				GameObject gameObject;
				UISprite uisprite;
				if (!this.keyToMapSprite.ContainsKey(num))
				{
					gameObject = this.transformSpritesParent.gameObject.AddChild(this.prefabMapSprite);
					uisprite = gameObject.transform.Find("Sprite").GetComponent<UISprite>();
					string mapIcon = mapObject.GetMapIcon();
					uisprite.atlas = this.xui.GetAtlasByName(((global::UnityEngine.Object)uisprite.atlas).name, mapIcon);
					uisprite.spriteName = mapIcon;
					uisprite.depth = mapObject.GetLayerForMapIcon();
					uisprite.gameObject.GetComponent<TweenAlpha>().enabled = mapObject.IsMapIconBlinking();
					this.keyToMapObject[num] = mapObject;
					this.keyToMapSprite[num] = gameObject;
				}
				else
				{
					gameObject = this.keyToMapSprite[num];
				}
				string text = (mapObject.IsShowName() ? mapObject.GetName() : null);
				if (text != null)
				{
					UILabel component = gameObject.transform.Find("Name").GetComponent<UILabel>();
					component.text = text;
					component.gameObject.SetActive(true);
					component.color = mapObject.GetMapIconColor();
				}
				float spriteZoomScaleFac = this.getSpriteZoomScaleFac();
				uisprite = gameObject.transform.Find("Sprite").GetComponent<UISprite>();
				Vector3 vector = mapObject.GetMapIconScale() * spriteZoomScaleFac;
				uisprite.width = (int)((float)this.cSpriteScale * vector.x);
				uisprite.height = (int)((float)this.cSpriteScale * vector.y);
				uisprite.color = mapObject.GetMapIconColor();
				uisprite.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, -mapObject.GetRotation().y);
				gameObject.transform.localPosition = this.worldPosToScreenPos(mapObject.GetPosition());
				if (mapObject.IsCenterOnLeftBottomCorner())
				{
					gameObject.transform.localPosition += new Vector3((float)(uisprite.width / 2), (float)(uisprite.height / 2), 0f);
				}
				if (_bConsiderInOnMouseOverCursor)
				{
					Vector3 vector2 = this.worldPosToScreenPos(mapObject.GetPosition());
					vector2.y = -vector2.y;
					Vector3 vector3 = this.mousePosToWindowPos(this.xui.playerUI.CursorController.GetScreenPosition());
					if (vector2.x > 0f && vector2.x < 712f && vector2.y > 0f && vector2.y < 712f && Utils.FastAbs((vector2 - vector3).magnitude) < 30f)
					{
						if (!this.bMapCursorSet)
						{
							this.SetMapCursor(true);
						}
						if (this.xui.ToolTipWindow.ToolTip != mapObject.GetName() && !string.IsNullOrEmpty(mapObject.GetName()))
						{
							this.xui.ToolTipWindow.ToolTip = mapObject.GetName();
							if (mapObject is MapObjectWaypoint)
							{
								this.selectWaypoint(((MapObjectWaypoint)mapObject).waypoint);
							}
						}
						flag = true;
					}
				}
				this.mapObjectsOnMapAlive.Add(num);
			}
		}
		if (_bConsiderInOnMouseOverCursor && !flag && this.bMapCursorSet)
		{
			this.SetMapCursor(false);
			this.xui.ToolTipWindow.ToolTip = string.Empty;
		}
	}

	// Token: 0x06007C72 RID: 31858 RVA: 0x002F3358 File Offset: 0x002F1558
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateNavObjectList()
	{
		bool flag = true;
		bool flag2 = false;
		List<NavObject> navObjectList = NavObjectManager.Instance.NavObjectList;
		this.navObjectsOnMapAlive.Clear();
		for (int i = 0; i < navObjectList.Count; i++)
		{
			NavObject navObject = navObjectList[i];
			if (!navObject.hiddenOnMap)
			{
				NavObjectMapSettings currentMapSettings = navObject.CurrentMapSettings;
				if (currentMapSettings != null)
				{
					int key = navObject.Key;
					GameObject gameObject;
					UISprite uisprite;
					if (!this.keyToNavObject.ContainsKey(key))
					{
						gameObject = this.transformSpritesParent.gameObject.AddChild(this.prefabMapSprite);
						uisprite = gameObject.transform.Find("Sprite").GetComponent<UISprite>();
						string spriteName = navObject.GetSpriteName(currentMapSettings);
						uisprite.atlas = this.xui.GetAtlasByName(((global::UnityEngine.Object)uisprite.atlas).name, spriteName);
						uisprite.spriteName = spriteName;
						uisprite.depth = currentMapSettings.Layer;
						this.keyToNavObject[key] = navObject;
						this.keyToNavSprite[key] = gameObject;
					}
					else
					{
						gameObject = this.keyToNavSprite[key];
					}
					EntityPlayer entityPlayer = navObject.TrackedEntity as EntityPlayer;
					string text = ((entityPlayer != null) ? entityPlayer.PlayerDisplayName : navObject.DisplayName);
					if (!string.IsNullOrEmpty(text))
					{
						UILabel component = gameObject.transform.Find("Name").GetComponent<UILabel>();
						component.text = text;
						component.font = this.xui.GetUIFontByName("ReferenceFont", true);
						component.gameObject.SetActive(true);
						component.color = (navObject.UseOverrideColor ? navObject.OverrideColor : currentMapSettings.Color);
					}
					else
					{
						gameObject.transform.Find("Name").GetComponent<UILabel>().text = "";
					}
					float spriteZoomScaleFac = this.getSpriteZoomScaleFac();
					uisprite = gameObject.transform.Find("Sprite").GetComponent<UISprite>();
					Vector3 vector = currentMapSettings.IconScaleVector * spriteZoomScaleFac;
					uisprite.width = Mathf.Clamp((int)((float)this.cSpriteScale * vector.x), 9, 100);
					uisprite.height = Mathf.Clamp((int)((float)this.cSpriteScale * vector.y), 9, 100);
					uisprite.color = (navObject.hiddenOnCompass ? Color.grey : (navObject.UseOverrideColor ? navObject.OverrideColor : currentMapSettings.Color));
					uisprite.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, -navObject.Rotation.y);
					gameObject.transform.localPosition = this.worldPosToScreenPos(navObject.GetPosition() + Origin.position);
					if (currentMapSettings.AdjustCenter)
					{
						gameObject.transform.localPosition += new Vector3((float)(uisprite.width / 2), (float)(uisprite.height / 2), 0f);
					}
					this.navObjectsOnMapAlive.Add((long)key);
				}
			}
		}
		if (flag && !flag2 && this.bMapCursorSet)
		{
			this.SetMapCursor(false);
			this.xui.ToolTipWindow.ToolTip = string.Empty;
		}
	}

	// Token: 0x06007C73 RID: 31859 RVA: 0x002F3698 File Offset: 0x002F1898
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateMapObjects()
	{
		WorldBase world = GameManager.Instance.World;
		this.navObjectsOnMapAlive.Clear();
		this.mapObjectsOnMapAlive.Clear();
		if (world.IsEditor() || this.showStaticData)
		{
			SpawnPointList spawnPointList = GameManager.Instance.GetSpawnPointList();
			for (int i = 0; i < spawnPointList.Count; i++)
			{
				SpawnPoint spawnPoint = spawnPointList[i];
				this.updateMapObject(EnumMapObjectType.StartPoint, ((long)spawnPoint.GetHashCode() << 32) | (long)((ulong)(-1)), "SpawnPoint", spawnPoint.spawnPosition.position, Vector3.one * 30f, this.prefabMapSpriteStartPoint);
			}
			this.dynamicPrefabBuffer.Clear();
			DynamicPrefabDecorator dynamicPrefabDecorator = GameManager.Instance.GetDynamicPrefabDecorator();
			if (dynamicPrefabDecorator != null)
			{
				dynamicPrefabDecorator.GetAllPrefabs(this.dynamicPrefabBuffer);
			}
			if (this.dynamicPrefabBuffer.Count > 0)
			{
				for (int j = 0; j < this.dynamicPrefabBuffer.Count; j++)
				{
					PrefabInstance prefabInstance = this.dynamicPrefabBuffer[j];
					Vector3 vector = prefabInstance.boundingBoxPosition.ToVector3() + prefabInstance.boundingBoxSize.ToVector3() * 0.5f;
					this.updateMapObject(EnumMapObjectType.Prefab, ((long)prefabInstance.id << 32) | (long)((ulong)(-3)), prefabInstance.name, vector, prefabInstance.boundingBoxSize.ToVector3(), this.prefabMapSpritePrefab);
				}
			}
		}
		this.updateNavObjectList();
		foreach (KeyValuePair<int, NavObject> keyValuePair in this.keyToNavObject.Dict)
		{
			if (!this.navObjectsOnMapAlive.Contains((long)keyValuePair.Key))
			{
				this.keyToNavObject.MarkToRemove(keyValuePair.Key);
				this.keyToNavSprite.MarkToRemove(keyValuePair.Key);
			}
		}
		foreach (KeyValuePair<long, MapObject> keyValuePair2 in this.keyToMapObject.Dict)
		{
			if (!this.mapObjectsOnMapAlive.Contains(keyValuePair2.Key))
			{
				this.keyToMapObject.MarkToRemove(keyValuePair2.Key);
				this.keyToMapSprite.MarkToRemove(keyValuePair2.Key);
			}
		}
		this.keyToNavObject.RemoveAllMarked(delegate(int _key)
		{
			this.keyToNavObject.Remove(_key);
		});
		this.keyToNavSprite.RemoveAllMarked(delegate(int _key)
		{
			global::UnityEngine.Object.Destroy(this.keyToNavSprite[_key]);
			this.keyToNavSprite.Remove(_key);
		});
		this.keyToMapObject.RemoveAllMarked(delegate(long _key)
		{
			this.keyToMapObject.Remove(_key);
		});
		this.keyToMapSprite.RemoveAllMarked(delegate(long _key)
		{
			global::UnityEngine.Object.Destroy(this.keyToMapSprite[_key]);
			this.keyToMapSprite.Remove(_key);
		});
		this.localPlayer.selectedSpawnPointKey = -1L;
	}

	// Token: 0x06007C74 RID: 31860 RVA: 0x002F3950 File Offset: 0x002F1B50
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateWaypointSelection()
	{
		Vector2 screenPosition = this.xui.playerUI.CursorController.GetScreenPosition();
		GameObject gameObject = null;
		float num = float.MaxValue;
		this.closestMouseOverNavObject = null;
		foreach (NavObject navObject in NavObjectManager.Instance.NavObjectList)
		{
			if (navObject != null && navObject.NavObjectClass != null && (!(navObject.TrackedEntity != null) || navObject.TrackedEntity.entityId != GameManager.Instance.World.GetPrimaryPlayerId()))
			{
				GameObject gameObject2 = this.keyToNavSprite[navObject.Key];
				if (gameObject2 != null)
				{
					Vector3 vector = this.xui.playerUI.uiCamera.cachedCamera.WorldToScreenPoint(gameObject2.transform.position);
					float num2 = Vector3.Distance(screenPosition, vector);
					if (num2 <= 20f && (this.closestMouseOverNavObject == null || (this.closestMouseOverNavObject != null && num2 < num)))
					{
						this.closestMouseOverNavObject = navObject;
						num = num2;
						gameObject = gameObject2;
					}
				}
			}
		}
		if (this.closestMouseOverNavObject != null)
		{
			if (this.selectMapSprite != gameObject)
			{
				if (this.selectMapSprite != null)
				{
					this.selectMapSprite.transform.localScale = Vector3.one;
				}
				this.selectMapSprite = gameObject;
				this.selectMapSprite.transform.localScale = Vector3.one * 1.5f;
				return;
			}
		}
		else if (this.selectMapSprite != null)
		{
			this.selectMapSprite.transform.localScale = Vector3.one;
			this.selectMapSprite = null;
		}
	}

	// Token: 0x06007C75 RID: 31861 RVA: 0x002F3B1C File Offset: 0x002F1D1C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 worldPosToScreenPos(Vector3 _worldPos)
	{
		return new Vector3((_worldPos.x - this.mapMiddlePosPixel.x) * 2.1190476f / this.zoomScale + (float)this.cTexMiddle.x, (_worldPos.z - this.mapMiddlePosPixel.y) * 2.1190476f / this.zoomScale - (float)this.cTexMiddle.y, 0f);
	}

	// Token: 0x06007C76 RID: 31862 RVA: 0x002F3B8C File Offset: 0x002F1D8C
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 mousePosToWindowPos(Vector3 _mousePos)
	{
		Vector2i mouseXUiPosition = this.xui.GetMouseXUiPosition();
		Vector3 vector = new Vector3((float)mouseXUiPosition.x, (float)mouseXUiPosition.y, 0f);
		vector.x += 217f;
		vector.y -= 362f;
		vector.y = -vector.y;
		return vector * 0.9493333f;
	}

	// Token: 0x06007C77 RID: 31863 RVA: 0x002F3BF8 File Offset: 0x002F1DF8
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 screenPosToWorldPos(Vector3 _mousePos, bool needY = false)
	{
		Vector3 vector = _mousePos;
		Vector3 vector2 = this.xui.playerUI.camera.WorldToScreenPoint(this.xuiTexture.UiTransform.position);
		vector.x -= vector2.x;
		vector.y -= vector2.y;
		vector.y *= -1f;
		Bounds xuiWindowScreenBounds = this.xui.GetXUiWindowScreenBounds(this.xuiTexture.UiTransform, false);
		Vector3 vector3 = xuiWindowScreenBounds.max - xuiWindowScreenBounds.min;
		float num = vector3.x / 336f;
		float num2 = (vector.x - vector3.x / 2f) / num * this.zoomScale + this.mapMiddlePosPixel.x;
		float num3 = -(vector.y - vector3.y / 2f) / num * this.zoomScale + this.mapMiddlePosPixel.y;
		float num4 = 0f;
		if (needY)
		{
			num4 = GameManager.Instance.World.GetHeightAt(num2, num3);
		}
		return new Vector3(num2, num4, num3);
	}

	// Token: 0x06007C78 RID: 31864 RVA: 0x002F3D1C File Offset: 0x002F1F1C
	[PublicizedFrom(EAccessModifier.Private)]
	public void teleportPlayerOnMap(Vector3 _screenPosition)
	{
		Vector3 vector = this.screenPosToWorldPos(_screenPosition, false);
		this.localPlayer.Teleport(new Vector3(vector.x, 180f, vector.z), float.MinValue);
	}

	// Token: 0x06007C79 RID: 31865 RVA: 0x002F3D58 File Offset: 0x002F1F58
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMapPressedLeft(XUiController _sender, int _mouseButton)
	{
		this.closeAllPopups();
		if (this.closestMouseOverNavObject == null)
		{
			if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
			{
				this.OpenWaypointPopup();
			}
			return;
		}
		this.closestMouseOverNavObject.hiddenOnCompass = !this.closestMouseOverNavObject.hiddenOnCompass;
		if (this.closestMouseOverNavObject.hiddenOnCompass)
		{
			GameManager.ShowTooltip(GameManager.Instance.World.GetPrimaryPlayer(), Localization.Get("compassWaypointHiddenTooltip", false, null), false, false, 0f);
		}
		if (this.closestMouseOverNavObject.NavObjectClass.NavObjectClassName == "quick_waypoint")
		{
			this.xui.playerUI.entityPlayer.navMarkerHidden = this.closestMouseOverNavObject.hiddenOnCompass;
			return;
		}
		Waypoint waypointForNavObject = this.xui.playerUI.entityPlayer.Waypoints.GetWaypointForNavObject(this.closestMouseOverNavObject);
		if (waypointForNavObject != null)
		{
			waypointForNavObject.hiddenOnCompass = this.closestMouseOverNavObject.hiddenOnCompass;
		}
	}

	// Token: 0x06007C7A RID: 31866 RVA: 0x002F3E50 File Offset: 0x002F2050
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMapPressed(XUiController _sender, int _mouseButton)
	{
		if (PlatformManager.NativePlatform.Input.CurrentInputStyle != PlayerInputManager.InputStyle.Keyboard)
		{
			return;
		}
		this.closeAllPopups();
		UICamera uiCamera = this.xui.playerUI.uiCamera;
		if (InputUtils.ControlKeyPressed)
		{
			if (GameStats.GetBool(EnumGameStats.IsTeleportEnabled) || GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
			{
				this.teleportPlayerOnMap(this.xui.playerUI.CursorController.GetScreenPosition());
				return;
			}
		}
		else
		{
			this.OpenWaypointPopup();
		}
	}

	// Token: 0x06007C7B RID: 31867 RVA: 0x002F3EC8 File Offset: 0x002F20C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenWaypointPopup()
	{
		this.nextMarkerMousePosition = this.xui.playerUI.CursorController.GetScreenPosition();
		this.xui.GetWindow("mapAreaChooseWaypoint").IsVisible = false;
		XUiV_Window window = this.xui.GetWindow("mapAreaSetWaypoint");
		window.Position = this.xui.GetMouseXUiPosition();
		window.IsVisible = true;
		this.xui.playerUI.CursorController.SetNavigationTargetLater(window.Controller.GetChildById("opt1").ViewComponent);
	}

	// Token: 0x06007C7C RID: 31868 RVA: 0x002F3F5E File Offset: 0x002F215E
	[PublicizedFrom(EAccessModifier.Private)]
	public void onMapHover(XUiController _sender, bool _isOver)
	{
		this.bMouseOverMap = _isOver;
		XUiC_Paging.IsOverPagingOverrideElement = _isOver;
	}

	// Token: 0x06007C7D RID: 31869 RVA: 0x002F3F6D File Offset: 0x002F216D
	[PublicizedFrom(EAccessModifier.Private)]
	public void onPlayerIconPressed(XUiController _sender, int _mouseButton)
	{
		this.PositionMapAt(this.localPlayer.GetPosition());
		this.closeAllPopups();
	}

	// Token: 0x06007C7E RID: 31870 RVA: 0x002F3F88 File Offset: 0x002F2188
	[PublicizedFrom(EAccessModifier.Private)]
	public void onBedrollIconPressed(XUiController _sender, int _mouseButton)
	{
		if (this.localPlayer.SpawnPoints.Count == 0)
		{
			return;
		}
		this.PositionMapAt(this.localPlayer.SpawnPoints[0].ToVector3());
		this.closeAllPopups();
	}

	// Token: 0x06007C7F RID: 31871 RVA: 0x002F3FCD File Offset: 0x002F21CD
	public void OnSetWaypoint()
	{
		this.localPlayer.navMarkerHidden = false;
		this.localPlayer.markerPosition = World.worldToBlockPos(this.screenPosToWorldPos(this.nextMarkerMousePosition, true));
		this.closeAllPopups();
	}

	// Token: 0x06007C80 RID: 31872 RVA: 0x002F3FFE File Offset: 0x002F21FE
	public void OnWaypointEntryChosen(string _iconName)
	{
		this.currentWaypointIconChosen = _iconName;
	}

	// Token: 0x06007C81 RID: 31873 RVA: 0x002F4008 File Offset: 0x002F2208
	public void OnWaypointCreated(string _name)
	{
		Waypoint w = new Waypoint();
		w.pos = World.worldToBlockPos(this.screenPosToWorldPos(this.nextMarkerMousePosition, true));
		w.icon = this.currentWaypointIconChosen;
		w.name.Update(_name, PlatformManager.MultiPlatform.User.PlatformUserId);
		this.xui.playerUI.entityPlayer.Waypoints.Collection.Add(w);
		this.closeAllPopups();
		((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
		w.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", w.pos.ToVector3(), w.icon, false, -1, null);
		w.navObject.IsActive = false;
		w.navObject.usingLocalizationId = w.bUsingLocalizationId;
		if (w.bIsAutoWaypoint || w.bUsingLocalizationId)
		{
			w.navObject.name = Localization.Get(w.name.Text, false, null);
		}
		else
		{
			GeneratedTextManager.GetDisplayText(w.name, delegate(string _filtered)
			{
				w.navObject.name = _filtered;
			}, true, false, GeneratedTextManager.TextFilteringMode.FilterWithSafeString, GeneratedTextManager.BbCodeSupportMode.SupportedAndAddEscapes);
		}
		this.selectWaypoint(w);
		Manager.PlayInsidePlayerHead("ui_waypoint_add", -1, 0f, false, false);
	}

	// Token: 0x06007C82 RID: 31874 RVA: 0x002F419F File Offset: 0x002F239F
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxStaticMapType_OnValueChanged(XUiController _sender, XUiC_MapArea.EStaticMapOverlay _oldvalue, XUiC_MapArea.EStaticMapOverlay _newvalue)
	{
		this.staticWorldTexture = null;
	}

	// Token: 0x06007C83 RID: 31875 RVA: 0x002F41A8 File Offset: 0x002F23A8
	public void RefreshVehiclePositionWaypoint(EntityVehicle _vehicle, bool _unloaded)
	{
		Log.Out("Refresh Vehicle Position Waypoint {0} {1}", new object[] { _vehicle.entityId, _unloaded });
		Waypoint waypoint = new Waypoint();
		waypoint.pos = World.worldToBlockPos(_vehicle.position);
		waypoint.icon = _vehicle.GetMapIcon();
		waypoint.ownerId = _vehicle.GetVehicle().OwnerId;
		waypoint.name.Update(Localization.Get(_vehicle.EntityName, false, null), PlatformManager.MultiPlatform.User.PlatformUserId);
		waypoint.lastKnownPositionEntityType = eLastKnownPositionEntityType.Vehicle;
		waypoint.lastKnownPositionEntityId = _vehicle.entityId;
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (!entityPlayer.Waypoints.ContainsLastKnownPositionWaypoint(_vehicle.entityId))
		{
			entityPlayer.Waypoints.Collection.Add(waypoint);
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
				waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", waypoint.pos.ToVector3(), waypoint.icon, false, -1, null);
				waypoint.navObject.IsActive = false;
				waypoint.navObject.OverrideSpriteName = _vehicle.GetMapIcon();
				waypoint.navObject.name = waypoint.name.Text;
				waypoint.navObject.usingLocalizationId = waypoint.bUsingLocalizationId;
				return;
			}
			this.RemoveVehicleLastKnownWaypoint(_vehicle);
			return;
		}
		else
		{
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				entityPlayer.Waypoints.UpdateEntityVehicleWayPoint(_vehicle, _unloaded);
				return;
			}
			this.RemoveVehicleLastKnownWaypoint(_vehicle);
			return;
		}
	}

	// Token: 0x06007C84 RID: 31876 RVA: 0x002F4348 File Offset: 0x002F2548
	public void RefreshAnimalPositionWaypoint(EntityAnimal _animal, bool _unloaded)
	{
		Log.Out("Refresh animal Position Waypoint {0} {1}", new object[] { _animal.entityId, _unloaded });
		Waypoint waypoint = new Waypoint();
		waypoint.pos = World.worldToBlockPos(_animal.position);
		waypoint.icon = _animal.GetMapIcon();
		waypoint.ownerId = PlatformManager.InternalLocalUserIdentifier;
		waypoint.name.Update(Localization.Get(_animal.EntityName, false, null), PlatformManager.MultiPlatform.User.PlatformUserId);
		waypoint.lastKnownPositionEntityType = eLastKnownPositionEntityType.Animal;
		waypoint.lastKnownPositionEntityId = _animal.entityId;
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		if (!entityPlayer.Waypoints.ContainsLastKnownPositionWaypoint(_animal.entityId))
		{
			entityPlayer.Waypoints.Collection.Add(waypoint);
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
				waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", waypoint.pos.ToVector3(), waypoint.icon, false, -1, null);
				waypoint.navObject.IsActive = false;
				waypoint.navObject.OverrideSpriteName = _animal.GetMapIcon();
				waypoint.navObject.name = waypoint.name.Text;
				waypoint.navObject.usingLocalizationId = waypoint.bUsingLocalizationId;
				return;
			}
			this.RemoveAnimalLastKnownWaypoint(_animal);
			return;
		}
		else
		{
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				entityPlayer.Waypoints.UpdateEntityAnimalWayPoint(_animal, _unloaded);
				return;
			}
			this.RemoveAnimalLastKnownWaypoint(_animal);
			return;
		}
	}

	// Token: 0x06007C85 RID: 31877 RVA: 0x002F44DF File Offset: 0x002F26DF
	public void RemoveVehicleLastKnownWaypoint(EntityVehicle _vehicle)
	{
		if (this.xui.playerUI.entityPlayer.Waypoints.TryRemoveLastKnownPositionWaypoint(_vehicle.entityId))
		{
			((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
		}
	}

	// Token: 0x06007C86 RID: 31878 RVA: 0x002F44DF File Offset: 0x002F26DF
	public void RemoveAnimalLastKnownWaypoint(EntityAnimal _animal)
	{
		if (this.xui.playerUI.entityPlayer.Waypoints.TryRemoveLastKnownPositionWaypoint(_animal.entityId))
		{
			((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
		}
	}

	// Token: 0x06007C87 RID: 31879 RVA: 0x002F4520 File Offset: 0x002F2720
	public void RefreshDronePositionWaypoint(EntityDrone _drone, bool _unloaded)
	{
		Log.Out("Refresh Drone Position Waypoint {0} {1}", new object[] { _drone.entityId, _unloaded });
		Waypoint waypoint = new Waypoint();
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		waypoint.pos = World.worldToBlockPos(_drone.position);
		waypoint.icon = _drone.GetMapIcon();
		waypoint.ownerId = _drone.OwnerID;
		waypoint.name.Update(Localization.Get(_drone.EntityName, false, null), PlatformManager.MultiPlatform.User.PlatformUserId);
		waypoint.lastKnownPositionEntityType = eLastKnownPositionEntityType.Drone;
		waypoint.lastKnownPositionEntityId = _drone.entityId;
		if (!entityPlayer.Waypoints.ContainsLastKnownPositionWaypoint(_drone.entityId))
		{
			entityPlayer.Waypoints.Collection.Add(waypoint);
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
				waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", waypoint.pos.ToVector3(), waypoint.icon, false, -1, null);
				waypoint.navObject.IsActive = false;
				waypoint.navObject.OverrideSpriteName = _drone.GetMapIcon();
				waypoint.navObject.name = waypoint.name.Text;
				waypoint.navObject.usingLocalizationId = waypoint.bUsingLocalizationId;
				return;
			}
			this.RemoveDronePositionWaypoint(_drone.entityId);
			return;
		}
		else
		{
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				entityPlayer.Waypoints.UpdateEntityDroneWayPoint(_drone, _drone.OrderState == EntityDrone.Orders.Follow, _unloaded);
				return;
			}
			this.RemoveDronePositionWaypoint(_drone.entityId);
			return;
		}
	}

	// Token: 0x06007C88 RID: 31880 RVA: 0x002F46CC File Offset: 0x002F28CC
	public void RefreshDronePositionWaypoint(int _entityId, Vector3i _pos, bool _unloaded)
	{
		Waypoint waypoint = new Waypoint();
		EntityClass entityClass = EntityClass.list[EntityClass.junkDroneClass];
		EntityPlayer entityPlayer = this.xui.playerUI.entityPlayer;
		waypoint.pos = _pos;
		waypoint.icon = entityClass.Properties.GetString(EntityClass.PropMapIcon);
		waypoint.ownerId = PlatformManager.MultiPlatform.User.PlatformUserId;
		waypoint.name.Update(Localization.Get(entityClass.entityClassName, false, null), PlatformManager.MultiPlatform.User.PlatformUserId);
		waypoint.lastKnownPositionEntityType = eLastKnownPositionEntityType.Drone;
		waypoint.lastKnownPositionEntityId = _entityId;
		if (!entityPlayer.Waypoints.ContainsLastKnownPositionWaypoint(_entityId))
		{
			entityPlayer.Waypoints.Collection.Add(waypoint);
			if (waypoint.CanBeViewedBy(PlatformManager.InternalLocalUserIdentifier))
			{
				((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
				waypoint.navObject = NavObjectManager.Instance.RegisterNavObject("waypoint", waypoint.pos.ToVector3(), waypoint.icon, false, -1, null);
				waypoint.navObject.IsActive = false;
				waypoint.navObject.OverrideSpriteName = waypoint.icon;
				waypoint.navObject.name = waypoint.name.Text;
				waypoint.navObject.usingLocalizationId = waypoint.bUsingLocalizationId;
				return;
			}
			this.RemoveDronePositionWaypoint(_entityId);
		}
	}

	// Token: 0x06007C89 RID: 31881 RVA: 0x002F4829 File Offset: 0x002F2A29
	public void RemoveDronePositionWaypoint(int _entityId)
	{
		if (this.xui.playerUI.entityPlayer.Waypoints.TryRemoveLastKnownPositionWaypoint(_entityId))
		{
			((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).UpdateWaypointsList(null);
		}
	}

	// Token: 0x06007C8A RID: 31882 RVA: 0x002F4864 File Offset: 0x002F2A64
	[PublicizedFrom(EAccessModifier.Private)]
	public void onWaypointIconPressed(XUiController _sender, int _mouseButton)
	{
		EntityPlayerLocal entityPlayer = this.xui.playerUI.entityPlayer;
		if (this.localPlayer.markerPosition == Vector3i.zero)
		{
			Manager.PlayInsidePlayerHead("ui_denied", -1, 0f, false, false);
		}
		else
		{
			Manager.PlayInsidePlayerHead("ui_waypoint_delete", -1, 0f, false, false);
		}
		this.localPlayer.markerPosition = Vector3i.zero;
	}

	// Token: 0x06007C8B RID: 31883 RVA: 0x002F48CF File Offset: 0x002F2ACF
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectWaypoint(Waypoint _w)
	{
		((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).SelectWaypoint(_w);
	}

	// Token: 0x06007C8C RID: 31884 RVA: 0x002F48EC File Offset: 0x002F2AEC
	[PublicizedFrom(EAccessModifier.Private)]
	public void selectWaypoint(NavObject _nav)
	{
		((XUiC_MapWaypointList)base.Parent.GetChildById("waypointList")).SelectWaypoint(_nav);
	}

	// Token: 0x06007C8D RID: 31885 RVA: 0x002F490C File Offset: 0x002F2B0C
	public void closeAllPopups()
	{
		this.xui.GetWindow("mapAreaSetWaypoint").IsVisible = false;
		this.xui.GetWindow("mapAreaChooseWaypoint").IsVisible = false;
		this.xui.GetWindow("mapAreaEnterWaypointName").IsVisible = false;
		this.xui.GetWindow("mapTrackingPopup").IsVisible = false;
		this.mapView.SelectCursorElement(false, false);
	}

	// Token: 0x06007C8E RID: 31886 RVA: 0x002F4980 File Offset: 0x002F2B80
	public void PositionMapAt(Vector3 _worldPos)
	{
		int num = (int)_worldPos.x;
		int num2 = (int)_worldPos.z;
		this.mapMiddlePosChunks = new Vector2((float)(World.toChunkXZ(num - 1024) * 16 + 1024), (float)(World.toChunkXZ(num2 - 1024) * 16 + 1024));
		this.mapMiddlePosPixel = this.mapMiddlePosChunks;
		this.mapMiddlePosPixel = GameManager.Instance.World.ClampToValidWorldPosForMap(this.mapMiddlePosPixel);
		this.updateFullMap();
	}

	// Token: 0x06007C8F RID: 31887 RVA: 0x002F4A06 File Offset: 0x002F2C06
	public override void Cleanup()
	{
		base.Cleanup();
		global::UnityEngine.Object.Destroy(this.mapTexture);
	}

	// Token: 0x04005BFC RID: 23548
	public const int MapDrawnSizeInChunks = 128;

	// Token: 0x04005BFD RID: 23549
	public const int MapDrawnSize = 2048;

	// Token: 0x04005BFE RID: 23550
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMinZoomScale = 0.7f;

	// Token: 0x04005BFF RID: 23551
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cMaxZoomScale = 6.15f;

	// Token: 0x04005C00 RID: 23552
	[PublicizedFrom(EAccessModifier.Private)]
	public int cSpriteScale = 50;

	// Token: 0x04005C01 RID: 23553
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MapOnScreenSize = 712;

	// Token: 0x04005C02 RID: 23554
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i cTexMiddle = new Vector2i(356, 356);

	// Token: 0x04005C03 RID: 23555
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MapSizeFull = 2048;

	// Token: 0x04005C04 RID: 23556
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MapSizeZoom1 = 336;

	// Token: 0x04005C05 RID: 23557
	[PublicizedFrom(EAccessModifier.Private)]
	public const float factorScreenSizeToDTM = 2.1190476f;

	// Token: 0x04005C06 RID: 23558
	[PublicizedFrom(EAccessModifier.Private)]
	public const float dragFactorSizeOfMap = 0.47191012f;

	// Token: 0x04005C07 RID: 23559
	[PublicizedFrom(EAccessModifier.Private)]
	public bool showStaticData;

	// Token: 0x04005C08 RID: 23560
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32[] staticWorldTexture;

	// Token: 0x04005C09 RID: 23561
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticWorldWidth;

	// Token: 0x04005C0A RID: 23562
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticWorldHeight;

	// Token: 0x04005C0B RID: 23563
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticMapLeft;

	// Token: 0x04005C0C RID: 23564
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticMapRight;

	// Token: 0x04005C0D RID: 23565
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticMapBottom;

	// Token: 0x04005C0E RID: 23566
	[PublicizedFrom(EAccessModifier.Private)]
	public int staticMapTop;

	// Token: 0x04005C0F RID: 23567
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D mapTexture;

	// Token: 0x04005C10 RID: 23568
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte mapMaskTransparency = 255;

	// Token: 0x04005C11 RID: 23569
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[][] fowChunkMaskAlphas = new byte[13][];

	// Token: 0x04005C12 RID: 23570
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapScrollTextureOffset;

	// Token: 0x04005C13 RID: 23571
	[PublicizedFrom(EAccessModifier.Private)]
	public int mapScrollTextureChunksOffsetX;

	// Token: 0x04005C14 RID: 23572
	[PublicizedFrom(EAccessModifier.Private)]
	public int mapScrollTextureChunksOffsetZ;

	// Token: 0x04005C15 RID: 23573
	[PublicizedFrom(EAccessModifier.Private)]
	public bool existingWaypointsInitialized;

	// Token: 0x04005C16 RID: 23574
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bMapInitialized;

	// Token: 0x04005C17 RID: 23575
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bShouldRedrawMap;

	// Token: 0x04005C18 RID: 23576
	[PublicizedFrom(EAccessModifier.Private)]
	public float timeToRedrawMap;

	// Token: 0x04005C19 RID: 23577
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bFowMaskEnabled;

	// Token: 0x04005C1A RID: 23578
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapMiddlePosChunks;

	// Token: 0x04005C1B RID: 23579
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapMiddlePosPixel;

	// Token: 0x04005C1C RID: 23580
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapMiddlePosChunksToServer;

	// Token: 0x04005C1D RID: 23581
	[PublicizedFrom(EAccessModifier.Private)]
	public float zoomScale;

	// Token: 0x04005C1E RID: 23582
	[PublicizedFrom(EAccessModifier.Private)]
	public float targetZoomScale;

	// Token: 0x04005C1F RID: 23583
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer localPlayer;

	// Token: 0x04005C20 RID: 23584
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Texture xuiTexture;

	// Token: 0x04005C21 RID: 23585
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 defaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	// Token: 0x04005C22 RID: 23586
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 hoverColor = new Color32(222, 206, 163, byte.MaxValue);

	// Token: 0x04005C23 RID: 23587
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 disabledColor = new Color32(96, 96, 96, byte.MaxValue);

	// Token: 0x04005C24 RID: 23588
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<long, MapObject> keyToMapObject = new DictionarySave<long, MapObject>();

	// Token: 0x04005C25 RID: 23589
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<int, NavObject> keyToNavObject = new DictionarySave<int, NavObject>();

	// Token: 0x04005C26 RID: 23590
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<int, GameObject> keyToNavSprite = new DictionarySave<int, GameObject>();

	// Token: 0x04005C27 RID: 23591
	[PublicizedFrom(EAccessModifier.Private)]
	public DictionarySave<long, GameObject> keyToMapSprite = new DictionarySave<long, GameObject>();

	// Token: 0x04005C28 RID: 23592
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong navObjectsOnMapAlive = new HashSetLong();

	// Token: 0x04005C29 RID: 23593
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSetLong mapObjectsOnMapAlive = new HashSetLong();

	// Token: 0x04005C2A RID: 23594
	[PublicizedFrom(EAccessModifier.Private)]
	public NavObject closestMouseOverNavObject;

	// Token: 0x04005C2B RID: 23595
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabMapSprite;

	// Token: 0x04005C2C RID: 23596
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabMapSpriteStartPoint;

	// Token: 0x04005C2D RID: 23597
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabMapSpritePrefab;

	// Token: 0x04005C2E RID: 23598
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabMapSpriteEntitySpawner;

	// Token: 0x04005C2F RID: 23599
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform transformSpritesParent;

	// Token: 0x04005C30 RID: 23600
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label uiLblPlayerPos;

	// Token: 0x04005C31 RID: 23601
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label uiLblBedrollPos;

	// Token: 0x04005C32 RID: 23602
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label uiLblCursorPos;

	// Token: 0x04005C33 RID: 23603
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label uiLblMapMarkerDistance;

	// Token: 0x04005C34 RID: 23604
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Label uiLblMapMarkerInfo;

	// Token: 0x04005C35 RID: 23605
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Button switchStaticMap;

	// Token: 0x04005C36 RID: 23606
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<XUiC_MapArea.EStaticMapOverlay> cbxStaticMapType;

	// Token: 0x04005C37 RID: 23607
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiController mapView;

	// Token: 0x04005C38 RID: 23608
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiV_Sprite crosshair;

	// Token: 0x04005C39 RID: 23609
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bMouseOverMap;

	// Token: 0x04005C3A RID: 23610
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 nextMarkerMousePosition = Vector3.zero;

	// Token: 0x04005C3B RID: 23611
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005C3C RID: 23612
	[PublicizedFrom(EAccessModifier.Private)]
	public string kilometers;

	// Token: 0x04005C3D RID: 23613
	[PublicizedFrom(EAccessModifier.Private)]
	public float mapScale = 1f;

	// Token: 0x04005C3E RID: 23614
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapPos;

	// Token: 0x04005C3F RID: 23615
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2 mapBGPos;

	// Token: 0x04005C40 RID: 23616
	[PublicizedFrom(EAccessModifier.Private)]
	public const float ZOOM_SPEED = 5f;

	// Token: 0x04005C41 RID: 23617
	[PublicizedFrom(EAccessModifier.Private)]
	public const KeyCode dragKey = KeyCode.Mouse0;

	// Token: 0x04005C42 RID: 23618
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MaxMapSymbolSize = 100;

	// Token: 0x04005C43 RID: 23619
	[PublicizedFrom(EAccessModifier.Private)]
	public const int MinMapSymbolSize = 9;

	// Token: 0x04005C44 RID: 23620
	[PublicizedFrom(EAccessModifier.Private)]
	public const float mapSpriteMouseOverDistance = 20f;

	// Token: 0x04005C45 RID: 23621
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject selectMapSprite;

	// Token: 0x04005C46 RID: 23622
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PrefabInstance> dynamicPrefabBuffer = new List<PrefabInstance>();

	// Token: 0x04005C47 RID: 23623
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bMapCursorSet;

	// Token: 0x04005C48 RID: 23624
	[PublicizedFrom(EAccessModifier.Private)]
	public string currentWaypointIconChosen;

	// Token: 0x02000F68 RID: 3944
	public enum EStaticMapOverlay
	{
		// Token: 0x04005C4A RID: 23626
		None,
		// Token: 0x04005C4B RID: 23627
		Radiation,
		// Token: 0x04005C4C RID: 23628
		Biomes
	}
}
