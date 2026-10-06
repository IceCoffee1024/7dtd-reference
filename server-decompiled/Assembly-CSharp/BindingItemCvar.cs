using System;

// Token: 0x02001105 RID: 4357
public class BindingItemCvar : BindingItem
{
	// Token: 0x06008A26 RID: 35366 RVA: 0x0034C730 File Offset: 0x0034A930
	public BindingItemCvar(BindingInfo _parent, string _sourceText)
		: base(_sourceText)
	{
		this.fieldName = this.fieldName.Replace("cvar(", "").Replace(")", "");
		if (this.fieldName.IndexOf(':') >= 0)
		{
			string[] array = this.fieldName.Split(':', StringSplitOptions.None);
			this.fieldName = array[0];
			this.format = array[1];
		}
		for (XUiController xuiController = _parent.View.Controller; xuiController != null; xuiController = xuiController.Parent)
		{
			if (xuiController.GetType() != typeof(XUiController))
			{
				xuiController.Bindings.AddBinding(_parent);
				return;
			}
		}
	}

	// Token: 0x06008A27 RID: 35367 RVA: 0x0034C7DC File Offset: 0x0034A9DC
	public override string GetValue()
	{
		return XUiM_Player.GetPlayer().GetCVar(this.fieldName).ToString(this.format);
	}

	// Token: 0x040066D7 RID: 26327
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
