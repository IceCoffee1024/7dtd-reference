using System;
using UnityEngine;

// Token: 0x0200161C RID: 5660
public class vp_SimpleCrosshair : MonoBehaviour
{
	// Token: 0x0600B1FF RID: 45567 RVA: 0x00426507 File Offset: 0x00424707
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B200 RID: 45568 RVA: 0x00426514 File Offset: 0x00424714
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B201 RID: 45569 RVA: 0x00426530 File Offset: 0x00424730
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B202 RID: 45570 RVA: 0x0042654C File Offset: 0x0042474C
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

	// Token: 0x170014DD RID: 5341
	// (get) Token: 0x0600B203 RID: 45571 RVA: 0x00426643 File Offset: 0x00424843
	// (set) Token: 0x0600B204 RID: 45572 RVA: 0x0042664B File Offset: 0x0042484B
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

	// Token: 0x040085C0 RID: 34240
	public Texture m_ImageCrosshair;

	// Token: 0x040085C1 RID: 34241
	public bool HideOnFirstPersonZoom = true;

	// Token: 0x040085C2 RID: 34242
	public bool HideOnDeath = true;

	// Token: 0x040085C3 RID: 34243
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
