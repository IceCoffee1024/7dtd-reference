using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F78 RID: 3960
[Preserve]
public class XUiC_LightEditor : XUiController
{
	// Token: 0x06007CD5 RID: 31957 RVA: 0x002F1F28 File Offset: 0x002F0128
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

	// Token: 0x06007CD6 RID: 31958 RVA: 0x002F2108 File Offset: 0x002F0308
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

	// Token: 0x06007CD7 RID: 31959 RVA: 0x002F2594 File Offset: 0x002F0794
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

	// Token: 0x06007CD8 RID: 31960 RVA: 0x002F275D File Offset: 0x002F095D
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CopyUIValues(out XUiC_LightEditor.copyValues);
		XUiC_LightEditor.hasCopiedValues = true;
		XUiC_LightEditor.copyColorPreset = this.cbxColorPresetList.Value;
		this.btnPaste.ViewComponent.IsVisible = true;
	}

	// Token: 0x06007CD9 RID: 31961 RVA: 0x002F2794 File Offset: 0x002F0994
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

	// Token: 0x06007CDA RID: 31962 RVA: 0x002F2864 File Offset: 0x002F0A64
	public void AssignRate(float _rate)
	{
		this.cbxRate.Value = (double)_rate;
	}

	// Token: 0x06007CDB RID: 31963 RVA: 0x002F2873 File Offset: 0x002F0A73
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOnOff_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.SwitchOnOff(!this.lightLOD.bSwitchedOn, true);
			this.SetEmissiveColor(this.light.color);
		}
	}

	// Token: 0x06007CDC RID: 31964 RVA: 0x002F28B0 File Offset: 0x002F0AB0
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

	// Token: 0x06007CDD RID: 31965 RVA: 0x002F294C File Offset: 0x002F0B4C
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

	// Token: 0x06007CDE RID: 31966 RVA: 0x002F2A2C File Offset: 0x002F0C2C
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

	// Token: 0x06007CDF RID: 31967 RVA: 0x002F2B0C File Offset: 0x002F0D0C
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

	// Token: 0x06007CE0 RID: 31968 RVA: 0x002F2BE9 File Offset: 0x002F0DE9
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightType_OnValueChanged(XUiController _sender, LightType _oldValue, LightType _newValue)
	{
		this.SetType(_newValue);
	}

	// Token: 0x06007CE1 RID: 31969 RVA: 0x002F2BF2 File Offset: 0x002F0DF2
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxColorPresetList_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		this.SetPreset(_newValue);
	}

	// Token: 0x06007CE2 RID: 31970 RVA: 0x002F2BFB File Offset: 0x002F0DFB
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRange_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRange((float)_newValue);
	}

	// Token: 0x06007CE3 RID: 31971 RVA: 0x002F2C05 File Offset: 0x002F0E05
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIntensity_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetIntensity((float)_newValue);
	}

	// Token: 0x06007CE4 RID: 31972 RVA: 0x002F2C0F File Offset: 0x002F0E0F
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxAngle_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetAngle((float)_newValue);
	}

	// Token: 0x06007CE5 RID: 31973 RVA: 0x002F2C19 File Offset: 0x002F0E19
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRate_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRate((float)_newValue);
	}

	// Token: 0x06007CE6 RID: 31974 RVA: 0x002F2C23 File Offset: 0x002F0E23
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightState_OnValueChanged(XUiController _sender, LightStateType _oldValue, LightStateType _newValue)
	{
		this.SetState(_newValue);
	}

	// Token: 0x06007CE7 RID: 31975 RVA: 0x002F2C2C File Offset: 0x002F0E2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightShadow_OnValueChanged(XUiController _sender, LightShadows _oldValue, LightShadows _newValue)
	{
		this.SetShadow(_newValue);
	}

	// Token: 0x06007CE8 RID: 31976 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxDelay_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
	}

	// Token: 0x06007CE9 RID: 31977 RVA: 0x002F2C38 File Offset: 0x002F0E38
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

	// Token: 0x06007CEA RID: 31978 RVA: 0x002A7AB5 File Offset: 0x002A5CB5
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007CEB RID: 31979 RVA: 0x002F2DA8 File Offset: 0x002F0FA8
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

	// Token: 0x06007CEC RID: 31980 RVA: 0x002F2F66 File Offset: 0x002F1166
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightType()
	{
		if (this.light != null)
		{
			this.cbxLightType.Value = this.light.type;
		}
	}

	// Token: 0x06007CED RID: 31981 RVA: 0x002F2F8C File Offset: 0x002F118C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightShadows()
	{
		if (this.light != null)
		{
			this.cbxLightShadow.Value = this.light.shadows;
		}
	}

	// Token: 0x06007CEE RID: 31982 RVA: 0x002F2FB2 File Offset: 0x002F11B2
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRange()
	{
		if (this.light != null)
		{
			this.cbxRange.Value = (double)this.light.range;
		}
	}

	// Token: 0x06007CEF RID: 31983 RVA: 0x002F2FD9 File Offset: 0x002F11D9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenIntensity()
	{
		if (this.lightLOD != null)
		{
			this.cbxIntensity.Value = (double)this.lightLOD.MaxIntensity;
		}
	}

	// Token: 0x06007CF0 RID: 31984 RVA: 0x002F3000 File Offset: 0x002F1200
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenSelectedColor()
	{
		if (this.light != null)
		{
			this.colorPicker.SelectedColor = this.light.color;
		}
	}

	// Token: 0x06007CF1 RID: 31985 RVA: 0x002F3026 File Offset: 0x002F1226
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenAngle()
	{
		if (this.light != null && this.light.type == LightType.Spot)
		{
			this.cbxAngle.Value = (double)this.light.spotAngle;
		}
	}

	// Token: 0x06007CF2 RID: 31986 RVA: 0x002F305C File Offset: 0x002F125C
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

	// Token: 0x06007CF3 RID: 31987 RVA: 0x002F30CD File Offset: 0x002F12CD
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenHex()
	{
		if (this.light != null)
		{
			this.SetHex(this.light.color);
		}
	}

	// Token: 0x06007CF4 RID: 31988 RVA: 0x002F30F4 File Offset: 0x002F12F4
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

	// Token: 0x06007CF5 RID: 31989 RVA: 0x002F314C File Offset: 0x002F134C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRate()
	{
		if (this.lightLOD != null)
		{
			this.cbxRate.Value = (double)this.lightLOD.StateRate;
		}
	}

	// Token: 0x06007CF6 RID: 31990 RVA: 0x002F3173 File Offset: 0x002F1373
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenDelay()
	{
		if (this.lightLOD != null)
		{
			this.cbxDelay.Value = (double)this.lightLOD.FluxDelay;
		}
	}

	// Token: 0x06007CF7 RID: 31991 RVA: 0x002F319C File Offset: 0x002F139C
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

	// Token: 0x06007CF8 RID: 31992 RVA: 0x002F321C File Offset: 0x002F141C
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

	// Token: 0x06007CF9 RID: 31993 RVA: 0x002F329C File Offset: 0x002F149C
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

	// Token: 0x06007CFA RID: 31994 RVA: 0x002F332C File Offset: 0x002F152C
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

	// Token: 0x06007CFB RID: 31995 RVA: 0x002F33B8 File Offset: 0x002F15B8
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

	// Token: 0x06007CFC RID: 31996 RVA: 0x002F3460 File Offset: 0x002F1660
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

	// Token: 0x06007CFD RID: 31997 RVA: 0x002F34AF File Offset: 0x002F16AF
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetIntensity(float _newIntensity)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.MaxIntensity = _newIntensity;
		}
	}

	// Token: 0x06007CFE RID: 31998 RVA: 0x002F34CB File Offset: 0x002F16CB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAngle(float _newAngle)
	{
		if (this.light != null)
		{
			this.light.spotAngle = _newAngle;
		}
	}

	// Token: 0x06007CFF RID: 31999 RVA: 0x002F34E7 File Offset: 0x002F16E7
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRate(float _newRate)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.StateRate = _newRate;
		}
	}

	// Token: 0x06007D00 RID: 32000 RVA: 0x002F3504 File Offset: 0x002F1704
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

	// Token: 0x06007D01 RID: 32001 RVA: 0x002F3598 File Offset: 0x002F1798
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetShadow(LightShadows _newShadow)
	{
		if (this.light != null)
		{
			this.light.shadows = _newShadow;
		}
	}

	// Token: 0x06007D02 RID: 32002 RVA: 0x002F35B4 File Offset: 0x002F17B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDelay(float _newDelay)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.FluxDelay = _newDelay;
		}
	}

	// Token: 0x06007D03 RID: 32003 RVA: 0x002F35D4 File Offset: 0x002F17D4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRGB(Color32 color)
	{
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
	}

	// Token: 0x06007D04 RID: 32004 RVA: 0x002F3628 File Offset: 0x002F1828
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetHex(Color32 color)
	{
		string text = ((color.r < 16) ? ("0" + color.r.ToString("X")) : color.r.ToString("X"));
		string text2 = ((color.g < 16) ? ("0" + color.g.ToString("X")) : color.g.ToString("X"));
		string text3 = ((color.b < 16) ? ("0" + color.b.ToString("X")) : color.b.ToString("X"));
		this.txtHex.Text = text + text2 + text3;
	}

	// Token: 0x06007D05 RID: 32005 RVA: 0x002F36F4 File Offset: 0x002F18F4
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

	// Token: 0x06007D06 RID: 32006 RVA: 0x002F37C6 File Offset: 0x002F19C6
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEmissiveColor(Color _color)
	{
		if (this.lightLOD)
		{
			this.lightLOD.SetEmissiveColor(_color);
		}
	}

	// Token: 0x06007D07 RID: 32007 RVA: 0x002F37E4 File Offset: 0x002F19E4
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

	// Token: 0x06007D08 RID: 32008 RVA: 0x002F3864 File Offset: 0x002F1A64
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

	// Token: 0x06007D09 RID: 32009 RVA: 0x002F38C1 File Offset: 0x002F1AC1
	public override void OnOpen()
	{
		base.OnOpen();
		this.isOpen = true;
	}

	// Token: 0x04005C66 RID: 23654
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

	// Token: 0x04005C67 RID: 23655
	public static string ID = "";

	// Token: 0x04005C68 RID: 23656
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<LightType> cbxLightType;

	// Token: 0x04005C69 RID: 23657
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxColorPresetList;

	// Token: 0x04005C6A RID: 23658
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightShadows> cbxLightShadow;

	// Token: 0x04005C6B RID: 23659
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightStateType> cbxLightState;

	// Token: 0x04005C6C RID: 23660
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRange;

	// Token: 0x04005C6D RID: 23661
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxIntensity;

	// Token: 0x04005C6E RID: 23662
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAngle;

	// Token: 0x04005C6F RID: 23663
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRate;

	// Token: 0x04005C70 RID: 23664
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDelay;

	// Token: 0x04005C71 RID: 23665
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ColorPicker colorPicker;

	// Token: 0x04005C72 RID: 23666
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelAngle;

	// Token: 0x04005C73 RID: 23667
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelRate;

	// Token: 0x04005C74 RID: 23668
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelDelay;

	// Token: 0x04005C75 RID: 23669
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOk;

	// Token: 0x04005C76 RID: 23670
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCopy;

	// Token: 0x04005C77 RID: 23671
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnPaste;

	// Token: 0x04005C78 RID: 23672
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCancel;

	// Token: 0x04005C79 RID: 23673
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnRestore;

	// Token: 0x04005C7A RID: 23674
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOnOff;

	// Token: 0x04005C7B RID: 23675
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorR;

	// Token: 0x04005C7C RID: 23676
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorG;

	// Token: 0x04005C7D RID: 23677
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorB;

	// Token: 0x04005C7E RID: 23678
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtHex;

	// Token: 0x04005C7F RID: 23679
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityLight tileEntityLight;

	// Token: 0x04005C80 RID: 23680
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005C81 RID: 23681
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldChangePreset = true;

	// Token: 0x04005C82 RID: 23682
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues startValues;

	// Token: 0x04005C83 RID: 23683
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues defaultValues;

	// Token: 0x04005C84 RID: 23684
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool hasCopiedValues;

	// Token: 0x04005C85 RID: 23685
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LightEditor.LightValues copyValues;

	// Token: 0x04005C86 RID: 23686
	[PublicizedFrom(EAccessModifier.Private)]
	public static string copyColorPreset;

	// Token: 0x04005C87 RID: 23687
	[PublicizedFrom(EAccessModifier.Private)]
	public WireFrameSphere rangeGizmo;

	// Token: 0x04005C88 RID: 23688
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04005C89 RID: 23689
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04005C8A RID: 23690
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04005C8B RID: 23691
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockLight block;

	// Token: 0x04005C8C RID: 23692
	public LightLOD lightLOD;

	// Token: 0x04005C8D RID: 23693
	[PublicizedFrom(EAccessModifier.Private)]
	public Light light;

	// Token: 0x02000F79 RID: 3961
	public struct LightValues
	{
		// Token: 0x06007D0C RID: 32012 RVA: 0x002F3BCC File Offset: 0x002F1DCC
		public bool IsEqual(XUiC_LightEditor.LightValues other)
		{
			return this.type == other.type && this.shadows == other.shadows && this.range == other.range && this.intensity == other.intensity && !(this.color != other.color) && this.spotAngle == other.spotAngle && !(this.emissiveColor != other.emissiveColor) && this.stateType == other.stateType && this.stateRate == other.stateRate && this.stateDelay == other.stateDelay;
		}

		// Token: 0x04005C8E RID: 23694
		public LightType type;

		// Token: 0x04005C8F RID: 23695
		public LightShadows shadows;

		// Token: 0x04005C90 RID: 23696
		public float range;

		// Token: 0x04005C91 RID: 23697
		public float intensity;

		// Token: 0x04005C92 RID: 23698
		public Color color;

		// Token: 0x04005C93 RID: 23699
		public float spotAngle;

		// Token: 0x04005C94 RID: 23700
		public Color emissiveColor;

		// Token: 0x04005C95 RID: 23701
		public LightStateType stateType;

		// Token: 0x04005C96 RID: 23702
		public float stateRate;

		// Token: 0x04005C97 RID: 23703
		public float stateDelay;
	}
}
