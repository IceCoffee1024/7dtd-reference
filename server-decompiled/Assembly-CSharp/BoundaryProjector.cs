using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200137E RID: 4990
public class BoundaryProjector : MonoBehaviour
{
	// Token: 0x06009CFE RID: 40190 RVA: 0x003B4BE8 File Offset: 0x003B2DE8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Awake()
	{
		Projector[] componentsInChildren = base.transform.GetComponentsInChildren<Projector>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.ProjectorList.Add(new BoundaryProjector.ProjectorEntry
			{
				Projector = componentsInChildren[i],
				EffectData = new BoundaryProjector.ProjectorEffectData()
			});
		}
		this.SetupProjectors();
	}

	// Token: 0x06009CFF RID: 40191 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetupProjectors()
	{
	}

	// Token: 0x06009D00 RID: 40192 RVA: 0x003B4C3C File Offset: 0x003B2E3C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		for (int i = 0; i < this.ProjectorList.Count; i++)
		{
			if (this.ProjectorList[i] != null && this.ProjectorList[i].Projector.gameObject.activeSelf)
			{
				BoundaryProjector.ProjectorEntry projectorEntry = this.ProjectorList[i];
				if (projectorEntry.EffectData.AutoRotate)
				{
					Vector3 eulerAngles = projectorEntry.Projector.transform.localRotation.eulerAngles;
					projectorEntry.Projector.transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y + Time.deltaTime * projectorEntry.EffectData.RotationSpeed, eulerAngles.z);
				}
				if (projectorEntry.EffectData.targetRadius != -1f)
				{
					projectorEntry.Projector.orthographicSize = Mathf.Lerp(projectorEntry.Projector.orthographicSize, projectorEntry.EffectData.targetRadius, Time.deltaTime);
					if (projectorEntry.Projector.orthographicSize == projectorEntry.EffectData.targetRadius)
					{
						projectorEntry.EffectData.targetRadius = -1f;
					}
				}
				if (projectorEntry.EffectData.IsGlowing)
				{
					Color color = projectorEntry.Projector.material.color;
					float num = Mathf.PingPong(Time.time, 0.25f);
					projectorEntry.Projector.material.color = new Color(color.r, color.g, color.b, 0.5f + num * 2f);
				}
			}
		}
		if (this.targetPos != BoundaryProjector.invalidPos)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.targetPos, Time.deltaTime);
			if (base.transform.position == this.targetPos)
			{
				this.targetPos = BoundaryProjector.invalidPos;
			}
		}
	}

	// Token: 0x06009D01 RID: 40193 RVA: 0x003B4E2C File Offset: 0x003B302C
	public void SetRadius(int projectorID, float size)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			if (this.ProjectorList[projectorID].Projector.orthographicSize == -1f || size == 0f)
			{
				this.ProjectorList[projectorID].Projector.orthographicSize = size;
				this.ProjectorList[projectorID].EffectData.targetRadius = -1f;
				return;
			}
			this.ProjectorList[projectorID].EffectData.targetRadius = size;
		}
	}

	// Token: 0x06009D02 RID: 40194 RVA: 0x003B4EC4 File Offset: 0x003B30C4
	public void SetAlpha(int projectorID, float alpha)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			Color color = this.ProjectorList[projectorID].Projector.material.color;
			this.ProjectorList[projectorID].Projector.material.color = new Color(color.r, color.g, color.b, alpha);
		}
	}

	// Token: 0x06009D03 RID: 40195 RVA: 0x003B4F3C File Offset: 0x003B313C
	public void SetGlow(int projectorID, bool isGlowing)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			Color color = this.ProjectorList[projectorID].Projector.material.color;
			this.ProjectorList[projectorID].EffectData.IsGlowing = isGlowing;
		}
	}

	// Token: 0x06009D04 RID: 40196 RVA: 0x003B4F98 File Offset: 0x003B3198
	public void SetAutoRotate(int projectorID, bool autoRotate, float rotateSpeed)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			this.ProjectorList[projectorID].EffectData.AutoRotate = autoRotate;
			this.ProjectorList[projectorID].EffectData.RotationSpeed = rotateSpeed;
		}
	}

	// Token: 0x06009D05 RID: 40197 RVA: 0x003B4FEF File Offset: 0x003B31EF
	public void SetMoveToPosition(Vector3 vNew)
	{
		if (base.transform.position.y == -999f)
		{
			base.transform.position = vNew;
			return;
		}
		this.targetPos = vNew;
	}

	// Token: 0x04007689 RID: 30345
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<BoundaryProjector.ProjectorEntry> ProjectorList = new List<BoundaryProjector.ProjectorEntry>();

	// Token: 0x0400768A RID: 30346
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Vector3 invalidPos = new Vector3(-999f, -999f, -999f);

	// Token: 0x0400768B RID: 30347
	public Vector3 targetPos = BoundaryProjector.invalidPos;

	// Token: 0x0400768C RID: 30348
	public bool IsInitialized;

	// Token: 0x0200137F RID: 4991
	public class ProjectorEntry
	{
		// Token: 0x0400768D RID: 30349
		public BoundaryProjector.ProjectorEffectData EffectData;

		// Token: 0x0400768E RID: 30350
		public Projector Projector;
	}

	// Token: 0x02001380 RID: 4992
	public class ProjectorEffectData
	{
		// Token: 0x0400768F RID: 30351
		public bool AutoRotate;

		// Token: 0x04007690 RID: 30352
		public float RotationSpeed;

		// Token: 0x04007691 RID: 30353
		public bool IsGlowing;

		// Token: 0x04007692 RID: 30354
		public float targetRadius = -1f;
	}
}
