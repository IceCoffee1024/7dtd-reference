using System;
using Platform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020011FE RID: 4606
public class GUIButtonPrompt : MonoBehaviour
{
	// Token: 0x060092F4 RID: 37620 RVA: 0x0037A193 File Offset: 0x00378393
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x060092F5 RID: 37621 RVA: 0x0037A1A1 File Offset: 0x003783A1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.RefreshIcon();
	}

	// Token: 0x060092F6 RID: 37622 RVA: 0x0037A1AC File Offset: 0x003783AC
	public void RefreshIcon()
	{
		PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyleFromSelectedIconStyle();
		this.image.sprite = ((inputStyle == PlayerInputManager.InputStyle.PS4) ? this.PSSprite : this.XBSprite);
	}

	// Token: 0x04006E0B RID: 28171
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite XBSprite;

	// Token: 0x04006E0C RID: 28172
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite PSSprite;

	// Token: 0x04006E0D RID: 28173
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Image image;
}
