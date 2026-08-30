using System;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x020007CF RID: 1999
[Preserve]
public class NetPackageEntityAnimationData : NetPackageEntityTargeted, IMemoryPoolableObject
{
	// Token: 0x06003B19 RID: 15129 RVA: 0x0017CBF2 File Offset: 0x0017ADF2
	public NetPackageEntityAnimationData Setup(int _entityId, List<AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyTo<AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B1A RID: 15130 RVA: 0x0017CC08 File Offset: 0x0017AE08
	public NetPackageEntityAnimationData Setup(int _entityId, Dictionary<int, AnimParamData> _animationParameterData)
	{
		base.Setup(_entityId);
		_animationParameterData.CopyValuesTo<int, AnimParamData>(this.animationParameterData);
		return this;
	}

	// Token: 0x06003B1B RID: 15131 RVA: 0x0017CC20 File Offset: 0x0017AE20
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.animationParameterData.Count);
		for (int i = 0; i < this.animationParameterData.Count; i++)
		{
			this.animationParameterData[i].Write(_writer);
		}
	}

	// Token: 0x06003B1C RID: 15132 RVA: 0x0017CC70 File Offset: 0x0017AE70
	public override void read(PooledBinaryReader _reader)
	{
		base.read(_reader);
		int num = _reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			this.animationParameterData.Add(AnimParamData.CreateFromBinary(_reader));
		}
	}

	// Token: 0x06003B1D RID: 15133 RVA: 0x0017CCA8 File Offset: 0x0017AEA8
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

	// Token: 0x06003B1E RID: 15134 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x06003B1F RID: 15135 RVA: 0x0017CD64 File Offset: 0x0017AF64
	public void Reset()
	{
		this.entityId = 0;
		this.animationParameterData.Clear();
	}

	// Token: 0x06003B20 RID: 15136 RVA: 0x0017CD78 File Offset: 0x0017AF78
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x04003002 RID: 12290
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<AnimParamData> animationParameterData = new List<AnimParamData>();

	// Token: 0x04003003 RID: 12291
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly CustomSampler getSampler = CustomSampler.Create("NetPackageEntityAnimationData.read", false);
}
