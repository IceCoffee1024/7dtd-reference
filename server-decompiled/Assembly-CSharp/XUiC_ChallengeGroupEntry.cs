using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EDB RID: 3803
[Preserve]
public class XUiC_ChallengeGroupEntry : XUiController
{
	// Token: 0x17000E08 RID: 3592
	// (get) Token: 0x0600774F RID: 30543 RVA: 0x002CD0E4 File Offset: 0x002CB2E4
	// (set) Token: 0x06007750 RID: 30544 RVA: 0x002CD0EC File Offset: 0x002CB2EC
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

	// Token: 0x06007751 RID: 30545 RVA: 0x002CD127 File Offset: 0x002CB327
	public override void Init()
	{
		base.Init();
		this.ChallengeList = base.GetChildByType<XUiC_ChallengeEntryList>();
		this.IsDirty = true;
	}

	// Token: 0x06007752 RID: 30546 RVA: 0x002CD144 File Offset: 0x002CB344
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

	// Token: 0x06007753 RID: 30547 RVA: 0x002CD344 File Offset: 0x002CB544
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

	// Token: 0x06007754 RID: 30548 RVA: 0x002CD3E6 File Offset: 0x002CB5E6
	public void Select()
	{
		this.Owner.SelectedGroup = this;
	}

	// Token: 0x06007755 RID: 30549 RVA: 0x000027FC File Offset: 0x000009FC
	public void UnSelect()
	{
	}

	// Token: 0x06007756 RID: 30550 RVA: 0x002CD3F4 File Offset: 0x002CB5F4
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.IsDirty = true;
	}

	// Token: 0x06007757 RID: 30551 RVA: 0x0008A715 File Offset: 0x00088915
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x04005810 RID: 22544
	public bool IsHovered;

	// Token: 0x04005811 RID: 22545
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x04005812 RID: 22546
	public XUiC_ChallengeEntryList ChallengeList;

	// Token: 0x04005813 RID: 22547
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> currentItems;

	// Token: 0x04005814 RID: 22548
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup group;

	// Token: 0x04005815 RID: 22549
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroupEntry entry;

	// Token: 0x04005816 RID: 22550
	public XUiC_ChallengeGroupList Owner;
}
