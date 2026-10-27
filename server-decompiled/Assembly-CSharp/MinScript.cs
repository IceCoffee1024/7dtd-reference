using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

// Token: 0x0200072E RID: 1838
public class MinScript
{
	// Token: 0x06003798 RID: 14232 RVA: 0x0016E823 File Offset: 0x0016CA23
	public static string ConvertFromUIText(string _text)
	{
		return _text.Replace("\n", "^");
	}

	// Token: 0x06003799 RID: 14233 RVA: 0x0016E835 File Offset: 0x0016CA35
	public static string ConvertToUIText(string _text)
	{
		if (_text == null)
		{
			return string.Empty;
		}
		return _text.Replace("^", "\n");
	}

	// Token: 0x0600379A RID: 14234 RVA: 0x0016E850 File Offset: 0x0016CA50
	public void SetText(string _text)
	{
		int num = 0;
		int length = _text.Length;
		int num2;
		for (int i = 0; i < length; i = num2 + 1)
		{
			num2 = _text.IndexOf('^', i, length - i);
			if (num2 < 0)
			{
				num2 = length;
			}
			while (i < length && _text[i] == ' ')
			{
				i++;
			}
			int num3 = num2 - i;
			if (num3 > 0 && _text[i] != '/')
			{
				int num4 = _text.IndexOf(' ', i, num3);
				if (num4 < 0)
				{
					num4 = num2;
				}
				string text = _text.Substring(i, num4 - i);
				MinScript.CmdLine cmdLine;
				if (MinScript.nameToCmds.TryGetValue(text, out cmdLine.command))
				{
					num4++;
					cmdLine.parameters = null;
					int num5 = num2 - num4;
					if (num5 > 0)
					{
						cmdLine.parameters = _text.Substring(num4, num5);
					}
					this.commandList.Add(cmdLine);
				}
			}
			num++;
		}
	}

	// Token: 0x0600379B RID: 14235 RVA: 0x0016E927 File Offset: 0x0016CB27
	public void Reset()
	{
		this.curIndex = -1;
	}

	// Token: 0x0600379C RID: 14236 RVA: 0x0016E930 File Offset: 0x0016CB30
	public void Restart()
	{
		this.curIndex = 0;
		this.sleep = 0f;
	}

	// Token: 0x0600379D RID: 14237 RVA: 0x0016E944 File Offset: 0x0016CB44
	public void Run(SleeperVolume _sv, EntityPlayer _player, float _countScale)
	{
		if (this.commandList == null)
		{
			return;
		}
		this.player = _player;
		this.countScale = _countScale;
		this.curIndex = 0;
		this.sleep = 0f;
	}

	// Token: 0x0600379E RID: 14238 RVA: 0x0016E96F File Offset: 0x0016CB6F
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsRunning()
	{
		return this.curIndex >= 0;
	}

