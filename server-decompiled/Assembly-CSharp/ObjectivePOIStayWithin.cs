using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009A5 RID: 2469
[Preserve]
public class ObjectivePOIStayWithin : BaseObjective
{
	// Token: 0x170007CA RID: 1994
	// (get) Token: 0x06004841 RID: 18497 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007CB RID: 1995
	// (get) Token: 0x06004842 RID: 18498 RVA: 0x001C1750 File Offset: 0x001BF950
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007CC RID: 1996
	// (get) Token: 0x06004843 RID: 18499 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06004844 RID: 18500 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004845 RID: 18501 RVA: 0x001C5F49 File Offset: 0x001C4149
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x06004846 RID: 18502 RVA: 0x001C5F5D File Offset: 0x001C415D
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}", this.keyword);
	}

	// Token: 0x06004847 RID: 18503 RVA: 0x001C5F75 File Offset: 0x001C4175
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x06004848 RID: 18504 RVA: 0x001C5F84 File Offset: 0x001C4184
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

	// Token: 0x06004849 RID: 18505 RVA: 0x001C5FDD File Offset: 0x001C41DD
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x0600484A RID: 18506 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(PooledBinaryReader _br)
	{
	}

	// Token: 0x0600484B RID: 18507 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(PooledBinaryWriter _bw)
	{
	}

	// Token: 0x0600484C RID: 18508 RVA: 0x001C6014 File Offset: 0x001C4214
	public override BaseObjective Clone()
	{
		ObjectivePOIStayWithin objectivePOIStayWithin = new ObjectivePOIStayWithin();
		this.CopyValues(objectivePOIStayWithin);
		return objectivePOIStayWithin;
	}

	// Token: 0x0600484D RID: 18509 RVA: 0x001C602F File Offset: 0x001C422F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectivePOIStayWithin objectivePOIStayWithin = (ObjectivePOIStayWithin)objective;
		objectivePOIStayWithin.outerRect = this.outerRect;
		objectivePOIStayWithin.innerRect = this.innerRect;
		objectivePOIStayWithin.offset = this.offset;
	}

	// Token: 0x0600484E RID: 18510 RVA: 0x001C6061 File Offset: 0x001C4261
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

	// Token: 0x0600484F RID: 18511 RVA: 0x001C6098 File Offset: 0x001C4298
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

	// Token: 0x06004850 RID: 18512 RVA: 0x001C6278 File Offset: 0x001C4478
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition() != Vector3.zero;
	}

	// Token: 0x06004851 RID: 18513 RVA: 0x001C628C File Offset: 0x001C448C
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

	// Token: 0x06004852 RID: 18514 RVA: 0x001C631F File Offset: 0x001C451F
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIStayWithin.PropRadius))
		{
			this.offset = (float)StringParsers.ParseSInt32(properties.Values[ObjectivePOIStayWithin.PropRadius], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06004853 RID: 18515 RVA: 0x001C6359 File Offset: 0x001C4559
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("position", this.position.ToString());
		parameters.Add("radius", this.offset);
	}

	// Token: 0x040039C6 RID: 14790
	public static string PropRadius = "radius";

	// Token: 0x040039C7 RID: 14791
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040039C8 RID: 14792
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSet;

	// Token: 0x040039C9 RID: 14793
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect outerRect;

	// Token: 0x040039CA RID: 14794
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect innerRect;

	// Token: 0x040039CB RID: 14795
	[PublicizedFrom(EAccessModifier.Private)]
	public float offset;

	// Token: 0x040039CC RID: 14796
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabBounds;

	// Token: 0x040039CD RID: 14797
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goBounds;
}
