using System;
using UnityEngine.Scripting;

// Token: 0x0200103A RID: 4154
[Preserve]
public abstract class XUiC_SignLayerSettings : XUiController
{
	// Token: 0x060083DE RID: 33758
	public abstract void SetLayer(SignData.SignLayer layer);

	// Token: 0x060083DF RID: 33759 RVA: 0x00322CB0 File Offset: 0x00320EB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetDefaultValue(string id, object value)
	{
		XUiC_SignEditorControl xuiC_SignEditorControl = base.GetChildById(id) as XUiC_SignEditorControl;
		if (xuiC_SignEditorControl != null)
		{
			xuiC_SignEditorControl.defaultValue = value;
		}
	}

	// Token: 0x060083E0 RID: 33760 RVA: 0x00089481 File Offset: 0x00087681
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayerSettings()
	{
	}

	// Token: 0x040061C8 RID: 25032
	public Action<string, bool> OnPreLayerSettingsChanged;

	// Token: 0x040061C9 RID: 25033
	public Action OnLayerSettingsChanged;
}
