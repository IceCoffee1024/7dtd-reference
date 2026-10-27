using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000212 RID: 530
[Preserve]
public class ConsoleCmdDismemberment : ConsoleCmdAbstract
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06001003 RID: 4099 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x00066AAB File Offset: 0x00064CAB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "testDismemberment", "tds" };
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00066AC3 File Offset: 0x00064CC3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Dismemberment testing toggle.";
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x00066ACC File Offset: 0x00064CCC
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
