using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F97 RID: 3991
[Preserve]
public class XUiC_LightEditor : XUiController
{
	// Token: 0x06007D34 RID: 32052 RVA: 0x002F6CC0 File Offset: 0x002F4EC0
	public static void Open(LocalPlayerUI _playerUi, TileEntityLight _te, Vector3i _blockPos, World _world, BlockLight _block)
	{
		XUiC_LightEditor childByType = _playerUi.xui.FindWindowGroupByName(XUiC_LightEditor.ID).GetChildByType<XUiC_LightEditor>();
		childByType.tileEntityLight = _te;
		childByType.blockPos = _blockPos;
		childByType.world = _world;
		ChunkCluster chunkCache = _world.ChunkCache;
		childByType.chunk = (Chunk)chunkCache.GetChunkSync(World.toChunkXZ(_blockPos.x), _blockPos.y, World.toChunkXZ(_blockPos.z));
		Transform transform = childByType.chunk.GetBlockEntity(_blockPos).transform.Find("MainLight");
		if (transform != null)
		{
			childByType.lightLOD = transform.GetComponent<LightLOD>();
			if (childByType.lightLOD != null)
			{
				childByType.light = childByType.lightLOD.GetLight();
			}
		}
		childByType.OpenAllValues();
		childByType.block = _block;
		_playerUi.windowManager.Open(XUiC_LightEditor.ID, true);
		if (childByType.light != null && childByType.light.type == LightType.Point)
		{
			childByType.panelAngle.IsVisible = false;
			childByType.rangeGizmo = global::UnityEngine.Object.Instantiate<WireFrameSphere>(Resources.Load<WireFrameSphere>("Prefabs/prefabSphereWF"), childByType.light.transform);
			childByType.rangeGizmo.center = childByType.light.gameObject.transform.position;
			childByType.rangeGizmo.name = "Range Gizmo";
			childByType.rangeGizmo.newRadius = childByType.light.range;
		}
		if (childByType.lightLOD != null)
		{
			if (childByType.lightLOD.LightStateType == LightStateType.Static || childByType.lightLOD.LightStateType == LightStateType.Fluctuating)
			{
				childByType.panelRate.IsVisible = false;
			}
			if (childByType.lightLOD.LightStateType != LightStateType.Fluctuating)
			{
				childByType.panelDelay.IsVisible = false;
			}
		}
		if (!XUiC_LightEditor.hasCopiedValues)
		{
			childByType.btnPaste.ViewComponent.IsVisible = false;
		}
		childByType.CopyUIValues(out childByType.startValues);
	}

