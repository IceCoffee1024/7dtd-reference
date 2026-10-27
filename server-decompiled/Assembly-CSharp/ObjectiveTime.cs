using System;
using System.Collections.Generic;
using System.Globalization;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009C2 RID: 2498
[Preserve]
public class ObjectiveTime : BaseObjective
{
	// Token: 0x170007EC RID: 2028
	// (get) Token: 0x06004922 RID: 18722 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Time;
		}
	}

	// Token: 0x170007ED RID: 2029
	// (get) Token: 0x06004923 RID: 18723 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UpdateUI
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007EE RID: 2030
	// (get) Token: 0x06004924 RID: 18724 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007EF RID: 2031
	// (get) Token: 0x06004925 RID: 18725 RVA: 0x001CAF74 File Offset: 0x001C9174
	public override string StatusText
	{
		get
		{
			if (this.currentTime > 0f)
			{
				return XUiM_PlayerBuffs.GetTimeString(this.currentTime);
			}
			if (base.Optional)
			{
				base.ObjectiveState = BaseObjective.ObjectiveStates.Failed;
				return Localization.Get("failed", false, null);
			}
			base.ObjectiveState = BaseObjective.ObjectiveStates.Complete;
			return Localization.Get("completed", false, null);
		}
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x001CAFC9 File Offset: 0x001C91C9
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveTime_keyword", false, null);
		this.dayLengthInSeconds = GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60;
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x001CAFED File Offset: 0x001C91ED
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}:", this.keyword);
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x001C7021 File Offset: 0x001C5221
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x001C9082 File Offset: 0x001C7282
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x001CB008 File Offset: 0x001C9208
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.Optional)
		{
			base.Complete = this.currentTime > 0f;
		}
		else
		{
			base.Complete = this.currentTime <= 0f;
		}
		if (base.Complete)
		{
			base.OwnerQuest.RefreshQuestCompletion(QuestClass.CompletionTypes.AutoComplete, null, true, null);
		}
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x001CB065 File Offset: 0x001C9265
	public override void Read(PooledBinaryReader _br)
	{
		this.currentTime = (float)_br.ReadUInt16();
		this.currentValue = 1;
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x001CB07B File Offset: 0x001C927B
	public override void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((ushort)this.currentTime);
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x001CB08A File Offset: 0x001C928A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveTime objectiveTime = (ObjectiveTime)objective;
		objectiveTime.currentTime = this.currentTime;
		objectiveTime.overrideType = this.overrideType;
		objectiveTime.overrideOffset = this.overrideOffset;
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x001CB0BC File Offset: 0x001C92BC
	public override BaseObjective Clone()
	{
		ObjectiveTime objectiveTime = new ObjectiveTime();
		this.CopyValues(objectiveTime);
		return objectiveTime;
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x001CB0D8 File Offset: 0x001C92D8
	public override void Update(float updateTime)
	{
		if (this.firstRun)
		{
			if (this.overrideType == ObjectiveTime.OverrideTypes.VoteTime && TwitchManager.HasInstance && TwitchManager.Current.IsVoting)
			{
				this.currentTime = TwitchManager.Current.VotingManager.VoteTimeRemaining + this.overrideOffset;
				this.firstRun = false;
				return;
			}
			if (this.Value.EqualsCaseInsensitive("day"))
			{
				this.currentTime = (float)this.dayLengthInSeconds;
			}
			else
			{
				this.currentTime = StringParsers.ParseFloat(this.Value, 0, -1, NumberStyles.Any);
			}
			this.firstRun = false;
		}
		this.currentTime -= updateTime;
		if (this.currentTime < 0f)
		{
			this.Refresh();
			base.HandleRemoveHooks();
		}
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x001CB197 File Offset: 0x001C9397
	public override void HandleFailed()
	{
		this.currentTime = 0f;
		base.Complete = false;
	}

	// Token: 0x06004931 RID: 18737 RVA: 0x001CB1AB File Offset: 0x001C93AB
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTime.PropTime, ref this.Value);
		properties.ParseEnum<ObjectiveTime.OverrideTypes>(ObjectiveTime.PropOverrideType, ref this.overrideType);
		properties.ParseFloat(ObjectiveTime.PropOverrideOffset, ref this.overrideOffset);
	}

	// Token: 0x06004932 RID: 18738 RVA: 0x001CB1E8 File Offset: 0x001C93E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("time", this.Value);
		parameters.Add("override_type", this.overrideType);
		parameters.Add("override_offset", this.overrideOffset);
	}

	// Token: 0x04003A4D RID: 14925
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTime;

	// Token: 0x04003A4E RID: 14926
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveTime.OverrideTypes overrideType;

	// Token: 0x04003A4F RID: 14927
	[PublicizedFrom(EAccessModifier.Private)]
	public float overrideOffset;

	// Token: 0x04003A50 RID: 14928
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstRun = true;

	// Token: 0x04003A51 RID: 14929
	public static string PropTime = "time";

	// Token: 0x04003A52 RID: 14930
	public static string PropOverrideType = "override_type";

	// Token: 0x04003A53 RID: 14931
	public static string PropOverrideOffset = "override_offset";

	// Token: 0x04003A54 RID: 14932
	[PublicizedFrom(EAccessModifier.Private)]
	public int dayLengthInSeconds;

	// Token: 0x020009C3 RID: 2499
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OverrideTypes
	{
		// Token: 0x04003A56 RID: 14934
		None,
		// Token: 0x04003A57 RID: 14935
		VoteTime
	}
}
