using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A0 RID: 2464
[Preserve]
public class ObjectiveStayWithin : BaseObjective
{
	// Token: 0x170007B1 RID: 1969
	// (get) Token: 0x060048AB RID: 18603 RVA: 0x00046C7A File Offset: 0x00044E7A
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Distance;
		}
	}

	// Token: 0x170007B2 RID: 1970
	// (get) Token: 0x060048AC RID: 18604 RVA: 0x001BE8E8 File Offset: 0x001BCAE8
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007B3 RID: 1971
	// (get) Token: 0x060048AD RID: 18605 RVA: 0x001C697C File Offset: 0x001C4B7C
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

	// Token: 0x060048AE RID: 18606 RVA: 0x001C2E6A File Offset: 0x001C106A
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x060048AF RID: 18607 RVA: 0x001BEDAC File Offset: 0x001BCFAC
	public override void SetupDisplay()
	{
		base.Description = this.keyword;
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x001C2E96 File Offset: 0x001C1096
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x001C6A0C File Offset: 0x001C4C0C
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		if (base.OwnerQuest == base.OwnerQuest.OwnerJournal.ActiveQuest || base.OwnerQuest.OwnerJournal.ActiveQuest == null)
		{
			QuestEventManager.Current.QuestBounds = default(Rect);
		}
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x001C2EFD File Offset: 0x001C10FD
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(BinaryReader _br)
	{
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(BinaryWriter _bw)
	{
	}

	// Token: 0x060048B5 RID: 18613 RVA: 0x001C6A5E File Offset: 0x001C4C5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectiveStayWithin objectiveStayWithin = (ObjectiveStayWithin)objective;
		objectiveStayWithin.maxDistance = this.maxDistance;
		objectiveStayWithin.displayDistance = this.displayDistance;
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x001C6A84 File Offset: 0x001C4C84
	public override BaseObjective Clone()
	{
		ObjectiveStayWithin objectiveStayWithin = new ObjectiveStayWithin();
		this.CopyValues(objectiveStayWithin);
		return objectiveStayWithin;
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x001C6AA0 File Offset: 0x001C4CA0
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

	// Token: 0x060048B8 RID: 18616 RVA: 0x001C6BE8 File Offset: 0x001C4DE8
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectiveStayWithin.PropRadius))
		{
			this.maxDistance = StringParsers.ParseFloat(properties.Values[ObjectiveStayWithin.PropRadius], 0, -1, NumberStyles.Any);
			this.displayDistance = ValueDisplayFormatters.Distance(this.maxDistance);
		}
	}

	// Token: 0x0400399A RID: 14746
	[PublicizedFrom(EAccessModifier.Private)]
	public float maxDistance = 50f;

	// Token: 0x0400399B RID: 14747
	[PublicizedFrom(EAccessModifier.Private)]
	public float currentDistance;

	// Token: 0x0400399C RID: 14748
	[PublicizedFrom(EAccessModifier.Private)]
	public string displayDistance = "0 km";

	// Token: 0x0400399D RID: 14749
	public static string PropRadius = "radius";

	// Token: 0x0400399E RID: 14750
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSetup;
}
