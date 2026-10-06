using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000F77 RID: 3959
[Preserve]
public class XUiC_LevelToolsGenericWindow : XUiController
{
	// Token: 0x06007C58 RID: 31832 RVA: 0x002F1E8B File Offset: 0x002F008B
	public override void Init()
	{
		base.Init();
		XUiC_LevelToolsGenericWindow.ID = base.WindowGroup.Id;
		this.initGenericButtons();
		this.initSpecialFeatures();
	}

	// Token: 0x06007C59 RID: 31833 RVA: 0x002F1EAF File Offset: 0x002F00AF
	public override void OnOpen()
	{
		base.OnOpen();
		this.onOpenSpecialFeatures();
	}

	// Token: 0x06007C5A RID: 31834 RVA: 0x002F1EBD File Offset: 0x002F00BD
	public override void Update(float _dt)
	{
		base.Update(_dt);
		this.updateGenericButtons();
	}

	// Token: 0x06007C5B RID: 31835 RVA: 0x002F1ECC File Offset: 0x002F00CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void initGenericButtons()
	{
		foreach (XUiC_ToggleButton xuiC_ToggleButton in base.GetChildrenByType<XUiC_ToggleButton>(null))
		{
			string id = xuiC_ToggleButton.ViewComponent.ID;
			string label = xuiC_ToggleButton.Label;
			NGuiAction nguiAction = XUiC_LevelToolsHelpers.BuildAction(id, label, true);
			if (nguiAction != null)
			{
				this.setToggle(xuiC_ToggleButton, nguiAction);
			}
		}
		foreach (XUiC_SimpleButton xuiC_SimpleButton in base.GetChildrenByType<XUiC_SimpleButton>(null))
		{
			string id2 = xuiC_SimpleButton.ViewComponent.ID;
			string text = xuiC_SimpleButton.Text;
			NGuiAction nguiAction2 = XUiC_LevelToolsHelpers.BuildAction(id2, text, false);
			if (nguiAction2 != null)
			{
				this.setButton(xuiC_SimpleButton, nguiAction2);
			}
		}
	}

	// Token: 0x06007C5C RID: 31836 RVA: 0x002F1F68 File Offset: 0x002F0168
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateGenericButtons()
	{
		foreach (ValueTuple<XUiC_ToggleButton, NGuiAction> valueTuple in this.toggleList)
		{
			XUiC_ToggleButton item = valueTuple.Item1;
			NGuiAction item2 = valueTuple.Item2;
			item.Value = item2.IsChecked();
			item.Enabled = item2.IsEnabled();
		}
		foreach (ValueTuple<XUiC_SimpleButton, NGuiAction> valueTuple2 in this.buttonList)
		{
			XUiC_SimpleButton item3 = valueTuple2.Item1;
			NGuiAction item4 = valueTuple2.Item2;
			item3.Enabled = item4.IsEnabled();
		}
	}

	// Token: 0x06007C5D RID: 31837 RVA: 0x002F2030 File Offset: 0x002F0230
	[PublicizedFrom(EAccessModifier.Private)]
	public void setToggle(XUiC_ToggleButton _toggle, NGuiAction _action)
	{
		_toggle.Label = XUiC_LevelToolsGenericWindow.buildCaption(_action);
		_toggle.OnValueChanged += delegate(XUiC_ToggleButton _, bool _)
		{
			_action.OnClick();
		};
		_toggle.Tooltip = _action.GetTooltip();
		this.toggleList.Add(new ValueTuple<XUiC_ToggleButton, NGuiAction>(_toggle, _action));
	}

	// Token: 0x06007C5E RID: 31838 RVA: 0x002F2098 File Offset: 0x002F0298
	[PublicizedFrom(EAccessModifier.Private)]
	public void setButton(XUiC_SimpleButton _button, NGuiAction _action)
	{
		_button.Text = XUiC_LevelToolsGenericWindow.buildCaption(_action);
		_button.OnPressed += delegate(XUiController _, int _)
		{
			_action.OnClick();
		};
		_button.Tooltip = _action.GetTooltip();
		this.buttonList.Add(new ValueTuple<XUiC_SimpleButton, NGuiAction>(_button, _action));
	}

