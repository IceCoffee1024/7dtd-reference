using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x020007E7 RID: 2023
[Preserve]
public class NetPackageEntityAnimationData : NetPackageEntityTargeted, IMemoryPoolableObject
{
	// Token: 0x06003BB2 RID: 15282 RVA: 0x00180613 File Offset: 0x0017E813
	public NetPackageEntityAnimationData Setup(int _entityId, List<AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyTo<AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003BB3 RID: 15283 RVA: 0x00180629 File Offset: 0x0017E829
	public NetPackageEntityAnimationData Setup(int _entityId, Dictionary<int, AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyValuesTo<int, AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x00180640 File Offset: 0x0017E840
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.animationParameterData.Count);
		for (int i = 0; i < this.animationParameterData.Count; i++)
		{
			this.animationParameterData[i].Write(_writer);
		}
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x00180690 File Offset: 0x0017E890
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.animationParameterData.Add(AnimParamData.CreateFromBinary(_reader));
		}
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x001806C8 File Offset: 0x0017E8C8
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

	// Token: 0x06003BB7 RID: 15287 RVA: 0x00180784 File Offset: 0x0017E984
	public void Reset()
	{
		this.entityId = 0;
		this.animationParameterData.Clear();
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x00180798 File Offset: 0x0017E998
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0400309A RID: 12442
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<AnimParamData> animationParameterData = new List<AnimParamData>();

	// Token: 0x0400309B RID: 12443
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CustomSampler getSampler = CustomSampler.Create("NetPackageEntityAnimationData.read", false);
}