	// Token: 0x0600379F RID: 14239 RVA: 0x0016E980 File Offset: 0x0016CB80
	public void Tick(SleeperVolume _sv)
	{
		if (this.curIndex < 0)
		{
			return;
		}
		if (this.sleep > 0f)
		{
			this.sleep -= 0.05f;
			if (this.sleep > 0f)
			{
				return;
			}
		}
		for (;;)
		{
			MinScript.CmdLine cmdLine = this.commandList[this.curIndex];
			ushort command = cmdLine.command;
			int num;
			switch (command)
			{
			case 1:
				Log.Out("MinScript " + cmdLine.parameters);
				break;
			case 2:
				break;
			case 3:
				if (this.loopCount <= 0)
				{
					string[] array = cmdLine.parameters.Split(' ', StringSplitOptions.None);
					if (array.Length == 2)
					{
						this.loopToIndex = this.FindLabel(array[0]);
						if (this.loopToIndex < 0)
						{
							Log.Warning("MinScript loop label {0} missing: {1}", new object[]
							{
								array[0],
								_sv
							});
						}
						else
						{
							this.loopCount = int.Parse(array[1]);
						}
					}
					else
					{
						Log.Warning("MinScript loop needs 2 params: {0}", new object[] { _sv });
					}
				}
				num = this.loopCount - 1;
				this.loopCount = num;
				if (num > 0)
				{
					this.curIndex = this.loopToIndex;
				}
				break;
			case 4:
				this.sleep = float.Parse(cmdLine.parameters ?? "1");
				break;
			default:
				if (command != 40)
				{
					switch (command)
					{
					case 50:
						if (cmdLine.parameters != null)
						{
							string[] array2 = cmdLine.parameters.Split(' ', StringSplitOptions.None);
							if (array2.Length >= 1)
							{
								float num2 = 1f;
								float num3 = 1f;
								if (array2.Length >= 2)
								{
									num2 = float.Parse(array2[1]);
									num3 = num2;
									if (array2.Length >= 3)
									{
										num3 = float.Parse(array2[2]);
									}
								}
								_sv.AddSpawnCount(array2[0], num2 * this.countScale, num3 * this.countScale);
							}
						}
						break;
					case 51:
					{
						int num4 = int.Parse(cmdLine.parameters ?? "0");
						if (_sv.GetAliveCount() > num4)
						{
							return;
						}
						break;
					}
					case 52:
						if (this.player)
						{
							if (cmdLine.parameters != null)
							{
								byte b = (byte)int.Parse(cmdLine.parameters);
								this.player.world.triggerManager.Trigger(this.player, _sv.PrefabInstance, b);
							}
							else
							{
								Log.Warning("MinScript trigger !param {0}", new object[] { _sv });
							}
						}
						break;
					}
				}
				else
				{
					GameManager.Instance.PlaySoundAtPositionServer(_sv.Center, cmdLine.parameters, AudioRolloffMode.Linear, 100, _sv.GetPlayerTouchedToUpdateId(), 1f);
				}
				break;
			}
			num = this.curIndex + 1;
			this.curIndex = num;
			if (num >= this.commandList.Count)
			{
				goto Block_20;
			}
			if (this.sleep > 0f)
			{
				return;
			}
		}
		return;
		Block_20:
		this.curIndex = -1;
	}

