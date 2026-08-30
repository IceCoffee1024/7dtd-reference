using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200127E RID: 4734
public class NGuiHUDText : MonoBehaviour
{
	// Token: 0x0600961F RID: 38431 RVA: 0x003872D0 File Offset: 0x003854D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static int Comparison(NGuiHUDText.Entry a, NGuiHUDText.Entry b)
	{
		if (a.movementStart < b.movementStart)
		{
			return -1;
		}
		if (a.movementStart > b.movementStart)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x170011BC RID: 4540
	// (get) Token: 0x06009620 RID: 38432 RVA: 0x003872F3 File Offset: 0x003854F3
	public bool isVisible
	{
		get
		{
			return this.mList.Count != 0;
		}
	}

	// Token: 0x170011BD RID: 4541
	// (get) Token: 0x06009621 RID: 38433 RVA: 0x00387303 File Offset: 0x00385503
	// (set) Token: 0x06009622 RID: 38434 RVA: 0x0038730B File Offset: 0x0038550B
	public INGUIFont ambigiousFont
	{
		get
		{
			return this.font;
		}
		set
		{
			this.font = value;
		}
	}

	// Token: 0x06009623 RID: 38435 RVA: 0x00387314 File Offset: 0x00385514
	[PublicizedFrom(EAccessModifier.Private)]
	public NGuiHUDText.Entry Create()
	{
		if (this.mUnused.Count > 0)
		{
			NGuiHUDText.Entry entry = this.mUnused[this.mUnused.Count - 1];
			this.mUnused.RemoveAt(this.mUnused.Count - 1);
			entry.time = Time.realtimeSinceStartup;
			entry.label.depth = NGUITools.CalculateNextDepth(base.gameObject);
			NGUITools.SetActive(entry.label.gameObject, true);
			entry.intialOffset = default(Vector3);
			entry.curveOffset = 0f;
			this.mList.Add(entry);
			return entry;
		}
		NGuiHUDText.Entry entry2 = new NGuiHUDText.Entry();
		entry2.time = Time.realtimeSinceStartup;
		entry2.label = base.gameObject.AddWidget<UILabel>(int.MaxValue);
		entry2.label.name = string.Format("Entry_{0}_Label", this.counter);
		entry2.label.font = this.ambigiousFont;
		entry2.label.fontSize = this.fontSize;
		entry2.label.fontStyle = this.fontStyle;
		entry2.label.applyGradient = this.applyGradient;
		entry2.label.gradientTop = this.gradientTop;
		entry2.label.gradientBottom = this.gradienBottom;
		entry2.label.effectStyle = this.effect;
		entry2.label.effectColor = this.effectColor;
		entry2.label.overflowMethod = UILabel.Overflow.ResizeFreely;
		entry2.label.cachedTransform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		entry2.isLabel = true;
		entry2.sprite = base.gameObject.AddWidget<UISprite>(int.MaxValue);
		entry2.sprite.name = string.Format("Entry_{0}_Sprite", this.counter);
		entry2.sprite.keepAspectRatio = UIWidget.AspectRatioSource.BasedOnHeight;
		this.mList.Add(entry2);
		this.counter++;
		return entry2;
	}

	// Token: 0x06009624 RID: 38436 RVA: 0x0038751D File Offset: 0x0038571D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Delete(NGuiHUDText.Entry ent)
	{
		this.mList.Remove(ent);
		this.mUnused.Add(ent);
		NGUITools.SetActive(ent.label.gameObject, false);
	}

	// Token: 0x06009625 RID: 38437 RVA: 0x0038754C File Offset: 0x0038574C
	public void Add(object obj, Color c, float stayDuration)
	{
		if (!base.enabled)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		bool flag = false;
		float num = 0f;
		if (obj is float)
		{
			flag = true;
			num = (float)obj;
		}
		else if (obj is int)
		{
			flag = true;
			num = (float)((int)obj);
		}
		if (flag)
		{
			if (num == 0f)
			{
				return;
			}
			int i = this.mList.Count;
			while (i > 0)
			{
				NGuiHUDText.Entry entry = this.mList[--i];
				if (entry.time + 1f >= realtimeSinceStartup && entry.val != 0f)
				{
					if (entry.val < 0f && num < 0f)
					{
						entry.val += num;
						entry.label.text = Mathf.RoundToInt(entry.val).ToString();
						return;
					}
					if (entry.val > 0f && num > 0f)
					{
						entry.val += num;
						entry.label.text = "+" + Mathf.RoundToInt(entry.val).ToString();
						return;
					}
				}
			}
		}
		NGuiHUDText.Entry entry2 = this.Create();
		entry2.stay = stayDuration;
		entry2.label.color = c;
		entry2.label.alpha = 0f;
		entry2.sprite.color = c;
		entry2.val = num;
		if (flag)
		{
			entry2.label.text = ((num < 0f) ? Mathf.RoundToInt(entry2.val).ToString() : ("+" + Mathf.RoundToInt(entry2.val).ToString()));
		}
		else
		{
			entry2.label.text = obj.ToString();
		}
		this.mList.Sort(new Comparison<NGuiHUDText.Entry>(NGuiHUDText.Comparison));
	}

	// Token: 0x06009626 RID: 38438 RVA: 0x00387740 File Offset: 0x00385940
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnEnable()
	{
		if (this.ambigiousFont == null)
		{
			foreach (NGUIFont nguifont in Resources.FindObjectsOfTypeAll<NGUIFont>())
			{
				if (nguifont.name.EqualsCaseInsensitive("ReferenceFont"))
				{
					this.ambigiousFont = nguifont;
					break;
				}
			}
			if (this.ambigiousFont == null)
			{
				Log.Error("NGuiHUDText font not found");
			}
		}
		this.fontStyle = this.font.dynamicFontStyle;
	}

	// Token: 0x06009627 RID: 38439 RVA: 0x003877AC File Offset: 0x003859AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnValidate()
	{
		INGUIFont ambigiousFont = this.ambigiousFont;
		if (ambigiousFont != null && ambigiousFont.isDynamic)
		{
			this.fontStyle = ambigiousFont.dynamicFontStyle;
			this.fontSize = ambigiousFont.defaultSize;
		}
	}

	// Token: 0x06009628 RID: 38440 RVA: 0x003877E4 File Offset: 0x003859E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void OnDisable()
	{
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			if (entry.label != null)
			{
				entry.label.enabled = false;
			}
			else
			{
				this.mList.RemoveAt(i);
			}
		}
	}

