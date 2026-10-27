using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Audio
{
	// Token: 0x02001B80 RID: 7040
	[Preserve]
	public class NetPackageAudio : NetPackageEntityTargeted
	{
		// Token: 0x0600D2EE RID: 53998 RVA: 0x004C85AE File Offset: 0x004C67AE
		public NetPackageAudio Setup(int _playOnEntityId, string _soundGroupName, float _occlusion, bool _play, bool _signalOnly = false, float _volumeScale = 1f)
		{
			base.Setup(_playOnEntityId);
			this.playOnEntity = true;
			this.soundGroupName = _soundGroupName;
			this.play = _play;
			this.occlusion = _occlusion;
			this.signalOnly = _signalOnly;
			this.volumeScale = _volumeScale;
			return this;
		}

		// Token: 0x0600D2EF RID: 53999 RVA: 0x004C85E5 File Offset: 0x004C67E5
		public NetPackageAudio Setup(Vector3 _position, string _soundGroupName, float _occlusion, bool _play, int _entityId = -1, float _volumeScale = 1f)
		{
			base.Setup(_entityId);
			this.playOnEntity = false;
			this.position = _position;
			this.soundGroupName = _soundGroupName;
			this.play = _play;
			this.occlusion = _occlusion;
			this.volumeScale = _volumeScale;
			return this;
		}

		// Token: 0x0600D2F0 RID: 54000 RVA: 0x004C861C File Offset: 0x004C681C
		public override void read(PooledBinaryReader _reader)
		{
			base.read(_reader);
			this.soundGroupName = _reader.ReadString();
			this.play = _reader.ReadBoolean();
			float num = _reader.ReadSingle();
			float num2 = _reader.ReadSingle();
			float num3 = _reader.ReadSingle();
			this.position.x = num;
			this.position.y = num2;
			this.position.z = num3;
			this.playOnEntity = _reader.ReadBoolean();
			this.occlusion = _reader.ReadSingle();
			this.volumeScale = _reader.ReadSingle();
			this.signalOnly = _reader.ReadBoolean();
		}

		// Token: 0x0600D2F1 RID: 54001 RVA: 0x004C86B4 File Offset: 0x004C68B4
		public override void write(PooledBinaryWriter _writer)
		{
			base.write(_writer);
			_writer.Write((this.soundGroupName != null) ? this.soundGroupName : "");
			_writer.Write(this.play);
			_writer.Write(this.position.x);
			_writer.Write(this.position.y);
			_writer.Write(this.position.z);
			_writer.Write(this.playOnEntity);
			_writer.Write(this.occlusion);
			_writer.Write(this.volumeScale);
			_writer.Write(this.signalOnly);
		}

		// Token: 0x0600D2F2 RID: 54002 RVA: 0x004C8758 File Offset: 0x004C6958
		public override void ProcessPackage(World _world, GameManager _callbacks)
		{
			if (_world == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.soundGroupName))
			{
				return;
			}
			if (this.playOnEntity && this.entityId >= 0)
			{
				Entity entity = _world.GetEntity(this.entityId);
				if (entity == null)
				{
					return;
				}
				if (GameManager.IsDedicatedServer && Manager.ServerAudio != null)
				{
					if (this.play)
					{
						Manager.ServerAudio.Play(entity, this.soundGroupName, this.occlusion, this.signalOnly, this.volumeScale);
						return;
					}
					Manager.ServerAudio.Stop(this.entityId, this.soundGroupName);
					return;
				}
				else if (!GameManager.IsDedicatedServer && Manager.ServerAudio != null)
				{
					if (this.play)
					{
						Manager.Play(entity, this.soundGroupName, this.volumeScale, false);
						Manager.ServerAudio.Play(entity, this.soundGroupName, this.occlusion, this.signalOnly, this.volumeScale);
						return;
					}
					Manager.Stop(this.entityId, this.soundGroupName);
					Manager.ServerAudio.Stop(this.entityId, this.soundGroupName);
					return;
				}
				else if (Manager.ServerAudio == null)
				{
					if (!this.play)
					{
						Manager.Stop(this.entityId, this.soundGroupName);
						return;
					}
					if (!this.signalOnly)
					{
						Manager.Play(entity, this.soundGroupName, this.volumeScale, false);
						return;
					}
				}
			}
			else if (GameManager.IsDedicatedServer && Manager.ServerAudio != null)
			{
				if (this.play)
				{
					Manager.ServerAudio.Play(this.position, this.soundGroupName, this.occlusion, this.entityId, this.volumeScale);
					return;
				}
				Manager.ServerAudio.Stop(this.position, this.soundGroupName);
				return;
			}
			else if (!GameManager.IsDedicatedServer && Manager.ServerAudio != null)
			{
				if (this.play)
				{
					Manager.Play(this.position, this.soundGroupName, this.entityId, false, this.volumeScale);
					Manager.ServerAudio.Play(this.position, this.soundGroupName, this.occlusion, this.entityId, this.volumeScale);
					return;
				}
				Manager.Stop(this.position, this.soundGroupName);
				Manager.ServerAudio.Stop(this.position, this.soundGroupName);
				return;
			}
			else if (Manager.ServerAudio == null)
			{
				if (this.play)
				{
					Manager.Play(this.position, this.soundGroupName, this.entityId, false, this.volumeScale);
					return;
				}
				Manager.Stop(this.position, this.soundGroupName);
			}
		}

		// Token: 0x0400A08F RID: 41103
		public string soundGroupName;

		// Token: 0x0400A090 RID: 41104
		public bool play;

		// Token: 0x0400A091 RID: 41105
		public Vector3 position;

		// Token: 0x0400A092 RID: 41106
		public bool playOnEntity;

		// Token: 0x0400A093 RID: 41107
		public float occlusion;

		// Token: 0x0400A094 RID: 41108
		public bool signalOnly;

		// Token: 0x0400A095 RID: 41109
		public float volumeScale;
	}
}
