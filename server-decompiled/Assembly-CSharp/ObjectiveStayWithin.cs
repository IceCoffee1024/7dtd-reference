using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009C1 RID: 2497
[Preserve]
public class ObjectiveStayWithin : BaseObjective
{
	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06004911 RID: 18705 RVA: 0x00047076 File Offset: 0x00045276
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Distance;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06004912 RID: 18706 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06004913 RID: 18707 RVA: 0x001CAC6C File Offset: 0x001C8E6C
	public override string StatusText
	{
		get
		{
			if (base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
			{
				if (this.currentDistance < this.maxDistance)
				{
					return ValueDisplayFormatters.Distance(this.currentDistance) + "/" + this.displayDistance;
				}
				base.ObjectiveState = BaseObjective.ObjectiveStates.Failed;
				return Localization.Get("failed", false, null);
			}
			else
			{
				if (base.OwnerQuest.CurrentState == Quest.QuestState.NotStarted)
				{
					return this.displayDistance;
				}
				if (base.ObjectiveState == BaseObjective.ObjectiveStates.Failed)
				{
					return Localization.Get("failed", false, null);
				}
				return Localization.Get("completed", false, null);
			}
		}
	}

	// Token: 0x06004914 RID: 18708 RVA: 0x001C6FF5 File Offset: 0x001C51F5
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x001C2CB7 File Offset: 0x001C0EB7
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x001C7021 File Offset: 0x001C5221
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x001CACFC File Offset: 0x001C8EFC
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		if (base.OwnerQuest == base.OwnerQuest.OwnerJournal.ActiveQuest || base.OwnerQuest.OwnerJournal.ActiveQuest == null)
		{
			QuestEventManager.Current.QuestBounds = default(Rect);
		}
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x001C7089 File Offset: 0x001C5289
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(PooledBinaryReader _br)
	{
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(PooledBinaryWriter _bw)
	{
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x001CAD4E File Offset: 0x001C8F4E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStayWithin objectiveStayWithin = (ObjectiveStayWithin)objective;
		objectiveStayWithin.maxDistance = this.maxDistance;
		objectiveStayWithin.displayDistance = this.displayDistance;
	}

	// Token: 0x0600491C RID: 18716 RVA: 0x001CAD74 File Offset: 0x001C8F74
	public override BaseObjective Clone()
	{
		ObjectiveStayWithin objectiveStayWithin = new ObjectiveStayWithin();
		this.CopyValues(objectiveStayWithin);
		return objectiveStayWithin;
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x001CAD90 File Offset: 0x001C8F90
	public override void Update(float updateTime)
	{
		Vector3 position = base.OwnerQuest.OwnerJournal.OwnerPlayer.position;
		Vector3 position2 = base.OwnerQuest.Position;
		if (!this.positionSetup)
		{
			if (base.OwnerQuest.GetPositionData(out position2, Quest.PositionDataTypes.Location))
			{
				base.OwnerQuest.Position = position2;
				QuestEventManager.Current.QuestBounds = new Rect(position2.x, position2.z, this.maxDistance, this.maxDistance);
				this.positionSetup = true;
			}
			else if (base.OwnerQuest.GetPositionData(out position2, Quest.PositionDataTypes.POIPosition))
			{
				base.OwnerQuest.Position = position2;
				QuestEventManager.Current.QuestBounds = new Rect(position2.x, position2.z, this.maxDistance, this.maxDistance);
				this.positionSetup = true;
			}
		}
		position.y = 0f;
		position2.y = 0f;
		this.currentDistance = (position - position2).magnitude;
		float num = this.currentDistance / this.maxDistance;
		if (num > 1f)
		{
			base.Complete = false;
			base.ObjectiveState = BaseObjective.ObjectiveStates.Failed;
			base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
			return;
		}
		if (num > 0.75f)
		{
			base.ObjectiveState = BaseObjective.ObjectiveStates.Warning;
			return;
		}
		base.ObjectiveState = BaseObjective.ObjectiveStates.Complete;
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x001CAED8 File Offset: 0x001C90D8
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveStayWithin.PropRadius))
		{
			this.maxDistance = StringParsers.ParseFloat(properties.Values[ObjectiveStayWithin.PropRadius], 0, -1, NumberStyles.Any);
			this.displayDistance = ValueDisplayFormatters.Distance(this.maxDistance);
		}
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x001CAF31 File Offset: 0x001C9131
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("max_distance", this.maxDistance);
	}

	// Token: 0x04003A48 RID: 14920
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 50f;

	// Token: 0x04003A49 RID: 14921
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x04003A4A RID: 14922
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayDistance = "0 km";

	// Token: 0x04003A4B RID: 14923
	public static string PropRadius = "radius";

	// Token: 0x04003A4C RID: 14924
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSetup;
}
