using System;
using UnityEngine;

// Token: 0x02001375 RID: 4981
public class ParticleChildSpawner : MonoBehaviour
{
	// Token: 0x06009CCE RID: 40142 RVA: 0x003B3CD8 File Offset: 0x003B1ED8
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

	// Token: 0x06009CCF RID: 40143 RVA: 0x003B3E80 File Offset: 0x003B2080
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

	// Token: 0x0400765F RID: 30303
	[Tooltip("One or more tags separated by commas.\nMatches if any 'Tag' is present")]
	public string tags;

	// Token: 0x04007660 RID: 30304
	[Tooltip("Check to require all 'Tags' to be present to match")]
	public bool HasAllTags;

	// Token: 0x04007661 RID: 30305
	[Tooltip("One or more tags separated by commas.\nFails to match if any 'Not Tag' is present")]
	public string notTags;

	// Token: 0x04007662 RID: 30306
	public ParticleChildSpawner.Data[] particles;

	// Token: 0x02001376 RID: 4982
	[Serializable]
	public struct Data
	{
		// Token: 0x04007663 RID: 30307
		public string particleName;

		// Token: 0x04007664 RID: 30308
		public string boneName;

		// Token: 0x04007665 RID: 30309
		public Vector2 mass;

		// Token: 0x04007666 RID: 30310
		public GameObject spawnedObj;
	}
}
