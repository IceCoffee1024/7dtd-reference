using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200099F RID: 2463
[Preserve]
public class ObjectivePOIStayWithin : BaseObjective
{
	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x0600488B RID: 18571 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007AB RID: 1963
	// (get) Token: 0x0600488C RID: 18572 RVA: 0x001C10DC File Offset: 0x001BF2DC
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007AC RID: 1964
	// (get) Token: 0x0600488D RID: 18573 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x0600488E RID: 18574 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x001C565E File Offset: 0x001C385E
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x001C5672 File Offset: 0x001C3872
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}", this.keyword);
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x001C568A File Offset: 0x001C388A
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x001C5698 File Offset: 0x001C3898
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

	// Token: 0x06004893 RID: 18579 RVA: 0x001C56F1 File Offset: 0x001C38F1
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(BinaryReader _br)
	{
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(BinaryWriter _bw)
	{
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x001C5728 File Offset: 0x001C3928
	public override BaseObjective Clone()
	{
		ObjectivePOIStayWithin objectivePOIStayWithin = new ObjectivePOIStayWithin();
		this.CopyValues(objectivePOIStayWithin);
		return objectivePOIStayWithin;
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x001C5743 File Offset: 0x001C3943
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectivePOIStayWithin objectivePOIStayWithin = (ObjectivePOIStayWithin)objective;
		objectivePOIStayWithin.outerRect = this.outerRect;
		objectivePOIStayWithin.innerRect = this.innerRect;
		objectivePOIStayWithin.offset = this.offset;
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x001C5775 File Offset: 0x001C3975
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

	// Token: 0x06004899 RID: 18585 RVA: 0x001C57AC File Offset: 0x001C39AC
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

	// Token: 0x0600489A RID: 18586 RVA: 0x001C598C File Offset: 0x001C3B8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition() != Vector3.zero;
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x001C59A0 File Offset: 0x001C3BA0
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

	// Token: 0x0600489C RID: 18588 RVA: 0x001C5A33 File Offset: 0x001C3C33
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIStayWithin.PropRadius))
		{
			this.offset = (float)StringParsers.ParseSInt32(properties.Values[ObjectivePOIStayWithin.PropRadius], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x040039C9 RID: 14793
	public static string PropRadius = "radius";

	// Token: 0x040039CA RID: 14794
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040039CB RID: 14795
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSet;

	// Token: 0x040039CC RID: 14796
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect outerRect;

	// Token: 0x040039CD RID: 14797
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect innerRect;

	// Token: 0x040039CE RID: 14798
	[PublicizedFrom(EAccessModifier.Private)]
	public float offset;

	// Token: 0x040039CF RID: 14799
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabBounds;

	// Token: 0x040039D0 RID: 14800
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goBounds;
}
