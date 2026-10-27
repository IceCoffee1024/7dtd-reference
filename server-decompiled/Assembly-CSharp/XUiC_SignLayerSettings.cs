using System;
using UnityEngine.Scripting;

// Token: 0x02001059 RID: 4185
[Preserve]
public abstract class XUiC_SignLayerSettings : XUiController
{
	// Token: 0x06008459 RID: 33881
	public abstract void SetLayer(SignData.SignLayer layer);

	// Token: 0x0600845A RID: 33882 RVA: 0x0032839C File Offset: 0x0032659C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void SetDefaultValue(string id, object value)
	{
		XUiC_SignEditorControl xuiC_SignEditorControl = base.GetChildById(id) as XUiC_SignEditorControl;
		if (xuiC_SignEditorControl != null)
		{
			xuiC_SignEditorControl.defaultValue = value;
		}
	}

	// Token: 0x0600845B RID: 33883 RVA: 0x00089935 File Offset: 0x00087B35
	[PublicizedFrom(EAccessModifier.Protected)]
	public XUiC_SignLayerSettings()
	{
	}

	// Token: 0x04006241 RID: 25153
	public Action<string, bool> OnPreLayerSettingsChanged;

	// Token: 0x04006242 RID: 25154
	public Action OnLayerSettingsChanged;
}
