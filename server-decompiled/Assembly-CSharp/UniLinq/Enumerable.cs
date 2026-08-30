using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178D RID: 6029
	public static class Enumerable
	{
		// Token: 0x0600BAAD RID: 47789 RVA: 0x00457760 File Offset: 0x00455960
		public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			Check.SourceAndFunc(source, func);
			TSource tsource3;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					throw Enumerable.EmptySequence();
				}
				TSource tsource = enumerator.Current;
				while (enumerator.MoveNext())
				{
					TSource tsource2 = enumerator.Current;
					tsource = func(tsource, tsource2);
				}
				tsource3 = tsource;
			}
			return tsource3;
		}

		// Token: 0x0600BAAE RID: 47790 RVA: 0x004577C8 File Offset: 0x004559C8
		public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SourceAndFunc(source, func);
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return taccumulate;
		}

		// Token: 0x0600BAAF RID: 47791 RVA: 0x0045781C File Offset: 0x00455A1C
		public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SourceAndFunc(source, func);
			if (resultSelector == null)
			{
				throw new ArgumentNullException("resultSelector");
			}
			TAccumulate taccumulate = seed;
			foreach (TSource tsource in source)
			{
				taccumulate = func(taccumulate, tsource);
			}
			return resultSelector(taccumulate);
		}

		// Token: 0x0600BAB0 RID: 47792 RVA: 0x00457884 File Offset: 0x00455A84
		public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BAB1 RID: 47793 RVA: 0x004578DC File Offset: 0x00455ADC
		public static bool Any<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count > 0;
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				flag = enumerator.MoveNext();
			}
			return flag;
		}

		// Token: 0x0600BAB2 RID: 47794 RVA: 0x00457930 File Offset: 0x00455B30
		public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BAB3 RID: 47795 RVA: 0x0012A165 File Offset: 0x00128365
		public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			return source;
		}

		// Token: 0x0600BAB4 RID: 47796 RVA: 0x00457988 File Offset: 0x00455B88
		public static double Average(this IEnumerable<int> source)
		{
			Check.Source(source);
			long num = 0L;
			int num2 = 0;
			foreach (int num3 in source)
			{
				checked
				{
					num += unchecked((long)num3);
				}
				num2++;
			}
			if (num2 == 0)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BAB5 RID: 47797 RVA: 0x004579EC File Offset: 0x00455BEC
		public static double Average(this IEnumerable<long> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (long num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BAB6 RID: 47798 RVA: 0x00457A50 File Offset: 0x00455C50
		public static double Average(this IEnumerable<double> source)
		{
			Check.Source(source);
			double num = 0.0;
			long num2 = 0L;
			foreach (double num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (double)num2;
		}

		// Token: 0x0600BAB7 RID: 47799 RVA: 0x00457ABC File Offset: 0x00455CBC
		public static float Average(this IEnumerable<float> source)
		{
			Check.Source(source);
			float num = 0f;
			long num2 = 0L;
			foreach (float num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (float)num2;
		}

		// Token: 0x0600BAB8 RID: 47800 RVA: 0x00457B24 File Offset: 0x00455D24
		public static decimal Average(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			decimal num = 0m;
			long num2 = 0L;
			foreach (decimal num3 in source)
			{
				num += num3;
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / num2;
		}

		// Token: 0x0600BAB9 RID: 47801 RVA: 0x00457B98 File Offset: 0x00455D98
		[PublicizedFrom(EAccessModifier.Private)]
		public static TResult? AverageNullable<TElement, TAggregate, TResult>(this IEnumerable<TElement?> source, Func<TAggregate, TElement, TAggregate> func, Func<TAggregate, long, TResult> result) where TElement : struct where TAggregate : struct where TResult : struct
		{
			Check.Source(source);
			TAggregate taggregate = default(TAggregate);
			long num = 0L;
			foreach (TElement? telement in source)
			{
				if (telement != null)
				{
					taggregate = func(taggregate, telement.Value);
					num += 1L;
				}
			}
			if (num == 0L)
			{
				return null;
			}
			return new TResult?(result(taggregate, num));
		}

		// Token: 0x0600BABA RID: 47802 RVA: 0x00457C24 File Offset: 0x00455E24
		public static double? Average(this IEnumerable<int?> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (int? num3 in source)
			{
				if (num3 != null)
				{
					num += (long)num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BABB RID: 47803 RVA: 0x00457CA4 File Offset: 0x00455EA4
		public static double? Average(this IEnumerable<long?> source)
		{
			Check.Source(source);
			long num = 0L;
			long num2 = 0L;
			foreach (long? num3 in source)
			{
				if (num3 != null)
				{
					checked
					{
						num += num3.Value;
					}
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BABC RID: 47804 RVA: 0x00457D24 File Offset: 0x00455F24
		public static double? Average(this IEnumerable<double?> source)
		{
			Check.Source(source);
			double num = 0.0;
			long num2 = 0L;
			foreach (double? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?(num / (double)num2);
		}

		// Token: 0x0600BABD RID: 47805 RVA: 0x00457DA8 File Offset: 0x00455FA8
		public static decimal? Average(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			decimal num = 0m;
			long num2 = 0L;
			foreach (decimal? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new decimal?(num / num2);
		}

		// Token: 0x0600BABE RID: 47806 RVA: 0x00457E38 File Offset: 0x00456038
		public static float? Average(this IEnumerable<float?> source)
		{
			Check.Source(source);
			float num = 0f;
			long num2 = 0L;
			foreach (float? num3 in source)
			{
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new float?(num / (float)num2);
		}

		// Token: 0x0600BABF RID: 47807 RVA: 0x00457EB8 File Offset: 0x004560B8
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += (long)selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BAC0 RID: 47808 RVA: 0x00457F24 File Offset: 0x00456124
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				int? num3 = selector(tsource);
				if (num3 != null)
				{
					num += (long)num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BAC1 RID: 47809 RVA: 0x00457FAC File Offset: 0x004561AC
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				checked
				{
					num += selector(tsource);
				}
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return (double)num / (double)num2;
		}

		// Token: 0x0600BAC2 RID: 47810 RVA: 0x00458018 File Offset: 0x00456218
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				long? num3 = selector(tsource);
				if (num3 != null)
				{
					checked
					{
						num += num3.Value;
					}
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?((double)num / (double)num2);
		}

		// Token: 0x0600BAC3 RID: 47811 RVA: 0x004580A0 File Offset: 0x004562A0
		public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (double)num2;
		}

		// Token: 0x0600BAC4 RID: 47812 RVA: 0x00458114 File Offset: 0x00456314
		public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				double? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new double?(num / (double)num2);
		}

		// Token: 0x0600BAC5 RID: 47813 RVA: 0x004581A4 File Offset: 0x004563A4
		public static float Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / (float)num2;
		}

		// Token: 0x0600BAC6 RID: 47814 RVA: 0x00458214 File Offset: 0x00456414
		public static float? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				float? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new float?(num / (float)num2);
		}

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x004582A0 File Offset: 0x004564A0
		public static decimal Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
				num2 += 1L;
			}
			if (num2 == 0L)
			{
				throw Enumerable.EmptySequence();
			}
			return num / num2;
		}

		// Token: 0x0600BAC8 RID: 47816 RVA: 0x0045831C File Offset: 0x0045651C
		public static decimal? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			long num2 = 0L;
			foreach (TSource tsource in source)
			{
				decimal? num3 = selector(tsource);
				if (num3 != null)
				{
					num += num3.Value;
					num2 += 1L;
				}
			}
			if (num2 == 0L)
			{
				return null;
			}
			return new decimal?(num / num2);
		}

		// Token: 0x0600BAC9 RID: 47817 RVA: 0x004583B4 File Offset: 0x004565B4
		public static IEnumerable<TResult> Cast<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			IEnumerable<TResult> enumerable = source as IEnumerable<TResult>;
			if (enumerable != null)
			{
				return enumerable;
			}
			return Enumerable.CreateCastIterator<TResult>(source);
		}

		// Token: 0x0600BACA RID: 47818 RVA: 0x004583D9 File Offset: 0x004565D9
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateCastIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				TResult tresult = (TResult)((object)obj);
				yield return tresult;
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BACB RID: 47819 RVA: 0x004583E9 File Offset: 0x004565E9
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return Enumerable.CreateConcatIterator<TSource>(first, second);
		}

		// Token: 0x0600BACC RID: 47820 RVA: 0x004583F9 File Offset: 0x004565F9
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateConcatIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			foreach (TSource tsource in first)
			{
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			foreach (TSource tsource2 in second)
			{
				yield return tsource2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BACD RID: 47821 RVA: 0x00458410 File Offset: 0x00456610
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Contains(value);
			}
			return source.Contains(value, null);
		}

		// Token: 0x0600BACE RID: 47822 RVA: 0x00458438 File Offset: 0x00456638
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			foreach (TSource tsource in source)
			{
				if (comparer.Equals(tsource, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BACF RID: 47823 RVA: 0x0045849C File Offset: 0x0045669C
		public static int Count<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Count;
			}
			int num = 0;
			checked
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BAD0 RID: 47824 RVA: 0x004584F4 File Offset: 0x004566F4
		public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndSelector(source, predicate);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					if (predicate(tsource))
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x0600BAD1 RID: 47825 RVA: 0x0045854C File Offset: 0x0045674C
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source.DefaultIfEmpty(default(TSource));
		}

		// Token: 0x0600BAD2 RID: 47826 RVA: 0x00458568 File Offset: 0x00456768
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			Check.Source(source);
			return Enumerable.CreateDefaultIfEmptyIterator<TSource>(source, defaultValue);
		}

		// Token: 0x0600BAD3 RID: 47827 RVA: 0x00458577 File Offset: 0x00456777
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateDefaultIfEmptyIterator<TSource>(IEnumerable<TSource> source, TSource defaultValue)
		{
			bool empty = true;
			foreach (TSource tsource in source)
			{
				empty = false;
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			if (empty)
			{
				yield return defaultValue;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BAD4 RID: 47828 RVA: 0x0045858E File Offset: 0x0045678E
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
		{
			return source.Distinct<TSource>(null);
		}

		// Token: 0x0600BAD5 RID: 47829 RVA: 0x00458597 File Offset: 0x00456797
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateDistinctIterator<TSource>(source, comparer);
		}

		// Token: 0x0600BAD6 RID: 47830 RVA: 0x004585B0 File Offset: 0x004567B0
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateDistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(comparer);
			foreach (TSource tsource in source)
			{
				if (!items.Contains(tsource))
				{
					items.Add(tsource);
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAD7 RID: 47831 RVA: 0x004585C8 File Offset: 0x004567C8
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index, Enumerable.Fallback fallback)
		{
			long num = 0L;
			foreach (TSource tsource in source)
			{
				long num2 = (long)index;
				long num3 = num;
				num = num3 + 1L;
				if (num2 == num3)
				{
					return tsource;
				}
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw new ArgumentOutOfRangeException();
			}
			return default(TSource);
		}

		// Token: 0x0600BAD8 RID: 47832 RVA: 0x00458630 File Offset: 0x00456830
		public static TSource ElementAt<TSource>(this IEnumerable<TSource> source, int index)
		{
			Check.Source(source);
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return list[index];
			}
			return source.ElementAt<TSource>(index, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BAD9 RID: 47833 RVA: 0x00458668 File Offset: 0x00456868
		public static TSource ElementAtOrDefault<TSource>(this IEnumerable<TSource> source, int index)
		{
			Check.Source(source);
			if (index < 0)
			{
				return default(TSource);
			}
			IList<TSource> list = source as IList<TSource>;
			if (list == null)
			{
				return source.ElementAt<TSource>(index, Enumerable.Fallback.Default);
			}
			if (index >= list.Count)
			{
				return default(TSource);
			}
			return list[index];
		}

		// Token: 0x0600BADA RID: 47834 RVA: 0x004586B6 File Offset: 0x004568B6
		public static IEnumerable<TResult> Empty<TResult>()
		{
			return new TResult[0];
		}

		// Token: 0x0600BADB RID: 47835 RVA: 0x004586BE File Offset: 0x004568BE
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Except<TSource>(second, null);
		}

		// Token: 0x0600BADC RID: 47836 RVA: 0x004586C8 File Offset: 0x004568C8
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateExceptIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BADD RID: 47837 RVA: 0x004586E3 File Offset: 0x004568E3
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateExceptIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(second, comparer);
			foreach (TSource tsource in first)
			{
				if (items.Add(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BADE RID: 47838 RVA: 0x00458704 File Offset: 0x00456904
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					return tsource;
				}
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return default(TSource);
		}

		// Token: 0x0600BADF RID: 47839 RVA: 0x00458768 File Offset: 0x00456968
		public static TSource First<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				if (list.Count != 0)
				{
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			throw Enumerable.EmptySequence();
		}

		// Token: 0x0600BAE0 RID: 47840 RVA: 0x004587D0 File Offset: 0x004569D0
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BAE1 RID: 47841 RVA: 0x004587E4 File Offset: 0x004569E4
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return default(TSource);
		}

		// Token: 0x0600BAE2 RID: 47842 RVA: 0x00458838 File Offset: 0x00456A38
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BAE3 RID: 47843 RVA: 0x00458849 File Offset: 0x00456A49
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.GroupBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BAE4 RID: 47844 RVA: 0x00458853 File Offset: 0x00456A53
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return source.CreateGroupByIterator<TSource, TKey>(keySelector, comparer);
		}

		// Token: 0x0600BAE5 RID: 47845 RVA: 0x00458864 File Offset: 0x00456A64
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<IGrouping<TKey, TSource>> CreateGroupByIterator<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, List<TSource>> dictionary = new Dictionary<TKey, List<TSource>>(comparer);
			List<TSource> nullList = new List<TSource>();
			int counter = 0;
			int nullCounter = -1;
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				if (tkey == null)
				{
					nullList.Add(tsource);
					if (nullCounter == -1)
					{
						nullCounter = counter;
						int num = counter;
						counter = num + 1;
					}
				}
				else
				{
					List<TSource> list;
					if (!dictionary.TryGetValue(tkey, out list))
					{
						list = new List<TSource>();
						dictionary.Add(tkey, list);
						int num = counter;
						counter = num + 1;
					}
					list.Add(tsource);
				}
			}
			counter = 0;
			foreach (KeyValuePair<TKey, List<TSource>> group in dictionary)
			{
				int num;
				if (counter == nullCounter)
				{
					yield return new Grouping<TKey, TSource>(default(TKey), nullList);
					num = counter;
					counter = num + 1;
				}
				yield return new Grouping<TKey, TSource>(group.Key, group.Value);
				num = counter;
				counter = num + 1;
				group = default(KeyValuePair<TKey, List<TSource>>);
			}
			Dictionary<TKey, List<TSource>>.Enumerator enumerator2 = default(Dictionary<TKey, List<TSource>>.Enumerator);
			if (counter == nullCounter)
			{
				yield return new Grouping<TKey, TSource>(default(TKey), nullList);
				int num = counter;
				counter = num + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BAE6 RID: 47846 RVA: 0x00458882 File Offset: 0x00456A82
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BAE7 RID: 47847 RVA: 0x0045888D File Offset: 0x00456A8D
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
		}

		// Token: 0x0600BAE8 RID: 47848 RVA: 0x004588A0 File Offset: 0x00456AA0
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<IGrouping<TKey, TElement>> CreateGroupByIterator<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Dictionary<TKey, List<TElement>> dictionary = new Dictionary<TKey, List<TElement>>(comparer);
			List<TElement> nullList = new List<TElement>();
			int counter = 0;
			int nullCounter = -1;
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				TElement telement = elementSelector(tsource);
				if (tkey == null)
				{
					nullList.Add(telement);
					if (nullCounter == -1)
					{
						nullCounter = counter;
						int num = counter;
						counter = num + 1;
					}
				}
				else
				{
					List<TElement> list;
					if (!dictionary.TryGetValue(tkey, out list))
					{
						list = new List<TElement>();
						dictionary.Add(tkey, list);
						int num = counter;
						counter = num + 1;
					}
					list.Add(telement);
				}
			}
			counter = 0;
			foreach (KeyValuePair<TKey, List<TElement>> group in dictionary)
			{
				int num;
				if (counter == nullCounter)
				{
					yield return new Grouping<TKey, TElement>(default(TKey), nullList);
					num = counter;
					counter = num + 1;
				}
				yield return new Grouping<TKey, TElement>(group.Key, group.Value);
				num = counter;
				counter = num + 1;
				group = default(KeyValuePair<TKey, List<TElement>>);
			}
			Dictionary<TKey, List<TElement>>.Enumerator enumerator2 = default(Dictionary<TKey, List<TElement>>.Enumerator);
			if (counter == nullCounter)
			{
				yield return new Grouping<TKey, TElement>(default(TKey), nullList);
				int num = counter;
				counter = num + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BAE9 RID: 47849 RVA: 0x004588C5 File Offset: 0x00456AC5
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, null);
		}

		// Token: 0x0600BAEA RID: 47850 RVA: 0x004588D1 File Offset: 0x00456AD1
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.GroupBySelectors(source, keySelector, elementSelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600BAEB RID: 47851 RVA: 0x004588E7 File Offset: 0x00456AE7
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupByIterator<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			IEnumerable<IGrouping<TKey, TElement>> enumerable = source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
			foreach (IGrouping<TKey, TElement> grouping in enumerable)
			{
				yield return resultSelector(grouping.Key, grouping);
			}
			IEnumerator<IGrouping<TKey, TElement>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAEC RID: 47852 RVA: 0x00458914 File Offset: 0x00456B14
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TResult>(keySelector, resultSelector, null);
		}

		// Token: 0x0600BAED RID: 47853 RVA: 0x0045891F File Offset: 0x00456B1F
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyResultSelectors(source, keySelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TResult>(keySelector, resultSelector, comparer);
		}

		// Token: 0x0600BAEE RID: 47854 RVA: 0x00458932 File Offset: 0x00456B32
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupByIterator<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			IEnumerable<IGrouping<TKey, TSource>> enumerable = source.GroupBy<TSource, TKey>(keySelector, comparer);
			foreach (IGrouping<TKey, TSource> grouping in enumerable)
			{
				yield return resultSelector(grouping.Key, grouping);
			}
			IEnumerator<IGrouping<TKey, TSource>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAEF RID: 47855 RVA: 0x00458957 File Offset: 0x00456B57
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			return outer.GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BAF0 RID: 47856 RVA: 0x00458965 File Offset: 0x00456B65
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BAF1 RID: 47857 RVA: 0x0045898A File Offset: 0x00456B8A
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			ILookup<TKey, TInner> innerKeys = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
			foreach (TOuter touter in outer)
			{
				TKey tkey = outerKeySelector(touter);
				if (tkey != null && innerKeys.Contains(tkey))
				{
					yield return resultSelector(touter, innerKeys[tkey]);
				}
				else
				{
					yield return resultSelector(touter, Enumerable.Empty<TInner>());
				}
			}
			IEnumerator<TOuter> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAF2 RID: 47858 RVA: 0x004589BF File Offset: 0x00456BBF
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Intersect<TSource>(second, null);
		}

		// Token: 0x0600BAF3 RID: 47859 RVA: 0x004589C9 File Offset: 0x00456BC9
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateIntersectIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BAF4 RID: 47860 RVA: 0x004589E4 File Offset: 0x00456BE4
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateIntersectIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(second, comparer);
			foreach (TSource tsource in first)
			{
				if (items.Remove(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAF5 RID: 47861 RVA: 0x00458A02 File Offset: 0x00456C02
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600BAF6 RID: 47862 RVA: 0x00458A27 File Offset: 0x00456C27
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateJoinIterator<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			ILookup<TKey, TInner> innerKeys = inner.ToLookup<TInner, TKey>(innerKeySelector, comparer);
			foreach (TOuter element in outer)
			{
				TKey tkey = outerKeySelector(element);
				if (tkey != null && innerKeys.Contains(tkey))
				{
					foreach (TInner tinner in innerKeys[tkey])
					{
						yield return resultSelector(element, tinner);
					}
					IEnumerator<TInner> enumerator2 = null;
				}
				element = default(TOuter);
			}
			IEnumerator<TOuter> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BAF7 RID: 47863 RVA: 0x00458A5C File Offset: 0x00456C5C
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600BAF8 RID: 47864 RVA: 0x00458A6C File Offset: 0x00456C6C
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			bool flag = true;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (predicate(tsource2))
				{
					tsource = tsource2;
					flag = false;
				}
			}
			if (!flag)
			{
				return tsource;
			}
			if (fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BAF9 RID: 47865 RVA: 0x00458AD4 File Offset: 0x00456CD4
		public static TSource Last<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null && collection.Count == 0)
			{
				throw Enumerable.EmptySequence();
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				return list[list.Count - 1];
			}
			bool flag = true;
			TSource tsource = default(TSource);
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					tsource = enumerator.Current;
					flag = false;
				}
			}
			if (!flag)
			{
				return tsource;
			}
			throw Enumerable.EmptySequence();
		}

		// Token: 0x0600BAFA RID: 47866 RVA: 0x00458B68 File Offset: 0x00456D68
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BAFB RID: 47867 RVA: 0x00458B7C File Offset: 0x00456D7C
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			IList<TSource> list = source as IList<TSource>;
			if (list == null)
			{
				TSource tsource = default(TSource);
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						tsource = enumerator.Current;
					}
				}
				return tsource;
			}
			if (list.Count <= 0)
			{
				return default(TSource);
			}
			return list[list.Count - 1];
		}

		// Token: 0x0600BAFC RID: 47868 RVA: 0x00458C04 File Offset: 0x00456E04
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BAFD RID: 47869 RVA: 0x00458C18 File Offset: 0x00456E18
		public static long LongCount<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return (long)array.Length;
			}
			long num = 0L;
			using (IEnumerator<TSource> enumerator = source.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num += 1L;
				}
			}
			return num;
		}

		// Token: 0x0600BAFE RID: 47870 RVA: 0x00458C70 File Offset: 0x00456E70
		public static long LongCount<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndSelector(source, predicate);
			long num = 0L;
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					num += 1L;
				}
			}
			return num;
		}

		// Token: 0x0600BAFF RID: 47871 RVA: 0x00458CCC File Offset: 0x00456ECC
		public static int Max(this IEnumerable<int> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MinValue;
			foreach (int num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB00 RID: 47872 RVA: 0x00458D30 File Offset: 0x00456F30
		public static long Max(this IEnumerable<long> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MinValue;
			foreach (long num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB01 RID: 47873 RVA: 0x00458D98 File Offset: 0x00456F98
		public static double Max(this IEnumerable<double> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MinValue;
			foreach (double num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB02 RID: 47874 RVA: 0x00458E00 File Offset: 0x00457000
		public static float Max(this IEnumerable<float> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MinValue;
			foreach (float num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB03 RID: 47875 RVA: 0x00458E64 File Offset: 0x00457064
		public static decimal Max(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (decimal num2 in source)
			{
				num = Math.Max(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB04 RID: 47876 RVA: 0x00458ECC File Offset: 0x004570CC
		public static int? Max(this IEnumerable<int?> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MinValue;
			foreach (int? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x0600BB05 RID: 47877 RVA: 0x00458F48 File Offset: 0x00457148
		public static long? Max(this IEnumerable<long?> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MinValue;
			foreach (long? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new long?(num);
		}

		// Token: 0x0600BB06 RID: 47878 RVA: 0x00458FC8 File Offset: 0x004571C8
		public static double? Max(this IEnumerable<double?> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MinValue;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x0600BB07 RID: 47879 RVA: 0x00459048 File Offset: 0x00457248
		public static float? Max(this IEnumerable<float?> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MinValue;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new float?(num);
		}

		// Token: 0x0600BB08 RID: 47880 RVA: 0x004590C4 File Offset: 0x004572C4
		public static decimal? Max(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Max(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new decimal?(num);
		}

		// Token: 0x0600BB09 RID: 47881 RVA: 0x00459148 File Offset: 0x00457348
		public static TSource Max<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (default(TSource) == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						if (tsource2 != null && (tsource == null || @default.Compare(tsource2, tsource) > 0))
						{
							tsource = tsource2;
						}
					}
					return tsource;
				}
			}
			bool flag = true;
			foreach (TSource tsource3 in source)
			{
				if (flag)
				{
					tsource = tsource3;
					flag = false;
				}
				else if (@default.Compare(tsource3, tsource) > 0)
				{
					tsource = tsource3;
				}
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return tsource;
		}

		// Token: 0x0600BB0A RID: 47882 RVA: 0x00459228 File Offset: 0x00457428
		public static int Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int num = int.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB0B RID: 47883 RVA: 0x00459294 File Offset: 0x00457494
		public static long Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long num = long.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB0C RID: 47884 RVA: 0x00459304 File Offset: 0x00457504
		public static double Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double num = double.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB0D RID: 47885 RVA: 0x00459374 File Offset: 0x00457574
		public static float Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float num = float.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB0E RID: 47886 RVA: 0x004593E0 File Offset: 0x004575E0
		public static decimal Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal num = decimal.MinValue;
			foreach (TSource tsource in source)
			{
				num = Math.Max(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB0F RID: 47887 RVA: 0x00459450 File Offset: 0x00457650
		[PublicizedFrom(EAccessModifier.Private)]
		public static U Iterate<T, U>(IEnumerable<T> source, U initValue, Func<T, U, U> selector)
		{
			bool flag = true;
			foreach (T t in source)
			{
				initValue = selector(t, initValue);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return initValue;
		}

		// Token: 0x0600BB10 RID: 47888 RVA: 0x004594AC File Offset: 0x004576AC
		public static int? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int? num = null;
			foreach (TSource tsource in source)
			{
				int? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					int? num3 = num2;
					int? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB11 RID: 47889 RVA: 0x00459554 File Offset: 0x00457754
		public static long? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long? num = null;
			foreach (TSource tsource in source)
			{
				long? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					long? num3 = num2;
					long? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB12 RID: 47890 RVA: 0x004595FC File Offset: 0x004577FC
		public static double? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double? num = null;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					double? num3 = num2;
					double? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB13 RID: 47891 RVA: 0x004596A4 File Offset: 0x004578A4
		public static float? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float? num = null;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					float? num3 = num2;
					float? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB14 RID: 47892 RVA: 0x0045974C File Offset: 0x0045794C
		public static decimal? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal? num = null;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					decimal? num3 = num2;
					decimal? num4 = num;
					if ((num3.GetValueOrDefault() > num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB15 RID: 47893 RVA: 0x004597F8 File Offset: 0x004579F8
		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Max<TResult>();
		}

		// Token: 0x0600BB16 RID: 47894 RVA: 0x00459810 File Offset: 0x00457A10
		public static int Min(this IEnumerable<int> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MaxValue;
			foreach (int num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB17 RID: 47895 RVA: 0x00459874 File Offset: 0x00457A74
		public static long Min(this IEnumerable<long> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MaxValue;
			foreach (long num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB18 RID: 47896 RVA: 0x004598DC File Offset: 0x00457ADC
		public static double Min(this IEnumerable<double> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MaxValue;
			foreach (double num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB19 RID: 47897 RVA: 0x00459944 File Offset: 0x00457B44
		public static float Min(this IEnumerable<float> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MaxValue;
			foreach (float num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB1A RID: 47898 RVA: 0x004599A8 File Offset: 0x00457BA8
		public static decimal Min(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (decimal num2 in source)
			{
				num = Math.Min(num2, num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return num;
		}

		// Token: 0x0600BB1B RID: 47899 RVA: 0x00459A10 File Offset: 0x00457C10
		public static int? Min(this IEnumerable<int?> source)
		{
			Check.Source(source);
			bool flag = true;
			int num = int.MaxValue;
			foreach (int? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new int?(num);
		}

		// Token: 0x0600BB1C RID: 47900 RVA: 0x00459A8C File Offset: 0x00457C8C
		public static long? Min(this IEnumerable<long?> source)
		{
			Check.Source(source);
			bool flag = true;
			long num = long.MaxValue;
			foreach (long? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new long?(num);
		}

		// Token: 0x0600BB1D RID: 47901 RVA: 0x00459B0C File Offset: 0x00457D0C
		public static double? Min(this IEnumerable<double?> source)
		{
			Check.Source(source);
			bool flag = true;
			double num = double.MaxValue;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new double?(num);
		}

		// Token: 0x0600BB1E RID: 47902 RVA: 0x00459B8C File Offset: 0x00457D8C
		public static float? Min(this IEnumerable<float?> source)
		{
			Check.Source(source);
			bool flag = true;
			float num = float.MaxValue;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new float?(num);
		}

		// Token: 0x0600BB1F RID: 47903 RVA: 0x00459C08 File Offset: 0x00457E08
		public static decimal? Min(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num = Math.Min(num2.Value, num);
					flag = false;
				}
			}
			if (flag)
			{
				return null;
			}
			return new decimal?(num);
		}

		// Token: 0x0600BB20 RID: 47904 RVA: 0x00459C8C File Offset: 0x00457E8C
		public static TSource Min<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			Comparer<TSource> @default = Comparer<TSource>.Default;
			TSource tsource = default(TSource);
			if (default(TSource) == null)
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TSource tsource2 = enumerator.Current;
						if (tsource2 != null && (tsource == null || @default.Compare(tsource2, tsource) < 0))
						{
							tsource = tsource2;
						}
					}
					return tsource;
				}
			}
			bool flag = true;
			foreach (TSource tsource3 in source)
			{
				if (flag)
				{
					tsource = tsource3;
					flag = false;
				}
				else if (@default.Compare(tsource3, tsource) < 0)
				{
					tsource = tsource3;
				}
			}
			if (flag)
			{
				throw Enumerable.EmptySequence();
			}
			return tsource;
		}

		// Token: 0x0600BB21 RID: 47905 RVA: 0x00459D6C File Offset: 0x00457F6C
		public static int Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int num = int.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB22 RID: 47906 RVA: 0x00459DD8 File Offset: 0x00457FD8
		public static long Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long num = long.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB23 RID: 47907 RVA: 0x00459E48 File Offset: 0x00458048
		public static double Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double num = double.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB24 RID: 47908 RVA: 0x00459EB8 File Offset: 0x004580B8
		public static float Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float num = float.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB25 RID: 47909 RVA: 0x00459F24 File Offset: 0x00458124
		public static decimal Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal num = decimal.MaxValue;
			foreach (TSource tsource in source)
			{
				num = Math.Min(selector(tsource), num);
				flag = false;
			}
			if (flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return num;
		}

		// Token: 0x0600BB26 RID: 47910 RVA: 0x00459F94 File Offset: 0x00458194
		public static int? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			int? num = null;
			foreach (TSource tsource in source)
			{
				int? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					int? num3 = num2;
					int? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB27 RID: 47911 RVA: 0x0045A03C File Offset: 0x0045823C
		public static long? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			long? num = null;
			foreach (TSource tsource in source)
			{
				long? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					long? num3 = num2;
					long? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB28 RID: 47912 RVA: 0x0045A0E4 File Offset: 0x004582E4
		public static float? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			float? num = null;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					float? num3 = num2;
					float? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB29 RID: 47913 RVA: 0x0045A18C File Offset: 0x0045838C
		public static double? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			double? num = null;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					double? num3 = num2;
					double? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB2A RID: 47914 RVA: 0x0045A234 File Offset: 0x00458434
		public static decimal? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			bool flag = true;
			decimal? num = null;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num == null)
				{
					num = num2;
				}
				else
				{
					decimal? num3 = num2;
					decimal? num4 = num;
					if ((num3.GetValueOrDefault() < num4.GetValueOrDefault()) & ((num3 != null) & (num4 != null)))
					{
						num = num2;
					}
				}
				flag = false;
			}
			if (flag)
			{
				return null;
			}
			return num;
		}

		// Token: 0x0600BB2B RID: 47915 RVA: 0x0045A2E0 File Offset: 0x004584E0
		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Min<TResult>();
		}

		// Token: 0x0600BB2C RID: 47916 RVA: 0x0045A2F5 File Offset: 0x004584F5
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			return Enumerable.CreateOfTypeIterator<TResult>(source);
		}

		// Token: 0x0600BB2D RID: 47917 RVA: 0x0045A303 File Offset: 0x00458503
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateOfTypeIterator<TResult>(IEnumerable source)
		{
			foreach (object obj in source)
			{
				if (obj is TResult)
				{
					yield return (TResult)((object)obj);
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB2E RID: 47918 RVA: 0x0045A313 File Offset: 0x00458513
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB2F RID: 47919 RVA: 0x0045A31D File Offset: 0x0045851D
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Ascending);
		}

		// Token: 0x0600BB30 RID: 47920 RVA: 0x0045A32F File Offset: 0x0045852F
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB31 RID: 47921 RVA: 0x0045A339 File Offset: 0x00458539
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Descending);
		}

		// Token: 0x0600BB32 RID: 47922 RVA: 0x0045A34B File Offset: 0x0045854B
		public static IEnumerable<int> Range(int start, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if ((long)start + (long)count - 1L > 2147483647L)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRangeIterator(start, count);
		}

		// Token: 0x0600BB33 RID: 47923 RVA: 0x0045A379 File Offset: 0x00458579
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<int> CreateRangeIterator(int start, int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return start + i;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BB34 RID: 47924 RVA: 0x0045A390 File Offset: 0x00458590
		public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRepeatIterator<TResult>(element, count);
		}

		// Token: 0x0600BB35 RID: 47925 RVA: 0x0045A3A3 File Offset: 0x004585A3
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateRepeatIterator<TResult>(TResult element, int count)
		{
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return element;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BB36 RID: 47926 RVA: 0x0045A3BA File Offset: 0x004585BA
		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return Enumerable.CreateReverseIterator<TSource>(source);
		}

		// Token: 0x0600BB37 RID: 47927 RVA: 0x0045A3C8 File Offset: 0x004585C8
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateReverseIterator<TSource>(IEnumerable<TSource> source)
		{
			TSource[] array = source.ToArray<TSource>();
			int num;
			for (int i = array.Length - 1; i >= 0; i = num - 1)
			{
				yield return array[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600BB38 RID: 47928 RVA: 0x0045A3D8 File Offset: 0x004585D8
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BB39 RID: 47929 RVA: 0x0045A3E8 File Offset: 0x004585E8
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			foreach (TSource tsource in source)
			{
				yield return selector(tsource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB3A RID: 47930 RVA: 0x0045A3FF File Offset: 0x004585FF
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BB3B RID: 47931 RVA: 0x0045A40F File Offset: 0x0045860F
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				yield return selector(tsource, counter);
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB3C RID: 47932 RVA: 0x0045A426 File Offset: 0x00458626
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x0045A436 File Offset: 0x00458636
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			foreach (TSource tsource in source)
			{
				foreach (TResult tresult in selector(tsource))
				{
					yield return tresult;
				}
				IEnumerator<TResult> enumerator2 = null;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB3E RID: 47934 RVA: 0x0045A44D File Offset: 0x0045864D
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600BB3F RID: 47935 RVA: 0x0045A45D File Offset: 0x0045865D
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				foreach (TResult tresult in selector(tsource, counter))
				{
					yield return tresult;
				}
				IEnumerator<TResult> enumerator2 = null;
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB40 RID: 47936 RVA: 0x0045A474 File Offset: 0x00458674
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BB41 RID: 47937 RVA: 0x0045A486 File Offset: 0x00458686
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> selector)
		{
			foreach (TSource element in source)
			{
				foreach (TCollection tcollection in collectionSelector(element))
				{
					yield return selector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB42 RID: 47938 RVA: 0x0045A4A4 File Offset: 0x004586A4
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600BB43 RID: 47939 RVA: 0x0045A4B6 File Offset: 0x004586B6
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TResult> CreateSelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> selector)
		{
			int counter = 0;
			foreach (TSource element in source)
			{
				TSource tsource = element;
				int num = counter;
				counter = num + 1;
				foreach (TCollection tcollection in collectionSelector(tsource, num))
				{
					yield return selector(element, tcollection);
				}
				IEnumerator<TCollection> enumerator2 = null;
				element = default(TSource);
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB44 RID: 47940 RVA: 0x0045A4D4 File Offset: 0x004586D4
		[PublicizedFrom(EAccessModifier.Private)]
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Enumerable.Fallback fallback)
		{
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (predicate(tsource2))
				{
					if (flag)
					{
						throw Enumerable.MoreThanOneMatchingElement();
					}
					flag = true;
					tsource = tsource2;
				}
			}
			if (!flag && fallback == Enumerable.Fallback.Throw)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BB45 RID: 47941 RVA: 0x0045A544 File Offset: 0x00458744
		public static TSource Single<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (flag)
				{
					throw Enumerable.MoreThanOneElement();
				}
				flag = true;
				tsource = tsource2;
			}
			if (!flag)
			{
				throw Enumerable.NoMatchingElement();
			}
			return tsource;
		}

		// Token: 0x0600BB46 RID: 47942 RVA: 0x0045A5AC File Offset: 0x004587AC
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600BB47 RID: 47943 RVA: 0x0045A5C0 File Offset: 0x004587C0
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			bool flag = false;
			TSource tsource = default(TSource);
			foreach (TSource tsource2 in source)
			{
				if (flag)
				{
					throw Enumerable.MoreThanOneMatchingElement();
				}
				flag = true;
				tsource = tsource2;
			}
			return tsource;
		}

		// Token: 0x0600BB48 RID: 47944 RVA: 0x0045A620 File Offset: 0x00458820
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600BB49 RID: 47945 RVA: 0x0045A631 File Offset: 0x00458831
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateSkipIterator<TSource>(source, count);
		}

		// Token: 0x0600BB4A RID: 47946 RVA: 0x0045A640 File Offset: 0x00458840
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			IEnumerator<TSource> enumerator = source.GetEnumerator();
			try
			{
				do
				{
					int num = count;
					count = num - 1;
					if (num <= 0)
					{
						goto Block_5;
					}
				}
				while (enumerator.MoveNext());
				yield break;
				Block_5:
				while (enumerator.MoveNext())
				{
					TSource tsource = enumerator.Current;
					yield return tsource;
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BB4B RID: 47947 RVA: 0x0045A657 File Offset: 0x00458857
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB4C RID: 47948 RVA: 0x0045A667 File Offset: 0x00458867
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			bool yield = false;
			foreach (TSource tsource in source)
			{
				if (yield)
				{
					yield return tsource;
				}
				else if (!predicate(tsource))
				{
					yield return tsource;
					yield = true;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB4D RID: 47949 RVA: 0x0045A67E File Offset: 0x0045887E
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB4E RID: 47950 RVA: 0x0045A68E File Offset: 0x0045888E
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateSkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			bool yield = false;
			foreach (TSource tsource in source)
			{
				if (yield)
				{
					yield return tsource;
				}
				else if (!predicate(tsource, counter))
				{
					yield return tsource;
					yield = true;
				}
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB4F RID: 47951 RVA: 0x0045A6A8 File Offset: 0x004588A8
		public static int Sum(this IEnumerable<int> source)
		{
			Check.Source(source);
			int num = 0;
			checked
			{
				foreach (int num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		// Token: 0x0600BB50 RID: 47952 RVA: 0x0045A6F8 File Offset: 0x004588F8
		public static int? Sum(this IEnumerable<int?> source)
		{
			Check.Source(source);
			int num = 0;
			checked
			{
				foreach (int? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new int?(num);
			}
		}

		// Token: 0x0600BB51 RID: 47953 RVA: 0x0045A75C File Offset: 0x0045895C
		public static int Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
		{
			Check.SourceAndSelector(source, selector);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		// Token: 0x0600BB52 RID: 47954 RVA: 0x0045A7B4 File Offset: 0x004589B4
		public static int? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, int?> selector)
		{
			Check.SourceAndSelector(source, selector);
			int num = 0;
			checked
			{
				foreach (TSource tsource in source)
				{
					int? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new int?(num);
			}
		}

		// Token: 0x0600BB53 RID: 47955 RVA: 0x0045A820 File Offset: 0x00458A20
		public static long Sum(this IEnumerable<long> source)
		{
			Check.Source(source);
			long num = 0L;
			checked
			{
				foreach (long num2 in source)
				{
					num += num2;
				}
				return num;
			}
		}

		// Token: 0x0600BB54 RID: 47956 RVA: 0x0045A870 File Offset: 0x00458A70
		public static long? Sum(this IEnumerable<long?> source)
		{
			Check.Source(source);
			long num = 0L;
			checked
			{
				foreach (long? num2 in source)
				{
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new long?(num);
			}
		}

		// Token: 0x0600BB55 RID: 47957 RVA: 0x0045A8D4 File Offset: 0x00458AD4
		public static long Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					num += selector(tsource);
				}
				return num;
			}
		}

		// Token: 0x0600BB56 RID: 47958 RVA: 0x0045A92C File Offset: 0x00458B2C
		public static long? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, long?> selector)
		{
			Check.SourceAndSelector(source, selector);
			long num = 0L;
			checked
			{
				foreach (TSource tsource in source)
				{
					long? num2 = selector(tsource);
					if (num2 != null)
					{
						num += num2.Value;
					}
				}
				return new long?(num);
			}
		}

		// Token: 0x0600BB57 RID: 47959 RVA: 0x0045A998 File Offset: 0x00458B98
		public static double Sum(this IEnumerable<double> source)
		{
			Check.Source(source);
			double num = 0.0;
			foreach (double num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BB58 RID: 47960 RVA: 0x0045A9F0 File Offset: 0x00458BF0
		public static double? Sum(this IEnumerable<double?> source)
		{
			Check.Source(source);
			double num = 0.0;
			foreach (double? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new double?(num);
		}

		// Token: 0x0600BB59 RID: 47961 RVA: 0x0045AA5C File Offset: 0x00458C5C
		public static double Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BB5A RID: 47962 RVA: 0x0045AABC File Offset: 0x00458CBC
		public static double? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, double?> selector)
		{
			Check.SourceAndSelector(source, selector);
			double num = 0.0;
			foreach (TSource tsource in source)
			{
				double? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new double?(num);
		}

		// Token: 0x0600BB5B RID: 47963 RVA: 0x0045AB30 File Offset: 0x00458D30
		public static float Sum(this IEnumerable<float> source)
		{
			Check.Source(source);
			float num = 0f;
			foreach (float num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BB5C RID: 47964 RVA: 0x0045AB84 File Offset: 0x00458D84
		public static float? Sum(this IEnumerable<float?> source)
		{
			Check.Source(source);
			float num = 0f;
			foreach (float? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new float?(num);
		}

		// Token: 0x0600BB5D RID: 47965 RVA: 0x0045ABEC File Offset: 0x00458DEC
		public static float Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BB5E RID: 47966 RVA: 0x0045AC48 File Offset: 0x00458E48
		public static float? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, float?> selector)
		{
			Check.SourceAndSelector(source, selector);
			float num = 0f;
			foreach (TSource tsource in source)
			{
				float? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new float?(num);
		}

		// Token: 0x0600BB5F RID: 47967 RVA: 0x0045ACB8 File Offset: 0x00458EB8
		public static decimal Sum(this IEnumerable<decimal> source)
		{
			Check.Source(source);
			decimal num = 0m;
			foreach (decimal num2 in source)
			{
				num += num2;
			}
			return num;
		}

		// Token: 0x0600BB60 RID: 47968 RVA: 0x0045AD10 File Offset: 0x00458F10
		public static decimal? Sum(this IEnumerable<decimal?> source)
		{
			Check.Source(source);
			decimal num = 0m;
			foreach (decimal? num2 in source)
			{
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new decimal?(num);
		}

		// Token: 0x0600BB61 RID: 47969 RVA: 0x0045AD7C File Offset: 0x00458F7C
		public static decimal Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				num += selector(tsource);
			}
			return num;
		}

		// Token: 0x0600BB62 RID: 47970 RVA: 0x0045ADDC File Offset: 0x00458FDC
		public static decimal? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, decimal?> selector)
		{
			Check.SourceAndSelector(source, selector);
			decimal num = 0m;
			foreach (TSource tsource in source)
			{
				decimal? num2 = selector(tsource);
				if (num2 != null)
				{
					num += num2.Value;
				}
			}
			return new decimal?(num);
		}

		// Token: 0x0600BB63 RID: 47971 RVA: 0x0045AE54 File Offset: 0x00459054
		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateTakeIterator<TSource>(source, count);
		}

		// Token: 0x0600BB64 RID: 47972 RVA: 0x0045AE63 File Offset: 0x00459063
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeIterator<TSource>(IEnumerable<TSource> source, int count)
		{
			if (count <= 0)
			{
				yield break;
			}
			int counter = 0;
			foreach (TSource tsource in source)
			{
				yield return tsource;
				int num = counter + 1;
				counter = num;
				if (num == count)
				{
					yield break;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB65 RID: 47973 RVA: 0x0045AE7A File Offset: 0x0045907A
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB66 RID: 47974 RVA: 0x0045AE8A File Offset: 0x0045908A
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource))
				{
					yield break;
				}
				yield return tsource;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB67 RID: 47975 RVA: 0x0045AEA1 File Offset: 0x004590A1
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB68 RID: 47976 RVA: 0x0045AEB1 File Offset: 0x004590B1
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateTakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				if (!predicate(tsource, counter))
				{
					yield break;
				}
				yield return tsource;
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB69 RID: 47977 RVA: 0x0045AEC8 File Offset: 0x004590C8
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB6A RID: 47978 RVA: 0x0045AED2 File Offset: 0x004590D2
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x0600BB6B RID: 47979 RVA: 0x0045AEE9 File Offset: 0x004590E9
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB6C RID: 47980 RVA: 0x0045AEF3 File Offset: 0x004590F3
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x0600BB6D RID: 47981 RVA: 0x0045AF0C File Offset: 0x0045910C
		public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			ICollection<TSource> collection = source as ICollection<TSource>;
			TSource[] array;
			if (collection == null)
			{
				int num = 0;
				array = new TSource[0];
				foreach (TSource tsource in source)
				{
					if (num == array.Length)
					{
						if (num == 0)
						{
							array = new TSource[4];
						}
						else
						{
							Array.Resize<TSource>(ref array, num * 2);
						}
					}
					array[num++] = tsource;
				}
				if (num != array.Length)
				{
					Array.Resize<TSource>(ref array, num);
				}
				return array;
			}
			if (collection.Count == 0)
			{
				return new TSource[0];
			}
			array = new TSource[collection.Count];
			collection.CopyTo(array, 0);
			return array;
		}

		// Token: 0x0600BB6E RID: 47982 RVA: 0x0045AFC4 File Offset: 0x004591C4
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BB6F RID: 47983 RVA: 0x0045AFD0 File Offset: 0x004591D0
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>(comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), elementSelector(tsource));
			}
			return dictionary;
		}

		// Token: 0x0600BB70 RID: 47984 RVA: 0x0045B040 File Offset: 0x00459240
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToDictionary<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB71 RID: 47985 RVA: 0x0045B04C File Offset: 0x0045924C
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>(comparer);
			foreach (TSource tsource in source)
			{
				dictionary.Add(keySelector(tsource), tsource);
			}
			return dictionary;
		}

		// Token: 0x0600BB72 RID: 47986 RVA: 0x0045B0B4 File Offset: 0x004592B4
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return new List<TSource>(source);
		}

		// Token: 0x0600BB73 RID: 47987 RVA: 0x0045B0C2 File Offset: 0x004592C2
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToLookup<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600BB74 RID: 47988 RVA: 0x0045B0CC File Offset: 0x004592CC
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			List<TSource> list = null;
			Dictionary<TKey, List<TSource>> dictionary = new Dictionary<TKey, List<TSource>>(comparer ?? EqualityComparer<TKey>.Default);
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				List<TSource> list2;
				if (tkey == null)
				{
					if (list == null)
					{
						list = new List<TSource>();
					}
					list2 = list;
				}
				else if (!dictionary.TryGetValue(tkey, out list2))
				{
					list2 = new List<TSource>();
					dictionary.Add(tkey, list2);
				}
				list2.Add(tsource);
			}
			return new Lookup<TKey, TSource>(dictionary, list);
		}

		// Token: 0x0600BB75 RID: 47989 RVA: 0x0045B174 File Offset: 0x00459374
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600BB76 RID: 47990 RVA: 0x0045B180 File Offset: 0x00459380
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			List<TElement> list = null;
			Dictionary<TKey, List<TElement>> dictionary = new Dictionary<TKey, List<TElement>>(comparer ?? EqualityComparer<TKey>.Default);
			foreach (TSource tsource in source)
			{
				TKey tkey = keySelector(tsource);
				List<TElement> list2;
				if (tkey == null)
				{
					if (list == null)
					{
						list = new List<TElement>();
					}
					list2 = list;
				}
				else if (!dictionary.TryGetValue(tkey, out list2))
				{
					list2 = new List<TElement>();
					dictionary.Add(tkey, list2);
				}
				list2.Add(elementSelector(tsource));
			}
			return new Lookup<TKey, TElement>(dictionary, list);
		}

		// Token: 0x0600BB77 RID: 47991 RVA: 0x0045B230 File Offset: 0x00459430
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.SequenceEqual<TSource>(second, null);
		}

		// Token: 0x0600BB78 RID: 47992 RVA: 0x0045B23C File Offset: 0x0045943C
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			bool flag;
			using (IEnumerator<TSource> enumerator = first.GetEnumerator())
			{
				using (IEnumerator<TSource> enumerator2 = second.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator2.MoveNext())
						{
							return false;
						}
						if (!comparer.Equals(enumerator.Current, enumerator2.Current))
						{
							return false;
						}
					}
					flag = !enumerator2.MoveNext();
				}
			}
			return flag;
		}

		// Token: 0x0600BB79 RID: 47993 RVA: 0x0045B2D4 File Offset: 0x004594D4
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return first.Union<TSource>(second, null);
		}

		// Token: 0x0600BB7A RID: 47994 RVA: 0x0045B2E5 File Offset: 0x004594E5
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateUnionIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600BB7B RID: 47995 RVA: 0x0045B300 File Offset: 0x00459500
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateUnionIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			HashSet<TSource> items = new HashSet<TSource>(comparer);
			foreach (TSource tsource in first)
			{
				if (!items.Contains(tsource))
				{
					items.Add(tsource);
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			foreach (TSource tsource2 in second)
			{
				if (!items.Contains(tsource2))
				{
					items.Add(tsource2);
					yield return tsource2;
				}
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB7C RID: 47996 RVA: 0x0045B320 File Offset: 0x00459520
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return Enumerable.CreateWhereIterator<TSource>(array, predicate);
			}
			return Enumerable.CreateWhereIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB7D RID: 47997 RVA: 0x0045B34D File Offset: 0x0045954D
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			foreach (TSource tsource in source)
			{
				if (predicate(tsource))
				{
					yield return tsource;
				}
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB7E RID: 47998 RVA: 0x0045B364 File Offset: 0x00459564
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(TSource[] source, Func<TSource, bool> predicate)
		{
			int num;
			for (int i = 0; i < source.Length; i = num)
			{
				TSource tsource = source[i];
				if (predicate(tsource))
				{
					yield return tsource;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600BB7F RID: 47999 RVA: 0x0045B37C File Offset: 0x0045957C
		public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			TSource[] array = source as TSource[];
			if (array != null)
			{
				return Enumerable.CreateWhereIterator<TSource>(array, predicate);
			}
			return Enumerable.CreateWhereIterator<TSource>(source, predicate);
		}

		// Token: 0x0600BB80 RID: 48000 RVA: 0x0045B3A9 File Offset: 0x004595A9
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			int counter = 0;
			foreach (TSource tsource in source)
			{
				if (predicate(tsource, counter))
				{
					yield return tsource;
				}
				int num = counter;
				counter = num + 1;
			}
			IEnumerator<TSource> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600BB81 RID: 48001 RVA: 0x0045B3C0 File Offset: 0x004595C0
		[PublicizedFrom(EAccessModifier.Private)]
		public static IEnumerable<TSource> CreateWhereIterator<TSource>(TSource[] source, Func<TSource, int, bool> predicate)
		{
			int num;
			for (int i = 0; i < source.Length; i = num)
			{
				TSource tsource = source[i];
				if (predicate(tsource, i))
				{
					yield return tsource;
				}
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x0600BB82 RID: 48002 RVA: 0x0045B3D7 File Offset: 0x004595D7
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception EmptySequence()
		{
			return new InvalidOperationException("Sequence contains no elements");
		}

		// Token: 0x0600BB83 RID: 48003 RVA: 0x0045B3E3 File Offset: 0x004595E3
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception NoMatchingElement()
		{
			return new InvalidOperationException("Sequence contains no matching element");
		}

		// Token: 0x0600BB84 RID: 48004 RVA: 0x0045B3EF File Offset: 0x004595EF
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneElement()
		{
			return new InvalidOperationException("Sequence contains more than one element");
		}

		// Token: 0x0600BB85 RID: 48005 RVA: 0x0045B3FB File Offset: 0x004595FB
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneMatchingElement()
		{
			return new InvalidOperationException("Sequence contains more than one matching element");
		}

		// Token: 0x0200178E RID: 6030
		[PublicizedFrom(EAccessModifier.Private)]
		public enum Fallback
		{
			// Token: 0x04008BE3 RID: 35811
			Default,
			// Token: 0x04008BE4 RID: 35812
			Throw
		}
	}
}
