using System;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020010A7 RID: 4263
[Preserve]
public class XUiC_TwitchVoteEntry : XUiController
{
	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x060087D8 RID: 34776 RVA: 0x0033DB3A File Offset: 0x0033BD3A
	// (set) Token: 0x060087D9 RID: 34777 RVA: 0x0033DB42 File Offset: 0x0033BD42
	public XUiC_TwitchWindow Owner { get; set; }

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x060087DA RID: 34778 RVA: 0x0033DB4B File Offset: 0x0033BD4B
	// (set) Token: 0x060087DB RID: 34779 RVA: 0x0033DB53 File Offset: 0x0033BD53
	public TwitchVoteEntry Vote
	{
		get
		{
			return this.vote;
		}
		set
		{
			this.vote = value;
			this.IsDirty = true;
		}
	}

	// Token: 0x060087DC RID: 34780 RVA: 0x0033DB64 File Offset: 0x0033BD64
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool GetBindingValueInternal(ref string value, string bindingName)
	{
		bool flag = this.vote != null;
		uint num = <PrivateImplementationDetails>.ComputeStringHash(bindingName);
		if (num <= 3057604319U)
		{
			if (num <= 2104701544U)
			{
				if (num != 546437152U)
				{
					if (num != 596770009U)
					{
						if (num == 2104701544U)
						{
							if (bindingName == "votecolor")
							{
								if (flag)
								{
									if (this.vote.Owner.IsHighest(this.vote))
									{
										value = this.positiveColor;
									}
									else
									{
										value = this.negativeColor;
									}
								}
								else
								{
									value = "0,0,0,0";
								}
								return true;
							}
						}
					}
					else if (bindingName == "hasvoteline2")
					{
						if (flag)
						{
							value = (this.vote.VoteClass.VoteLine2 != "").ToString();
						}
						else
						{
							value = "false";
						}
						return true;
					}
				}
				else if (bindingName == "hasvoteline1")
				{
					if (flag)
					{
						value = (this.vote.VoteClass.VoteLine1 != "").ToString();
					}
					else
					{
						value = "false";
					}
					return true;
				}
			}
			else if (num != 2382705624U)
			{
				if (num != 2840813878U)
				{
					if (num == 3057604319U)
					{
						if (bindingName == "voteline2")
						{
							if (flag)
							{
								value = this.vote.VoteClass.VoteLine2;
							}
							return true;
						}
					}
				}
				else if (bindingName == "votename")
				{
					if (flag)
					{
						if (this.vote.Index == 2 && this.vote.Owner.UseMystery)
						{
							value = "?????";
						}
						else
						{
							value = this.vote.VoteClass.Display;
						}
					}
					else
					{
						value = "";
					}
					return true;
				}
			}
			else if (bindingName == "votefill")
			{
				if (flag)
				{
					if (this.vote.VoteCount > 0)
					{
						value = ((float)this.vote.VoteCount / (float)this.vote.Owner.VoteCount).ToString();
					}
					else
					{
						value = "0";
					}
				}
				else
				{
					value = "0";
				}
				return true;
			}
		}
		else if (num <= 3604996386U)
		{
			if (num != 3074381938U)
			{
				if (num != 3366025627U)
				{
					if (num == 3604996386U)
					{
						if (bindingName == "votecommand")
						{
							if (this.isWinner)
							{
								value = "";
							}
							else
							{
								value = (flag ? this.vote.VoteCommand : "");
							}
							return true;
						}
					}
				}
				else if (bindingName == "hasvote")
				{
					value = flag.ToString();
					return true;
				}
			}
			else if (bindingName == "voteline1")
			{
				if (flag)
				{
					value = this.vote.VoteClass.VoteLine1;
				}
				return true;
			}
		}
		else if (num != 3735793316U)
		{
			if (num != 3918524111U)
			{
				if (num == 4025064268U)
				{
					if (bindingName == "line1textcolor")
					{
						if (flag)
						{
							switch (this.vote.VoteClass.DisplayType)
							{
							case TwitchVote.VoteDisplayTypes.GoodBad:
								value = this.textBadColor;
								break;
							case TwitchVote.VoteDisplayTypes.Special:
								value = this.textGoodColor;
								break;
							case TwitchVote.VoteDisplayTypes.HordeBuffed:
								value = this.selectedTextColor;
								break;
							}
						}
						else
						{
							value = "255,255,255";
						}
						return true;
					}
				}
			}
			else if (bindingName == "line2textcolor")
			{
				if (flag)
				{
					if (this.vote.VoteClass.DisplayType == TwitchVote.VoteDisplayTypes.GoodBad)
					{
						value = this.textBadColor;
					}
				}
				else
				{
					value = "255,255,255";
				}
				return true;
			}
		}
		else if (bindingName == "votecount")
		{
			if (flag)
			{
				if (!this.isWinner)
				{
					float num2 = 0f;
					if (this.vote.VoteCount > 0)
					{
						num2 = (float)this.vote.VoteCount / (float)this.vote.Owner.VoteCount;
					}
					value = this.voteCountFormatterInt.Format((int)(num2 * 100f));
				}
				else
				{
					value = "";
				}
			}
			else
			{
				value = "";
			}
			return true;
		}
		return base.GetBindingValueInternal(ref value, bindingName);
	}

