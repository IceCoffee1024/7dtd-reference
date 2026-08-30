using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x020007DA RID: 2010
[Preserve]
public class NetPackageEntityAnimationData : NetPackageEntityTargeted, IMemoryPoolableObject
{
	// Token: 0x06003B78 RID: 15224 RVA: 0x0017EFCA File Offset: 0x0017D1CA
	public NetPackageEntityAnimationData Setup(int _entityId, List<AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyTo<AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B79 RID: 15225 RVA: 0x0017EFE0 File Offset: 0x0017D1E0
	public NetPackageEntityAnimationData Setup(int _entityId, Dictionary<int, AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyValuesTo<int, AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B7A RID: 15226 RVA: 0x0017EFF8 File Offset: 0x0017D1F8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.animationParameterData.Count);
		for (int i = 0; i < this.animationParameterData.Count; i++)
		{
			this.animationParameterData[i].Write(_writer);
		}
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x0017F048 File Offset: 0x0017D248
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.animationParameterData.Add(AnimParamData.CreateFromBinary(_reader));
		}
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0017F080 File Offset: 0x0017D280
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		EntityAlive entityAlive = _world.GetEntity(this.entityId) as EntityAlive;
		if (entityAlive == null || !entityAlive.isEntityRemote)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageEntityAnimationData>().Setup(this.entityId, this.animationParameterData), false, -1, this.entityId, this.entityId, null, 192, false);
		}
		if (entityAlive.emodel == null)
		{
			return;
		}
		AvatarController avatarController = entityAlive.emodel.avatarController;
		if (avatarController == null)
		{
			return;
		}
		List<AnimParamData> list = new List<AnimParamData>();
		this.animationParameterData.CopyTo<AnimParamData>(list);
		avatarController.SetAnimParameters(list);
	}

	// Token: 0x06003B7D RID: 15229 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x06003B7E RID: 15230 RVA: 0x0017F13C File Offset: 0x0017D33C
	public void Reset()
	{
		this.entityId = 0;
		this.animationParameterData.Clear();
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x0017F150 File Offset: 0x0017D350
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x04003081 RID: 12417
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<AnimParamData> animationParameterData = new List<AnimParamData>();

	// Token: 0x04003082 RID: 12418
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CustomSampler getSampler = CustomSampler.Create("NetPackageEntityAnimationData.read", false);
}
