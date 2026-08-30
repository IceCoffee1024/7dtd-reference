using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007A4 RID: 1956
[Preserve]
public class NetPackageAnimateBlock : NetPackage
{
	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06003A2D RID: 14893 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x0017B4E4 File Offset: 0x001796E4
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, int _animationInteger = 0)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationInteger = _animationInteger;
		this.animType = 0;
		return this;
	}

	// Token: 0x06003A2F RID: 14895 RVA: 0x0017B503 File Offset: 0x00179703
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater, bool _animationBool = false)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animationBool = _animationBool;
		this.animType = 1;
		return this;
	}

	// Token: 0x06003A30 RID: 14896 RVA: 0x0017B522 File Offset: 0x00179722
	public NetPackageAnimateBlock Setup(Vector3i _blockPosition, string _animParamater)
	{
		this.blockPosition = _blockPosition;
		this.animParamater = _animParamater;
		this.animType = 2;
		return this;
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x0017B53A File Offset: 0x0017973A
	public override void read(PooledBinaryReader _reader)
	{
		this.blockPosition = StreamUtils.ReadVector3i(_reader);
		this.animParamater = _reader.ReadString();
		this.animType = _reader.ReadInt32();
		this.animationInteger = _reader.ReadInt32();
		this.animationBool = _reader.ReadBoolean();
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x0017B578 File Offset: 0x00179778
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		StreamUtils.Write(_writer, this.blockPosition);
		_writer.Write(this.animParamater);
		_writer.Write(this.animType);
		_writer.Write(this.animationInteger);
		_writer.Write(this.animationBool);
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x0017B5C8 File Offset: 0x001797C8
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

	// Token: 0x06003A34 RID: 14900 RVA: 0x0017B629 File Offset: 0x00179829
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

	// Token: 0x06003A35 RID: 14901 RVA: 0x0017B640 File Offset: 0x00179840
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

	// Token: 0x06003A36 RID: 14902 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04002FBC RID: 12220
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3i blockPosition;

	// Token: 0x04002FBD RID: 12221
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animParamater;

	// Token: 0x04002FBE RID: 12222
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animType;

	// Token: 0x04002FBF RID: 12223
	[PublicizedFrom(EAccessModifier.Protected)]
	public int animationInteger;

	// Token: 0x04002FC0 RID: 12224
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool animationBool;

	// Token: 0x04002FC1 RID: 12225
	[PublicizedFrom(EAccessModifier.Protected)]
	public string animationTrigger;
}
