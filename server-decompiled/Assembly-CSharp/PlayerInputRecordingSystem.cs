using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class PlayerInputRecordingSystem
{
	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x0600426C RID: 17004 RVA: 0x001A1A75 File Offset: 0x0019FC75
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

	// Token: 0x0600426D RID: 17005 RVA: 0x000027FC File Offset: 0x000009FC
	public void Record(MovementInput _movement, int _frameNr)
	{
	}

	// Token: 0x0600426E RID: 17006 RVA: 0x001A1A90 File Offset: 0x0019FC90
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

	// Token: 0x0600426F RID: 17007 RVA: 0x001A1AF3 File Offset: 0x0019FCF3
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

	// Token: 0x06004270 RID: 17008 RVA: 0x001A1B24 File Offset: 0x0019FD24
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

	// Token: 0x06004271 RID: 17009 RVA: 0x001A1C86 File Offset: 0x0019FE86
	public void SetStartPosition(EntityPlayer _player)
	{
		if (this.recording.Count > 0)
		{
			_player.SetPosition(this.recording[0].pos, true);
			_player.SetRotation(this.recording[0].rot);
		}
	}

	// Token: 0x06004272 RID: 17010 RVA: 0x001A1CC8 File Offset: 0x0019FEC8
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

	// Token: 0x06004273 RID: 17011 RVA: 0x001A1D60 File Offset: 0x0019FF60
	public void SetAutoSaveTo(string _filename)
	{
		this.autoSaveFilename = _filename;
	}

	// Token: 0x06004274 RID: 17012 RVA: 0x001A1D69 File Offset: 0x0019FF69
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

	// Token: 0x06004275 RID: 17013 RVA: 0x001A1D89 File Offset: 0x0019FF89
	public void Save(string _filename)
	{
		this.doSave(GameIO.GetSaveGameDir() + "/" + _filename);
	}

	// Token: 0x06004276 RID: 17014 RVA: 0x001A1DA4 File Offset: 0x0019FFA4
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

	// Token: 0x04003606 RID: 13830
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 1;

	// Token: 0x04003607 RID: 13831
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerInputRecordingSystem mInstance;

	// Token: 0x04003608 RID: 13832
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerInputRecordingSystem.SPosRot> recording = new List<PlayerInputRecordingSystem.SPosRot>();

	// Token: 0x04003609 RID: 13833
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x0400360A RID: 13834
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTickTime;

	// Token: 0x0400360B RID: 13835
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong relativeStartTickTime;

	// Token: 0x0400360C RID: 13836
	[PublicizedFrom(EAccessModifier.Private)]
	public int startFrameCount;

	// Token: 0x0400360D RID: 13837
	[PublicizedFrom(EAccessModifier.Private)]
	public float startTime;

	// Token: 0x0400360E RID: 13838
	[PublicizedFrom(EAccessModifier.Private)]
	public string autoSaveFilename;

	// Token: 0x0200090C RID: 2316
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SPosRot
	{
		// Token: 0x06004278 RID: 17016 RVA: 0x001A1E4B File Offset: 0x001A004B
		public void Write(BinaryWriter _bw)
		{
			_bw.Write(this.ticks);
			StreamUtils.Write(_bw, this.pos);
			StreamUtils.Write(_bw, this.rot);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x001A1E71 File Offset: 0x001A0071
		public void Read(BinaryReader _br)
		{
			this.ticks = _br.ReadInt32();
			this.pos = StreamUtils.ReadVector3(_br);
			this.rot = StreamUtils.ReadVector3(_br);
		}

		// Token: 0x0400360F RID: 13839
		public Vector3 pos;

		// Token: 0x04003610 RID: 13840
		public Vector3 rot;

		// Token: 0x04003611 RID: 13841
		public int ticks;
	}
}
