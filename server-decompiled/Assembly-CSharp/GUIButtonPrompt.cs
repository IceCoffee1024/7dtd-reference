using System;
using Platform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001232 RID: 4658
public class GUIButtonPrompt : MonoBehaviour
{
	// Token: 0x06009486 RID: 38022 RVA: 0x00383CB7 File Offset: 0x00381EB7
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06009487 RID: 38023 RVA: 0x00383CC5 File Offset: 0x00381EC5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.RefreshIcon();
	}

	// Token: 0x06009488 RID: 38024 RVA: 0x00383CD0 File Offset: 0x00381ED0
	public void RefreshIcon()
	{
		PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyleFromSelectedIconStyle();
		this.image.sprite = ((inputStyle == PlayerInputManager.InputStyle.PS4) ? this.PSSprite : this.XBSprite);
	}

	// Token: 0x04006F4E RID: 28494
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite XBSprite;

	// Token: 0x04006F4F RID: 28495
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite PSSprite;

	// Token: 0x04006F50 RID: 28496
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Image image;
}
