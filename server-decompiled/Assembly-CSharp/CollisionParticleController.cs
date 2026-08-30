using System;
using UnityEngine;

// Token: 0x020008B7 RID: 2231
public class CollisionParticleController
{
	// Token: 0x06004093 RID: 16531 RVA: 0x00193EB5 File Offset: 0x001920B5
	public void Init(int _entityId, string _colliderSurfaceCategory, string _collisionSurfaceCategory, int _layerMask)
	{
		this.entityId = _entityId;
		this.particleEffectName = string.Format("impact_{0}_on_{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.soundName = string.Format("{0}hit{1}", _colliderSurfaceCategory, _collisionSurfaceCategory);
		this.layerMask = _layerMask;
		this.Reset();
	}

	// Token: 0x06004094 RID: 16532 RVA: 0x00193EF0 File Offset: 0x001920F0
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

	// Token: 0x06004095 RID: 16533 RVA: 0x00193FA5 File Offset: 0x001921A5
	public void Reset()
	{
		this.hasHit = false;
	}

	// Token: 0x04003444 RID: 13380
	[PublicizedFrom(EAccessModifier.Private)]
	public bool hasHit;

	// Token: 0x04003445 RID: 13381
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003446 RID: 13382
	[PublicizedFrom(EAccessModifier.Private)]
	public string particleEffectName;

	// Token: 0x04003447 RID: 13383
	[PublicizedFrom(EAccessModifier.Private)]
	public string soundName;

	// Token: 0x04003448 RID: 13384
	[PublicizedFrom(EAccessModifier.Private)]
	public int layerMask;
}
