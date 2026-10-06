using System;
using UnityEngine.Scripting;

// Token: 0x02001049 RID: 4169
[Preserve]
public abstract class XUiC_SignLayerSettings : XUiController
{
	// Token: 0x06008407 RID: 33799
	public abstract void SetLayer(SignData.SignLayer layer);

	// Token: 0x06008408 RID: 33800 RVA: 0x00326698 File Offset: 0x00324898
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetDefaultValue(string id, object value)
	{
		XUiC_SignEditorControl xuiC_SignEditorControl = base.GetChildById(id) as XUiC_SignEditorControl;
		if (xuiC_SignEditorControl != null)
		{
			xuiC_SignEditorControl.defaultValue = value;
		}
	}

	// Token: 0x06008409 RID: 33801 RVA: 0x00089701 File Offset: 0x00087901
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayerSettings()
	{
	}

	// Token: 0x04006203 RID: 25091
	public Action<string, bool> OnPreLayerSettingsChanged;

	// Token: 0x04006204 RID: 25092
	public Action OnLayerSettingsChanged;
}
