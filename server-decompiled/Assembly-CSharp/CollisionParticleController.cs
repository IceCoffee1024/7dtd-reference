using System;
using UnityEngine;

// Token: 0x020008C8 RID: 2248
public class CollisionParticleController
{
	// Token: 0x0600408E RID: 16526 RVA: 0x00196091 File Offset: 0x00194291
	public void Init(int _entityId, string _colliderSurfaceCategory, string _collisionSurfaceCategory, int _layerMask)
	{
		this.entityId = _entityId;
		this.particleEffectName = string.Format("impact_{0}_on_{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.soundName = string.Format("{0}hit{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.layerMask = _layerMask;
		this.Reset();
	}

	// Token: 0x0600408F RID: 16527 RVA: 0x001960CC File Offset: 0x001942CC
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

	// Token: 0x06004090 RID: 16528 RVA: 0x00196181 File Offset: 0x00194381
	public void Reset()
	{
		this.hasHit = false;
	}

	// Token: 0x040034B9 RID: 13497
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasHit;

	// Token: 0x040034BA RID: 13498
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040034BB RID: 13499
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;

	// Token: 0x040034BC RID: 13500
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x040034BD RID: 13501
	[PublicizedFrom(EAccessModifier.Private)]
	public int layerMask;
}
