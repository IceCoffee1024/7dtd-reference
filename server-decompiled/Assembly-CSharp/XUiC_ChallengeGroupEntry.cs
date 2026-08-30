using System;
using System.Collections.Generic;
using Challenges;
using UniLinq;
using UnityEngine.Scripting;

// Token: 0x02000EC6 RID: 3782
[Preserve]
public class XUiC_ChallengeGroupEntry : XUiController
{
	// Token: 0x17000DEE RID: 3566
	// (get) Token: 0x060076AE RID: 30382 RVA: 0x002C9BC0 File Offset: 0x002C7DC0
	// (set) Token: 0x060076AF RID: 30383 RVA: 0x002C9BC8 File Offset: 0x002C7DC8
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

	// Token: 0x060076B0 RID: 30384 RVA: 0x002C9C03 File Offset: 0x002C7E03
	public override void Init()
	{
		base.Init();
		this.ChallengeList = base.GetChildByType<XUiC_ChallengeEntryList>();
		this.IsDirty = true;
	}

	// Token: 0x060076B1 RID: 30385 RVA: 0x002C9C20 File Offset: 0x002C7E20
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

	// Token: 0x060076B2 RID: 30386 RVA: 0x002C9E20 File Offset: 0x002C8020
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

	// Token: 0x060076B3 RID: 30387 RVA: 0x002C9EC2 File Offset: 0x002C80C2
	public void Select()
	{
		this.Owner.SelectedGroup = this;
	}

	// Token: 0x060076B4 RID: 30388 RVA: 0x000027FC File Offset: 0x000009FC
	public void UnSelect()
	{
	}

	// Token: 0x060076B5 RID: 30389 RVA: 0x002C9ED0 File Offset: 0x002C80D0
	public override void OnOpen()
	{
		base.OnOpen();
		this.player = this.xui.playerUI.entityPlayer;
		this.IsDirty = true;
	}

	// Token: 0x060076B6 RID: 30390 RVA: 0x0008A069 File Offset: 0x00088269
	public void Refresh()
	{
		this.IsDirty = true;
	}

	// Token: 0x0400576B RID: 22379
	public bool IsHovered;

	// Token: 0x0400576C RID: 22380
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x0400576D RID: 22381
	public XUiC_ChallengeEntryList ChallengeList;

	// Token: 0x0400576E RID: 22382
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Challenge> currentItems;

	// Token: 0x0400576F RID: 22383
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroup group;

	// Token: 0x04005770 RID: 22384
	[PublicizedFrom(EAccessModifier.Private)]
	public ChallengeGroupEntry entry;

	// Token: 0x04005771 RID: 22385
	public XUiC_ChallengeGroupList Owner;
}