	// Token: 0x060037A0 RID: 14240 RVA: 0x0016EC4C File Offset: 0x0016CE4C
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindLabel(string _name)
	{
		int count = this.commandList.Count;
		for (int i = 0; i < count; i++)
		{
			MinScript.CmdLine cmdLine = this.commandList[i];
			if (cmdLine.command == 2 && cmdLine.parameters == _name)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060037A1 RID: 14241 RVA: 0x0016EC98 File Offset: 0x0016CE98
	public static MinScript Read(BinaryReader _br)
	{
		_br.ReadByte();
		MinScript minScript = new MinScript();
		minScript.curIndex = (int)_br.ReadInt16();
		if (minScript.curIndex >= 0)
		{
			minScript.sleep = _br.ReadSingle();
		}
		int num = (int)_br.ReadUInt16();
		for (int i = 0; i < num; i++)
		{
			MinScript.CmdLine cmdLine;
			cmdLine.command = _br.ReadUInt16();
			cmdLine.parameters = null;
			int num2 = (int)_br.ReadByte();
			if (num2 > 0)
			{
				_br.Read(MinScript.tempBytes, 0, num2);
				int chars = Encoding.UTF8.GetChars(MinScript.tempBytes, 0, num2, MinScript.tempChars, 0);
				cmdLine.parameters = new string(MinScript.tempChars, 0, chars);
			}
			minScript.commandList.Add(cmdLine);
		}
		return minScript;
	}

	// Token: 0x060037A2 RID: 14242 RVA: 0x0016ED52 File Offset: 0x0016CF52
	public bool HasData()
	{
		return this.commandList.Count > 0;
	}

	// Token: 0x060037A3 RID: 14243 RVA: 0x0016ED64 File Offset: 0x0016CF64
	public void Write(BinaryWriter _bw)
	{
		_bw.Write(1);
		_bw.Write((short)this.curIndex);
		if (this.curIndex >= 0)
		{
			_bw.Write(this.sleep);
		}
		int num = this.commandList.Count;
		if (num >= 1000)
		{
			num = 1;
			Log.Error("MinScript Write error: {0}", new object[] { this });
		}
		_bw.Write((ushort)num);
		for (int i = 0; i < num; i++)
		{
			MinScript.CmdLine cmdLine = this.commandList[i];
			_bw.Write(cmdLine.command);
			if (cmdLine.parameters != null && cmdLine.parameters.Length > 0)
			{
				for (int j = 0; j < cmdLine.parameters.Length; j++)
				{
					MinScript.tempChars[j] = cmdLine.parameters[j];
				}
				byte b = (byte)Encoding.UTF8.GetBytes(MinScript.tempChars, 0, cmdLine.parameters.Length, MinScript.tempBytes, 0);
				_bw.Write(b);
				_bw.Write(MinScript.tempBytes, 0, (int)b);
			}
			else
			{
				_bw.Write(0);
			}
		}
	}

	// Token: 0x060037A4 RID: 14244 RVA: 0x0016EE7C File Offset: 0x0016D07C
	public override string ToString()
	{
		return string.Format("cmds {0}, index {1}, sleep {2}", this.commandList.Count, this.curIndex, this.sleep);
	}

	// Token: 0x060037A5 RID: 14245 RVA: 0x0016EEAE File Offset: 0x0016D0AE
	[Conditional("DEBUG_MINSCRIPTLOG")]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogMS(string format, params object[] args)
	{
		format = string.Format("{0} {1} MinScript {2}", GameManager.frameTime.ToCultureInvariantString(), GameManager.frameCount, format);
		Log.Out(format, args);
	}

	// Token: 0x04002D42 RID: 11586
	[PublicizedFrom(EAccessModifier.Private)]
	public const byte cVersion = 1;

	// Token: 0x04002D43 RID: 11587
	[PublicizedFrom(EAccessModifier.Private)]
	public const char cLineSepChar = '^';

	// Token: 0x04002D44 RID: 11588
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cLineSepStr = "^";

	// Token: 0x04002D45 RID: 11589
	[PublicizedFrom(EAccessModifier.Private)]
	public List<MinScript.CmdLine> commandList = new List<MinScript.CmdLine>();

	// Token: 0x04002D46 RID: 11590
	[PublicizedFrom(EAccessModifier.Private)]
	public int curIndex;

	// Token: 0x04002D47 RID: 11591
	[PublicizedFrom(EAccessModifier.Private)]
	public float sleep;

	// Token: 0x04002D48 RID: 11592
	[PublicizedFrom(EAccessModifier.Private)]
	public int loopCount;

	// Token: 0x04002D49 RID: 11593
	[PublicizedFrom(EAccessModifier.Private)]
	public int loopToIndex;

	// Token: 0x04002D4A RID: 11594
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdNop = 0;

	// Token: 0x04002D4B RID: 11595
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdLog = 1;

	// Token: 0x04002D4C RID: 11596
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdLabel = 2;

	// Token: 0x04002D4D RID: 11597
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdLoop = 3;

	// Token: 0x04002D4E RID: 11598
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdSleep = 4;

	// Token: 0x04002D4F RID: 11599
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdSound = 40;

	// Token: 0x04002D50 RID: 11600
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdSpawn = 50;

	// Token: 0x04002D51 RID: 11601
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdWaitAlive = 51;

	// Token: 0x04002D52 RID: 11602
	[PublicizedFrom(EAccessModifier.Private)]
	public const ushort cCmdTrigger = 52;

	// Token: 0x04002D53 RID: 11603
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<string, ushort> nameToCmds = new Dictionary<string, ushort>
	{
		{ "log", 1 },
		{ "label", 2 },
		{ "loop", 3 },
		{ "sleep", 4 },
		{ "sound", 40 },
		{ "spawn", 50 },
		{ "trigger", 52 },
		{ "waitalive", 51 }
	};

	// Token: 0x04002D54 RID: 11604
	[PublicizedFrom(EAccessModifier.Private)]
	public static byte[] tempBytes = new byte[256];

	// Token: 0x04002D55 RID: 11605
	[PublicizedFrom(EAccessModifier.Private)]
	public static char[] tempChars = new char[256];

	// Token: 0x04002D56 RID: 11606
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayer player;

	// Token: 0x04002D57 RID: 11607
	[PublicizedFrom(EAccessModifier.Private)]
	public float countScale = 1f;

	// Token: 0x0200072F RID: 1839
	[PublicizedFrom(EAccessModifier.Private)]
	public struct CmdLine
	{
		// Token: 0x04002D58 RID: 11608
		public ushort command;

		// Token: 0x04002D59 RID: 11609
		public string parameters;
	}
}
