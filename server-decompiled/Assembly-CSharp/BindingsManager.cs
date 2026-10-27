using System;
using System.Collections.Generic;
using Unity.Profiling;

// Token: 0x02001128 RID: 4392
public class BindingsManager
{
	// Token: 0x06008AF0 RID: 35568 RVA: 0x00350AA4 File Offset: 0x0034ECA4
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

	// Token: 0x06008AF1 RID: 35569 RVA: 0x00350B04 File Offset: 0x0034ED04
	public void AddBinding(IBindingInstance _info)
	{
		if (!this.bindingList.Contains(_info))
		{
			this.bindingList.Add(_info);
		}
	}

	// Token: 0x06008AF2 RID: 35570 RVA: 0x00350B20 File Offset: 0x0034ED20
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

	// Token: 0x06008AF3 RID: 35571 RVA: 0x00350B84 File Offset: 0x0034ED84
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

	// Token: 0x04006744 RID: 26436
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<IBindingInstance> bindingList = new List<IBindingInstance>();

	// Token: 0x04006745 RID: 26437
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilerMarker pmControllerRefreshBindings = new ProfilerMarker("XC.RefreshBindings");

	// Token: 0x04006746 RID: 26438
	[PublicizedFrom(EAccessModifier.Private)]
	public static ProfilerMarker pmControllerCreateBinding = new ProfilerMarker("XC.CreateBinding");

	// Token: 0x04006747 RID: 26439
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker pmReplaceCvars = new ProfilerMarker("ParseCVars");
}
