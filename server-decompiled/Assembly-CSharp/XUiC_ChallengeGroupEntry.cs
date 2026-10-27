using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EF5 RID: 3829
[Preserve]
public class XUiC_ChallengeGroupEntry : XUiController
{
	// Token: 0x17000E29 RID: 3625
	// (get) Token: 0x06007770 RID: 30576 RVA: 0x002D1A14 File Offset: 0x002CFC14
	// (set) Token: 0x06007771 RID: 30577 RVA: 0x002D1A1C File Offset: 0x002CFC1C
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

	// Token: 0x06007772 RID: 30578 RVA: 0x002D1A57 File Offset: 0x002CFC57
	public override void Init()
	{
		base.Init();
		this.ChallengeList = base.GetChildByType<XUiC_ChallengeEntryList>();
		this.IsDirty = true;
	}

	// Token: 0x06007773 RID: 30579 RVA: 0x002D1A74 File Offset: 0x002CFC74
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

	// Token: 0x06007774 RID: 30580 RVA: 0x002D1C74 File Offset: 0x002CFE74
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

	// Token: 0x06007775 RID: 30581 RVA: 0x002D1D16 File Offset: 0x002CFF16
	public void Select()
	{
		this.Owner.SelectedGroup = this;
	}

	// Token: 0x06007776 RID: 30582 RVA: 0x000027FC File Offset: 0x000009FC
	public void UnSelect()
	{
	}

	// Token: 0x06007777 RID: 30583 RVA: 0x002D1D24 File Offset: 0x002CFF24
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.IsDirty = true;
	}

	// Token: 0x06007778 RID: 30584 RVA: 0x0008ABC9 File Offset: 0x00088DC9
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400586D RID: 22637
	public bool IsHovered;

	// Token: 0x0400586E RID: 22638
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400586F RID: 22639
	public XUiC_ChallengeEntryList ChallengeList;

	// Token: 0x04005870 RID: 22640
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> currentItems;

	// Token: 0x04005871 RID: 22641
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup group;

	// Token: 0x04005872 RID: 22642
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroupEntry entry;

	// Token: 0x04005873 RID: 22643
	public XUiC_ChallengeGroupList Owner;
}
