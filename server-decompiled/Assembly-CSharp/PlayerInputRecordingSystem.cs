using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000915 RID: 2325
public class PlayerInputRecordingSystem
{
	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x0600428F RID: 17039 RVA: 0x001A2B1D File Offset: 0x001A0D1D
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

	// Token: 0x06004290 RID: 17040 RVA: 0x000027FC File Offset: 0x000009FC
	public void Record(MovementInput _movement, int _frameNr)
	{
	}

	// Token: 0x06004291 RID: 17041 RVA: 0x001A2B38 File Offset: 0x001A0D38
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

	// Token: 0x06004292 RID: 17042 RVA: 0x001A2B9B File Offset: 0x001A0D9B
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

	// Token: 0x06004293 RID: 17043 RVA: 0x001A2BCC File Offset: 0x001A0DCC
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

	// Token: 0x06004294 RID: 17044 RVA: 0x001A2D2E File Offset: 0x001A0F2E
	public void SetStartPosition(EntityPlayer _player)
	{
		if (this.recording.Count > 0)
		{
			_player.SetPosition(this.recording[0].pos, true);
			_player.SetRotation(this.recording[0].rot);
		}
	}

	// Token: 0x06004295 RID: 17045 RVA: 0x001A2D70 File Offset: 0x001A0F70
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

	// Token: 0x06004296 RID: 17046 RVA: 0x001A2E08 File Offset: 0x001A1008
	public void SetAutoSaveTo(string _filename)
	{
		this.autoSaveFilename = _filename;
	}

	// Token: 0x06004297 RID: 17047 RVA: 0x001A2E11 File Offset: 0x001A1011
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

	// Token: 0x06004298 RID: 17048 RVA: 0x001A2E31 File Offset: 0x001A1031
	public void Save(string _filename)
	{
		this.doSave(GameIO.GetSaveGameDir() + "/" + _filename);
	}

	// Token: 0x06004299 RID: 17049 RVA: 0x001A2E4C File Offset: 0x001A104C
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

	// Token: 0x04003626 RID: 13862
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 1;

	// Token: 0x04003627 RID: 13863
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerInputRecordingSystem mInstance;

	// Token: 0x04003628 RID: 13864
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerInputRecordingSystem.SPosRot> recording = new List<PlayerInputRecordingSystem.SPosRot>();

	// Token: 0x04003629 RID: 13865
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x0400362A RID: 13866
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTickTime;

	// Token: 0x0400362B RID: 13867
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong relativeStartTickTime;

	// Token: 0x0400362C RID: 13868
	[PublicizedFrom(EAccessModifier.Private)]
	public int startFrameCount;

	// Token: 0x0400362D RID: 13869
	[PublicizedFrom(EAccessModifier.Private)]
	public float startTime;

	// Token: 0x0400362E RID: 13870
	[PublicizedFrom(EAccessModifier.Private)]
	public string autoSaveFilename;

	// Token: 0x02000916 RID: 2326
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SPosRot
	{
		// Token: 0x0600429B RID: 17051 RVA: 0x001A2EF3 File Offset: 0x001A10F3
		public void Write(BinaryWriter _bw)
		{
			_bw.Write(this.ticks);
			StreamUtils.Write(_bw, this.pos);
			StreamUtils.Write(_bw, this.rot);
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x001A2F19 File Offset: 0x001A1119
		public void Read(BinaryReader _br)
		{
			this.ticks = _br.ReadInt32();
			this.pos = StreamUtils.ReadVector3(_br);
			this.rot = StreamUtils.ReadVector3(_br);
		}

		// Token: 0x0400362F RID: 13871
		public Vector3 pos;

		// Token: 0x04003630 RID: 13872
		public Vector3 rot;

		// Token: 0x04003631 RID: 13873
		public int ticks;
	}
}
