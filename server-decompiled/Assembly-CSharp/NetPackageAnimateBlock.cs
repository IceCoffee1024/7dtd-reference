using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007A7 RID: 1959
[Preserve]
public class NetPackageAnimateBlock : NetPackage
{
	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x06003A6F RID: 14959 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A70 RID: 14960 RVA: 0x0017BC9C File Offset: 0x00179E9C
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, int _animationInteger = 0)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationInteger = _animationInteger;
		this.animType = 0;
		return this;
	}

	// Token: 0x06003A71 RID: 14961 RVA: 0x0017BCBB File Offset: 0x00179EBB
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, bool _animationBool = false)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationBool = _animationBool;
		this.animType = 1;
		return this;
	}

	// Token: 0x06003A72 RID: 14962 RVA: 0x0017BCDA File Offset: 0x00179EDA
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animType = 2;
		return this;
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0017BCF2 File Offset: 0x00179EF2
	public override void read(PooledBinaryReader _reader)
	{
		this.blockPosition = StreamUtils.ReadVector3i(_reader);
		this.animParamater = _reader.ReadString();
		this.animType = _reader.ReadInt32();
		this.animationInteger = _reader.ReadInt32();
		this.animationBool = _reader.ReadBoolean();
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0017BD30 File Offset: 0x00179F30
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.Write(_writer, this.blockPosition);
		_writer.Write(this.animParamater);
		_writer.Write(this.animType);
		_writer.Write(this.animationInteger);
		_writer.Write(this.animationBool);
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x0017BD80 File Offset: 0x00179F80
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if ((Chunk)_world.GetChunkFromWorldPos(this.blockPosition) != null)
		{
			BlockEntityData blockEntity = _world.ChunkCache.GetBlockEntity(this.blockPosition);
			if (blockEntity != null)
			{
				if (blockEntity.transform == null)
				{
					GameManager.Instance.StartCoroutine(this.WaitForBEDTransform(blockEntity));
					return;
				}
				this.AnimateBlock(blockEntity);
			}
		}
	}

	// Token: 0x06003A76 RID: 14966 RVA: 0x0017BDE1 File Offset: 0x00179FE1
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator WaitForBEDTransform(BlockEntityData bed)
	{
		int num;
		for (int frames = 0; frames < 10; frames = num + 1)
		{
			yield return 0;
			if (bed == null)
			{
				yield break;
			}
			if (bed.transform != null)
			{
				this.AnimateBlock(bed);
				yield break;
			}
			num = frames;
		}
		yield break;
	}

	// Token: 0x06003A77 RID: 14967 RVA: 0x0017BDF8 File Offset: 0x00179FF8
	[PublicizedFrom(EAccessModifier.Private)]
	public void AnimateBlock(BlockEntityData bed)
	{
		Animator[] componentsInChildren = bed.transform.GetComponentsInChildren<Animator>();
		if (componentsInChildren != null)
		{
			for (int i = componentsInChildren.Length - 1; i >= 0; i--)
			{
				Animator animator = componentsInChildren[i];
				animator.enabled = true;
				switch (this.animType)
				{
				case 0:
					animator.SetInteger(this.animParamater, this.animationInteger);
					break;
				case 1:
					animator.SetBool(this.animParamater, this.animationBool);
					break;
				case 2:
					animator.SetTrigger(this.animParamater);
					break;
				}
			}
		}
	}

	// Token: 0x04002FB6 RID: 12214
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i blockPosition;

	// Token: 0x04002FB7 RID: 12215
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animParamater;

	// Token: 0x04002FB8 RID: 12216
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animType;

	// Token: 0x04002FB9 RID: 12217
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animationInteger;

	// Token: 0x04002FBA RID: 12218
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool animationBool;

	// Token: 0x04002FBB RID: 12219
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animationTrigger;
}
