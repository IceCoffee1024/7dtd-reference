using System;
using UnityEngine;

// Token: 0x0200164D RID: 5709
public class vp_SimpleCrosshair : MonoBehaviour
{
	// Token: 0x0600B2E9 RID: 45801 RVA: 0x0042C5E7 File Offset: 0x0042A7E7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B2EA RID: 45802 RVA: 0x0042C5F4 File Offset: 0x0042A7F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B2EB RID: 45803 RVA: 0x0042C610 File Offset: 0x0042A810
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B2EC RID: 45804 RVA: 0x0042C62C File Offset: 0x0042A82C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnGUI()
	{
		if (this.m_ImageCrosshair == null)
		{
			return;
		}
		if (this.HideOnFirstPersonZoom && this.m_Player.Zoom.Active && this.m_Player.IsFirstPerson.Get())
		{
			return;
		}
		if (this.HideOnDeath && this.m_Player.Dead.Active)
		{
			return;
		}
		GUI.color = new Color(1f, 1f, 1f, 0.8f);
		GUI.DrawTexture(new Rect((float)Screen.width * 0.5f - (float)this.m_ImageCrosshair.width * 0.5f, (float)Screen.height * 0.5f - (float)this.m_ImageCrosshair.height * 0.5f, (float)this.m_ImageCrosshair.width, (float)this.m_ImageCrosshair.height), this.m_ImageCrosshair);
		GUI.color = Color.white;
	}

	// Token: 0x1700152C RID: 5420
	// (get) Token: 0x0600B2ED RID: 45805 RVA: 0x0042C723 File Offset: 0x0042A923
	// (set) Token: 0x0600B2EE RID: 45806 RVA: 0x0042C72B File Offset: 0x0042A92B
	public virtual Texture OnValue_Crosshair
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return this.m_ImageCrosshair;
		}
		[PublicizedFrom(EAccessModifier.Protected)]
		set
		{
			this.m_ImageCrosshair = value;
		}
	}

	// Token: 0x04008657 RID: 34391
	public Texture m_ImageCrosshair;

	// Token: 0x04008658 RID: 34392
	public bool HideOnFirstPersonZoom = true;

	// Token: 0x04008659 RID: 34393
	public bool HideOnDeath = true;

	// Token: 0x0400865A RID: 34394
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
