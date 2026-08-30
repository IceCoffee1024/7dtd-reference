using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200098E RID: 2446
[Preserve]
public class ObjectivePOIStayWithin : BaseObjective
{
	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x06004805 RID: 18437 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x06004806 RID: 18438 RVA: 0x001BE8E8 File Offset: 0x001BCAE8
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x1700079A RID: 1946
	// (get) Token: 0x06004807 RID: 18439 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x1700079B RID: 1947
	// (get) Token: 0x06004808 RID: 18440 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004809 RID: 18441 RVA: 0x001C2E6A File Offset: 0x001C106A
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x001C2E7E File Offset: 0x001C107E
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}", this.keyword);
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x001C2E96 File Offset: 0x001C1096
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x001C2EA4 File Offset: 0x001C10A4
	public override void RemoveHooks()
	{
		QuestEventManager.Current.RemoveObjectiveToBeUpdated(this);
		if (base.OwnerQuest == base.OwnerQuest.OwnerJournal.ActiveQuest)
		{
			QuestEventManager.Current.QuestBounds = default(Rect);
		}
		if (this.goBounds != null)
		{
			global::UnityEngine.Object.Destroy(this.goBounds);
		}
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x001C2EFD File Offset: 0x001C10FD
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(BinaryReader _br)
	{
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(BinaryWriter _bw)
	{
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x001C2F34 File Offset: 0x001C1134
	public override BaseObjective Clone()
	{
		ObjectivePOIStayWithin objectivePOIStayWithin = new ObjectivePOIStayWithin();
		this.CopyValues(objectivePOIStayWithin);
		return objectivePOIStayWithin;
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x001C2F4F File Offset: 0x001C114F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectivePOIStayWithin objectivePOIStayWithin = (ObjectivePOIStayWithin)objective;
		objectivePOIStayWithin.outerRect = this.outerRect;
		objectivePOIStayWithin.innerRect = this.innerRect;
		objectivePOIStayWithin.offset = this.offset;
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x001C2F81 File Offset: 0x001C1181
	[PublicizedFrom(EAccessModifier.Protected)]
	public GameObject CreateBoundsViewer()
	{
		if (this.prefabBounds == null)
		{
			this.prefabBounds = Resources.Load<GameObject>("Prefabs/prefabPOIBounds");
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.prefabBounds);
		gameObject.name = "QuestBounds";
		return gameObject;
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x001C2FB8 File Offset: 0x001C11B8
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 GetPosition()
	{
		Vector3 vector;
		if (base.OwnerQuest.GetPositionData(out this.position, Quest.PositionDataTypes.POIPosition) && base.OwnerQuest.GetPositionData(out vector, Quest.PositionDataTypes.POISize))
		{
			PrefabInstance prefabAtPosition = GameManager.Instance.GetDynamicPrefabDecorator().GetPrefabAtPosition(this.position, null, null);
			if (prefabAtPosition != null)
			{
				base.OwnerQuest.Position = this.position;
				this.positionSet = true;
				this.outerRect = new Rect(this.position.x - this.offset, this.position.z - this.offset, vector.x + this.offset * 2f, vector.z + this.offset * 2f);
				this.innerRect = new Rect(this.position.x, this.position.z, vector.x, vector.z);
				float rotationAngle = prefabAtPosition.RotationAngle;
				this.outerRect = GeometryUtils.RotateRectAboutY(this.outerRect, rotationAngle);
				this.innerRect = GeometryUtils.RotateRectAboutY(this.innerRect, rotationAngle);
				QuestEventManager.Current.QuestBounds = this.outerRect;
				if (this.goBounds == null)
				{
					this.goBounds = this.CreateBoundsViewer();
				}
				this.goBounds.GetComponent<POIBoundsHelper>().SetPosition(new Vector3(this.outerRect.center.x, base.OwnerQuest.OwnerJournal.OwnerPlayer.position.y, this.outerRect.center.y) - Origin.position, new Vector3(this.outerRect.width, 200f, this.outerRect.height));
				base.CurrentValue = 2;
				return this.position;
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x001C3198 File Offset: 0x001C1398
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition() != Vector3.zero;
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x001C31AC File Offset: 0x001C13AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_Update()
	{
		if (!this.positionSet)
		{
			this.GetPosition();
			return;
		}
		Vector3 vector = base.OwnerQuest.OwnerJournal.OwnerPlayer.position;
		Vector3 vector2 = base.OwnerQuest.Position;
		vector.y = vector.z;
		if (!this.outerRect.Contains(vector))
		{
			base.Complete = false;
			base.ObjectiveState = BaseObjective.ObjectiveStates.Failed;
			base.OwnerQuest.CloseQuest(Quest.QuestState.Failed, null);
			return;
		}
		if (this.innerRect.Contains(vector))
		{
			base.ObjectiveState = BaseObjective.ObjectiveStates.Complete;
			return;
		}
		base.ObjectiveState = BaseObjective.ObjectiveStates.Warning;
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x001C323F File Offset: 0x001C143F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIStayWithin.PropRadius))
		{
			this.offset = (float)StringParsers.ParseSInt32(properties.Values[ObjectivePOIStayWithin.PropRadius], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x04003938 RID: 14648
	public static string PropRadius = "radius";

	// Token: 0x04003939 RID: 14649
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x0400393A RID: 14650
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSet;

	// Token: 0x0400393B RID: 14651
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect outerRect;

	// Token: 0x0400393C RID: 14652
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect innerRect;

	// Token: 0x0400393D RID: 14653
	[PublicizedFrom(EAccessModifier.Private)]
	public float offset;

	// Token: 0x0400393E RID: 14654
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabBounds;

	// Token: 0x0400393F RID: 14655
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goBounds;
}
