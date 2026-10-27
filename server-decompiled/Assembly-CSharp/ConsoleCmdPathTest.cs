using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamePath;
using Pathfinding;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000256 RID: 598
[Preserve]
public class ConsoleCmdPathTest : ConsoleCmdAbstract
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x060011B0 RID: 4528 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsExecuteOnClient
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0006F734 File Offset: 0x0006D934
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "pathtest" };
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x0006F744 File Offset: 0x0006D944
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Usage. Toggle a path test mode.");
		stringBuilder.AppendLine(" breakblocks - toggles path allowed to break blocks");
		stringBuilder.AppendLine(" climbladders - toggles path allowed to break blocks");
		stringBuilder.AppendLine(" climbwalls - toggles path allowed to break blocks");
		return stringBuilder.ToString();
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0006F780 File Offset: 0x0006D980
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			return;
		}
		if (_params.Count > 0)
		{
			foreach (string text in _params)
			{
				string text2 = text.ToLower();
				bool on = text2.StartsWith("+");
				bool off = text2.StartsWith("-");
				if (on | off)
				{
					text2 = text2.Substring(1);
				}
				Func<bool, bool> func = (bool currentValue) => on || (!off && !currentValue);
				if (string.Equals(text2, "breakblocks", StringComparison.OrdinalIgnoreCase))
				{
					ConsoleCmdPathTest.canBreakBlocks = func(ConsoleCmdPathTest.canBreakBlocks);
				}
				else if (string.Equals(text2, "climbladders", StringComparison.OrdinalIgnoreCase))
				{
					ConsoleCmdPathTest.canClimbLadders = func(ConsoleCmdPathTest.canClimbLadders);
				}
				else if (string.Equals(text2, "climbwalls", StringComparison.OrdinalIgnoreCase))
				{
					ConsoleCmdPathTest.canClimbWalls = func(ConsoleCmdPathTest.canClimbWalls);
				}
			}
		}
		if (this.executionsCoroutine != null)
		{
			EAIPathTest eaipathTest = this.aiPathTest;
			if (eaipathTest != null)
			{
				eaipathTest.CancelTargetMove();
			}
			GameManager.Instance.StopCoroutine(this.executionsCoroutine);
			this.executionsCoroutine = null;
			return;
		}
		this.executionsCoroutine = GameManager.Instance.StartCoroutine(this.ExecutePathTestCoroutine());
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x0006F8D8 File Offset: 0x0006DAD8
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator ExecutePathTestCoroutine()
	{
		EntityPlayerLocal player = GameManager.Instance.World.GetPrimaryPlayer();
		this.aiPathTest = null;
		string text;
		EntityAlive entityAlive = ItemActionAttack.FindHitEntityNoTagCheck(player.HitInfo, out text) as EntityAlive;
		if (entityAlive != null)
		{
			EAIManager aiManager = entityAlive.aiManager;
			EAIPathTest eaipathTest;
			if (aiManager == null)
			{
				eaipathTest = null;
			}
			else
			{
				List<EAIPathTest> tasks = aiManager.GetTasks<EAIPathTest>();
				eaipathTest = ((tasks != null) ? tasks.FirstOrDefault<EAIPathTest>() : null);
			}
			this.aiPathTest = eaipathTest;
		}
		PathInfoSingleTarget pathInfo = new PathInfoSingleTarget(player, Vector3.zero, false, 1f, null);
		ASPPathFinder pathFinder = new ASPPathFinder(pathInfo, false, false, false);
		GraphNode[] pathNodes = null;
		pathInfo.state = PathInfo.State.Done;
		pathInfo.OnPathResult = delegate(Path p)
		{
			List<GraphNode> path = p.path;
			pathNodes = ((path != null) ? path.ToArray() : null);
		};
		for (;;)
		{
			if (pathInfo.canBreakBlocks == ConsoleCmdPathTest.canBreakBlocks && pathFinder.canClimbLadders == ConsoleCmdPathTest.canClimbLadders)
			{
				bool flag = pathFinder.canClimbLadders != ConsoleCmdPathTest.canClimbLadders;
			}
			pathInfo.canBreakBlocks = ConsoleCmdPathTest.canBreakBlocks;
			pathFinder.canClimbLadders = ConsoleCmdPathTest.canClimbLadders;
			pathFinder.canClimbWalls = ConsoleCmdPathTest.canClimbWalls;
			WorldRayHitInfo hitInfo = player.HitInfo;
			if (hitInfo.bHitValid)
			{
				if (Input.GetMouseButton(2))
				{
					pathInfo.targetPos = World.blockToTransformPos(hitInfo.hit.blockPos);
					ConsoleCmdPathTest.recalculatePath = true;
				}
				else if (Input.GetMouseButton(3))
				{
					pathInfo.hasStart = !pathInfo.hasStart;
					pathInfo.startPos = World.blockToTransformPos(hitInfo.hit.blockPos);
					ConsoleCmdPathTest.recalculatePath = true;
				}
			}
			if (ConsoleCmdPathTest.recalculatePath)
			{
				ConsoleCmdPathTest.recalculatePath = false;
				if (this.aiPathTest != null)
				{
					pathNodes = null;
					this.aiPathTest.SetTargetMove(pathInfo.targetPos, player, ConsoleCmdPathTest.canBreakBlocks);
				}
				else if (pathInfo.state == PathInfo.State.Done)
				{
					ConsoleCmdPathTest.recalculatePath = false;
					pathFinder.Calculate(pathInfo.hasStart ? pathInfo.startPos : player.position);
				}
			}
			if (pathNodes != null)
			{
				for (int i = 1; i < pathNodes.Length; i++)
				{
					AstarVoxelGrid.VoxelNode voxelNode = (AstarVoxelGrid.VoxelNode)pathNodes[i - 1];
					AstarVoxelGrid.VoxelNode voxelNode2 = (AstarVoxelGrid.VoxelNode)pathNodes[i];
					Vector3 vector = (Vector3)voxelNode.position;
					Vector3 vector2 = (Vector3)voxelNode2.position;
					uint num = Math.Max(voxelNode.Tag, voxelNode2.Tag);
					Color color = Color.white;
					switch (num)
					{
					case 0U:
						color = Color.green;
						break;
					case 1U:
						color = Color.grey;
						break;
					case 2U:
						color = Color.yellow;
						break;
					case 3U:
						color = Color.blue;
						break;
					case 4U:
						color = Color.cyan;
						break;
					}
					Debug.DrawLine(vector, vector2, color, 0.11f);
				}
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0006F8E7 File Offset: 0x0006DAE7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enable a path testing utility mode";
	}

	// Token: 0x04000CE4 RID: 3300
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine executionsCoroutine;

	// Token: 0x04000CE5 RID: 3301
	[PublicizedFrom(EAccessModifier.Private)]
	public EAIPathTest aiPathTest;

	// Token: 0x04000CE6 RID: 3302
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool recalculatePath = true;

	// Token: 0x04000CE7 RID: 3303
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canBreakBlocks = false;

	// Token: 0x04000CE8 RID: 3304
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canClimbLadders = false;

	// Token: 0x04000CE9 RID: 3305
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool canClimbWalls = false;
}
