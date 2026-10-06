using System;
using UnityEngine;

// Token: 0x020015C3 RID: 5571
public class vp_FPSDemoPlaceHolderMessenger : MonoBehaviour
{
	// Token: 0x0600AE02 RID: 44546 RVA: 0x0040ED5E File Offset: 0x0040CF5E
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.Player = base.transform.root.GetComponent<vp_FPPlayerEventHandler>();
		if (this.Player == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600AE03 RID: 44547 RVA: 0x0040ED8C File Offset: 0x0040CF8C
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

	// Token: 0x0400815A RID: 33114
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler Player;

	// Token: 0x0400815B RID: 33115
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasSwingingMaceIn3rdPersonLastFrame;

	// Token: 0x0400815C RID: 33116
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasClimbingIn3rdPersonLastFrame;
}
