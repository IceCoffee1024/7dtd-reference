using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public class PlayerInputRecordingSystem
{
	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x060042DA RID: 17114 RVA: 0x001A1971 File Offset: 0x0019FB71
	public static PlayerInputRecordingSystem Instance
	{
		get
		{
			if (PlayerInputRecordingSystem.mInstance == null)
			{
				PlayerInputRecordingSystem.mInstance = new PlayerInputRecordingSystem();
			}
			return PlayerInputRecordingSystem.mInstance;
		}
	}

	// Token: 0x060042DB RID: 17115 RVA: 0x000027FC File Offset: 0x000009FC
	public void Record(MovementInput _movement, int _frameNr)
	{
	}

	// Token: 0x060042DC RID: 17116 RVA: 0x001A198C File Offset: 0x0019FB8C
	public void Record(EntityPlayer _player, ulong _ticks)
	{
		if (this.startTickTime == 0UL)
		{
			this.startTickTime = GameTimer.Instance.ticks;
		}
		this.recording.Add(new PlayerInputRecordingSystem.SPosRot
		{
			pos = _player.position,
			rot = _player.rotation,
			ticks = (int)(_ticks - this.startTickTime)
		});
	}

	// Token: 0x060042DD RID: 17117 RVA: 0x001A19EF File Offset: 0x0019FBEF
	public void Reset(bool _bClearRecordings = false)
	{
		this.index = 0;
		if (_bClearRecordings)
		{
			this.recording.Clear();
			this.startTickTime = 0UL;
		}
		this.relativeStartTickTime = 0UL;
		this.autoSaveFilename = null;
	}

	// Token: 0x060042DE RID: 17118 RVA: 0x001A1A20 File Offset: 0x0019FC20
	public bool Play(EntityPlayer _player, bool _bPlayRelativeToNow = false)
	{
		if (this.relativeStartTickTime == 0UL)
		{
			if (_bPlayRelativeToNow)
			{
				this.relativeStartTickTime = GameTimer.Instance.ticks;
			}
			else
			{
				this.relativeStartTickTime = this.startTickTime;
			}
			this.startFrameCount = Time.frameCount;
			this.startTime = Time.time;
		}
		while (this.index < this.recording.Count)
		{
			int ticks = this.recording[this.index].ticks;
			if (GameTimer.Instance.ticks < this.relativeStartTickTime + (ulong)((long)ticks))
			{
				break;
			}
			_player.SetPosition(this.recording[this.index].pos, true);
			_player.SetRotation(this.recording[this.index].rot);
			this.index++;
		}
		if (this.index == this.recording.Count)
		{
			Log.Out("Playing ended. Frames=" + (Time.frameCount - this.startFrameCount).ToString() + " avg fps=" + ((float)(Time.frameCount - this.startFrameCount) / (Time.time - this.startTime)).ToString("0.0"));
			GUIWindowConsole.Open();
			this.index++;
		}
		return this.index < this.recording.Count;
	}

	// Token: 0x060042DF RID: 17119 RVA: 0x001A1B82 File Offset: 0x0019FD82
	public void SetStartPosition(EntityPlayer _player)
	{
		if (this.recording.Count > 0)
		{
			_player.SetPosition(this.recording[0].pos, true);
			_player.SetRotation(this.recording[0].rot);
		}
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x001A1BC4 File Offset: 0x0019FDC4
	public void Load(string _filename)
	{
		using (BinaryReader binaryReader = new BinaryReader(SdFile.OpenRead(GameIO.GetSaveGameDir() + "/" + _filename + ".rec")))
		{
			this.recording.Clear();
			binaryReader.ReadByte();
			this.startTickTime = binaryReader.ReadUInt64();
			int num = (int)binaryReader.ReadUInt32();
			for (int i = 0; i < num; i++)
			{
				PlayerInputRecordingSystem.SPosRot sposRot = default(PlayerInputRecordingSystem.SPosRot);
				sposRot.Read(binaryReader);
				this.recording.Add(sposRot);
			}
		}
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x001A1C5C File Offset: 0x0019FE5C
	public void SetAutoSaveTo(string _filename)
	{
		this.autoSaveFilename = _filename;
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x001A1C65 File Offset: 0x0019FE65
	public bool AutoSave()
	{
		if (this.autoSaveFilename != null)
		{
			this.doSave(this.autoSaveFilename);
			this.autoSaveFilename = null;
			return true;
		}
		return false;
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x001A1C85 File Offset: 0x0019FE85
	public void Save(string _filename)
	{
		this.doSave(GameIO.GetSaveGameDir() + "/" + _filename);
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x001A1CA0 File Offset: 0x0019FEA0
	[PublicizedFrom(EAccessModifier.Private)]
	public void doSave(string _filename)
	{
		using (BinaryWriter binaryWriter = new BinaryWriter(SdFile.Open(_filename + ".rec", FileMode.Create, FileAccess.Write, FileShare.Read)))
		{
			binaryWriter.Write(1);
			binaryWriter.Write(this.startTickTime);
			binaryWriter.Write((uint)this.recording.Count);
			for (int i = 0; i < this.recording.Count; i++)
			{
				this.recording[i].Write(binaryWriter);
			}
		}
	}

	// Token: 0x04003612 RID: 13842
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 1;

	// Token: 0x04003613 RID: 13843
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerInputRecordingSystem mInstance;

	// Token: 0x04003614 RID: 13844
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerInputRecordingSystem.SPosRot> recording = new List<PlayerInputRecordingSystem.SPosRot>();

	// Token: 0x04003615 RID: 13845
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04003616 RID: 13846
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTickTime;

	// Token: 0x04003617 RID: 13847
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong relativeStartTickTime;

	// Token: 0x04003618 RID: 13848
	[PublicizedFrom(EAccessModifier.Private)]
	public int startFrameCount;

	// Token: 0x04003619 RID: 13849
	[PublicizedFrom(EAccessModifier.Private)]
	public float startTime;

	// Token: 0x0400361A RID: 13850
	[PublicizedFrom(EAccessModifier.Private)]
	public string autoSaveFilename;

	// Token: 0x02000907 RID: 2311
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SPosRot
	{
		// Token: 0x060042E6 RID: 17126 RVA: 0x001A1D47 File Offset: 0x0019FF47
		public void Write(BinaryWriter _bw)
		{
			_bw.Write(this.ticks);
			StreamUtils.Write(_bw, this.pos);
			StreamUtils.Write(_bw, this.rot);
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x001A1D6D File Offset: 0x0019FF6D
		public void Read(BinaryReader _br)
		{
			this.ticks = _br.ReadInt32();
			this.pos = StreamUtils.ReadVector3(_br);
			this.rot = StreamUtils.ReadVector3(_br);
		}

		// Token: 0x0400361B RID: 13851
		public Vector3 pos;

		// Token: 0x0400361C RID: 13852
		public Vector3 rot;

		// Token: 0x0400361D RID: 13853
		public int ticks;
	}
}
