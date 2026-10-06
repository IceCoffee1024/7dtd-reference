using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F87 RID: 3975
[Preserve]
public class XUiC_LightEditor : XUiController
{
	// Token: 0x06007CE5 RID: 31973 RVA: 0x002F5038 File Offset: 0x002F3238
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

	// Token: 0x06007CE6 RID: 31974 RVA: 0x002F5218 File Offset: 0x002F3418
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

	// Token: 0x06007CE7 RID: 31975 RVA: 0x002F56A4 File Offset: 0x002F38A4
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

	// Token: 0x06007CE8 RID: 31976 RVA: 0x002F586D File Offset: 0x002F3A6D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CopyUIValues(out XUiC_LightEditor.copyValues);
		XUiC_LightEditor.hasCopiedValues = true;
		XUiC_LightEditor.copyColorPreset = this.cbxColorPresetList.Value;
		this.btnPaste.ViewComponent.IsVisible = true;
	}

	// Token: 0x06007CE9 RID: 31977 RVA: 0x002F58A4 File Offset: 0x002F3AA4
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

	// Token: 0x06007CEA RID: 31978 RVA: 0x002F5974 File Offset: 0x002F3B74
	public void AssignRate(float _rate)
	{
		this.cbxRate.Value = (double)_rate;
	}

	// Token: 0x06007CEB RID: 31979 RVA: 0x002F5983 File Offset: 0x002F3B83
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOnOff_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.SwitchOnOff(!this.lightLOD.bSwitchedOn, true);
			this.SetEmissiveColor(this.light.color);
		}
	}

	// Token: 0x06007CEC RID: 31980 RVA: 0x002F59C0 File Offset: 0x002F3BC0
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

	// Token: 0x06007CED RID: 31981 RVA: 0x002F5A5C File Offset: 0x002F3C5C
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

	// Token: 0x06007CEE RID: 31982 RVA: 0x002F5B3C File Offset: 0x002F3D3C
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

	// Token: 0x06007CEF RID: 31983 RVA: 0x002F5C1C File Offset: 0x002F3E1C
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

	// Token: 0x06007CF0 RID: 31984 RVA: 0x002F5CF9 File Offset: 0x002F3EF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightType_OnValueChanged(XUiController _sender, LightType _oldValue, LightType _newValue)
	{
		this.SetType(_newValue);
	}

	// Token: 0x06007CF1 RID: 31985 RVA: 0x002F5D02 File Offset: 0x002F3F02
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxColorPresetList_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		this.SetPreset(_newValue);
	}

	// Token: 0x06007CF2 RID: 31986 RVA: 0x002F5D0B File Offset: 0x002F3F0B
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRange_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRange((float)_newValue);
	}

	// Token: 0x06007CF3 RID: 31987 RVA: 0x002F5D15 File Offset: 0x002F3F15
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIntensity_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetIntensity((float)_newValue);
	}

	// Token: 0x06007CF4 RID: 31988 RVA: 0x002F5D1F File Offset: 0x002F3F1F
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxAngle_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetAngle((float)_newValue);
	}

	// Token: 0x06007CF5 RID: 31989 RVA: 0x002F5D29 File Offset: 0x002F3F29
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRate_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRate((float)_newValue);
	}

	// Token: 0x06007CF6 RID: 31990 RVA: 0x002F5D33 File Offset: 0x002F3F33
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightState_OnValueChanged(XUiController _sender, LightStateType _oldValue, LightStateType _newValue)
	{
		this.SetState(_newValue);
	}

	// Token: 0x06007CF7 RID: 31991 RVA: 0x002F5D3C File Offset: 0x002F3F3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightShadow_OnValueChanged(XUiController _sender, LightShadows _oldValue, LightShadows _newValue)
	{
		this.SetShadow(_newValue);
	}

	// Token: 0x06007CF8 RID: 31992 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxDelay_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
	}

	// Token: 0x06007CF9 RID: 31993 RVA: 0x002F5D48 File Offset: 0x002F3F48
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

	// Token: 0x06007CFA RID: 31994 RVA: 0x002AAE45 File Offset: 0x002A9045
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007CFB RID: 31995 RVA: 0x002F5EB8 File Offset: 0x002F40B8
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

	// Token: 0x06007CFC RID: 31996 RVA: 0x002F6076 File Offset: 0x002F4276
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightType()
	{
		if (this.light != null)
		{
			this.cbxLightType.Value = this.light.type;
		}
	}

	// Token: 0x06007CFD RID: 31997 RVA: 0x002F609C File Offset: 0x002F429C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightShadows()
	{
		if (this.light != null)
		{
			this.cbxLightShadow.Value = this.light.shadows;
		}
	}

	// Token: 0x06007CFE RID: 31998 RVA: 0x002F60C2 File Offset: 0x002F42C2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRange()
	{
		if (this.light != null)
		{
			this.cbxRange.Value = (double)this.light.range;
		}
	}

	// Token: 0x06007CFF RID: 31999 RVA: 0x002F60E9 File Offset: 0x002F42E9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenIntensity()
	{
		if (this.lightLOD != null)
		{
			this.cbxIntensity.Value = (double)this.lightLOD.MaxIntensity;
		}
	}

	// Token: 0x06007D00 RID: 32000 RVA: 0x002F6110 File Offset: 0x002F4310
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenSelectedColor()
	{
		if (this.light != null)
		{
			this.colorPicker.SelectedColor = this.light.color;
		}
	}

	// Token: 0x06007D01 RID: 32001 RVA: 0x002F6136 File Offset: 0x002F4336
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenAngle()
	{
		if (this.light != null && this.light.type == LightType.Spot)
		{
			this.cbxAngle.Value = (double)this.light.spotAngle;
		}
	}

	// Token: 0x06007D02 RID: 32002 RVA: 0x002F616C File Offset: 0x002F436C
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

	// Token: 0x06007D03 RID: 32003 RVA: 0x002F61DD File Offset: 0x002F43DD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenHex()
	{
		if (this.light != null)
		{
			this.SetHex(this.light.color);
		}
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x002F6204 File Offset: 0x002F4404
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

	// Token: 0x06007D05 RID: 32005 RVA: 0x002F625C File Offset: 0x002F445C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRate()
	{
		if (this.lightLOD != null)
		{
			this.cbxRate.Value = (double)this.lightLOD.StateRate;
		}
	}

	// Token: 0x06007D06 RID: 32006 RVA: 0x002F6283 File Offset: 0x002F4483
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenDelay()
	{
		if (this.lightLOD != null)
		{
			this.cbxDelay.Value = (double)this.lightLOD.FluxDelay;
		}
	}

	// Token: 0x06007D07 RID: 32007 RVA: 0x002F62AC File Offset: 0x002F44AC
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

	// Token: 0x06007D08 RID: 32008 RVA: 0x002F632C File Offset: 0x002F452C
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

	// Token: 0x06007D09 RID: 32009 RVA: 0x002F63AC File Offset: 0x002F45AC
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

	// Token: 0x06007D0A RID: 32010 RVA: 0x002F643C File Offset: 0x002F463C
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

	// Token: 0x06007D0B RID: 32011 RVA: 0x002F64C8 File Offset: 0x002F46C8
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

	// Token: 0x06007D0C RID: 32012 RVA: 0x002F6570 File Offset: 0x002F4770
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

	// Token: 0x06007D0D RID: 32013 RVA: 0x002F65BF File Offset: 0x002F47BF
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetIntensity(float _newIntensity)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.MaxIntensity = _newIntensity;
		}
	}

	// Token: 0x06007D0E RID: 32014 RVA: 0x002F65DB File Offset: 0x002F47DB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAngle(float _newAngle)
	{
		if (this.light != null)
		{
			this.light.spotAngle = _newAngle;
		}
	}

	// Token: 0x06007D0F RID: 32015 RVA: 0x002F65F7 File Offset: 0x002F47F7
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRate(float _newRate)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.StateRate = _newRate;
		}
	}

	// Token: 0x06007D10 RID: 32016 RVA: 0x002F6614 File Offset: 0x002F4814
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

	// Token: 0x06007D11 RID: 32017 RVA: 0x002F66A8 File Offset: 0x002F48A8
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetShadow(LightShadows _newShadow)
	{
		if (this.light != null)
		{
			this.light.shadows = _newShadow;
		}
	}

	// Token: 0x06007D12 RID: 32018 RVA: 0x002F66C4 File Offset: 0x002F48C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDelay(float _newDelay)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.FluxDelay = _newDelay;
		}
	}

	// Token: 0x06007D13 RID: 32019 RVA: 0x002F66E4 File Offset: 0x002F48E4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRGB(Color32 color)
	{
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
	}

	// Token: 0x06007D14 RID: 32020 RVA: 0x002F6738 File Offset: 0x002F4938
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetHex(Color32 color)
	{
		string text = ((color.r < 16) ? ("0" + color.r.ToString("X")) : color.r.ToString("X"));
		string text2 = ((color.g < 16) ? ("0" + color.g.ToString("X")) : color.g.ToString("X"));
		string text3 = ((color.b < 16) ? ("0" + color.b.ToString("X")) : color.b.ToString("X"));
		this.txtHex.Text = text + text2 + text3;
	}

	// Token: 0x06007D15 RID: 32021 RVA: 0x002F6804 File Offset: 0x002F4A04
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

	// Token: 0x06007D16 RID: 32022 RVA: 0x002F68D6 File Offset: 0x002F4AD6
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEmissiveColor(Color _color)
	{
		if (this.lightLOD)
		{
			this.lightLOD.SetEmissiveColor(_color);
		}
	}

	// Token: 0x06007D17 RID: 32023 RVA: 0x002F68F4 File Offset: 0x002F4AF4
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

	// Token: 0x06007D18 RID: 32024 RVA: 0x002F6974 File Offset: 0x002F4B74
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

	// Token: 0x06007D19 RID: 32025 RVA: 0x002F69D1 File Offset: 0x002F4BD1
	public override void OnOpen()
	{
		base.OnOpen();
		this.isOpen = true;
	}

	// Token: 0x04005C8F RID: 23695
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

	// Token: 0x04005C90 RID: 23696
	public static string ID = "";

	// Token: 0x04005C91 RID: 23697
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<LightType> cbxLightType;

	// Token: 0x04005C92 RID: 23698
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxColorPresetList;

	// Token: 0x04005C93 RID: 23699
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightShadows> cbxLightShadow;

	// Token: 0x04005C94 RID: 23700
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightStateType> cbxLightState;

	// Token: 0x04005C95 RID: 23701
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRange;

	// Token: 0x04005C96 RID: 23702
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxIntensity;

	// Token: 0x04005C97 RID: 23703
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAngle;

	// Token: 0x04005C98 RID: 23704
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRate;

	// Token: 0x04005C99 RID: 23705
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDelay;

	// Token: 0x04005C9A RID: 23706
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ColorPicker colorPicker;

	// Token: 0x04005C9B RID: 23707
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelAngle;

	// Token: 0x04005C9C RID: 23708
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelRate;

	// Token: 0x04005C9D RID: 23709
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelDelay;

	// Token: 0x04005C9E RID: 23710
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOk;

	// Token: 0x04005C9F RID: 23711
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCopy;

	// Token: 0x04005CA0 RID: 23712
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnPaste;

	// Token: 0x04005CA1 RID: 23713
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCancel;

	// Token: 0x04005CA2 RID: 23714
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnRestore;

	// Token: 0x04005CA3 RID: 23715
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOnOff;

	// Token: 0x04005CA4 RID: 23716
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorR;

	// Token: 0x04005CA5 RID: 23717
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorG;

	// Token: 0x04005CA6 RID: 23718
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorB;

	// Token: 0x04005CA7 RID: 23719
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtHex;

	// Token: 0x04005CA8 RID: 23720
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityLight tileEntityLight;

	// Token: 0x04005CA9 RID: 23721
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005CAA RID: 23722
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldChangePreset = true;

	// Token: 0x04005CAB RID: 23723
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues startValues;

	// Token: 0x04005CAC RID: 23724
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues defaultValues;

	// Token: 0x04005CAD RID: 23725
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool hasCopiedValues;

	// Token: 0x04005CAE RID: 23726
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LightEditor.LightValues copyValues;

	// Token: 0x04005CAF RID: 23727
	[PublicizedFrom(EAccessModifier.Private)]
	public static string copyColorPreset;

	// Token: 0x04005CB0 RID: 23728
	[PublicizedFrom(EAccessModifier.Private)]
	public WireFrameSphere rangeGizmo;

	// Token: 0x04005CB1 RID: 23729
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04005CB2 RID: 23730
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04005CB3 RID: 23731
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04005CB4 RID: 23732
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockLight block;

	// Token: 0x04005CB5 RID: 23733
	public LightLOD lightLOD;

	// Token: 0x04005CB6 RID: 23734
	[PublicizedFrom(EAccessModifier.Private)]
	public Light light;

	// Token: 0x02000F88 RID: 3976
	public struct LightValues
	{
		// Token: 0x06007D1C RID: 32028 RVA: 0x002F6CDC File Offset: 0x002F4EDC
		public bool IsEqual(XUiC_LightEditor.LightValues other)
		{
			return this.type == other.type && this.shadows == other.shadows && this.range == other.range && this.intensity == other.intensity && !(this.color != other.color) && this.spotAngle == other.spotAngle && !(this.emissiveColor != other.emissiveColor) && this.stateType == other.stateType && this.stateRate == other.stateRate && this.stateDelay == other.stateDelay;
		}

		// Token: 0x04005CB7 RID: 23735
		public LightType type;

		// Token: 0x04005CB8 RID: 23736
		public LightShadows shadows;

		// Token: 0x04005CB9 RID: 23737
		public float range;

		// Token: 0x04005CBA RID: 23738
		public float intensity;

		// Token: 0x04005CBB RID: 23739
		public Color color;

		// Token: 0x04005CBC RID: 23740
		public float spotAngle;

		// Token: 0x04005CBD RID: 23741
		public Color emissiveColor;

		// Token: 0x04005CBE RID: 23742
		public LightStateType stateType;

		// Token: 0x04005CBF RID: 23743
		public float stateRate;

		// Token: 0x04005CC0 RID: 23744
		public float stateDelay;
	}
}
