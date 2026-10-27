using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001390 RID: 5008
public class BoundaryProjector : MonoBehaviour
{
	// Token: 0x06009D56 RID: 40278 RVA: 0x003B6FEC File Offset: 0x003B51EC
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

	// Token: 0x06009D57 RID: 40279 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetupProjectors()
	{
	}

	// Token: 0x06009D58 RID: 40280 RVA: 0x003B7040 File Offset: 0x003B5240
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

	// Token: 0x06009D59 RID: 40281 RVA: 0x003B7230 File Offset: 0x003B5430
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

	// Token: 0x06009D5A RID: 40282 RVA: 0x003B72C8 File Offset: 0x003B54C8
	public void SetAlpha(int projectorID, float alpha)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			Color color = this.ProjectorList[projectorID].Projector.material.color;
			this.ProjectorList[projectorID].Projector.material.color = new Color(color.r, color.g, color.b, alpha);
		}
	}

	// Token: 0x06009D5B RID: 40283 RVA: 0x003B7340 File Offset: 0x003B5540
	public void SetGlow(int projectorID, bool isGlowing)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			Color color = this.ProjectorList[projectorID].Projector.material.color;
			this.ProjectorList[projectorID].EffectData.IsGlowing = isGlowing;
		}
	}

	// Token: 0x06009D5C RID: 40284 RVA: 0x003B739C File Offset: 0x003B559C
	public void SetAutoRotate(int projectorID, bool autoRotate, float rotateSpeed)
	{
		if (projectorID < this.ProjectorList.Count && this.ProjectorList[projectorID] != null)
		{
			this.ProjectorList[projectorID].EffectData.AutoRotate = autoRotate;
			this.ProjectorList[projectorID].EffectData.RotationSpeed = rotateSpeed;
		}
	}

	// Token: 0x06009D5D RID: 40285 RVA: 0x003B73F3 File Offset: 0x003B55F3
	public void SetMoveToPosition(Vector3 vNew)
	{
		if (base.transform.position.y == -999f)
		{
			base.transform.position = vNew;
			return;
		}
		this.targetPos = vNew;
	}

	// Token: 0x040076CA RID: 30410
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<BoundaryProjector.ProjectorEntry> ProjectorList = new List<BoundaryProjector.ProjectorEntry>();

	// Token: 0x040076CB RID: 30411
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static Vector3 invalidPos = new Vector3(-999f, -999f, -999f);

	// Token: 0x040076CC RID: 30412
	public Vector3 targetPos = BoundaryProjector.invalidPos;

	// Token: 0x040076CD RID: 30413
	public bool IsInitialized;

	// Token: 0x02001391 RID: 5009
	public class ProjectorEntry
	{
		// Token: 0x040076CE RID: 30414
		public BoundaryProjector.ProjectorEffectData EffectData;

		// Token: 0x040076CF RID: 30415
		public Projector Projector;
	}

	// Token: 0x02001392 RID: 5010
	public class ProjectorEffectData
	{
		// Token: 0x040076D0 RID: 30416
		public bool AutoRotate;

		// Token: 0x040076D1 RID: 30417
		public float RotationSpeed;

		// Token: 0x040076D2 RID: 30418
		public bool IsGlowing;

		// Token: 0x040076D3 RID: 30419
		public float targetRadius = -1f;
	}
}
