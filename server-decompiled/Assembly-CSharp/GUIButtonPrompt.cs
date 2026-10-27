using System;
using Platform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001244 RID: 4676
public class GUIButtonPrompt : MonoBehaviour
{
	// Token: 0x060094DD RID: 38109 RVA: 0x003860B3 File Offset: 0x003842B3
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x060094DE RID: 38110 RVA: 0x003860C1 File Offset: 0x003842C1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.RefreshIcon();
	}

	// Token: 0x060094DF RID: 38111 RVA: 0x003860CC File Offset: 0x003842CC
	public void RefreshIcon()
	{
		PlayerInputManager.InputStyle inputStyle = PlayerInputManager.InputStyleFromSelectedIconStyle();
		this.image.sprite = ((inputStyle == PlayerInputManager.InputStyle.PS4) ? this.PSSprite : this.XBSprite);
	}

	// Token: 0x04006F8F RID: 28559
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite XBSprite;

	// Token: 0x04006F90 RID: 28560
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public Sprite PSSprite;

	// Token: 0x04006F91 RID: 28561
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Image image;
}
