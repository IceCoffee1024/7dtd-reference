using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200079A RID: 1946
[Preserve]
public class NetPackageAnimateBlock : NetPackage
{
	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060039D5 RID: 14805 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x060039D6 RID: 14806 RVA: 0x00179264 File Offset: 0x00177464
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, int _animationInteger = 0)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationInteger = _animationInteger;
		this.animType = 0;
		return this;
	}

	// Token: 0x060039D7 RID: 14807 RVA: 0x00179283 File Offset: 0x00177483
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, bool _animationBool = false)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationBool = _animationBool;
		this.animType = 1;
		return this;
	}

	// Token: 0x060039D8 RID: 14808 RVA: 0x001792A2 File Offset: 0x001774A2
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animType = 2;
		return this;
	}

	// Token: 0x060039D9 RID: 14809 RVA: 0x001792BA File Offset: 0x001774BA
	public override void read(PooledBinaryReader _reader)
	{
		this.blockPosition = StreamUtils.ReadVector3i(_reader);
		this.animParamater = _reader.ReadString();
		this.animType = _reader.ReadInt32();
		this.animationInteger = _reader.ReadInt32();
		this.animationBool = _reader.ReadBoolean();
	}

	// Token: 0x060039DA RID: 14810 RVA: 0x001792F8 File Offset: 0x001774F8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.Write(_writer, this.blockPosition);
		_writer.Write(this.animParamater);
		_writer.Write(this.animType);
		_writer.Write(this.animationInteger);
		_writer.Write(this.animationBool);
	}

	// Token: 0x060039DB RID: 14811 RVA: 0x00179348 File Offset: 0x00177548
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

	// Token: 0x060039DC RID: 14812 RVA: 0x001793A9 File Offset: 0x001775A9
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

	// Token: 0x060039DD RID: 14813 RVA: 0x001793C0 File Offset: 0x001775C0
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

	// Token: 0x060039DE RID: 14814 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002F43 RID: 12099
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i blockPosition;

	// Token: 0x04002F44 RID: 12100
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animParamater;

	// Token: 0x04002F45 RID: 12101
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animType;

	// Token: 0x04002F46 RID: 12102
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animationInteger;

	// Token: 0x04002F47 RID: 12103
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool animationBool;

	// Token: 0x04002F48 RID: 12104
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animationTrigger;
}
