using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007AF RID: 1967
[Preserve]
public class NetPackageAnimateBlock : NetPackage
{
	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06003A86 RID: 14982 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A87 RID: 14983 RVA: 0x0017CC34 File Offset: 0x0017AE34
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, int _animationInteger = 0)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationInteger = _animationInteger;
		this.animType = 0;
		return this;
	}

	// Token: 0x06003A88 RID: 14984 RVA: 0x0017CC53 File Offset: 0x0017AE53
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, bool _animationBool = false)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationBool = _animationBool;
		this.animType = 1;
		return this;
	}

	// Token: 0x06003A89 RID: 14985 RVA: 0x0017CC72 File Offset: 0x0017AE72
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animType = 2;
		return this;
	}

	// Token: 0x06003A8A RID: 14986 RVA: 0x0017CC8A File Offset: 0x0017AE8A
	public override void read(PooledBinaryReader _reader)
	{
		this.blockPosition = StreamUtils.ReadVector3i(_reader);
		this.animParamater = _reader.ReadString();
		this.animType = _reader.ReadInt32();
		this.animationInteger = _reader.ReadInt32();
		this.animationBool = _reader.ReadBoolean();
	}

	// Token: 0x06003A8B RID: 14987 RVA: 0x0017CCC8 File Offset: 0x0017AEC8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.Write(_writer, this.blockPosition);
		_writer.Write(this.animParamater);
		_writer.Write(this.animType);
		_writer.Write(this.animationInteger);
		_writer.Write(this.animationBool);
	}

	// Token: 0x06003A8C RID: 14988 RVA: 0x0017CD18 File Offset: 0x0017AF18
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

	// Token: 0x06003A8D RID: 14989 RVA: 0x0017CD79 File Offset: 0x0017AF79
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

	// Token: 0x06003A8E RID: 14990 RVA: 0x0017CD90 File Offset: 0x0017AF90
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

	// Token: 0x04002FD2 RID: 12242
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i blockPosition;

	// Token: 0x04002FD3 RID: 12243
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animParamater;

	// Token: 0x04002FD4 RID: 12244
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animType;

	// Token: 0x04002FD5 RID: 12245
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animationInteger;

	// Token: 0x04002FD6 RID: 12246
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool animationBool;

	// Token: 0x04002FD7 RID: 12247
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animationTrigger;
}