	// Token: 0x06007D35 RID: 32053 RVA: 0x002F6EA0 File Offset: 0x002F50A0
	public override void Init()
	{
		base.Init();
		XUiC_LightEditor.ID = base.WindowGroup.Id;
		this.cbxLightType = (XUiC_ComboBoxList<LightType>)base.GetChildById("cbxLightType");
		this.cbxLightType.Elements.Add(LightType.Point);
		this.cbxLightType.Elements.Add(LightType.Spot);
		this.cbxLightType.OnValueChanged += this.CbxLightType_OnValueChanged;
		this.cbxLightShadow = (XUiC_ComboBoxEnum<LightShadows>)base.GetChildById("cbxLightShadow");
		this.cbxLightShadow.OnValueChanged += this.CbxLightShadow_OnValueChanged;
		this.cbxColorPresetList = (XUiC_ComboBoxList<string>)base.GetChildById("cbxPresets");
		this.cbxLightState = (XUiC_ComboBoxEnum<LightStateType>)base.GetChildById("cbxLightState");
		this.cbxLightState.OnValueChanged += this.CbxLightState_OnValueChanged;
		foreach (KeyValuePair<string, Color> keyValuePair in XUiC_LightEditor.ColorPresets)
		{
			this.cbxColorPresetList.Elements.Add(keyValuePair.Key);
		}
		this.cbxColorPresetList.OnValueChanged += this.CbxColorPresetList_OnValueChanged;
		this.colorPicker = (XUiC_ColorPicker)base.GetChildById("lightColor");
		this.colorPicker.OnSelectedColorChanged += this.ColorPicker_OnSelectedColorChanged;
		this.panelAngle = base.GetChildById("panelAngle").ViewComponent;
		this.panelRate = base.GetChildById("panelRate").ViewComponent;
		this.panelDelay = base.GetChildById("panelDelay").ViewComponent;
		this.cbxRange = (XUiC_ComboBoxFloat)base.GetChildById("cbxRange");
		this.cbxRange.OnValueChanged += this.CbxRange_OnValueChanged;
		this.cbxAngle = (XUiC_ComboBoxFloat)base.GetChildById("cbxAngle");
		this.cbxAngle.OnValueChanged += this.CbxAngle_OnValueChanged;
		this.cbxRate = (XUiC_ComboBoxFloat)base.GetChildById("cbxRate");
		this.cbxRate.OnValueChanged += this.CbxRate_OnValueChanged;
		this.cbxDelay = (XUiC_ComboBoxFloat)base.GetChildById("cbxDelay");
		this.cbxDelay.OnValueChanged += this.CbxDelay_OnValueChanged;
		this.cbxIntensity = (XUiC_ComboBoxFloat)base.GetChildById("cbxIntensity");
		this.cbxIntensity.OnValueChanged += this.CbxIntensity_OnValueChanged;
		this.btnOk = base.GetChildById("btnOk").GetChildByType<XUiC_SimpleButton>();
		this.btnOk.OnPressed += this.BtnSave_OnPressed;
		this.btnCancel = base.GetChildById("btnCancel").GetChildByType<XUiC_SimpleButton>();
		this.btnCancel.OnPressed += this.BtnCancel_OnPressed;
		this.btnOnOff = base.GetChildById("btnOnOff").GetChildByType<XUiC_SimpleButton>();
		this.btnOnOff.OnPressed += this.BtnOnOff_OnPressed;
		this.btnRestore = base.GetChildById("btnRestoreDefaults").GetChildByType<XUiC_SimpleButton>();
		this.btnRestore.OnPressed += this.BtnRestore_OnPressed;
		this.btnCopy = base.GetChildById("btnCopy").GetChildByType<XUiC_SimpleButton>();
		this.btnCopy.OnPressed += this.BtnCopy_OnPressed;
		this.btnPaste = base.GetChildById("btnPaste").GetChildByType<XUiC_SimpleButton>();
		this.btnPaste.OnPressed += this.BtnPaste_OnPressed;
		this.txtColorR = base.GetChildById("txtColorR") as XUiC_TextInput;
		this.txtColorR.OnChangeHandler += this.TxtColorR_OnChangeHandler;
		this.txtColorG = base.GetChildById("txtColorG") as XUiC_TextInput;
		this.txtColorG.OnChangeHandler += this.TxtColorG_OnChangeHandler;
		this.txtColorB = base.GetChildById("txtColorB") as XUiC_TextInput;
		this.txtColorB.OnChangeHandler += this.TxtColorB_OnChangeHandler;
		this.txtHex = base.GetChildById("txtHex") as XUiC_TextInput;
		UIInput uiinput = this.txtHex.UIInput;
		uiinput.onValidate = (UIInput.OnValidate)Delegate.Combine(uiinput.onValidate, new UIInput.OnValidate(GameUtils.ValidateHexInput));
		this.txtHex.OnChangeHandler += this.TxtHex_OnChangeHandler;
	}

