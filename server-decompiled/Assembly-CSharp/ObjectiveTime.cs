using System;
using System.Globalization;
using System.IO;
using Twitch;
using UnityEngine.Scripting;

// Token: 0x020009B2 RID: 2482
[Preserve]
public class ObjectiveTime : BaseObjective
{
	// Token: 0x170007C6 RID: 1990
	// (get) Token: 0x06004941 RID: 18753 RVA: 0x0002F168 File Offset: 0x0002D368
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Time;
		}
	}

	// Token: 0x170007C7 RID: 1991
	// (get) Token: 0x06004942 RID: 18754 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool UpdateUI
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170007C8 RID: 1992
	// (get) Token: 0x06004943 RID: 18755 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170007C9 RID: 1993
	// (get) Token: 0x06004944 RID: 18756 RVA: 0x001C9460 File Offset: 0x001C7660
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

	// Token: 0x06004945 RID: 18757 RVA: 0x001C94B5 File Offset: 0x001C76B5
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveTime_keyword", false, null);
		this.dayLengthInSeconds = GamePrefs.GetInt(EnumGamePrefs.DayNightLength) * 60;
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x001C94D9 File Offset: 0x001C76D9
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}:", this.keyword);
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x001C568A File Offset: 0x001C388A
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x001C7666 File Offset: 0x001C5866
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
	}

	// Token: 0x06004949 RID: 18761 RVA: 0x001C94F4 File Offset: 0x001C76F4
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

	// Token: 0x0600494A RID: 18762 RVA: 0x001C9551 File Offset: 0x001C7751
	public override void Read(BinaryReader _br)
	{
		this.currentTime = (float)_br.ReadUInt16();
		this.currentValue = 1;
	}

	// Token: 0x0600494B RID: 18763 RVA: 0x001C9567 File Offset: 0x001C7767
	public override void Write(BinaryWriter _bw)
	{
		_bw.Write((ushort)this.currentTime);
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x001C9576 File Offset: 0x001C7776
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveTime objectiveTime = (ObjectiveTime)objective;
		objectiveTime.currentTime = this.currentTime;
		objectiveTime.overrideType = this.overrideType;
		objectiveTime.overrideOffset = this.overrideOffset;
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x001C95A8 File Offset: 0x001C77A8
	public override BaseObjective Clone()
	{
		ObjectiveTime objectiveTime = new ObjectiveTime();
		this.CopyValues(objectiveTime);
		return objectiveTime;
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x001C95C4 File Offset: 0x001C77C4
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

	// Token: 0x0600494F RID: 18767 RVA: 0x001C9683 File Offset: 0x001C7883
	public override void HandleFailed()
	{
		this.currentTime = 0f;
		base.Complete = false;
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x001C9697 File Offset: 0x001C7897
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		properties.ParseString(ObjectiveTime.PropTime, ref this.Value);
		properties.ParseEnum<ObjectiveTime.OverrideTypes>(ObjectiveTime.PropOverrideType, ref this.overrideType);
		properties.ParseFloat(ObjectiveTime.PropOverrideOffset, ref this.overrideOffset);
	}

	// Token: 0x04003A30 RID: 14896
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentTime;

	// Token: 0x04003A31 RID: 14897
	[PublicizedFrom(EAccessModifier.Private)]
	public ObjectiveTime.OverrideTypes overrideType;

	// Token: 0x04003A32 RID: 14898
	[PublicizedFrom(EAccessModifier.Private)]
	public float overrideOffset;

	// Token: 0x04003A33 RID: 14899
	[PublicizedFrom(EAccessModifier.Private)]
	public bool firstRun = true;

	// Token: 0x04003A34 RID: 14900
	public static string PropTime = "time";

	// Token: 0x04003A35 RID: 14901
	public static string PropOverrideType = "override_type";

	// Token: 0x04003A36 RID: 14902
	public static string PropOverrideOffset = "override_offset";

	// Token: 0x04003A37 RID: 14903
	[PublicizedFrom(EAccessModifier.Private)]
	public int dayLengthInSeconds;

	// Token: 0x020009B3 RID: 2483
	[PublicizedFrom(EAccessModifier.Private)]
	public enum OverrideTypes
	{
		// Token: 0x04003A39 RID: 14905
		None,
		// Token: 0x04003A3A RID: 14906
		VoteTime
	}
}
