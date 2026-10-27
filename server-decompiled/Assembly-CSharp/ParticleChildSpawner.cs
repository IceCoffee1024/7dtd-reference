using System;
using UnityEngine;

// Token: 0x02001387 RID: 4999
public class ParticleChildSpawner : MonoBehaviour
{
	// Token: 0x06009D26 RID: 40230 RVA: 0x003B60DC File Offset: 0x003B42DC
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

	// Token: 0x06009D27 RID: 40231 RVA: 0x003B6284 File Offset: 0x003B4484
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

	// Token: 0x040076A0 RID: 30368
	[Tooltip("One or more tags separated by commas.\nMatches if any 'Tag' is present")]
	public string tags;

	// Token: 0x040076A1 RID: 30369
	[Tooltip("Check to require all 'Tags' to be present to match")]
	public bool HasAllTags;

	// Token: 0x040076A2 RID: 30370
	[Tooltip("One or more tags separated by commas.\nFails to match if any 'Not Tag' is present")]
	public string notTags;

	// Token: 0x040076A3 RID: 30371
	public ParticleChildSpawner.Data[] particles;

	// Token: 0x02001388 RID: 5000
	[Serializable]
	public struct Data
	{
		// Token: 0x040076A4 RID: 30372
		public string particleName;

		// Token: 0x040076A5 RID: 30373
		public string boneName;

		// Token: 0x040076A6 RID: 30374
		public Vector2 mass;

		// Token: 0x040076A7 RID: 30375
		public GameObject spawnedObj;
	}
}