	// Token: 0x06007C5F RID: 31839 RVA: 0x002F20FD File Offset: 0x002F02FD
	[PublicizedFrom(EAccessModifier.Private)]
	public static string buildCaption(NGuiAction _action)
	{
		return _action.GetText() + " " + _action.GetHotkey().GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.KeyboardWithParentheses, null);
	}

	// Token: 0x06007C60 RID: 31840 RVA: 0x002F211D File Offset: 0x002F031D
	[PublicizedFrom(EAccessModifier.Private)]
	public void initSpecialFeatures()
	{
		this.initBlockHighlighter();
		this.initBlockReplacer();
		this.initShapeMaterialReplacer();
	}

	// Token: 0x06007C61 RID: 31841 RVA: 0x002F2134 File Offset: 0x002F0334
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenSpecialFeatures()
	{
		List<string> list = null;
		if (!this.blockListsInitDone)
		{
			list = new List<string>();
			foreach (Block block in Block.list)
			{
				if (block != null)
				{
					list.Add(block.GetBlockName());
				}
			}
			this.blockListsInitDone = true;
		}
		this.onOpenBlockHighlighter(list);
		this.onOpenBlockReplacer(list);
		this.onOpenShapeMaterialReplacer(list);
	}

	// Token: 0x06007C62 RID: 31842 RVA: 0x002F2194 File Offset: 0x002F0394
	[PublicizedFrom(EAccessModifier.Private)]
	public void initBlockHighlighter()
	{
		this.dropdownHighlightBlockName = base.GetChildById("txtHighlightBlockName") as XUiC_DropDown;
		if (this.dropdownHighlightBlockName != null)
		{
			this.blockListsInitDone = false;
			this.dropdownHighlightBlockName.OnChangeHandler += this.HighlightBlock_OnChangeHandler;
			this.dropdownHighlightBlockName.OnSubmitHandler += this.HighlightBlock_OnSubmitHandler;
		}
	}

	// Token: 0x06007C63 RID: 31843 RVA: 0x002F21F4 File Offset: 0x002F03F4
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenBlockHighlighter(List<string> _allBlockNames)
	{
		if (_allBlockNames != null && this.dropdownHighlightBlockName != null)
		{
			this.dropdownHighlightBlockName.AllEntries.AddRange(_allBlockNames);
			this.dropdownHighlightBlockName.UpdateFilteredList();
		}
	}

	// Token: 0x06007C64 RID: 31844 RVA: 0x002F2220 File Offset: 0x002F0420
	[PublicizedFrom(EAccessModifier.Private)]
	public void HighlightBlock_OnChangeHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode)
	{
		bool flag = Block.GetBlockByName(this.dropdownHighlightBlockName.Text, true) != null;
		this.dropdownHighlightBlockName.TextInput.ActiveTextColor = (flag ? Color.white : Color.red);
	}

	// Token: 0x06007C65 RID: 31845 RVA: 0x002F2264 File Offset: 0x002F0464
	[PublicizedFrom(EAccessModifier.Private)]
	public void HighlightBlock_OnSubmitHandler(XUiController _sender, string _text)
	{
		Block blockByName = Block.GetBlockByName(this.dropdownHighlightBlockName.Text, true);
		if (blockByName != null)
		{
			PrefabEditModeManager.Instance.HighlightBlocks(blockByName);
		}
	}

	// Token: 0x06007C66 RID: 31846 RVA: 0x002F2294 File Offset: 0x002F0494
	[PublicizedFrom(EAccessModifier.Private)]
	public void initBlockReplacer()
	{
		this.txtOldBlockId = base.GetChildById("txtOldBlockId") as XUiC_DropDown;
		this.txtNewBlockId = base.GetChildById("txtNewBlockId") as XUiC_DropDown;
		if (this.txtOldBlockId != null)
		{
			this.blockListsInitDone = false;
			this.txtOldBlockId.OnChangeHandler += this.ReplaceBlockIds_OnChangeHandler;
			this.txtOldBlockId.OnSubmitHandler += this.ReplaceBlockIds_OnSubmitHandler;
			XUiC_TextInput textInput = this.txtOldBlockId.TextInput;
			XUiC_DropDown xuiC_DropDown = this.txtNewBlockId;
			textInput.SelectOnTab = ((xuiC_DropDown != null) ? xuiC_DropDown.TextInput : null);
		}
		if (this.txtNewBlockId != null)
		{
			this.blockListsInitDone = false;
			this.txtNewBlockId.OnChangeHandler += this.ReplaceBlockIds_OnChangeHandler;
			this.txtNewBlockId.OnSubmitHandler += this.ReplaceBlockIds_OnSubmitHandler;
			XUiC_TextInput textInput2 = this.txtNewBlockId.TextInput;
			XUiC_DropDown xuiC_DropDown2 = this.txtOldBlockId;
			textInput2.SelectOnTab = ((xuiC_DropDown2 != null) ? xuiC_DropDown2.TextInput : null);
		}
		this.btnReplaceBlockIds = base.GetChildById("btnReplaceBlockIds") as XUiC_SimpleButton;
		if (this.btnReplaceBlockIds != null)
		{
			this.btnReplaceBlockIds.OnPressed += this.BtnReplaceBlockIds_OnPressed;
		}
	}

	// Token: 0x06007C67 RID: 31847 RVA: 0x002F23C0 File Offset: 0x002F05C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenBlockReplacer(List<string> _allBlockNames)
	{
		this.ReplaceBlockIds_OnChangeHandler(this, null, true, true);
		if (_allBlockNames != null)
		{
			if (this.txtOldBlockId != null)
			{
				this.txtOldBlockId.AllEntries.AddRange(_allBlockNames);
				this.txtOldBlockId.UpdateFilteredList();
			}
			if (this.txtNewBlockId != null)
			{
				this.txtNewBlockId.AllEntries.AddRange(_allBlockNames);
				this.txtNewBlockId.UpdateFilteredList();
			}
		}
	}

	// Token: 0x06007C68 RID: 31848 RVA: 0x002F2424 File Offset: 0x002F0624
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReplaceBlockIds_OnChangeHandler(XUiController _sender, string _text, bool _validInput, bool _changeFromCode)
	{
		if (this.txtOldBlockId == null || this.txtNewBlockId == null || this.btnReplaceBlockIds == null)
		{
			return;
		}
		bool flag = Block.GetBlockByName(this.txtOldBlockId.Text, true) != null;
		bool flag2 = Block.GetBlockByName(this.txtNewBlockId.Text, true) != null;
		this.txtOldBlockId.TextInput.ActiveTextColor = (flag ? Color.white : Color.red);
		this.txtNewBlockId.TextInput.ActiveTextColor = (flag2 ? Color.white : Color.red);
		this.btnReplaceBlockIds.Enabled = flag && flag2;
	}

	// Token: 0x06007C69 RID: 31849 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReplaceBlockIds_OnSubmitHandler(XUiController _sender, string _text)
	{
	}

	// Token: 0x06007C6A RID: 31850 RVA: 0x002F24C0 File Offset: 0x002F06C0
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnReplaceBlockIds_OnPressed(XUiController _sender, int _mouseButton)
	{
		Block blockByName = Block.GetBlockByName(this.txtOldBlockId.Text, true);
		Block blockByName2 = Block.GetBlockByName(this.txtNewBlockId.Text, true);
		if (blockByName == null)
		{
			return;
		}
		if (blockByName2 == null)
		{
			return;
		}
		XUiC_LevelToolsHelpers.ReplaceBlockId(blockByName, blockByName2);
	}

	// Token: 0x06007C6B RID: 31851 RVA: 0x002F2500 File Offset: 0x002F0700
	[PublicizedFrom(EAccessModifier.Private)]
	public void initShapeMaterialReplacer()
	{
		this.txtOldShapeMaterial = base.GetChildById("txtOldShapeMaterial") as XUiC_DropDown;
		this.txtNewShapeMaterial = base.GetChildById("txtNewShapeMaterial") as XUiC_DropDown;
		if (this.txtOldShapeMaterial != null)
		{
			this.blockListsInitDone = false;
			this.txtOldShapeMaterial.OnChangeHandler += this.ReplaceShapeMaterial_OnChangeHandler;
			this.txtOldShapeMaterial.OnSubmitHandler += this.ReplaceShapeMaterial_OnSubmitHandler;
			XUiC_TextInput textInput = this.txtOldShapeMaterial.TextInput;
			XUiC_DropDown xuiC_DropDown = this.txtNewShapeMaterial;
			textInput.SelectOnTab = ((xuiC_DropDown != null) ? xuiC_DropDown.TextInput : null);
		}
		if (this.txtNewShapeMaterial != null)
		{
			this.blockListsInitDone = false;
			this.txtNewShapeMaterial.OnChangeHandler += this.ReplaceShapeMaterial_OnChangeHandler;
			this.txtNewShapeMaterial.OnSubmitHandler += this.ReplaceShapeMaterial_OnSubmitHandler;
			XUiC_TextInput textInput2 = this.txtNewShapeMaterial.TextInput;
			XUiC_DropDown xuiC_DropDown2 = this.txtOldShapeMaterial;
			textInput2.SelectOnTab = ((xuiC_DropDown2 != null) ? xuiC_DropDown2.TextInput : null);
		}
		this.btnReplaceShapeMaterials = base.GetChildById("btnReplaceShapeMaterials") as XUiC_SimpleButton;
		if (this.btnReplaceShapeMaterials != null)
		{
			this.btnReplaceShapeMaterials.OnPressed += this.BtnReplaceShapeMaterial_OnPressed;
		}
		XUiController childById = base.GetChildById("btnReplaceShapeMaterialSwitchInOut");
		XUiV_Button xuiV_Button = ((childById != null) ? childById.ViewComponent : null) as XUiV_Button;
		if (xuiV_Button != null)
		{
			xuiV_Button.Controller.OnPress += delegate(XUiController _sender, int _button)
			{
				if (this.txtOldShapeMaterial == null || this.txtNewShapeMaterial == null)
				{
					return;
				}
				XUiC_DropDown xuiC_DropDown3 = this.txtOldShapeMaterial;
				XUiC_DropDown xuiC_DropDown4 = this.txtNewShapeMaterial;
				string text = this.txtNewShapeMaterial.Text;
				string text2 = this.txtOldShapeMaterial.Text;
				xuiC_DropDown3.Text = text;
				xuiC_DropDown4.Text = text2;
				this.ReplaceShapeMaterial_OnChangeHandler(this, null, true, true);
			};
		}
	}

	// Token: 0x06007C6C RID: 31852 RVA: 0x002F2664 File Offset: 0x002F0864
	[PublicizedFrom(EAccessModifier.Private)]
	public void onOpenShapeMaterialReplacer(List<string> _allBlockNames)
	{
		this.ReplaceShapeMaterial_OnChangeHandler(this, null, true, true);
		if (_allBlockNames != null)
		{
			HashSet<string> autoShapeMaterials = Block.GetAutoShapeMaterials();
			if (this.txtOldShapeMaterial != null)
			{
				this.txtOldShapeMaterial.AllEntries.AddRange(autoShapeMaterials);
				this.txtOldShapeMaterial.UpdateFilteredList();
			}
			if (this.txtNewShapeMaterial != null)
			{
				this.txtNewShapeMaterial.AllEntries.AddRange(autoShapeMaterials);
				this.txtNewShapeMaterial.UpdateFilteredList();
			}
		}
	}

	// Token: 0x06007C6D RID: 31853 RVA: 0x002F26CC File Offset: 0x002F08CC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReplaceShapeMaterial_OnChangeHandler(XUiController _sender, string _text, bool _validInput, bool _changefromcode)
	{
		if (this.txtOldShapeMaterial == null || this.txtNewShapeMaterial == null || this.btnReplaceShapeMaterials == null)
		{
			return;
		}
		HashSet<string> autoShapeMaterials = Block.GetAutoShapeMaterials();
		bool flag = autoShapeMaterials.Contains(this.txtOldShapeMaterial.Text);
		bool flag2 = autoShapeMaterials.Contains(this.txtNewShapeMaterial.Text);
		this.txtOldShapeMaterial.TextInput.ActiveTextColor = (flag ? Color.white : Color.red);
		this.txtNewShapeMaterial.TextInput.ActiveTextColor = (flag2 ? Color.white : Color.red);
		this.btnReplaceShapeMaterials.Enabled = flag && flag2;
	}

	// Token: 0x06007C6E RID: 31854 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void ReplaceShapeMaterial_OnSubmitHandler(XUiController _sender, string _text)
	{
	}

	// Token: 0x06007C6F RID: 31855 RVA: 0x002F2768 File Offset: 0x002F0968
	[PublicizedFrom(EAccessModifier.Private)]
	public void BtnReplaceShapeMaterial_OnPressed(XUiController _sender, int _mouseButton)
	{
		HashSet<string> autoShapeMaterials = Block.GetAutoShapeMaterials();
		string text = this.txtOldShapeMaterial.Text;
		string text2 = this.txtNewShapeMaterial.Text;
		if (!autoShapeMaterials.TryGetValue(text, out text))
		{
			return;
		}
		if (!autoShapeMaterials.TryGetValue(text2, out text2))
		{
			return;
		}
		XUiC_LevelToolsHelpers.ReplaceBlockShapeMaterials(text, text2);
	}

	// Token: 0x04005C11 RID: 23569
	public static string ID = "";

	// Token: 0x04005C12 RID: 23570
	[TupleElementNames(new string[] { "toggle", "action" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ValueTuple<XUiC_ToggleButton, NGuiAction>> toggleList = new List<ValueTuple<XUiC_ToggleButton, NGuiAction>>();

	// Token: 0x04005C13 RID: 23571
	[TupleElementNames(new string[] { "button", "action" })]
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ValueTuple<XUiC_SimpleButton, NGuiAction>> buttonList = new List<ValueTuple<XUiC_SimpleButton, NGuiAction>>();

	// Token: 0x04005C14 RID: 23572
	[PublicizedFrom(EAccessModifier.Private)]
	public bool blockListsInitDone;

	// Token: 0x04005C15 RID: 23573
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown dropdownHighlightBlockName;

	// Token: 0x04005C16 RID: 23574
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown txtOldBlockId;

	// Token: 0x04005C17 RID: 23575
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown txtNewBlockId;

	// Token: 0x04005C18 RID: 23576
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnReplaceBlockIds;

	// Token: 0x04005C19 RID: 23577
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown txtOldShapeMaterial;

	// Token: 0x04005C1A RID: 23578
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_DropDown txtNewShapeMaterial;

	// Token: 0x04005C1B RID: 23579
	[PublicizedFrom(EAccessModifier.Private)]
	public XUiC_SimpleButton btnReplaceShapeMaterials;
}
