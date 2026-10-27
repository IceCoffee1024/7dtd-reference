using System;
using UnityEngine;

// Token: 0x020015D5 RID: 5589
public class vp_FPSDemoPlaceHolderMessenger : MonoBehaviour
{
	// Token: 0x0600AE5A RID: 44634 RVA: 0x00411162 File Offset: 0x0040F362
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		this.Player = base.transform.root.GetComponent<vp_FPPlayerEventHandler>();
		if (this.Player == null)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600AE5B RID: 44635 RVA: 0x00411190 File Offset: 0x0040F390
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

	// Token: 0x0400819B RID: 33179
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public vp_FPPlayerEventHandler Player;

	// Token: 0x0400819C RID: 33180
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasSwingingMaceIn3rdPersonLastFrame;

	// Token: 0x0400819D RID: 33181
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_WasClimbingIn3rdPersonLastFrame;
}
