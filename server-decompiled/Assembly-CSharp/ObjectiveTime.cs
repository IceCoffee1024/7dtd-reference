using System;
using System.Globalization;
using System.IO;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009A1 RID: 2465
[Preserve]
public class ObjectiveTime : BaseObjective
{
	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x060048BB RID: 18619 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Time;
		}
	}

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x060048BC RID: 18620 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool UpdateUI
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x060048BD RID: 18621 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x060048BE RID: 18622 RVA: 0x001C6C6C File Offset: 0x001C4E6C
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

	// Token: 0x060048BF RID: 18623 RVA: 0x001C6CC1 File Offset: 0x001C4EC1
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveTime_keyword", false, null);
		this.dayLengthInSeconds = GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60;
	}

	// Token: 0x060048C0 RID: 18624 RVA: 0x001C6CE5 File Offset: 0x001C4EE5
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}:", this.keyword);
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x001C2E96 File Offset: 0x001C1096
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x001C4E72 File Offset: 0x001C3072
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x001C6D00 File Offset: 0x001C4F00
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

	// Token: 0x060048C4 RID: 18628 RVA: 0x001C6D5D File Offset: 0x001C4F5D
	public override void Read(BinaryReader _br)
	{
		this.currentTime = (float)_br.ReadUInt16();
		this.currentValue = 1;
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x001C6D73 File Offset: 0x001C4F73
	public override void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.currentTime);
	}

	// Token: 0x060048C6 RID: 18630 RVA: 0x001C6D82 File Offset: 0x001C4F82
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveTime objectiveTime = (ObjectiveTime)objective;
		objectiveTime.currentTime = this.currentTime;
		objectiveTime.overrideType = this.overrideType;
		objectiveTime.overrideOffset = this.overrideOffset;
	}

	// Token: 0x060048C7 RID: 18631 RVA: 0x001C6DB4 File Offset: 0x001C4FB4
	public override BaseObjective Clone()
	{
		ObjectiveTime objectiveTime = new ObjectiveTime();
		this.CopyValues(objectiveTime);
		return objectiveTime;
	}

	// Token: 0x060048C8 RID: 18632 RVA: 0x001C6DD0 File Offset: 0x001C4FD0
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

	// Token: 0x060048C9 RID: 18633 RVA: 0x001C6E8F File Offset: 0x001C508F
	public override void HandleFailed()
	{
		this.currentTime = 0f;
		base.Complete = false;
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x001C6EA3 File Offset: 0x001C50A3
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTime.PropTime, ref this.Value);
		properties.ParseEnum<ObjectiveTime.OverrideTypes>(ObjectiveTime.PropOverrideType, ref this.overrideType);
		properties.ParseFloat(ObjectiveTime.PropOverrideOffset, ref this.overrideOffset);
	}

	// Token: 0x0400399F RID: 14751
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTime;

	// Token: 0x040039A0 RID: 14752
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveTime.OverrideTypes overrideType;

	// Token: 0x040039A1 RID: 14753
	[PublicizedFrom(EAccessModifier.Private)]
	public float overrideOffset;

	// Token: 0x040039A2 RID: 14754
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstRun = true;

	// Token: 0x040039A3 RID: 14755
	public static string PropTime = "time";

	// Token: 0x040039A4 RID: 14756
	public static string PropOverrideType = "override_type";

	// Token: 0x040039A5 RID: 14757
	public static string PropOverrideOffset = "override_offset";

	// Token: 0x040039A6 RID: 14758
	[PublicizedFrom(EAccessModifier.Private)]
	public int dayLengthInSeconds;

	// Token: 0x020009A2 RID: 2466
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OverrideTypes
	{
		// Token: 0x040039A8 RID: 14760
		None,
		// Token: 0x040039A9 RID: 14761
		VoteTime
	}
}
