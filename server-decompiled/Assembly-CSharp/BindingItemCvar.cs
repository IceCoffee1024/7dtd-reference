using System;

// Token: 0x02001115 RID: 4373
public class BindingItemCvar : BindingItem
{
	// Token: 0x06008A7A RID: 35450 RVA: 0x0034E7E0 File Offset: 0x0034C9E0
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

	// Token: 0x06008A7B RID: 35451 RVA: 0x0034E88C File Offset: 0x0034CA8C
	public override string GetValue()
	{
		return XUiM_Player.GetPlayer().GetCVar(this.fieldName).ToString(this.format);
	}

	// Token: 0x04006717 RID: 26391
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly string format;
}