	// Token: 0x060087DD RID: 34781 RVA: 0x0033DFA4 File Offset: 0x0033C1A4
	public override bool ParseAttribute(string name, string value)
	{
		if (name == "positive_color")
		{
			this.positiveColor = value;
			return true;
		}
		if (name == "negative_color")
		{
			this.negativeColor = value;
			return true;
		}
		if (name == "disabled_color")
		{
			this.disabledColor = value;
			return true;
		}
		if (name == "selected_color")
		{
			this.selectedTextColor = value;
			return true;
		}
		if (name == "bad_color")
		{
			this.textBadColor = value;
			return true;
		}
		if (!(name == "good_color"))
		{
			return base.ParseAttribute(name, value);
		}
		this.textGoodColor = value;
		return true;
	}

	// Token: 0x060087DE RID: 34782 RVA: 0x0008B2BB File Offset: 0x000894BB
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleOnCountChanged(XUiController _sender, OnCountChangedEventArgs _e)
	{
		base.RefreshBindings();
	}

	// Token: 0x060087DF RID: 34783 RVA: 0x0033E040 File Offset: 0x0033C240
	public override void Update(float _dt)
	{
		if (this.Vote != null && this.Vote.UIDirty)
		{
			this.IsDirty = true;
			this.Vote.UIDirty = false;
		}
		if (this.IsDirty)
		{
			base.RefreshBindings();
			this.IsDirty = false;
		}
		base.Update(_dt);
	}

	// Token: 0x0400652F RID: 25903
	[PublicizedFrom(EAccessModifier.Private)]
	public TwitchVoteEntry vote;

	// Token: 0x04006530 RID: 25904
	public bool isWinner;

	// Token: 0x04006531 RID: 25905
	[PublicizedFrom(EAccessModifier.Private)]
	public string positiveColor = "0,0,255";

	// Token: 0x04006532 RID: 25906
	[PublicizedFrom(EAccessModifier.Private)]
	public string negativeColor = "255,0,0";

	// Token: 0x04006533 RID: 25907
	[PublicizedFrom(EAccessModifier.Private)]
	public string textBadColor = "255,175,175";

	// Token: 0x04006534 RID: 25908
	[PublicizedFrom(EAccessModifier.Private)]
	public string textGoodColor = "175,175,255";

	// Token: 0x04006535 RID: 25909
	[PublicizedFrom(EAccessModifier.Private)]
	public string selectedTextColor = "222,206,163";

	// Token: 0x04006536 RID: 25910
	[PublicizedFrom(EAccessModifier.Private)]
	public string disabledColor = "80,80,80";

	// Token: 0x04006538 RID: 25912
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isReady;

	// Token: 0x04006539 RID: 25913
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CachedStringFormatter<int> voteCountFormatterInt = new CachedStringFormatter<int>((int _i) => _i.ToString() + "%");
}