	// Token: 0x06007D36 RID: 32054 RVA: 0x002F732C File Offset: 0x002F552C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnPaste_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (XUiC_LightEditor.hasCopiedValues)
		{
			this.SetIntensity((float)(this.cbxIntensity.Value = (double)XUiC_LightEditor.copyValues.intensity));
			this.SetRange((float)(this.cbxRange.Value = (double)XUiC_LightEditor.copyValues.range));
			this.cbxLightType.Value = XUiC_LightEditor.copyValues.type;
			this.SetType(XUiC_LightEditor.copyValues.type);
			this.SetPreset(this.cbxColorPresetList.Value = XUiC_LightEditor.copyColorPreset);
			this.cbxLightState.Value = XUiC_LightEditor.copyValues.stateType;
			this.SetState(XUiC_LightEditor.copyValues.stateType);
			this.cbxLightShadow.Value = XUiC_LightEditor.copyValues.shadows;
			this.SetShadow(XUiC_LightEditor.copyValues.shadows);
			this.SetAngle((float)(this.cbxAngle.Value = (double)XUiC_LightEditor.copyValues.spotAngle));
			this.SetRate((float)(this.cbxRate.Value = (double)XUiC_LightEditor.copyValues.stateRate));
			this.SetDelay((float)(this.cbxDelay.Value = (double)XUiC_LightEditor.copyValues.stateDelay));
			this.colorPicker.SelectedColor = XUiC_LightEditor.copyValues.color;
			this.SetHex(XUiC_LightEditor.copyValues.color);
			this.SetRGB(XUiC_LightEditor.copyValues.color);
			this.panelAngle.IsVisible = XUiC_LightEditor.copyValues.type != LightType.Point;
			if (this.rangeGizmo != null)
			{
				this.rangeGizmo.gameObject.GetComponent<LineRenderer>().enabled = XUiC_LightEditor.copyValues.type == LightType.Point;
			}
		}
	}

	// Token: 0x06007D37 RID: 32055 RVA: 0x002F74F5 File Offset: 0x002F56F5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CopyUIValues(out XUiC_LightEditor.copyValues);
		XUiC_LightEditor.hasCopiedValues = true;
		XUiC_LightEditor.copyColorPreset = this.cbxColorPresetList.Value;
		this.btnPaste.ViewComponent.IsVisible = true;
	}

	// Token: 0x06007D38 RID: 32056 RVA: 0x002F752C File Offset: 0x002F572C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CopyUIValues(out XUiC_LightEditor.LightValues values)
	{
		values = default(XUiC_LightEditor.LightValues);
		values.type = this.cbxLightType.Value;
		values.shadows = this.cbxLightShadow.Value;
		values.intensity = (float)this.cbxIntensity.Value;
		values.range = (float)this.cbxRange.Value;
		values.color = this.colorPicker.SelectedColor;
		values.spotAngle = (float)this.cbxAngle.Value;
		if (this.lightLOD)
		{
			values.emissiveColor = this.lightLOD.EmissiveColor;
		}
		values.stateType = this.cbxLightState.Value;
		values.stateRate = (float)this.cbxRate.Value;
		values.stateDelay = (float)this.cbxDelay.Value;
	}

	// Token: 0x06007D39 RID: 32057 RVA: 0x002F75FC File Offset: 0x002F57FC
	public void AssignRate(float _rate)
	{
		this.cbxRate.Value = (double)_rate;
	}

	// Token: 0x06007D3A RID: 32058 RVA: 0x002F760B File Offset: 0x002F580B
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOnOff_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.SwitchOnOff(!this.lightLOD.bSwitchedOn, true);
			this.SetEmissiveColor(this.light.color);
		}
	}

	// Token: 0x06007D3B RID: 32059 RVA: 0x002F7648 File Offset: 0x002F5848
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtHex_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		if (_text.Length < 6)
		{
			return;
		}
		if (this.isOpen)
		{
			NumberStyles numberStyles = NumberStyles.HexNumber;
			byte b = (byte)int.Parse(_text.Substring(0, 2), numberStyles);
			byte b2 = (byte)int.Parse(_text.Substring(2, 2), numberStyles);
			byte b3 = (byte)int.Parse(_text.Substring(4, 2), numberStyles);
			Color32 color = new Color32(b, b2, b3, byte.MaxValue);
			this.colorPicker.SelectedColor = (this.light.color = color);
			this.SetRGB(color);
			this.SetEmissiveColor(color);
		}
	}

	// Token: 0x06007D3C RID: 32060 RVA: 0x002F76E4 File Offset: 0x002F58E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtColorB_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		int num;
		if (this.isOpen && int.TryParse(_text, out num))
		{
			num = Mathf.Clamp(num, 0, 255);
			if (!_text.Equals(num.ToString()))
			{
				this.txtColorB.Text = num.ToString();
			}
			if (this.light != null)
			{
				Color color = new Color(this.light.color.r, this.light.color.g, (float)num / 255f);
				this.light.color = color;
				this.SetEmissiveColor(color);
				this.colorPicker.SelectedColor = color;
				this.SetHex(color);
				if (this.shouldChangePreset)
				{
					this.cbxColorPresetList.Value = XUiC_LightEditor.ColorPresets.Keys.First<string>();
				}
			}
		}
	}

	// Token: 0x06007D3D RID: 32061 RVA: 0x002F77C4 File Offset: 0x002F59C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtColorG_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		int num;
		if (this.isOpen && int.TryParse(_text, out num))
		{
			num = Mathf.Clamp(num, 0, 255);
			if (!_text.Equals(num.ToString()))
			{
				this.txtColorG.Text = num.ToString();
			}
			if (this.light != null)
			{
				Color color = new Color(this.light.color.r, (float)num / 255f, this.light.color.b);
				this.light.color = color;
				this.SetEmissiveColor(color);
				this.colorPicker.SelectedColor = color;
				this.SetHex(color);
				if (this.shouldChangePreset)
				{
					this.cbxColorPresetList.Value = XUiC_LightEditor.ColorPresets.Keys.First<string>();
				}
			}
		}
	}

	// Token: 0x06007D3E RID: 32062 RVA: 0x002F78A4 File Offset: 0x002F5AA4
	[PublicizedFrom(EAccessModifier.Private)]
	public void TxtColorR_OnChangeHandler(XUiController _sender, string _text, bool _changeFromCode)
	{
		int num;
		if (this.isOpen && int.TryParse(_text, out num))
		{
			num = Mathf.Clamp(num, 0, 255);
			if (!_text.Equals(num.ToString()))
			{
				this.txtColorR.Text = num.ToString();
			}
			if (this.light != null)
			{
				Color color = new Color((float)num / 255f, this.light.color.g, this.light.color.b);
				this.light.color = color;
				this.SetEmissiveColor(color);
				this.colorPicker.SelectedColor = color;
				this.SetHex(color);
				if (this.shouldChangePreset)
				{
					this.cbxColorPresetList.Value = XUiC_LightEditor.ColorPresets.Keys.First<string>();
				}
			}
		}
	}

	// Token: 0x06007D3F RID: 32063 RVA: 0x002F7981 File Offset: 0x002F5B81
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightType_OnValueChanged(XUiController _sender, LightType _oldValue, LightType _newValue)
	{
		this.SetType(_newValue);
	}

	// Token: 0x06007D40 RID: 32064 RVA: 0x002F798A File Offset: 0x002F5B8A
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxColorPresetList_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		this.SetPreset(_newValue);
	}

	// Token: 0x06007D41 RID: 32065 RVA: 0x002F7993 File Offset: 0x002F5B93
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRange_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRange((float)_newValue);
	}

	// Token: 0x06007D42 RID: 32066 RVA: 0x002F799D File Offset: 0x002F5B9D
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIntensity_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetIntensity((float)_newValue);
	}

	// Token: 0x06007D43 RID: 32067 RVA: 0x002F79A7 File Offset: 0x002F5BA7
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxAngle_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetAngle((float)_newValue);
	}

	// Token: 0x06007D44 RID: 32068 RVA: 0x002F79B1 File Offset: 0x002F5BB1
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRate_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRate((float)_newValue);
	}

	// Token: 0x06007D45 RID: 32069 RVA: 0x002F79BB File Offset: 0x002F5BBB
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightState_OnValueChanged(XUiController _sender, LightStateType _oldValue, LightStateType _newValue)
	{
		this.SetState(_newValue);
	}

	// Token: 0x06007D46 RID: 32070 RVA: 0x002F79C4 File Offset: 0x002F5BC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightShadow_OnValueChanged(XUiController _sender, LightShadows _oldValue, LightShadows _newValue)
	{
		this.SetShadow(_newValue);
	}

	// Token: 0x06007D47 RID: 32071 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxDelay_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
	}

	// Token: 0x06007D48 RID: 32072 RVA: 0x002F79D0 File Offset: 0x002F5BD0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnRestore_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (!this.light || !this.lightLOD)
		{
			return;
		}
		this.LoadDefaultValues();
		this.cbxLightType.Value = this.defaultValues.type;
		this.cbxLightShadow.Value = this.defaultValues.shadows;
		this.cbxRange.Value = (double)this.defaultValues.range;
		this.cbxIntensity.Value = (double)this.defaultValues.intensity;
		this.colorPicker.SelectedColor = this.defaultValues.color;
		this.cbxAngle.Value = (double)this.defaultValues.spotAngle;
		this.cbxLightState.Value = this.defaultValues.stateType;
		this.cbxRate.Value = (double)this.defaultValues.stateRate;
		this.cbxDelay.Value = (double)this.defaultValues.stateDelay;
		this.panelRate.IsVisible = false;
		this.panelDelay.IsVisible = false;
		this.SetRGB(this.defaultValues.color);
		this.SetHex(this.defaultValues.color);
		if (this.rangeGizmo != null)
		{
			this.rangeGizmo.newRadius = this.defaultValues.range;
		}
		this.SetLightFromValues(this.defaultValues);
	}

	// Token: 0x06007D49 RID: 32073 RVA: 0x002AC36D File Offset: 0x002AA56D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007D4A RID: 32074 RVA: 0x002F7B40 File Offset: 0x002F5D40
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnSave_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.LoadDefaultValues();
		this.CopyUIValues(out this.startValues);
		if (this.defaultValues.IsEqual(this.startValues))
		{
			if (this.tileEntityLight != null)
			{
				this.chunk.RemoveTileEntity(this.world, this.tileEntityLight);
			}
		}
		else
		{
			if (this.tileEntityLight == null)
			{
				BlockLight blockLight = this.world.GetBlock(this.blockPos).Block as BlockLight;
				if (blockLight != null)
				{
					TileEntity tileEntity = this.chunk.GetTileEntity(World.toBlock(this.blockPos));
					if (tileEntity == null)
					{
						tileEntity = blockLight.CreateTileEntity(this.chunk);
						tileEntity.localChunkPos = World.toBlock(this.blockPos);
						this.chunk.AddTileEntity(tileEntity);
					}
					this.tileEntityLight = tileEntity as TileEntityLight;
				}
			}
			this.tileEntityLight.LightType = this.startValues.type;
			this.tileEntityLight.LightShadows = this.startValues.shadows;
			this.tileEntityLight.LightRange = this.startValues.range;
			this.tileEntityLight.LightIntensity = this.startValues.intensity;
			this.tileEntityLight.LightColor = this.startValues.color;
			this.tileEntityLight.LightAngle = this.startValues.spotAngle;
			this.tileEntityLight.LightState = this.startValues.stateType;
			this.tileEntityLight.Rate = this.startValues.stateRate;
			this.tileEntityLight.Delay = this.startValues.stateDelay;
			this.tileEntityLight.SetModified();
		}
		this.xui.playerUI.windowManager.Close("lightProperties");
	}

	// Token: 0x06007D4B RID: 32075 RVA: 0x002F7CFE File Offset: 0x002F5EFE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightType()
	{
		if (this.light != null)
		{
			this.cbxLightType.Value = this.light.type;
		}
	}

	// Token: 0x06007D4C RID: 32076 RVA: 0x002F7D24 File Offset: 0x002F5F24
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightShadows()
	{
		if (this.light != null)
		{
			this.cbxLightShadow.Value = this.light.shadows;
		}
	}

	// Token: 0x06007D4D RID: 32077 RVA: 0x002F7D4A File Offset: 0x002F5F4A
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRange()
	{
		if (this.light != null)
		{
			this.cbxRange.Value = (double)this.light.range;
		}
	}

	// Token: 0x06007D4E RID: 32078 RVA: 0x002F7D71 File Offset: 0x002F5F71
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenIntensity()
	{
		if (this.lightLOD != null)
		{
			this.cbxIntensity.Value = (double)this.lightLOD.MaxIntensity;
		}
	}

	// Token: 0x06007D4F RID: 32079 RVA: 0x002F7D98 File Offset: 0x002F5F98
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenSelectedColor()
	{
		if (this.light != null)
		{
			this.colorPicker.SelectedColor = this.light.color;
		}
	}

	// Token: 0x06007D50 RID: 32080 RVA: 0x002F7DBE File Offset: 0x002F5FBE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenAngle()
	{
		if (this.light != null && this.light.type == LightType.Spot)
		{
			this.cbxAngle.Value = (double)this.light.spotAngle;
		}
	}

	// Token: 0x06007D51 RID: 32081 RVA: 0x002F7DF4 File Offset: 0x002F5FF4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenColorRGB()
	{
		if (this.light != null)
		{
			Color32 color = this.light.color;
			this.txtColorR.Text = color.r.ToString();
			this.txtColorG.Text = color.g.ToString();
			this.txtColorB.Text = color.b.ToString();
		}
	}

	// Token: 0x06007D52 RID: 32082 RVA: 0x002F7E65 File Offset: 0x002F6065
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenHex()
	{
		if (this.light != null)
		{
			this.SetHex(this.light.color);
		}
	}

	// Token: 0x06007D53 RID: 32083 RVA: 0x002F7E8C File Offset: 0x002F608C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenState()
	{
		if (this.lightLOD != null)
		{
			this.cbxLightState.Value = this.lightLOD.LightStateType;
			if (this.lightLOD.LightStateType == LightStateType.Static || this.lightLOD.LightStateType == LightStateType.Fluctuating)
			{
				this.panelRate.IsVisible = false;
			}
		}
	}

	// Token: 0x06007D54 RID: 32084 RVA: 0x002F7EE4 File Offset: 0x002F60E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRate()
	{
		if (this.lightLOD != null)
		{
			this.cbxRate.Value = (double)this.lightLOD.StateRate;
		}
	}

	// Token: 0x06007D55 RID: 32085 RVA: 0x002F7F0B File Offset: 0x002F610B
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenDelay()
	{
		if (this.lightLOD != null)
		{
			this.cbxDelay.Value = (double)this.lightLOD.FluxDelay;
		}
	}

	// Token: 0x06007D56 RID: 32086 RVA: 0x002F7F34 File Offset: 0x002F6134
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenAllValues()
	{
		if (this.light != null && this.lightLOD != null)
		{
			this.startValues = this.MakeValues(this.lightLOD);
		}
		this.OpenLightType();
		this.OpenLightShadows();
		this.OpenRange();
		this.OpenAngle();
		this.OpenIntensity();
		this.OpenSelectedColor();
		this.OpenColorRGB();
		this.OpenHex();
		this.OpenState();
		this.OpenRate();
		this.OpenDelay();
	}

	// Token: 0x06007D57 RID: 32087 RVA: 0x002F7FB4 File Offset: 0x002F61B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LoadDefaultValues()
	{
		Transform transform = DataLoader.LoadAsset<GameObject>(this.block.Properties.GetValue("Model"), false).transform.Find("MainLight");
		if (!transform)
		{
			Log.Error("MainLight missing for {0}", new object[] { this.block.Properties.GetValue("Model") });
			return;
		}
		LightLOD component = transform.GetComponent<LightLOD>();
		this.defaultValues = this.MakeValues(component);
	}

	// Token: 0x06007D58 RID: 32088 RVA: 0x002F8034 File Offset: 0x002F6234
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues MakeValues(LightLOD _lightLOD)
	{
		Light light = _lightLOD.GetLight();
		XUiC_LightEditor.LightValues lightValues;
		lightValues.type = light.type;
		lightValues.shadows = light.shadows;
		lightValues.range = light.range;
		lightValues.intensity = light.intensity;
		lightValues.color = light.color;
		lightValues.spotAngle = light.spotAngle;
		lightValues.emissiveColor = _lightLOD.EmissiveColor;
		lightValues.stateType = LightStateType.Static;
		lightValues.stateRate = 1f;
		lightValues.stateDelay = 1f;
		return lightValues;
	}

	// Token: 0x06007D59 RID: 32089 RVA: 0x002F80C4 File Offset: 0x002F62C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetType(LightType _newType)
	{
		if (this.light != null)
		{
			this.light.type = _newType;
		}
		if (_newType == LightType.Point)
		{
			this.panelAngle.IsVisible = false;
			if (this.rangeGizmo != null)
			{
				this.rangeGizmo.gameObject.GetComponent<LineRenderer>().enabled = true;
				return;
			}
		}
		else
		{
			this.panelAngle.IsVisible = true;
			if (this.rangeGizmo != null)
			{
				this.rangeGizmo.gameObject.GetComponent<LineRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x06007D5A RID: 32090 RVA: 0x002F8150 File Offset: 0x002F6350
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetPreset(string _newPreset)
	{
		if (_newPreset.ToString() != "Custom" && this.light != null)
		{
			this.light.color = XUiC_LightEditor.ColorPresets[_newPreset];
			this.SetEmissiveColor(this.light.color);
			this.shouldChangePreset = false;
			this.SetRGB(this.light.color);
			this.SetHex(this.light.color);
			this.shouldChangePreset = true;
			this.colorPicker.SelectedColor = XUiC_LightEditor.ColorPresets[_newPreset];
		}
	}

	// Token: 0x06007D5B RID: 32091 RVA: 0x002F81F8 File Offset: 0x002F63F8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRange(float _newRange)
	{
		if (this.light != null && this.lightLOD != null)
		{
			this.lightLOD.SetRange(_newRange);
			if (this.rangeGizmo != null)
			{
				this.rangeGizmo.newRadius = _newRange;
			}
		}
	}

	// Token: 0x06007D5C RID: 32092 RVA: 0x002F8247 File Offset: 0x002F6447
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetIntensity(float _newIntensity)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.MaxIntensity = _newIntensity;
		}
	}

	// Token: 0x06007D5D RID: 32093 RVA: 0x002F8263 File Offset: 0x002F6463
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAngle(float _newAngle)
	{
		if (this.light != null)
		{
			this.light.spotAngle = _newAngle;
		}
	}

	// Token: 0x06007D5E RID: 32094 RVA: 0x002F827F File Offset: 0x002F647F
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRate(float _newRate)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.StateRate = _newRate;
		}
	}

	// Token: 0x06007D5F RID: 32095 RVA: 0x002F829C File Offset: 0x002F649C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetState(LightStateType _newState)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.LightStateType = _newState;
			this.panelRate.IsVisible = _newState == LightStateType.Pulsing || _newState == LightStateType.Blinking;
			if (this.panelRate.IsVisible)
			{
				this.cbxRate.Value = (double)this.lightLOD.StateRate;
			}
			this.panelDelay.IsVisible = _newState == LightStateType.Fluctuating;
			if (this.panelDelay.IsVisible)
			{
				this.cbxDelay.Value = (double)this.lightLOD.FluxDelay;
			}
		}
	}

	// Token: 0x06007D60 RID: 32096 RVA: 0x002F8330 File Offset: 0x002F6530
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetShadow(LightShadows _newShadow)
	{
		if (this.light != null)
		{
			this.light.shadows = _newShadow;
		}
	}

	// Token: 0x06007D61 RID: 32097 RVA: 0x002F834C File Offset: 0x002F654C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDelay(float _newDelay)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.FluxDelay = _newDelay;
		}
	}

	// Token: 0x06007D62 RID: 32098 RVA: 0x002F836C File Offset: 0x002F656C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRGB(Color32 color)
	{
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
	}

	// Token: 0x06007D63 RID: 32099 RVA: 0x002F83C0 File Offset: 0x002F65C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetHex(Color32 color)
	{
		string text = ((color.r < 16) ? ("0" + color.r.ToString("X")) : color.r.ToString("X"));
		string text2 = ((color.g < 16) ? ("0" + color.g.ToString("X")) : color.g.ToString("X"));
		string text3 = ((color.b < 16) ? ("0" + color.b.ToString("X")) : color.b.ToString("X"));
		this.txtHex.Text = text + text2 + text3;
	}

	// Token: 0x06007D64 RID: 32100 RVA: 0x002F848C File Offset: 0x002F668C
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetLightFromValues(XUiC_LightEditor.LightValues values)
	{
		if (this.light && this.lightLOD)
		{
			this.light.type = values.type;
			this.light.shadows = values.shadows;
			this.lightLOD.SetRange(values.range);
			this.lightLOD.MaxIntensity = values.intensity;
			this.light.color = values.color;
			this.light.spotAngle = values.spotAngle;
			this.SetEmissiveColor(values.emissiveColor);
			this.lightLOD.LightStateType = values.stateType;
			this.lightLOD.StateRate = values.stateRate;
			this.lightLOD.FluxDelay = values.stateDelay;
		}
	}

	// Token: 0x06007D65 RID: 32101 RVA: 0x002F855E File Offset: 0x002F675E
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEmissiveColor(Color _color)
	{
		if (this.lightLOD)
		{
			this.lightLOD.SetEmissiveColor(_color);
		}
	}

	// Token: 0x06007D66 RID: 32102 RVA: 0x002F857C File Offset: 0x002F677C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ColorPicker_OnSelectedColorChanged(Color color)
	{
		if (this.light != null)
		{
			this.light.color = this.colorPicker.SelectedColor;
			this.cbxColorPresetList.Value = XUiC_LightEditor.ColorPresets.Keys.First<string>();
			this.SetRGB(this.light.color);
			this.SetHex(this.light.color);
		}
		this.SetEmissiveColor(color);
	}

	// Token: 0x06007D67 RID: 32103 RVA: 0x002F85FC File Offset: 0x002F67FC
	public override void OnClose()
	{
		Log.Out("CLOSE " + this.startValues.stateType.ToString());
		this.SetLightFromValues(this.startValues);
		base.OnClose();
		if (this.rangeGizmo != null)
		{
			this.rangeGizmo.KillWF();
		}
	}

	// Token: 0x06007D68 RID: 32104 RVA: 0x002F8659 File Offset: 0x002F6859
	public override void OnOpen()
	{
		base.OnOpen();
		this.isOpen = true;
	}

	// Token: 0x04005CC9 RID: 23753
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, Color> ColorPresets = new Dictionary<string, Color>
	{
		{
			Localization.Get("xuiLightPropColorPresetCustom", false, null),
			new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetCandle", false, null),
			new Color32(byte.MaxValue, 147, 41, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColor40WattWhite", false, null),
			new Color32(byte.MaxValue, 197, 143, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPreset100WattWhite", false, null),
			new Color32(byte.MaxValue, 214, 170, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetHalogen", false, null),
			new Color32(byte.MaxValue, 241, 224, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetCarbonArc", false, null),
			new Color32(byte.MaxValue, 250, 244, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetHighNoonSun", false, null),
			new Color32(byte.MaxValue, byte.MaxValue, 251, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetFluorescent", false, null),
			new Color32(244, byte.MaxValue, 250, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetWarmFluorescent", false, null),
			new Color32(byte.MaxValue, 244, 229, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetCoolFluorescent", false, null),
			new Color32(212, 235, byte.MaxValue, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetFullSpectrum", false, null),
			new Color32(byte.MaxValue, 244, 242, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetMercury", false, null),
			new Color32(216, 247, byte.MaxValue, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetSodium", false, null),
			new Color32(byte.MaxValue, 209, 178, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetHighPressureSodium", false, null),
			new Color32(byte.MaxValue, 183, 76, byte.MaxValue)
		},
		{
			Localization.Get("xuiLightPropColorPresetHalide", false, null),
			new Color32(242, 252, byte.MaxValue, byte.MaxValue)
		}
	};

	// Token: 0x04005CCA RID: 23754
	public static string ID = "";

	// Token: 0x04005CCB RID: 23755
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<LightType> cbxLightType;

	// Token: 0x04005CCC RID: 23756
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxColorPresetList;

	// Token: 0x04005CCD RID: 23757
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightShadows> cbxLightShadow;

	// Token: 0x04005CCE RID: 23758
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightStateType> cbxLightState;

	// Token: 0x04005CCF RID: 23759
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRange;

	// Token: 0x04005CD0 RID: 23760
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxIntensity;

	// Token: 0x04005CD1 RID: 23761
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAngle;

	// Token: 0x04005CD2 RID: 23762
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRate;

	// Token: 0x04005CD3 RID: 23763
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDelay;

	// Token: 0x04005CD4 RID: 23764
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ColorPicker colorPicker;

	// Token: 0x04005CD5 RID: 23765
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelAngle;

	// Token: 0x04005CD6 RID: 23766
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelRate;

	// Token: 0x04005CD7 RID: 23767
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelDelay;

	// Token: 0x04005CD8 RID: 23768
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOk;

	// Token: 0x04005CD9 RID: 23769
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCopy;

	// Token: 0x04005CDA RID: 23770
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnPaste;

	// Token: 0x04005CDB RID: 23771
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCancel;

	// Token: 0x04005CDC RID: 23772
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnRestore;

	// Token: 0x04005CDD RID: 23773
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOnOff;

	// Token: 0x04005CDE RID: 23774
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorR;

	// Token: 0x04005CDF RID: 23775
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorG;

	// Token: 0x04005CE0 RID: 23776
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorB;

	// Token: 0x04005CE1 RID: 23777
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtHex;

	// Token: 0x04005CE2 RID: 23778
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityLight tileEntityLight;

	// Token: 0x04005CE3 RID: 23779
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005CE4 RID: 23780
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldChangePreset = true;

	// Token: 0x04005CE5 RID: 23781
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues startValues;

	// Token: 0x04005CE6 RID: 23782
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues defaultValues;

	// Token: 0x04005CE7 RID: 23783
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool hasCopiedValues;

	// Token: 0x04005CE8 RID: 23784
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LightEditor.LightValues copyValues;

	// Token: 0x04005CE9 RID: 23785
	[PublicizedFrom(EAccessModifier.Private)]
	public static string copyColorPreset;

	// Token: 0x04005CEA RID: 23786
	[PublicizedFrom(EAccessModifier.Private)]
	public WireFrameSphere rangeGizmo;

	// Token: 0x04005CEB RID: 23787
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04005CEC RID: 23788
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04005CED RID: 23789
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04005CEE RID: 23790
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockLight block;

	// Token: 0x04005CEF RID: 23791
	public LightLOD lightLOD;

	// Token: 0x04005CF0 RID: 23792
	[PublicizedFrom(EAccessModifier.Private)]
	public Light light;

	// Token: 0x02000F98 RID: 3992
	public struct LightValues
	{
		// Token: 0x06007D6B RID: 32107 RVA: 0x002F8964 File Offset: 0x002F6B64
		public bool IsEqual(XUiC_LightEditor.LightValues other)
		{
			return this.type == other.type && this.shadows == other.shadows && this.range == other.range && this.intensity == other.intensity && !(this.color != other.color) && this.spotAngle == other.spotAngle && !(this.emissiveColor != other.emissiveColor) && this.stateType == other.stateType && this.stateRate == other.stateRate && this.stateDelay == other.stateDelay;
		}

		// Token: 0x04005CF1 RID: 23793
		public LightType type;

		// Token: 0x04005CF2 RID: 23794
		public LightShadows shadows;

		// Token: 0x04005CF3 RID: 23795
		public float range;

		// Token: 0x04005CF4 RID: 23796
		public float intensity;

		// Token: 0x04005CF5 RID: 23797
		public Color color;

		// Token: 0x04005CF6 RID: 23798
		public float spotAngle;

		// Token: 0x04005CF7 RID: 23799
		public Color emissiveColor;

		// Token: 0x04005CF8 RID: 23800
		public LightStateType stateType;

		// Token: 0x04005CF9 RID: 23801
		public float stateRate;

		// Token: 0x04005CFA RID: 23802
		public float stateDelay;
	}
}
