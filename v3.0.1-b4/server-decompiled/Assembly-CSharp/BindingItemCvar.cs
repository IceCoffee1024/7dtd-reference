using System;

// Token: 0x020010DB RID: 4315
public class BindingItemCvar : BindingItem
{
	// Token: 0x060088E2 RID: 35042 RVA: 0x00343EE4 File Offset: 0x003420E4
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

	// Token: 0x060088E3 RID: 35043 RVA: 0x00343F90 File Offset: 0x00342190
	public override string GetValue()
	{
		return XUiM_Player.GetPlayer().GetCVar(this.fieldName).ToString(this.format);
	}

	// Token: 0x040065C7 RID: 26055
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
