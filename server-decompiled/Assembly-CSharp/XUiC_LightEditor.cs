using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F5F RID: 3935
[Preserve]
public class XUiC_LightEditor : XUiController
{
	// Token: 0x06007BE3 RID: 31715 RVA: 0x002ED754 File Offset: 0x002EB954
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

	// Token: 0x06007BE4 RID: 31716 RVA: 0x002ED934 File Offset: 0x002EBB34
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

	// Token: 0x06007BE5 RID: 31717 RVA: 0x002EDDC0 File Offset: 0x002EBFC0
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

	// Token: 0x06007BE6 RID: 31718 RVA: 0x002EDF89 File Offset: 0x002EC189
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCopy_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.CopyUIValues(out XUiC_LightEditor.copyValues);
		XUiC_LightEditor.hasCopiedValues = true;
		XUiC_LightEditor.copyColorPreset = this.cbxColorPresetList.Value;
		this.btnPaste.ViewComponent.IsVisible = true;
	}

	// Token: 0x06007BE7 RID: 31719 RVA: 0x002EDFC0 File Offset: 0x002EC1C0
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

	// Token: 0x06007BE8 RID: 31720 RVA: 0x002EE090 File Offset: 0x002EC290
	public void AssignRate(float _rate)
	{
		this.cbxRate.Value = (double)_rate;
	}

	// Token: 0x06007BE9 RID: 31721 RVA: 0x002EE09F File Offset: 0x002EC29F
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnOnOff_OnPressed(XUiController _sender, int _mouseButton)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.SwitchOnOff(!this.lightLOD.bSwitchedOn, true);
			this.SetEmissiveColor(this.light.color);
		}
	}

	// Token: 0x06007BEA RID: 31722 RVA: 0x002EE0DC File Offset: 0x002EC2DC
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

	// Token: 0x06007BEB RID: 31723 RVA: 0x002EE178 File Offset: 0x002EC378
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

	// Token: 0x06007BEC RID: 31724 RVA: 0x002EE258 File Offset: 0x002EC458
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

	// Token: 0x06007BED RID: 31725 RVA: 0x002EE338 File Offset: 0x002EC538
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

	// Token: 0x06007BEE RID: 31726 RVA: 0x002EE415 File Offset: 0x002EC615
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightType_OnValueChanged(XUiController _sender, LightType _oldValue, LightType _newValue)
	{
		this.SetType(_newValue);
	}

	// Token: 0x06007BEF RID: 31727 RVA: 0x002EE41E File Offset: 0x002EC61E
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxColorPresetList_OnValueChanged(XUiController _sender, string _oldValue, string _newValue)
	{
		this.SetPreset(_newValue);
	}

	// Token: 0x06007BF0 RID: 31728 RVA: 0x002EE427 File Offset: 0x002EC627
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRange_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRange((float)_newValue);
	}

	// Token: 0x06007BF1 RID: 31729 RVA: 0x002EE431 File Offset: 0x002EC631
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxIntensity_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetIntensity((float)_newValue);
	}

	// Token: 0x06007BF2 RID: 31730 RVA: 0x002EE43B File Offset: 0x002EC63B
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxAngle_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetAngle((float)_newValue);
	}

	// Token: 0x06007BF3 RID: 31731 RVA: 0x002EE445 File Offset: 0x002EC645
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxRate_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
		this.SetRate((float)_newValue);
	}

	// Token: 0x06007BF4 RID: 31732 RVA: 0x002EE44F File Offset: 0x002EC64F
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightState_OnValueChanged(XUiController _sender, LightStateType _oldValue, LightStateType _newValue)
	{
		this.SetState(_newValue);
	}

	// Token: 0x06007BF5 RID: 31733 RVA: 0x002EE458 File Offset: 0x002EC658
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxLightShadow_OnValueChanged(XUiController _sender, LightShadows _oldValue, LightShadows _newValue)
	{
		this.SetShadow(_newValue);
	}

	// Token: 0x06007BF6 RID: 31734 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void CbxDelay_OnValueChanged(XUiController _sender, double _oldValue, double _newValue)
	{
	}

	// Token: 0x06007BF7 RID: 31735 RVA: 0x002EE464 File Offset: 0x002EC664
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

	// Token: 0x06007BF8 RID: 31736 RVA: 0x002A4621 File Offset: 0x002A2821
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnCancel_OnPressed(XUiController _sender, int _mouseButton)
	{
		this.xui.playerUI.windowManager.Close(base.WindowGroup, false);
	}

	// Token: 0x06007BF9 RID: 31737 RVA: 0x002EE5D4 File Offset: 0x002EC7D4
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

	// Token: 0x06007BFA RID: 31738 RVA: 0x002EE792 File Offset: 0x002EC992
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightType()
	{
		if (this.light != null)
		{
			this.cbxLightType.Value = this.light.type;
		}
	}

	// Token: 0x06007BFB RID: 31739 RVA: 0x002EE7B8 File Offset: 0x002EC9B8
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenLightShadows()
	{
		if (this.light != null)
		{
			this.cbxLightShadow.Value = this.light.shadows;
		}
	}

	// Token: 0x06007BFC RID: 31740 RVA: 0x002EE7DE File Offset: 0x002EC9DE
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRange()
	{
		if (this.light != null)
		{
			this.cbxRange.Value = (double)this.light.range;
		}
	}

	// Token: 0x06007BFD RID: 31741 RVA: 0x002EE805 File Offset: 0x002ECA05
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenIntensity()
	{
		if (this.lightLOD != null)
		{
			this.cbxIntensity.Value = (double)this.lightLOD.MaxIntensity;
		}
	}

	// Token: 0x06007BFE RID: 31742 RVA: 0x002EE82C File Offset: 0x002ECA2C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenSelectedColor()
	{
		if (this.light != null)
		{
			this.colorPicker.SelectedColor = this.light.color;
		}
	}

	// Token: 0x06007BFF RID: 31743 RVA: 0x002EE852 File Offset: 0x002ECA52
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenAngle()
	{
		if (this.light != null && this.light.type == LightType.Spot)
		{
			this.cbxAngle.Value = (double)this.light.spotAngle;
		}
	}

	// Token: 0x06007C00 RID: 31744 RVA: 0x002EE888 File Offset: 0x002ECA88
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

	// Token: 0x06007C01 RID: 31745 RVA: 0x002EE8F9 File Offset: 0x002ECAF9
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenHex()
	{
		if (this.light != null)
		{
			this.SetHex(this.light.color);
		}
	}

	// Token: 0x06007C02 RID: 31746 RVA: 0x002EE920 File Offset: 0x002ECB20
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

	// Token: 0x06007C03 RID: 31747 RVA: 0x002EE978 File Offset: 0x002ECB78
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenRate()
	{
		if (this.lightLOD != null)
		{
			this.cbxRate.Value = (double)this.lightLOD.StateRate;
		}
	}

	// Token: 0x06007C04 RID: 31748 RVA: 0x002EE99F File Offset: 0x002ECB9F
	[PublicizedFrom(EAccessModifier.Private)]
	public void OpenDelay()
	{
		if (this.lightLOD != null)
		{
			this.cbxDelay.Value = (double)this.lightLOD.FluxDelay;
		}
	}

	// Token: 0x06007C05 RID: 31749 RVA: 0x002EE9C8 File Offset: 0x002ECBC8
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

	// Token: 0x06007C06 RID: 31750 RVA: 0x002EEA48 File Offset: 0x002ECC48
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

	// Token: 0x06007C07 RID: 31751 RVA: 0x002EEAC8 File Offset: 0x002ECCC8
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

	// Token: 0x06007C08 RID: 31752 RVA: 0x002EEB58 File Offset: 0x002ECD58
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

	// Token: 0x06007C09 RID: 31753 RVA: 0x002EEBE4 File Offset: 0x002ECDE4
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

	// Token: 0x06007C0A RID: 31754 RVA: 0x002EEC8C File Offset: 0x002ECE8C
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

	// Token: 0x06007C0B RID: 31755 RVA: 0x002EECDB File Offset: 0x002ECEDB
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetIntensity(float _newIntensity)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.MaxIntensity = _newIntensity;
		}
	}

	// Token: 0x06007C0C RID: 31756 RVA: 0x002EECF7 File Offset: 0x002ECEF7
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetAngle(float _newAngle)
	{
		if (this.light != null)
		{
			this.light.spotAngle = _newAngle;
		}
	}

	// Token: 0x06007C0D RID: 31757 RVA: 0x002EED13 File Offset: 0x002ECF13
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRate(float _newRate)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.StateRate = _newRate;
		}
	}

	// Token: 0x06007C0E RID: 31758 RVA: 0x002EED30 File Offset: 0x002ECF30
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

	// Token: 0x06007C0F RID: 31759 RVA: 0x002EEDC4 File Offset: 0x002ECFC4
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetShadow(LightShadows _newShadow)
	{
		if (this.light != null)
		{
			this.light.shadows = _newShadow;
		}
	}

	// Token: 0x06007C10 RID: 31760 RVA: 0x002EEDE0 File Offset: 0x002ECFE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetDelay(float _newDelay)
	{
		if (this.lightLOD != null)
		{
			this.lightLOD.FluxDelay = _newDelay;
		}
	}

	// Token: 0x06007C11 RID: 31761 RVA: 0x002EEE00 File Offset: 0x002ED000
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetRGB(Color32 color)
	{
		this.txtColorR.Text = color.r.ToString();
		this.txtColorG.Text = color.g.ToString();
		this.txtColorB.Text = color.b.ToString();
	}

	// Token: 0x06007C12 RID: 31762 RVA: 0x002EEE54 File Offset: 0x002ED054
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetHex(Color32 color)
	{
		string text = ((color.r < 16) ? ("0" + color.r.ToString("X")) : color.r.ToString("X"));
		string text2 = ((color.g < 16) ? ("0" + color.g.ToString("X")) : color.g.ToString("X"));
		string text3 = ((color.b < 16) ? ("0" + color.b.ToString("X")) : color.b.ToString("X"));
		this.txtHex.Text = text + text2 + text3;
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x002EEF20 File Offset: 0x002ED120
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

	// Token: 0x06007C14 RID: 31764 RVA: 0x002EEFF2 File Offset: 0x002ED1F2
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetEmissiveColor(Color _color)
	{
		if (this.lightLOD)
		{
			this.lightLOD.SetEmissiveColor(_color);
		}
	}

	// Token: 0x06007C15 RID: 31765 RVA: 0x002EF010 File Offset: 0x002ED210
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

	// Token: 0x06007C16 RID: 31766 RVA: 0x002EF090 File Offset: 0x002ED290
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

	// Token: 0x06007C17 RID: 31767 RVA: 0x002EF0ED File Offset: 0x002ED2ED
	public override void OnOpen()
	{
		base.OnOpen();
		this.isOpen = true;
	}

	// Token: 0x04005B9F RID: 23455
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

	// Token: 0x04005BA0 RID: 23456
	public static string ID = "";

	// Token: 0x04005BA1 RID: 23457
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<LightType> cbxLightType;

	// Token: 0x04005BA2 RID: 23458
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxList<string> cbxColorPresetList;

	// Token: 0x04005BA3 RID: 23459
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightShadows> cbxLightShadow;

	// Token: 0x04005BA4 RID: 23460
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxEnum<LightStateType> cbxLightState;

	// Token: 0x04005BA5 RID: 23461
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRange;

	// Token: 0x04005BA6 RID: 23462
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxIntensity;

	// Token: 0x04005BA7 RID: 23463
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxAngle;

	// Token: 0x04005BA8 RID: 23464
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxRate;

	// Token: 0x04005BA9 RID: 23465
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ComboBoxFloat cbxDelay;

	// Token: 0x04005BAA RID: 23466
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_ColorPicker colorPicker;

	// Token: 0x04005BAB RID: 23467
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelAngle;

	// Token: 0x04005BAC RID: 23468
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelRate;

	// Token: 0x04005BAD RID: 23469
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiView panelDelay;

	// Token: 0x04005BAE RID: 23470
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOk;

	// Token: 0x04005BAF RID: 23471
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCopy;

	// Token: 0x04005BB0 RID: 23472
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnPaste;

	// Token: 0x04005BB1 RID: 23473
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnCancel;

	// Token: 0x04005BB2 RID: 23474
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnRestore;

	// Token: 0x04005BB3 RID: 23475
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnOnOff;

	// Token: 0x04005BB4 RID: 23476
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorR;

	// Token: 0x04005BB5 RID: 23477
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorG;

	// Token: 0x04005BB6 RID: 23478
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtColorB;

	// Token: 0x04005BB7 RID: 23479
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_TextInput txtHex;

	// Token: 0x04005BB8 RID: 23480
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityLight tileEntityLight;

	// Token: 0x04005BB9 RID: 23481
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isOpen;

	// Token: 0x04005BBA RID: 23482
	[PublicizedFrom(EAccessModifier.Private)]
	public bool shouldChangePreset = true;

	// Token: 0x04005BBB RID: 23483
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues startValues;

	// Token: 0x04005BBC RID: 23484
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_LightEditor.LightValues defaultValues;

	// Token: 0x04005BBD RID: 23485
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool hasCopiedValues;

	// Token: 0x04005BBE RID: 23486
	[PublicizedFrom(EAccessModifier.Private)]
	public static XUiC_LightEditor.LightValues copyValues;

	// Token: 0x04005BBF RID: 23487
	[PublicizedFrom(EAccessModifier.Private)]
	public static string copyColorPreset;

	// Token: 0x04005BC0 RID: 23488
	[PublicizedFrom(EAccessModifier.Private)]
	public WireFrameSphere rangeGizmo;

	// Token: 0x04005BC1 RID: 23489
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i blockPos;

	// Token: 0x04005BC2 RID: 23490
	[PublicizedFrom(EAccessModifier.Private)]
	public World world;

	// Token: 0x04005BC3 RID: 23491
	[PublicizedFrom(EAccessModifier.Private)]
	public Chunk chunk;

	// Token: 0x04005BC4 RID: 23492
	[PublicizedFrom(EAccessModifier.Private)]
	public BlockLight block;

	// Token: 0x04005BC5 RID: 23493
	public LightLOD lightLOD;

	// Token: 0x04005BC6 RID: 23494
	[PublicizedFrom(EAccessModifier.Private)]
	public Light light;

	// Token: 0x02000F60 RID: 3936
	public struct LightValues
	{
		// Token: 0x06007C1A RID: 31770 RVA: 0x002EF3F8 File Offset: 0x002ED5F8
		public bool IsEqual(XUiC_LightEditor.LightValues other)
		{
			return this.type == other.type && this.shadows == other.shadows && this.range == other.range && this.intensity == other.intensity && !(this.color != other.color) && this.spotAngle == other.spotAngle && !(this.emissiveColor != other.emissiveColor) && this.stateType == other.stateType && this.stateRate == other.stateRate && this.stateDelay == other.stateDelay;
		}

		// Token: 0x04005BC7 RID: 23495
		public LightType type;

		// Token: 0x04005BC8 RID: 23496
		public LightShadows shadows;

		// Token: 0x04005BC9 RID: 23497
		public float range;

		// Token: 0x04005BCA RID: 23498
		public float intensity;

		// Token: 0x04005BCB RID: 23499
		public Color color;

		// Token: 0x04005BCC RID: 23500
		public float spotAngle;

		// Token: 0x04005BCD RID: 23501
		public Color emissiveColor;

		// Token: 0x04005BCE RID: 23502
		public LightStateType stateType;

		// Token: 0x04005BCF RID: 23503
		public float stateRate;

		// Token: 0x04005BD0 RID: 23504
		public float stateDelay;
	}
}
