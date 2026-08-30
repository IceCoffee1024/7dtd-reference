using System;
using System.Collections.Generic;
using Unity.Profiling;

// Token: 0x020010EA RID: 4330
public class BindingsManager
{
	// Token: 0x06008949 RID: 35145 RVA: 0x00345F28 File Offset: 0x00344128
	public void RefreshBindings()
	{
		using (this.pmControllerRefreshBindings.Auto())
		{
			for (int i = 0; i < this.bindingList.Count; i++)
			{
				this.bindingList[i].RefreshValue();
			}
		}
	}

	// Token: 0x0600894A RID: 35146 RVA: 0x00345F88 File Offset: 0x00344188
	public void AddBinding(IBindingInstance _info)
	{
		if (!this.bindingList.Contains(_info))
		{
			this.bindingList.Add(_info);
		}
	}

	// Token: 0x0600894B RID: 35147 RVA: 0x00345FA4 File Offset: 0x003441A4
	public static IBindingInstance CreateBinding(XUiView _view, string _attribute, string _value)
	{
		IBindingInstance bindingInstance2;
		using (BindingsManager.pmControllerCreateBinding.Auto())
		{
			BindingNcalcFunctions.RegisterNcalcFunctions();
			IBindingInstance bindingInstance;
			if (BindingInfoNcalc.IsFullNcalcBinding(_value))
			{
				bindingInstance = new BindingInfoNcalc(_view, _attribute, _value);
				bindingInstance.RefreshValue();
			}
			else
			{
				bindingInstance = new BindingInfo(_view, _attribute, _value);
			}
			bindingInstance2 = bindingInstance;
		}
		return bindingInstance2;
	}

	// Token: 0x0600894C RID: 35148 RVA: 0x00346008 File Offset: 0x00344208
	public static string ReplaceCVars(string _fullText)
	{
		string text4;
		using (BindingsManager.pmReplaceCvars.Auto())
		{
			for (int num = _fullText.IndexOf("{cvar(", StringComparison.Ordinal); num != -1; num = _fullText.IndexOf("{cvar(", num, StringComparison.Ordinal))
			{
				string text = _fullText.Substring(num, _fullText.IndexOf('}', num) + 1 - num);
				string text2 = "";
				int num2 = text.IndexOf('(') + 1;
				string text3 = text.Substring(num2, text.IndexOf(')') - num2);
				if (text3.IndexOf(':') >= 0)
				{
					string[] array = text3.Split(':', StringSplitOptions.None);
					text3 = array[0];
					text2 = array[1];
				}
				_fullText = _fullText.Replace(text, XUiM_Player.GetPlayer().GetCVar(text3).ToString(text2));
			}
			text4 = _fullText;
		}
		return text4;
	}

	// Token: 0x040065EE RID: 26094
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IBindingInstance> bindingList = new List<IBindingInstance>();

	// Token: 0x040065EF RID: 26095
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerRefreshBindings = new ProfilerMarker("XC.RefreshBindings");

	// Token: 0x040065F0 RID: 26096
	[PublicizedFrom(EAccessModifier.Private)]
	public static ProfilerMarker pmControllerCreateBinding = new ProfilerMarker("XC.CreateBinding");

	// Token: 0x040065F1 RID: 26097
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker pmReplaceCvars = new ProfilerMarker("ParseCVars");
}
