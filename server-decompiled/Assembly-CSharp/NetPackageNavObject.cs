using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000811 RID: 2065
[Preserve]
public class NetPackageNavObject : NetPackage
{
	// Token: 0x06003CA9 RID: 15529 RVA: 0x00183859 File Offset: 0x00181A59
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

	// Token: 0x06003CAA RID: 15530 RVA: 0x00183889 File Offset: 0x00181A89
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

	// Token: 0x06003CAB RID: 15531 RVA: 0x001838C2 File Offset: 0x00181AC2
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

	// Token: 0x06003CAC RID: 15532 RVA: 0x001838FC File Offset: 0x00181AFC
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

	// Token: 0x06003CAD RID: 15533 RVA: 0x0018396C File Offset: 0x00181B6C
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

	// Token: 0x06003CAE RID: 15534 RVA: 0x001839EC File Offset: 0x00181BEC
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

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06003CAF RID: 15535 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003143 RID: 12611
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectClass;

	// Token: 0x04003144 RID: 12612
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003145 RID: 12613
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usingLocalizationId;

	// Token: 0x04003146 RID: 12614
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003147 RID: 12615
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;

	// Token: 0x04003148 RID: 12616
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003149 RID: 12617
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useOverrideColor;

	// Token: 0x0400314A RID: 12618
	[PublicizedFrom(EAccessModifier.Private)]
	public Color overrideColor;
}
