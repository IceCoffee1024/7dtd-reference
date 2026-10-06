using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class TemporaryObject : MonoBehaviour
{
	// Token: 0x06002C6B RID: 11371 RVA: 0x0011A55C File Offset: 0x0011875C
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

	// Token: 0x06002C6C RID: 11372 RVA: 0x0011A6EA File Offset: 0x001188EA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Start()
	{
		this.coroutine = base.StartCoroutine(this.DestroyLater());
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x0011A6FE File Offset: 0x001188FE
	public void Restart()
	{
		base.gameObject.SetActive(true);
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
		}
		this.Start();
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x0011A726 File Offset: 0x00118926
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

	// Token: 0x06002C6F RID: 11375 RVA: 0x0011A738 File Offset: 0x00118938
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

	// Token: 0x04002240 RID: 8768
	public float life = 2f;

	// Token: 0x04002241 RID: 8769
	public bool destroyMaterials;

	// Token: 0x04002242 RID: 8770
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Coroutine coroutine;
}
