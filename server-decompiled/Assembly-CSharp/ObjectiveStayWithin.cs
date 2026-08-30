using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009B1 RID: 2481
[Preserve]
public class ObjectiveStayWithin : BaseObjective
{
	// Token: 0x170007C3 RID: 1987
	// (get) Token: 0x06004931 RID: 18737 RVA: 0x00046EDA File Offset: 0x000450DA
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Distance;
		}
	}

	// Token: 0x170007C4 RID: 1988
	// (get) Token: 0x06004932 RID: 18738 RVA: 0x001C10DC File Offset: 0x001BF2DC
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007C5 RID: 1989
	// (get) Token: 0x06004933 RID: 18739 RVA: 0x001C9170 File Offset: 0x001C7370
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

	// Token: 0x06004934 RID: 18740 RVA: 0x001C565E File Offset: 0x001C385E
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x001C15A0 File Offset: 0x001BF7A0
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
	}

	// Token: 0x06004936 RID: 18742 RVA: 0x001C568A File Offset: 0x001C388A
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004937 RID: 18743 RVA: 0x001C9200 File Offset: 0x001C7400
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		if (base.OwnerQuest == base.OwnerQuest.OwnerJournal.ActiveQuest || base.OwnerQuest.OwnerJournal.ActiveQuest == null)
		{
			QuestEventManager.Current.QuestBounds = default(Rect);
		}
	}

	// Token: 0x06004938 RID: 18744 RVA: 0x001C56F1 File Offset: 0x001C38F1
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(BinaryReader _br)
	{
	}

	// Token: 0x0600493A RID: 18746 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(BinaryWriter _bw)
	{
	}

	// Token: 0x0600493B RID: 18747 RVA: 0x001C9252 File Offset: 0x001C7452
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStayWithin objectiveStayWithin = (ObjectiveStayWithin)objective;
		objectiveStayWithin.maxDistance = this.maxDistance;
		objectiveStayWithin.displayDistance = this.displayDistance;
	}

	// Token: 0x0600493C RID: 18748 RVA: 0x001C9278 File Offset: 0x001C7478
	public override BaseObjective Clone()
	{
		ObjectiveStayWithin objectiveStayWithin = new ObjectiveStayWithin();
		this.CopyValues(objectiveStayWithin);
		return objectiveStayWithin;
	}

	// Token: 0x0600493D RID: 18749 RVA: 0x001C9294 File Offset: 0x001C7494
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

	// Token: 0x0600493E RID: 18750 RVA: 0x001C93DC File Offset: 0x001C75DC
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveStayWithin.PropRadius))
		{
			this.maxDistance = StringParsers.ParseFloat(properties.Values[ObjectiveStayWithin.PropRadius], 0, -1, NumberStyles.Any);
			this.displayDistance = ValueDisplayFormatters.Distance(this.maxDistance);
		}
	}

	// Token: 0x04003A2B RID: 14891
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 50f;

	// Token: 0x04003A2C RID: 14892
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x04003A2D RID: 14893
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayDistance = "0 km";

	// Token: 0x04003A2E RID: 14894
	public static string PropRadius = "radius";

	// Token: 0x04003A2F RID: 14895
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSetup;
}
