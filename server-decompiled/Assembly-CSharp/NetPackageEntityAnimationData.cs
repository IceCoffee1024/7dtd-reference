using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x020007DD RID: 2013
[Preserve]
public class NetPackageEntityAnimationData : NetPackageEntityTargeted, IMemoryPoolableObject
{
	// Token: 0x06003B8F RID: 15247 RVA: 0x0017F50F File Offset: 0x0017D70F
	public NetPackageEntityAnimationData Setup(int _entityId, List<AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyTo<AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0017F525 File Offset: 0x0017D725
	public NetPackageEntityAnimationData Setup(int _entityId, Dictionary<int, AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyValuesTo<int, AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0017F53C File Offset: 0x0017D73C
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.animationParameterData.Count);
		for (int i = 0; i < this.animationParameterData.Count; i++)
		{
			this.animationParameterData[i].Write(_writer);
		}
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0017F58C File Offset: 0x0017D78C
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.animationParameterData.Add(AnimParamData.CreateFromBinary(_reader));
		}
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0017F5C4 File Offset: 0x0017D7C4
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

	// Token: 0x06003B94 RID: 15252 RVA: 0x0017F680 File Offset: 0x0017D880
	public void Reset()
	{
		this.entityId = 0;
		this.animationParameterData.Clear();
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0017F694 File Offset: 0x0017D894
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0400307A RID: 12410
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<AnimParamData> animationParameterData = new List<AnimParamData>();

	// Token: 0x0400307B RID: 12411
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CustomSampler getSampler = CustomSampler.Create("NetPackageEntityAnimationData.read", false);
}
