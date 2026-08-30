using System;
using UnityEngine;

// Token: 0x02001601 RID: 5633
public class vp_SimpleCrosshair : MonoBehaviour
{
	// Token: 0x0600B102 RID: 45314 RVA: 0x004211C7 File Offset: 0x0041F3C7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B103 RID: 45315 RVA: 0x004211D4 File Offset: 0x0041F3D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B104 RID: 45316 RVA: 0x004211F0 File Offset: 0x0041F3F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B105 RID: 45317 RVA: 0x0042120C File Offset: 0x0041F40C
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

	// Token: 0x170014A4 RID: 5284
	// (get) Token: 0x0600B106 RID: 45318 RVA: 0x00421303 File Offset: 0x0041F503
	// (set) Token: 0x0600B107 RID: 45319 RVA: 0x0042130B File Offset: 0x0041F50B
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

	// Token: 0x040084E7 RID: 34023
	public Texture m_ImageCrosshair;

	// Token: 0x040084E8 RID: 34024
	public bool HideOnFirstPersonZoom = true;

	// Token: 0x040084E9 RID: 34025
	public bool HideOnDeath = true;

	// Token: 0x040084EA RID: 34026
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
