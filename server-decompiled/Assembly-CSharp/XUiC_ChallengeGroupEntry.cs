using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EEA RID: 3818
[Preserve]
public class XUiC_ChallengeGroupEntry : XUiController
{
	// Token: 0x17000E25 RID: 3621
	// (get) Token: 0x0600774A RID: 30538 RVA: 0x002D04BC File Offset: 0x002CE6BC
	// (set) Token: 0x0600774B RID: 30539 RVA: 0x002D04C4 File Offset: 0x002CE6C4
	public ChallengeGroupEntry Entry
	{
		get
		{
			return this.entry;
		}
		set
		{
			base.ViewComponent.Enabled = value != null;
			this.entry = value;
			this.group = ((this.entry != null) ? this.entry.ChallengeGroup : null);
			this.IsDirty = true;
		}
	}

	// Token: 0x0600774C RID: 30540 RVA: 0x002D04FF File Offset: 0x002CE6FF
	public override void Init()
	{
		base.Init();
		this.ChallengeList = base.GetChildByType<XUiC_ChallengeEntryList>();
		this.IsDirty = true;
	}

	// Token: 0x0600774D RID: 30541 RVA: 0x002D051C File Offset: 0x002CE71C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.group != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 1566407741U)
		{
			if (num != 541430242U)
			{
				if (num != 694121954U)
				{
					if (num == 1566407741U)
					{
						if (bindingName == "hasentry")
						{
							value = (flag ? "true" : "false");
							return true;
						}
					}
				}
				else if (bindingName == "hasreset")
				{
					value = (flag ? (this.group.DayReset != -1).ToString() : "false");
					return true;
				}
			}
			else if (bindingName == "intro_disabled")
			{
				value = (flag ? (!ChallengeJournal.IntroChallengesEnabled && this.group.IsIntro).ToString() : "false");
				return true;
			}
		}
		else if (num <= 2446454527U)
		{
			if (num != 2444835803U)
			{
				if (num == 2446454527U)
				{
					if (bindingName == "groupobjective")
					{
						value = (flag ? this.group.ObjectiveText : "");
						return true;
					}
				}
			}
			else if (bindingName == "groupreward")
			{
				value = (flag ? this.group.RewardText : "");
				return true;
			}
		}
		else if (num != 3504806855U)
		{
			if (num == 4037261514U)
			{
				if (bindingName == "resetday")
				{
					if (flag)
					{
						value = ((this.group.DayReset == -1) ? "" : this.entry.LastUpdateDay.ToString());
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
		}
		else if (bindingName == "groupname")
		{
			value = "";
			if (flag)
			{
				value = this.group.Title;
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x0600774E RID: 30542 RVA: 0x002D071C File Offset: 0x002CE91C
	public override void Update(float _dt)
	{
		if (this.group != null && this.group.UIDirty)
		{
			this.IsDirty = true;
			this.group.UIDirty = false;
		}
		if (this.IsDirty)
		{
			this.currentItems = this.player.challengeJournal.Challenges.Where<Challenge>((Challenge item) => item.ChallengeGroup == this.group).ToList<Challenge>();
			if (this.ChallengeList != null)
			{
				this.ChallengeList.Owner = this;
				this.ChallengeList.SetChallengeEntryList(this.currentItems);
			}
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0600774F RID: 30543 RVA: 0x002D07BE File Offset: 0x002CE9BE
	public void Select()
	{
		this.Owner.SelectedGroup = this;
	}

	// Token: 0x06007750 RID: 30544 RVA: 0x000027FC File Offset: 0x000009FC
	public void UnSelect()
	{
	}

	// Token: 0x06007751 RID: 30545 RVA: 0x002D07CC File Offset: 0x002CE9CC
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.IsDirty = true;
	}

	// Token: 0x06007752 RID: 30546 RVA: 0x0008A995 File Offset: 0x00088B95
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x04005848 RID: 22600
	public bool IsHovered;

	// Token: 0x04005849 RID: 22601
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400584A RID: 22602
	public XUiC_ChallengeEntryList ChallengeList;

	// Token: 0x0400584B RID: 22603
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> currentItems;

	// Token: 0x0400584C RID: 22604
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup group;

	// Token: 0x0400584D RID: 22605
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroupEntry entry;

	// Token: 0x0400584E RID: 22606
	public XUiC_ChallengeGroupList Owner;
}
