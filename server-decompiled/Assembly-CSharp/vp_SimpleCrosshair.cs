using System;
using UnityEngine;

// Token: 0x0200163B RID: 5691
public class vp_SimpleCrosshair : MonoBehaviour
{
	// Token: 0x0600B291 RID: 45713 RVA: 0x0042A1E3 File Offset: 0x004283E3
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		this.m_Player = global::UnityEngine.Object.FindAnyObjectByType<vp_FPPlayerEventHandler>();
	}

	// Token: 0x0600B292 RID: 45714 RVA: 0x0042A1F0 File Offset: 0x004283F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEnable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Register(this);
		}
	}

	// Token: 0x0600B293 RID: 45715 RVA: 0x0042A20C File Offset: 0x0042840C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnDisable()
	{
		if (this.m_Player != null)
		{
			this.m_Player.Unregister(this);
		}
	}

	// Token: 0x0600B294 RID: 45716 RVA: 0x0042A228 File Offset: 0x00428428
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

	// Token: 0x17001520 RID: 5408
	// (get) Token: 0x0600B295 RID: 45717 RVA: 0x0042A31F File Offset: 0x0042851F
	// (set) Token: 0x0600B296 RID: 45718 RVA: 0x0042A327 File Offset: 0x00428527
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

	// Token: 0x04008616 RID: 34326
	public Texture m_ImageCrosshair;

	// Token: 0x04008617 RID: 34327
	public bool HideOnFirstPersonZoom = true;

	// Token: 0x04008618 RID: 34328
	public bool HideOnDeath = true;

	// Token: 0x04008619 RID: 34329
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public vp_FPPlayerEventHandler m_Player;
}
