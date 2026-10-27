using System;
using UnityEngine;

// Token: 0x020008D2 RID: 2258
public class CollisionParticleController
{
	// Token: 0x060040B1 RID: 16561 RVA: 0x00197139 File Offset: 0x00195339
	public void Init(int _entityId, string _colliderSurfaceCategory, string _collisionSurfaceCategory, int _layerMask)
	{
		this.entityId = _entityId;
		this.particleEffectName = string.Format("impact_{0}_on_{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.soundName = string.Format("{0}hit{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.layerMask = _layerMask;
		this.Reset();
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x00197174 File Offset: 0x00195374
	public void CheckCollision(Vector3 worldPos, Vector3 direction, float distance, int originEntityId = -1)
	{
		if (this.hasHit)
		{
			return;
		}
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(worldPos - Origin.position, direction), out raycastHit, distance, this.layerMask))
		{
			Vector3 vector = raycastHit.point + Origin.position;
			float lightBrightness = GameManager.Instance.World.GetLightBrightness(World.worldToBlockPos(vector));
			GameManager.Instance.SpawnParticleEffectServer(new ParticleEffect(this.particleEffectName, vector, Quaternion.FromToRotation(Vector3.up, raycastHit.normal), lightBrightness, Color.white, this.soundName, null, 1f, ""), (originEntityId == -1) ? this.entityId : originEntityId, false, false);
			this.hasHit = true;
		}
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x00197229 File Offset: 0x00195429
	public void Reset()
	{
		this.hasHit = false;
	}

	// Token: 0x040034D9 RID: 13529
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasHit;

	// Token: 0x040034DA RID: 13530
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040034DB RID: 13531
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;

	// Token: 0x040034DC RID: 13532
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x040034DD RID: 13533
	[PublicizedFrom(EAccessModifier.Private)]
	public int layerMask;
}
