using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000808 RID: 2056
[Preserve]
public class NetPackageNavObject : NetPackage
{
	// Token: 0x06003C80 RID: 15488 RVA: 0x001818A5 File Offset: 0x0017FAA5
	public NetPackageNavObject Setup(string _navObjectClass, string _displayName, Vector3 _position, bool _isAdd, bool _usingLocalizationId, int _entityId = -1)
	{
		this.navObjectClass = _navObjectClass;
		this.name = _displayName;
		this.position = _position;
		this.isAdd = _isAdd;
		this.usingLocalizationId = _usingLocalizationId;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x001818D5 File Offset: 0x0017FAD5
	public NetPackageNavObject Setup(int _entityId)
	{
		this.navObjectClass = "";
		this.name = "";
		this.position = Vector3.zero;
		this.isAdd = false;
		this.usingLocalizationId = false;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x0018190E File Offset: 0x0017FB0E
	public NetPackageNavObject Setup(string _navObjectClass, string _displayName, Vector3 _position, bool _isAdd, Color _overrideColor, bool _usingLocalizationId)
	{
		this.navObjectClass = _navObjectClass;
		this.name = _displayName;
		this.position = _position;
		this.isAdd = _isAdd;
		this.usingLocalizationId = _usingLocalizationId;
		this.useOverrideColor = true;
		this.overrideColor = _overrideColor;
		return this;
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x00181948 File Offset: 0x0017FB48
	public override void read(PooledBinaryReader _br)
	{
		this.navObjectClass = _br.ReadString();
		this.name = _br.ReadString();
		this.position = StreamUtils.ReadVector3(_br);
		this.isAdd = _br.ReadBoolean();
		this.useOverrideColor = _br.ReadBoolean();
		this.overrideColor = StreamUtils.ReadColor32(_br);
		this.usingLocalizationId = _br.ReadBoolean();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x001819B8 File Offset: 0x0017FBB8
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.navObjectClass);
		_bw.Write(this.name ?? "");
		StreamUtils.Write(_bw, this.position);
		_bw.Write(this.isAdd);
		_bw.Write(this.useOverrideColor);
		StreamUtils.WriteColor32(_bw, this.overrideColor);
		_bw.Write(this.usingLocalizationId);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x00181A38 File Offset: 0x0017FC38
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (_world.IsRemote())
		{
			if (this.isAdd)
			{
				NavObject navObject = ((this.entityId == -1) ? null : NavObjectManager.Instance.GetNavObjectByEntityID(this.entityId));
				if (navObject != null && navObject.TrackType == NavObject.TrackTypes.Entity)
				{
					return;
				}
				NavObject navObject2 = NavObjectManager.Instance.RegisterNavObject(this.navObjectClass, this.position, "", false, -1, null);
				navObject2.name = this.name;
				navObject2.usingLocalizationId = this.usingLocalizationId;
				navObject2.EntityID = this.entityId;
				if (this.useOverrideColor)
				{
					navObject2.UseOverrideColor = true;
					navObject2.OverrideColor = this.overrideColor;
					return;
				}
			}
			else
			{
				if (this.entityId != -1)
				{
					NavObjectManager.Instance.UnRegisterNavObjectByEntityID(this.entityId);
					return;
				}
				NavObjectManager.Instance.UnRegisterNavObjectByPosition(this.position, this.navObjectClass);
			}
		}
	}

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x06003C86 RID: 15494 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x00104CB5 File Offset: 0x00102EB5
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x040030E0 RID: 12512
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectClass;

	// Token: 0x040030E1 RID: 12513
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x040030E2 RID: 12514
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usingLocalizationId;

	// Token: 0x040030E3 RID: 12515
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x040030E4 RID: 12516
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;

	// Token: 0x040030E5 RID: 12517
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x040030E6 RID: 12518
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useOverrideColor;

	// Token: 0x040030E7 RID: 12519
	[PublicizedFrom(EAccessModifier.Private)]
	public Color overrideColor;
}
