using System;
using UnityEngine;

// Token: 0x02001355 RID: 4949
public class ParticleChildSpawner : MonoBehaviour
{
	// Token: 0x06009C22 RID: 39970 RVA: 0x003AF15C File Offset: 0x003AD35C
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

	// Token: 0x06009C23 RID: 39971 RVA: 0x003AF304 File Offset: 0x003AD504
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

	// Token: 0x040075E4 RID: 30180
	[Tooltip("One or more tags separated by commas.\nMatches if any 'Tag' is present")]
	public string tags;

	// Token: 0x040075E5 RID: 30181
	[Tooltip("Check to require all 'Tags' to be present to match")]
	public bool HasAllTags;

	// Token: 0x040075E6 RID: 30182
	[Tooltip("One or more tags separated by commas.\nFails to match if any 'Not Tag' is present")]
	public string notTags;

	// Token: 0x040075E7 RID: 30183
	public ParticleChildSpawner.Data[] particles;

	// Token: 0x02001356 RID: 4950
	[Serializable]
	public struct Data
	{
		// Token: 0x040075E8 RID: 30184
		public string particleName;

		// Token: 0x040075E9 RID: 30185
		public string boneName;

		// Token: 0x040075EA RID: 30186
		public Vector2 mass;

		// Token: 0x040075EB RID: 30187
		public GameObject spawnedObj;
	}
}
