using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020008FA RID: 2298
public class PlayerInputRecordingSystem
{
	// Token: 0x17000700 RID: 1792
	// (get) Token: 0x06004275 RID: 17013 RVA: 0x0019F899 File Offset: 0x0019DA99
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

	// Token: 0x06004276 RID: 17014 RVA: 0x000027FC File Offset: 0x000009FC
	public void Record(MovementInput _movement, int _frameNr)
	{
	}

	// Token: 0x06004277 RID: 17015 RVA: 0x0019F8B4 File Offset: 0x0019DAB4
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

	// Token: 0x06004278 RID: 17016 RVA: 0x0019F917 File Offset: 0x0019DB17
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

	// Token: 0x06004279 RID: 17017 RVA: 0x0019F948 File Offset: 0x0019DB48
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

	// Token: 0x0600427A RID: 17018 RVA: 0x0019FAAA File Offset: 0x0019DCAA
	public void SetStartPosition(EntityPlayer _player)
	{
		if (this.recording.Count > 0)
		{
			_player.SetPosition(this.recording[0].pos, true);
			_player.SetRotation(this.recording[0].rot);
		}
	}

	// Token: 0x0600427B RID: 17019 RVA: 0x0019FAEC File Offset: 0x0019DCEC
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

	// Token: 0x0600427C RID: 17020 RVA: 0x0019FB84 File Offset: 0x0019DD84
	public void SetAutoSaveTo(string _filename)
	{
		this.autoSaveFilename = _filename;
	}

	// Token: 0x0600427D RID: 17021 RVA: 0x0019FB8D File Offset: 0x0019DD8D
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

	// Token: 0x0600427E RID: 17022 RVA: 0x0019FBAD File Offset: 0x0019DDAD
	public void Save(string _filename)
	{
		this.doSave(GameIO.GetSaveGameDir() + "/" + _filename);
	}

	// Token: 0x0600427F RID: 17023 RVA: 0x0019FBC8 File Offset: 0x0019DDC8
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

	// Token: 0x04003591 RID: 13713
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentSaveVersion = 1;

	// Token: 0x04003592 RID: 13714
	[PublicizedFrom(EAccessModifier.Private)]
	public static PlayerInputRecordingSystem mInstance;

	// Token: 0x04003593 RID: 13715
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PlayerInputRecordingSystem.SPosRot> recording = new List<PlayerInputRecordingSystem.SPosRot>();

	// Token: 0x04003594 RID: 13716
	[PublicizedFrom(EAccessModifier.Private)]
	public int index;

	// Token: 0x04003595 RID: 13717
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong startTickTime;

	// Token: 0x04003596 RID: 13718
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong relativeStartTickTime;

	// Token: 0x04003597 RID: 13719
	[PublicizedFrom(EAccessModifier.Private)]
	public int startFrameCount;

	// Token: 0x04003598 RID: 13720
	[PublicizedFrom(EAccessModifier.Private)]
	public float startTime;

	// Token: 0x04003599 RID: 13721
	[PublicizedFrom(EAccessModifier.Private)]
	public string autoSaveFilename;

	// Token: 0x020008FB RID: 2299
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SPosRot
	{
		// Token: 0x06004281 RID: 17025 RVA: 0x0019FC6F File Offset: 0x0019DE6F
		public void Write(BinaryWriter _bw)
		{
			_bw.Write(this.ticks);
			StreamUtils.Write(_bw, this.pos);
			StreamUtils.Write(_bw, this.rot);
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x0019FC95 File Offset: 0x0019DE95
		public void Read(BinaryReader _br)
		{
			this.ticks = _br.ReadInt32();
			this.pos = StreamUtils.ReadVector3(_br);
			this.rot = StreamUtils.ReadVector3(_br);
		}

		// Token: 0x0400359A RID: 13722
		public Vector3 pos;

		// Token: 0x0400359B RID: 13723
		public Vector3 rot;

		// Token: 0x0400359C RID: 13724
		public int ticks;
	}
}
