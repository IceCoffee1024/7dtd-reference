using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020009AF RID: 2479
[Preserve]
public class ObjectivePOIStayWithin : BaseObjective
{
	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06004864 RID: 18532 RVA: 0x00010E62 File Offset: 0x0000F062
	public override BaseObjective.ObjectiveValueTypes ObjectiveValueType
	{
		get
		{
			return BaseObjective.ObjectiveValueTypes.Boolean;
		}
	}

	// Token: 0x170007D1 RID: 2001
	// (get) Token: 0x06004865 RID: 18533 RVA: 0x001C27D4 File Offset: 0x001C09D4
	public override bool UpdateUI
	{
		get
		{
			return base.ObjectiveState != BaseObjective.ObjectiveStates.Failed;
		}
	}

	// Token: 0x170007D2 RID: 2002
	// (get) Token: 0x06004866 RID: 18534 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool useUpdateLoop
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return true;
		}
	}

	// Token: 0x170007D3 RID: 2003
	// (get) Token: 0x06004867 RID: 18535 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool ShowInQuestLog
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004868 RID: 18536 RVA: 0x001C6FF5 File Offset: 0x001C51F5
	public override void SetupObjective()
	{
		this.keyword = Localization.Get("ObjectiveStayWithin_keyword", false, null);
	}

	// Token: 0x06004869 RID: 18537 RVA: 0x001C7009 File Offset: 0x001C5209
	public override void SetupDisplay()
	{
		base.Description = string.Format("{0}", this.keyword);
	}

	// Token: 0x0600486A RID: 18538 RVA: 0x001C7021 File Offset: 0x001C5221
	public override void AddHooks()
	{
		QuestEventManager.Current.AddObjectiveToBeUpdated(this);
	}

	// Token: 0x0600486B RID: 18539 RVA: 0x001C7030 File Offset: 0x001C5230
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

	// Token: 0x0600486C RID: 18540 RVA: 0x001C7089 File Offset: 0x001C5289
	public override void Refresh()
	{
		this.SetupDisplay();
		if (base.ObjectiveState == BaseObjective.ObjectiveStates.NotStarted && base.OwnerQuest.CurrentState == Quest.QuestState.InProgress)
		{
			return;
		}
		base.Complete = base.OwnerQuest.CurrentState != Quest.QuestState.Failed;
	}

	// Token: 0x0600486D RID: 18541 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Read(PooledBinaryReader _br)
	{
	}

	// Token: 0x0600486E RID: 18542 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Write(PooledBinaryWriter _bw)
	{
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x001C70C0 File Offset: 0x001C52C0
	public override BaseObjective Clone()
	{
		ObjectivePOIStayWithin objectivePOIStayWithin = new ObjectivePOIStayWithin();
		this.CopyValues(objectivePOIStayWithin);
		return objectivePOIStayWithin;
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x001C70DB File Offset: 0x001C52DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void CopyValues(BaseObjective objective)
	{
		base.CopyValues(objective);
		ObjectivePOIStayWithin objectivePOIStayWithin = (ObjectivePOIStayWithin)objective;
		objectivePOIStayWithin.outerRect = this.outerRect;
		objectivePOIStayWithin.innerRect = this.innerRect;
		objectivePOIStayWithin.offset = this.offset;
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x001C710D File Offset: 0x001C530D
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

	// Token: 0x06004872 RID: 18546 RVA: 0x001C7144 File Offset: 0x001C5344
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

	// Token: 0x06004873 RID: 18547 RVA: 0x001C7324 File Offset: 0x001C5524
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateState_NeedSetup()
	{
		this.GetPosition() != Vector3.zero;
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x001C7338 File Offset: 0x001C5538
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

	// Token: 0x06004875 RID: 18549 RVA: 0x001C73CB File Offset: 0x001C55CB
	public override void ParseProperties(DynamicProperties properties)
	{
		base.ParseProperties(properties);
		if (properties.Values.ContainsKey(ObjectivePOIStayWithin.PropRadius))
		{
			this.offset = (float)StringParsers.ParseSInt32(properties.Values[ObjectivePOIStayWithin.PropRadius], 0, -1, NumberStyles.Integer);
		}
	}

	// Token: 0x06004876 RID: 18550 RVA: 0x001C7405 File Offset: 0x001C5605
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InternalToParametersDictionary(ref Dictionary<string, object> parameters)
	{
		parameters.Add("position", this.position.ToString());
		parameters.Add("radius", this.offset);
	}

	// Token: 0x040039E6 RID: 14822
	public static string PropRadius = "radius";

	// Token: 0x040039E7 RID: 14823
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040039E8 RID: 14824
	[PublicizedFrom(EAccessModifier.Private)]
	public bool positionSet;

	// Token: 0x040039E9 RID: 14825
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect outerRect;

	// Token: 0x040039EA RID: 14826
	[PublicizedFrom(EAccessModifier.Private)]
	public Rect innerRect;

	// Token: 0x040039EB RID: 14827
	[PublicizedFrom(EAccessModifier.Private)]
	public float offset;

	// Token: 0x040039EC RID: 14828
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject prefabBounds;

	// Token: 0x040039ED RID: 14829
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject goBounds;
}
