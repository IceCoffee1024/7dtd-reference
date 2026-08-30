using System;
using SandboxOptions;
using UnityEngine.Scripting;

// Token: 0x02000DDE RID: 3550
[Preserve]
public class XUiC_ServerBrowserGamePrefSelectorComboSandboxOption : XUiC_ServerBrowserGamePrefSelectorCombo
{
	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x06006C9E RID: 27806 RVA: 0x002A50BC File Offset: 0x002A32BC
	// (set) Token: 0x06006C9F RID: 27807 RVA: 0x002A50C4 File Offset: 0x002A32C4
	[XuiXmlAttribute("sandboxoption", false)]
	public global::SandboxOptions.SandboxOptions SandboxOption
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get;
		[PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x002A50D0 File Offset: 0x002A32D0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void setupOptions()
	{
		this.controlCombo.Elements.Clear();
		this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Custom, ""));
		if (this.allowAny)
		{
			this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(XUiC_ServerBrowserGamePrefSelectorCombo.EOptionValueType.Any, Localization.Get("goAnyValue", false, null)));
		}
		string[] array;
		switch (this.valueType)
		{
		case GamePrefs.EnumType.Int:
		{
			BaseSandboxOption baseSandboxOption = SandboxOptionManager.Current.SandboxOptionsDict[this.SandboxOption];
			if (baseSandboxOption.OptionType != BaseSandboxOption.OptionTypes.Int && baseSandboxOption.OptionType != BaseSandboxOption.OptionTypes.Float)
			{
				Log.Error(string.Concat(new string[]
				{
					"SandboxOption ",
					this.SandboxOption.ToStringCached<global::SandboxOptions.SandboxOptions>(),
					" for ",
					base.getGameInfoName(),
					" is neither Int nor Float option. Hierarchy: ",
					base.GetXuiHierarchy()
				}));
				return;
			}
			SandboxOptionValueSet valueSet = baseSandboxOption.GetValueSet();
			int valueCount = valueSet.GetValueCount();
			array = new string[valueCount];
			int[] array2 = new int[valueCount];
			for (int i = 0; i < valueCount; i++)
			{
				array[i] = valueSet.GetDisplayAtIndex(i, null);
				int num;
				valueSet.GetIntValue(i, out num);
				array2[i] = num;
			}
			goto IL_0240;
		}
		case GamePrefs.EnumType.String:
			throw new Exception("Option type string not supported for SandboxOptions. " + base.getGameInfoName() + ". Hierarchy: " + base.GetXuiHierarchy());
		case GamePrefs.EnumType.Bool:
		{
			BaseSandboxOption baseSandboxOption2 = SandboxOptionManager.Current.SandboxOptionsDict[this.SandboxOption];
			if (baseSandboxOption2.OptionType != BaseSandboxOption.OptionTypes.Bool)
			{
				Log.Error(string.Concat(new string[]
				{
					"SandboxOption ",
					this.SandboxOption.ToStringCached<global::SandboxOptions.SandboxOptions>(),
					" for ",
					base.getGameInfoName(),
					" is not a Bool option. Hierarchy: ",
					base.GetXuiHierarchy()
				}));
				return;
			}
			SandboxOptionValueSet valueSet2 = baseSandboxOption2.GetValueSet();
			int valueCount2 = valueSet2.GetValueCount();
			array = new string[valueCount2];
			int[] array2 = new int[valueCount2];
			for (int j = 0; j < valueCount2; j++)
			{
				array[j] = valueSet2.GetDisplayAtIndex(j, null);
				bool flag;
				valueSet2.GetBoolValue(j, out flag);
				array2[j] = (flag ? 1 : 0);
			}
			goto IL_0240;
		}
		}
		throw new NotSupportedException("Not a valid GameInfoX: " + this.viewComponent.ID);
		IL_0240:
		if (array == null)
		{
			throw new Exception("Illegal option setup for " + base.getGameInfoName() + " (names null)");
		}
		if (this.valueType != GamePrefs.EnumType.String)
		{
			int[] array2;
			if (array2 == null)
			{
				throw new Exception("Illegal option setup for " + base.getGameInfoName() + " (values still null)");
			}
			for (int k = 0; k < array2.Length; k++)
			{
				this.controlCombo.Elements.Add(new XUiC_ServerBrowserGamePrefSelectorCombo.GameOptionValue(array2[k], array[k]));
			}
		}
		this.controlCombo.MinIndex = (((this.valueType == GamePrefs.EnumType.Int || this.valueType == GamePrefs.EnumType.String) && (base.ForceCustom || base.AllowCustom)) ? 0 : 1);
		base.setDefaultValues();
	}
}