	// Token: 0x06009629 RID: 38441 RVA: 0x0038783C File Offset: 0x00385A3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Update()
	{
		float time = RealTime.time;
		Keyframe[] keys = this.offsetCurve.keys;
		Keyframe[] keys2 = this.alphaCurve.keys;
		Keyframe[] keys3 = this.scaleCurve.keys;
		float time2 = keys[keys.Length - 1].time;
		float time3 = keys2[keys2.Length - 1].time;
		float num = Mathf.Max(keys3[keys3.Length - 1].time, Mathf.Max(time2, time3));
		int i = this.mList.Count;
		while (i > 0)
		{
			NGuiHUDText.Entry entry = this.mList[--i];
			float num2 = time - entry.movementStart;
			entry.curveOffset = this.offsetCurve.Evaluate(num2);
			entry.label.alpha = this.alphaCurve.Evaluate(num2);
			float num3 = this.scaleCurve.Evaluate(time - entry.time);
			if (num3 < 0.001f)
			{
				num3 = 0.001f;
			}
			entry.label.cachedTransform.localScale = new Vector3(num3, num3, num3);
			if (num2 > num)
			{
				this.Delete(entry);
			}
			else
			{
				entry.label.enabled = true;
			}
		}
		float num4 = 0f;
		float num5 = 0f;
		for (int j = 0; j < this.mList.Count; j++)
		{
			NGuiHUDText.Entry entry2 = this.mList[j];
			if (this.verticalStack)
			{
				num5 += (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height);
				num4 = Mathf.Max(num4, (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width));
			}
			else
			{
				num4 += (float)(entry2.isLabel ? entry2.label.width : entry2.sprite.width);
				num5 = Mathf.Max(num5, (float)(entry2.isLabel ? entry2.label.height : entry2.sprite.height));
			}
		}
		if (this.verticalStack)
		{
			float num6 = 0f;
			for (int k = 0; k < this.mList.Count; k++)
			{
				NGuiHUDText.Entry entry3 = this.mList[k];
				num6 = Mathf.Max(num6, entry3.curveOffset);
				if (entry3.isLabel)
				{
					entry3.label.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.label.cachedTransform.localScale.y * (float)entry3.label.fontSize);
				}
				else
				{
					entry3.sprite.cachedTransform.localPosition = new Vector3(0f, num6, 0f) + entry3.intialOffset * num5;
					num6 += Mathf.Round(entry3.sprite.cachedTransform.localScale.y * (float)entry3.sprite.height);
				}
			}
			return;
		}
		float num7 = 0f;
		for (int l = 0; l < this.mList.Count; l++)
		{
			NGuiHUDText.Entry entry4 = this.mList[l];
			if (entry4.isLabel)
			{
				entry4.label.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.label.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.label.width;
			}
			else
			{
				entry4.sprite.cachedTransform.localPosition = new Vector3(num7 + ((float)entry4.sprite.width - num4) / 2f, entry4.curveOffset, 0f) + entry4.intialOffset * num5;
				num7 += (float)entry4.sprite.width;
			}
		}
	}

	// Token: 0x0600962A RID: 38442 RVA: 0x00387C90 File Offset: 0x00385E90
	public void SetEntry(int _index, string _input, bool _isSprite, INGUIAtlas _spriteAtlas = null)
	{
		if (this.mList.Count <= _index)
		{
			return;
		}
		this.mList[_index].isLabel = !_isSprite;
		if (!_isSprite)
		{
			this.mList[_index].label.text = _input;
			this.mList[_index].sprite.atlas = null;
			this.mList[_index].sprite.spriteName = string.Empty;
			return;
		}
		this.mList[_index].label.text = string.Empty;
		this.mList[_index].sprite.atlas = (string.IsNullOrEmpty(_input) ? null : _spriteAtlas);
		this.mList[_index].sprite.spriteName = _input;
	}

	// Token: 0x0600962B RID: 38443 RVA: 0x00387D63 File Offset: 0x00385F63
	public void SetEntrySize(int _index, int _size)
	{
		this.mList[_index].label.fontSize = _size;
		this.mList[_index].sprite.height = _size;
	}

	// Token: 0x0600962C RID: 38444 RVA: 0x00387D93 File Offset: 0x00385F93
	public void SetEntryOffset(int _index, Vector3 _offset)
	{
		this.mList[_index].intialOffset = _offset;
	}

	// Token: 0x0600962D RID: 38445 RVA: 0x00387DA7 File Offset: 0x00385FA7
	public void SetEntryColor(int _index, Color _c)
	{
		this.mList[_index].label.color = _c;
		this.mList[_index].sprite.color = _c;
	}

	// Token: 0x0400703B RID: 28731
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string uiFontName = "ReferenceFont";

	// Token: 0x0400703C RID: 28732
	[HideInInspector]
	[SerializeField]
	[PublicizedFrom(EAccessModifier.Private)]
	public INGUIFont font;

	// Token: 0x0400703D RID: 28733
	public int fontSize = 20;

	// Token: 0x0400703E RID: 28734
	public FontStyle fontStyle;

	// Token: 0x0400703F RID: 28735
	public bool applyGradient;

	// Token: 0x04007040 RID: 28736
	public Color gradientTop = Color.white;

	// Token: 0x04007041 RID: 28737
	public Color gradienBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x04007042 RID: 28738
	public UILabel.Effect effect;

	// Token: 0x04007043 RID: 28739
	public Color effectColor = Color.black;

	// Token: 0x04007044 RID: 28740
	public bool verticalStack;

	// Token: 0x04007045 RID: 28741
	public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(3f, 40f)
	});

	// Token: 0x04007046 RID: 28742
	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(1f, 1f),
		new Keyframe(3f, 0f)
	});

	// Token: 0x04007047 RID: 28743
	public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 1f)
	});

	// Token: 0x04007048 RID: 28744
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mList = new List<NGuiHUDText.Entry>();

	// Token: 0x04007049 RID: 28745
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<NGuiHUDText.Entry> mUnused = new List<NGuiHUDText.Entry>();

	// Token: 0x0400704A RID: 28746
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int counter;

	// Token: 0x0200127F RID: 4735
	[PublicizedFrom(EAccessModifier.Protected)]
	public class Entry
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x0600962F RID: 38447 RVA: 0x00387EF0 File Offset: 0x003860F0
		public float movementStart
		{
			get
			{
				return this.time + this.stay;
			}
		}

		// Token: 0x0400704B RID: 28747
		public float time;

		// Token: 0x0400704C RID: 28748
		public float stay;

		// Token: 0x0400704D RID: 28749
		public Vector3 intialOffset;

		// Token: 0x0400704E RID: 28750
		public float curveOffset;

		// Token: 0x0400704F RID: 28751
		public float val;

		// Token: 0x04007050 RID: 28752
		public UILabel label;

		// Token: 0x04007051 RID: 28753
		public UISprite sprite;

		// Token: 0x04007052 RID: 28754
		public bool isLabel;
	}
}
