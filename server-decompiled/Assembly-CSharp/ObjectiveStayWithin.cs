using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009B7 RID: 2487
[Preserve]
public class ObjectiveStayWithin : BaseObjective
{
	// Token: 0x170007E3 RID: 2019
	// (get) Token: 0x060048EE RID: 18670 RVA: 0x000470B6 File Offset: 0x000452B6
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Distance;
		}
	}

	// Token: 0x170007E4 RID: 2020
	// (get) Token: 0x060048EF RID: 18671 RVA: 0x001C1750 File Offset: 0x001BF950
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007E5 RID: 2021
	// (get) Token: 0x060048F0 RID: 18672 RVA: 0x001C9B98 File Offset: 0x001C7D98
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

	// Token: 0x060048F1 RID: 18673 RVA: 0x001C5F49 File Offset: 0x001C4149
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x001C1C33 File Offset: 0x001BFE33
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x001C5F75 File Offset: 0x001C4175
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x001C9C28 File Offset: 0x001C7E28
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		if (base.OwnerQuest == base.OwnerQuest.OwnerJournal.ActiveQuest || base.OwnerQuest.OwnerJournal.ActiveQuest == null)
		{
			QuestEventManager.Current.QuestBounds = default(Rect);
		}
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x001C5FDD File Offset: 0x001C41DD
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(PooledBinaryReader _br)
	{
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(PooledBinaryWriter _bw)
	{
	}

	// Token: 0x060048F8 RID: 18680 RVA: 0x001C9C7A File Offset: 0x001C7E7A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStayWithin objectiveStayWithin = (ObjectiveStayWithin)objective;
		objectiveStayWithin.maxDistance = this.maxDistance;
		objectiveStayWithin.displayDistance = this.displayDistance;
	}

	// Token: 0x060048F9 RID: 18681 RVA: 0x001C9CA0 File Offset: 0x001C7EA0
	public override BaseObjective Clone()
	{
		ObjectiveStayWithin objectiveStayWithin = new ObjectiveStayWithin();
		this.CopyValues(objectiveStayWithin);
		return objectiveStayWithin;
	}

	// Token: 0x060048FA RID: 18682 RVA: 0x001C9CBC File Offset: 0x001C7EBC
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

	// Token: 0x060048FB RID: 18683 RVA: 0x001C9E04 File Offset: 0x001C8004
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveStayWithin.PropRadius))
		{
			this.maxDistance = StringParsers.ParseFloat(properties.Values[ObjectiveStayWithin.PropRadius], 0, -1, NumberStyles.Any);
			this.displayDistance = ValueDisplayFormatters.Distance(this.maxDistance);
		}
	}

	// Token: 0x060048FC RID: 18684 RVA: 0x001C9E5D File Offset: 0x001C805D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("max_distance", this.maxDistance);
	}

	// Token: 0x04003A28 RID: 14888
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 50f;

	// Token: 0x04003A29 RID: 14889
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x04003A2A RID: 14890
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayDistance = "0 km";

	// Token: 0x04003A2B RID: 14891
	public static string PropRadius = "radius";

	// Token: 0x04003A2C RID: 14892
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSetup;
}
