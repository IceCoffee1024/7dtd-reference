using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class TemporaryObject : MonoBehaviour
{
	// Token: 0x06002C4B RID: 11339 RVA: 0x00118718 File Offset: 0x00116918
	public void SetLife(float _life)
	{
		this.life = _life;
		float num = Utils.FastMax(_life - 1f, 0.1f);
		float num2 = 0.1f;
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			float duration = componentsInChildren[i].main.duration;
			if (duration > num2)
			{
				num2 = duration;
			}
		}
		float num3 = num / num2;
		float num4 = num2 * 0.5f;
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			float duration2 = main.duration;
			if (duration2 >= num4)
			{
				particleSystem.Stop(false);
				main.duration = duration2 * num3;
				ParticleSystem.MinMaxCurve startDelay = main.startDelay;
				ParticleSystemCurveMode particleSystemCurveMode = startDelay.mode;
				if (particleSystemCurveMode != ParticleSystemCurveMode.Constant)
				{
					if (particleSystemCurveMode != ParticleSystemCurveMode.TwoConstants)
					{
						startDelay.curveMultiplier *= num3;
					}
					else
					{
						startDelay.constantMin *= num3;
						startDelay.constantMax *= num3;
					}
				}
				else
				{
					startDelay.constant *= num3;
				}
				main.startDelay = startDelay;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				particleSystemCurveMode = startLifetime.mode;
				if (particleSystemCurveMode != ParticleSystemCurveMode.Constant)
				{
					if (particleSystemCurveMode != ParticleSystemCurveMode.TwoConstants)
					{
						startLifetime.curveMultiplier *= num3;
					}
					else
					{
						startLifetime.constantMin *= num3;
						startLifetime.constantMax *= num3;
					}
				}
				else
				{
					startLifetime.constant *= num3;
				}
				main.startLifetime = startLifetime;
				particleSystem.Play(false);
			}
		}
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x001188A6 File Offset: 0x00116AA6
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.coroutine = base.StartCoroutine(this.DestroyLater());
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x001188BA File Offset: 0x00116ABA
	public void Restart()
	{
		base.gameObject.SetActive(true);
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.Start();
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x001188E2 File Offset: 0x00116AE2
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DestroyLater()
	{
		yield return new WaitForSeconds(this.life);
		if (this.destroyMaterials)
		{
			Utils.CleanupMaterialsOfRenderers<Renderer[]>(base.transform.GetComponentsInChildren<Renderer>());
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x001188F4 File Offset: 0x00116AF4
	[Conditional("DEBUG_TEMPOBJ")]
	public void LogTO(string _format = "", params object[] _args)
	{
		_format = string.Format("{0} TemporaryObject {1}, id {2}, life {3}, {4}", new object[]
		{
			GameManager.frameCount,
			base.gameObject.GetGameObjectPath(),
			base.gameObject.GetInstanceID(),
			this.life,
			_format
		});
		Log.Warning(_format, _args);
	}

	// Token: 0x04002206 RID: 8710
	public float life = 2f;

	// Token: 0x04002207 RID: 8711
	public bool destroyMaterials;

	// Token: 0x04002208 RID: 8712
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine coroutine;
}
