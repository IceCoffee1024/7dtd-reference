using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000210 RID: 528
[Preserve]
public class ConsoleCmdDismemberment : ConsoleCmdAbstract
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000FFF RID: 4095 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x000668DB File Offset: 0x00064ADB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testDismemberment", "tds" };
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x000668F3 File Offset: 0x00064AF3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dismemberment testing toggle.";
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x000668FC File Offset: 0x00064AFC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count > 0 && _params[0].ContainsCaseInsensitive("debuglog"))
		{
			DismembermentManager.DebugLogEnabled = !DismembermentManager.DebugLogEnabled;
			Log.Out("Dismemberment debug log enabled: " + DismembermentManager.DebugLogEnabled.ToString());
		}
		if (!GamePrefs.GetBool(EnumGamePrefs.DebugMenuEnabled))
		{
			return;
		}
		if (_params.Count == 0)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			primaryPlayer.DebugDismembermentChance = !primaryPlayer.DebugDismembermentChance;
			Log.Out("Dismemberment testing enabled: " + primaryPlayer.DebugDismembermentChance.ToString());
			return;
		}
		int i = 0;
		while (i < _params.Count)
		{
			if (_params[i].ContainsCaseInsensitive("bodypart"))
			{
				if (_params.Count <= i)
				{
					Log.Out("Dismemberment bodypart(s) invalid number of params: " + _params.Count.ToString());
					return;
				}
				EnumBodyPartHit enumBodyPartHit = EnumBodyPartHit.None;
				if (Enum.TryParse<EnumBodyPartHit>(_params[i + 1], true, out enumBodyPartHit))
				{
					DismembermentManager.DebugBodyPartHit = enumBodyPartHit;
					Log.Out("Dismemberment test bodypart(s): " + enumBodyPartHit.ToString());
					return;
				}
				Log.Out("Dismemberment bodypart unknown: " + _params[i + 1]);
				return;
			}
			else
			{
				if (_params[i].ContainsCaseInsensitive("arms"))
				{
					DismembermentManager.DebugShowArmRotations = !DismembermentManager.DebugShowArmRotations;
					Log.Out("Dismemberment debug arm rotations: " + DismembermentManager.DebugShowArmRotations.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("explosions"))
				{
					DismembermentManager.DebugDismemberExplosions = !DismembermentManager.DebugDismemberExplosions;
					Log.Out("Dismemberment debug explosions: " + DismembermentManager.DebugDismemberExplosions.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("matrix"))
				{
					DismembermentManager.DebugBulletTime = !DismembermentManager.DebugBulletTime;
					Log.Out("Dismemberment debug bullet time: " + DismembermentManager.DebugBulletTime.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("blood"))
				{
					DismembermentManager.DebugBulletTime = !DismembermentManager.DebugBloodParticles;
					Log.Out("Dismemberment debug blood particles: " + DismembermentManager.DebugBloodParticles.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("noparts"))
				{
					DismembermentManager.DebugDontCreateParts = !DismembermentManager.DebugDontCreateParts;
					Log.Out("Dismemberment debug dont create parts: " + DismembermentManager.DebugDontCreateParts.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("legacy"))
				{
					DismembermentManager.DebugUseLegacy = !DismembermentManager.DebugUseLegacy;
					Log.Out("Dismemberment debug use legacy parts: " + DismembermentManager.DebugUseLegacy.ToString());
				}
				if (_params[i].ContainsCaseInsensitive("explosive"))
				{
					DismembermentManager.DebugExplosiveCleanup = !DismembermentManager.DebugExplosiveCleanup;
					Log.Out("Dismemberment debug use explosive cleanup: " + DismembermentManager.DebugExplosiveCleanup.ToString());
				}
				i++;
			}
		}
	}
}
