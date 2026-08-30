using System;
using UnityEngine;

// Token: 0x02001586 RID: 5510
public class vp_FPSDemoPlaceHolderMessenger : MonoBehaviour
{
	// Token: 0x0600AC52 RID: 44114 RVA: 0x00404BCA File Offset: 0x00402DCA
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.Player = base.transform.root.GetComponent<vp_FPPlayerEventHandler>();
		if (this.Player == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600AC53 RID: 44115 RVA: 0x00404BF8 File Offset: 0x00402DF8
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

	// Token: 0x04007FFE RID: 32766
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler Player;

	// Token: 0x04007FFF RID: 32767
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasSwingingMaceIn3rdPersonLastFrame;

	// Token: 0x04008000 RID: 32768
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasClimbingIn3rdPersonLastFrame;
}
