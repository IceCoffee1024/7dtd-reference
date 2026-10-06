using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000228 RID: 552
[Preserve]
public class ConsoleCmdGraph : ConsoleCmdAbstract
{
	// Token: 0x17000194 RID: 404
	// (get) Token: 0x060010A0 RID: 4256 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000195 RID: 405
	// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x00069DF9 File Offset: 0x00067FF9
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "graph" };
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x00069E09 File Offset: 0x00068009
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Draws graphs on screen";
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x00069E10 File Offset: 0x00068010
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Graph commands:\n# - 0 removes all graphs, 1+ sets graphs height\ncvar <name> <count> <max> - show graph of a cvar, line count (0 hides), max graph value (default is 1)\ndr <count> - show dynamic res graph, line count (0 hides)\nfps <count> <fps max> - show fps graph, line count (0 hides), max graph value\npe <name> <count> <max> - show graph of a passive effect (healthmax..), line count (0 hides), max graph value (default is 1)\nspf <count> <spf max> - show seconds per frame graph, line count (0 hides), max graph value\nstat <name> <count> <max> - show graph of a stat (health, stamina..) with line count (0 hides), max graph value (default is 1)\ntex <name> <count> - show texture graph (mem or stream), line count (0 hides)";
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00069E18 File Offset: 0x00068018
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		if (GameManager.Instance.World == null)
		{
			return;
		}
		GameRenderManager renderManager = GameManager.Instance.World.GetPrimaryPlayer().renderManager;
		GameGraphManager graphManager = renderManager.graphManager;
		string text = _params[0].ToLower();
		if (char.IsDigit(text[0]))
		{
			int num = int.Parse(text);
			if (num == 0)
			{
				graphManager.RemoveAll();
				return;
			}
			graphManager.SetHeight(num);
			return;
		}
		else
		{
			if (text == "dr")
			{
				int num2 = 0;
				if (_params.Count >= 2)
				{
					int.TryParse(_params[1], out num2);
				}
				graphManager.Add("Dynamic Res", new GameGraphManager.Graph.Callback(renderManager.DynamicResolutionUpdateGraph), num2, 1f, 0.5f);
				return;
			}
			if (text == "fps")
			{
				int num3 = 0;
				if (_params.Count >= 2)
				{
					int.TryParse(_params[1], out num3);
				}
				int num4 = 100;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num4);
				}
				graphManager.Add("FPS", new GameGraphManager.Graph.Callback(renderManager.FPSUpdateGraph), num3, (float)num4, 60f);
				return;
			}
			if (text == "spf")
			{
				int num5 = 0;
				if (_params.Count >= 2)
				{
					int.TryParse(_params[1], out num5);
				}
				int num6 = 100;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num6);
				}
				int num7 = 17;
				if (_params.Count >= 4)
				{
					int.TryParse(_params[3], out num7);
				}
				graphManager.Add("SPF", new GameGraphManager.Graph.Callback(renderManager.SPFUpdateGraph), num5, (float)num6, (float)num7);
				return;
			}
			if (text == "tex" && _params.Count >= 2)
			{
				string text2 = _params[1].ToLower();
				int num8 = 0;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num8);
				}
				GameGraphManager.Graph.Callback callback;
				if (text2 == "mem")
				{
					callback = delegate(ref float value)
					{
						value = Texture.currentTextureMemory / 1048576UL;
						return true;
					};
					graphManager.Add("Tex Current", callback, num8, 12288f, 6144f);
					callback = delegate(ref float value)
					{
						value = Texture.desiredTextureMemory / 1048576UL;
						return true;
					};
					graphManager.Add("Tex Desired", callback, num8, 12288f, 6144f);
					callback = delegate(ref float value)
					{
						value = Texture.totalTextureMemory / 1048576UL;
						return true;
					};
					graphManager.Add("Tex Total", callback, num8, 12288f, 6144f);
					return;
				}
				if (!(text2 == "stream"))
				{
					return;
				}
				callback = delegate(ref float value)
				{
					value = Texture.streamingTextureCount;
					return true;
				};
				graphManager.Add("TStream Textures", callback, num8, 3000f, 0f);
				callback = delegate(ref float value)
				{
					value = Texture.streamingTextureLoadingCount;
					return true;
				};
				graphManager.Add("TStream Loading", callback, num8, 100f, 0f);
				callback = delegate(ref float value)
				{
					value = Texture.streamingRendererCount;
					return true;
				};
				graphManager.Add("TStream Renderers", callback, num8, 25000f, 0f);
				return;
			}
			else if (text == "cvar")
			{
				if (_params.Count < 2)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No cvar name");
					return;
				}
				string text3 = _params[1];
				int num9 = 0;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num9);
				}
				int num10 = 1;
				if (_params.Count >= 4)
				{
					int.TryParse(_params[3], out num10);
				}
				graphManager.AddCVar("CVar " + text3, num9, text3, (float)num10, 0f);
				return;
			}
			else if (text == "pe")
			{
				if (_params.Count < 2)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No pe name");
					return;
				}
				PassiveEffects passiveEffects;
				Enum.TryParse<PassiveEffects>(_params[1], true, out passiveEffects);
				int num11 = 0;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num11);
				}
				int num12 = 1;
				if (_params.Count >= 4)
				{
					int.TryParse(_params[3], out num12);
				}
				graphManager.AddPassiveEffect("PE " + passiveEffects.ToStringCached<PassiveEffects>(), num11, passiveEffects, (float)num12, 0f);
				return;
			}
			else
			{
				if (!(text == "stat"))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown command " + _params[0]);
					return;
				}
				if (_params.Count < 2)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No stat name");
					return;
				}
				string text4 = _params[1];
				int num13 = 0;
				if (_params.Count >= 3)
				{
					int.TryParse(_params[2], out num13);
				}
				int num14 = 1;
				if (_params.Count >= 4)
				{
					int.TryParse(_params[3], out num14);
				}
				graphManager.AddStat("Stat " + text4, num13, text4, (float)num14, 0f);
				return;
			}
		}
	}
}
