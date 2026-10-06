using System;
using System.Collections.Generic;
using System.Globalization;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009B8 RID: 2488
[Preserve]
public class ObjectiveTime : BaseObjective
{
	// Token: 0x170007E6 RID: 2022
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x0002F260 File Offset: 0x0002D460
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Time;
		}
	}

	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06004900 RID: 18688 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool UpdateUI
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06004902 RID: 18690 RVA: 0x001C9EA0 File Offset: 0x001C80A0
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

	// Token: 0x06004903 RID: 18691 RVA: 0x001C9EF5 File Offset: 0x001C80F5
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveTime_keyword", false, null);
		this.dayLengthInSeconds = GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60;
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x001C9F19 File Offset: 0x001C8119
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}:", this.keyword);
	}

	// Token: 0x06004905 RID: 18693 RVA: 0x001C5F75 File Offset: 0x001C4175
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004906 RID: 18694 RVA: 0x001C7FD6 File Offset: 0x001C61D6
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
	}

	// Token: 0x06004907 RID: 18695 RVA: 0x001C9F34 File Offset: 0x001C8134
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

	// Token: 0x06004908 RID: 18696 RVA: 0x001C9F91 File Offset: 0x001C8191
	public override void Read(PooledBinaryReader _br)
	{
		this.currentTime = (float)_br.ReadUInt16();
		this.currentValue = 1;
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x001C9FA7 File Offset: 0x001C81A7
	public override void Write(PooledBinaryWriter _bw)
	{
		_bw.Write((ushort)this.currentTime);
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x001C9FB6 File Offset: 0x001C81B6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveTime objectiveTime = (ObjectiveTime)objective;
		objectiveTime.currentTime = this.currentTime;
		objectiveTime.overrideType = this.overrideType;
		objectiveTime.overrideOffset = this.overrideOffset;
	}

	// Token: 0x0600490B RID: 18699 RVA: 0x001C9FE8 File Offset: 0x001C81E8
	public override BaseObjective Clone()
	{
		ObjectiveTime objectiveTime = new ObjectiveTime();
		this.CopyValues(objectiveTime);
		return objectiveTime;
	}

	// Token: 0x0600490C RID: 18700 RVA: 0x001CA004 File Offset: 0x001C8204
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

	// Token: 0x0600490D RID: 18701 RVA: 0x001CA0C3 File Offset: 0x001C82C3
	public override void HandleFailed()
	{
		this.currentTime = 0f;
		base.Complete = false;
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x001CA0D7 File Offset: 0x001C82D7
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTime.PropTime, ref this.Value);
		properties.ParseEnum<ObjectiveTime.OverrideTypes>(ObjectiveTime.PropOverrideType, ref this.overrideType);
		properties.ParseFloat(ObjectiveTime.PropOverrideOffset, ref this.overrideOffset);
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x001CA114 File Offset: 0x001C8314
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("time", this.Value);
		parameters.Add("override_type", this.overrideType);
		parameters.Add("override_offset", this.overrideOffset);
	}

	// Token: 0x04003A2D RID: 14893
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTime;

	// Token: 0x04003A2E RID: 14894
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveTime.OverrideTypes overrideType;

	// Token: 0x04003A2F RID: 14895
	[PublicizedFrom(EAccessModifier.Private)]
	public float overrideOffset;

	// Token: 0x04003A30 RID: 14896
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstRun = true;

	// Token: 0x04003A31 RID: 14897
	public static string PropTime = "time";

	// Token: 0x04003A32 RID: 14898
	public static string PropOverrideType = "override_type";

	// Token: 0x04003A33 RID: 14899
	public static string PropOverrideOffset = "override_offset";

	// Token: 0x04003A34 RID: 14900
	[PublicizedFrom(EAccessModifier.Private)]
	public int dayLengthInSeconds;

	// Token: 0x020009B9 RID: 2489
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OverrideTypes
	{
		// Token: 0x04003A36 RID: 14902
		None,
		// Token: 0x04003A37 RID: 14903
		VoteTime
	}
}
