using System;
using UnityEngine;

// Token: 0x0200133B RID: 4923
public class ParticleChildSpawner : MonoBehaviour
{
	// Token: 0x06009B27 RID: 39719 RVA: 0x003A9EB4 File Offset: 0x003A80B4
	[PublicizedFrom(EAccessModifier.Private)]
	public void Start()
	{
		EntityAlive componentInParent = base.GetComponentInParent<EntityAlive>();
		if (!componentInParent)
		{
			Log.Warning("ParticleChildSpawner !entity");
			return;
		}
		if (this.HasAllTags)
		{
			if (!componentInParent.HasAllTags(FastTags<TagGroup.Global>.Parse(this.tags)))
			{
				return;
			}
		}
		else if (!componentInParent.HasAnyTags(FastTags<TagGroup.Global>.Parse(this.tags)))
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.notTags) && componentInParent.HasAnyTags(FastTags<TagGroup.Global>.Parse(this.notTags)))
		{
			return;
		}
		for (int i = 0; i < this.particles.Length; i++)
		{
			float num = EntityClass.list[componentInParent.entityClass].MassKg * 2.2f;
			if (num >= this.particles[i].mass.x && num <= this.particles[i].mass.y)
			{
				Transform transform = componentInParent.emodel.GetModelTransform();
				transform = transform.FindInChildren(this.particles[i].boneName);
				if (transform)
				{
					GameObject asset = LoadManager.LoadAssetFromAddressables<GameObject>("ParticleEffects/" + this.particles[i].particleName + ".prefab", null, null, false, true, false).Asset;
					if (!asset)
					{
						Log.Warning("ParticleChildSpawner {0}, no asset {1}", new object[]
						{
							base.name,
							this.particles[i].particleName
						});
					}
					else
					{
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(asset);
						gameObject.transform.SetParent(transform, false);
						this.particles[i].spawnedObj = gameObject;
					}
				}
			}
		}
	}

	// Token: 0x06009B28 RID: 39720 RVA: 0x003AA05C File Offset: 0x003A825C
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDestroy()
	{
		for (int i = 0; i < this.particles.Length; i++)
		{
			GameObject spawnedObj = this.particles[i].spawnedObj;
			if (spawnedObj)
			{
				global::UnityEngine.Object.Destroy(spawnedObj);
			}
		}
	}

	// Token: 0x0400750C RID: 29964
	[Tooltip("One or more tags separated by commas.\nMatches if any 'Tag' is present")]
	public string tags;

	// Token: 0x0400750D RID: 29965
	[Tooltip("Check to require all 'Tags' to be present to match")]
	public bool HasAllTags;

	// Token: 0x0400750E RID: 29966
	[Tooltip("One or more tags separated by commas.\nFails to match if any 'Not Tag' is present")]
	public string notTags;

	// Token: 0x0400750F RID: 29967
	public ParticleChildSpawner.Data[] particles;

	// Token: 0x0200133C RID: 4924
	[Serializable]
	public struct Data
	{
		// Token: 0x04007510 RID: 29968
		public string particleName;

		// Token: 0x04007511 RID: 29969
		public string boneName;

		// Token: 0x04007512 RID: 29970
		public Vector2 mass;

		// Token: 0x04007513 RID: 29971
		public GameObject spawnedObj;
	}
}
