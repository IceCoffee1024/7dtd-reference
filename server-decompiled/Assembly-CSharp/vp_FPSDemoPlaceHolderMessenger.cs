using System;
using UnityEngine;

// Token: 0x020015A1 RID: 5537
public class vp_FPSDemoPlaceHolderMessenger : MonoBehaviour
{
	// Token: 0x0600AD4F RID: 44367 RVA: 0x00409F0A File Offset: 0x0040810A
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.Player = base.transform.root.GetComponent<vp_FPPlayerEventHandler>();
		if (this.Player == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600AD50 RID: 44368 RVA: 0x00409F38 File Offset: 0x00408138
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.Player == null)
		{
			return;
		}
		if (!this.Player.IsFirstPerson.Get() && this.Player.Climb.Active)
		{
			if (!this.m_WasClimbingIn3rdPersonLastFrame)
			{
				this.m_WasClimbingIn3rdPersonLastFrame = true;
				vp_Timer.In(0f, delegate
				{
					this.Player.HUDText.Send("PLACEHOLDER CLIMB ANIMATION");
				}, 3, 1f, null);
			}
		}
		else
		{
			this.m_WasClimbingIn3rdPersonLastFrame = false;
		}
		if (!this.Player.IsFirstPerson.Get() && this.Player.CurrentWeaponIndex.Get() == 4 && this.Player.Attack.Active)
		{
			if (!this.m_WasSwingingMaceIn3rdPersonLastFrame)
			{
				this.m_WasSwingingMaceIn3rdPersonLastFrame = true;
				vp_Timer.In(0f, delegate
				{
					this.Player.HUDText.Send("PLACEHOLDER MELEE ANIMATION");
				}, 3, 1f, null);
				return;
			}
		}
		else
		{
			this.m_WasSwingingMaceIn3rdPersonLastFrame = false;
		}
	}

	// Token: 0x040080D7 RID: 32983
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler Player;

	// Token: 0x040080D8 RID: 32984
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasSwingingMaceIn3rdPersonLastFrame;

	// Token: 0x040080D9 RID: 32985
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasClimbingIn3rdPersonLastFrame;
}
