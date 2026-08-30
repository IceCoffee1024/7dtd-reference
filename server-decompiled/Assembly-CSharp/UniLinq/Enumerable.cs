using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001767 RID: 5991
	public static class Enumerable
	{
		// Token: 0x0600B92A RID: 47402 RVA: 0x004519E8 File Offset: 0x0044FBE8
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

		// Token: 0x0600B92B RID: 47403 RVA: 0x00451A50 File Offset: 0x0044FC50
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

		// Token: 0x0600B92C RID: 47404 RVA: 0x00451AA4 File Offset: 0x0044FCA4
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

		// Token: 0x0600B92D RID: 47405 RVA: 0x00451B0C File Offset: 0x0044FD0C
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

		// Token: 0x0600B92E RID: 47406 RVA: 0x00451B64 File Offset: 0x0044FD64
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

		// Token: 0x0600B92F RID: 47407 RVA: 0x00451BB8 File Offset: 0x0044FDB8
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

		// Token: 0x0600B930 RID: 47408 RVA: 0x001290F5 File Offset: 0x001272F5
		public static IEnumerable<TSource> AsEnumerable<TSource>(this IEnumerable<TSource> source)
		{
			return source;
		}

		// Token: 0x0600B931 RID: 47409 RVA: 0x00451C10 File Offset: 0x0044FE10
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

		// Token: 0x0600B932 RID: 47410 RVA: 0x00451C74 File Offset: 0x0044FE74
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

		// Token: 0x0600B933 RID: 47411 RVA: 0x00451CD8 File Offset: 0x0044FED8
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

		// Token: 0x0600B934 RID: 47412 RVA: 0x00451D44 File Offset: 0x0044FF44
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

		// Token: 0x0600B935 RID: 47413 RVA: 0x00451DAC File Offset: 0x0044FFAC
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

		// Token: 0x0600B936 RID: 47414 RVA: 0x00451E20 File Offset: 0x00450020
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

		// Token: 0x0600B937 RID: 47415 RVA: 0x00451EAC File Offset: 0x004500AC
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

		// Token: 0x0600B938 RID: 47416 RVA: 0x00451F2C File Offset: 0x0045012C
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

		// Token: 0x0600B939 RID: 47417 RVA: 0x00451FAC File Offset: 0x004501AC
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

		// Token: 0x0600B93A RID: 47418 RVA: 0x00452030 File Offset: 0x00450230
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

		// Token: 0x0600B93B RID: 47419 RVA: 0x004520C0 File Offset: 0x004502C0
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

		// Token: 0x0600B93C RID: 47420 RVA: 0x00452140 File Offset: 0x00450340
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

		// Token: 0x0600B93D RID: 47421 RVA: 0x004521AC File Offset: 0x004503AC
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

		// Token: 0x0600B93E RID: 47422 RVA: 0x00452234 File Offset: 0x00450434
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

		// Token: 0x0600B93F RID: 47423 RVA: 0x004522A0 File Offset: 0x004504A0
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

		// Token: 0x0600B940 RID: 47424 RVA: 0x00452328 File Offset: 0x00450528
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

		// Token: 0x0600B941 RID: 47425 RVA: 0x0045239C File Offset: 0x0045059C
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

		// Token: 0x0600B942 RID: 47426 RVA: 0x0045242C File Offset: 0x0045062C
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

		// Token: 0x0600B943 RID: 47427 RVA: 0x0045249C File Offset: 0x0045069C
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

		// Token: 0x0600B944 RID: 47428 RVA: 0x00452528 File Offset: 0x00450728
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

		// Token: 0x0600B945 RID: 47429 RVA: 0x004525A4 File Offset: 0x004507A4
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

		// Token: 0x0600B946 RID: 47430 RVA: 0x0045263C File Offset: 0x0045083C
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

		// Token: 0x0600B947 RID: 47431 RVA: 0x00452661 File Offset: 0x00450861
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

		// Token: 0x0600B948 RID: 47432 RVA: 0x00452671 File Offset: 0x00450871
		public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return Enumerable.CreateConcatIterator<TSource>(first, second);
		}

		// Token: 0x0600B949 RID: 47433 RVA: 0x00452681 File Offset: 0x00450881
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

		// Token: 0x0600B94A RID: 47434 RVA: 0x00452698 File Offset: 0x00450898
		public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
		{
			ICollection<TSource> collection = source as ICollection<TSource>;
			if (collection != null)
			{
				return collection.Contains(value);
			}
			return source.Contains(value, null);
		}

		// Token: 0x0600B94B RID: 47435 RVA: 0x004526C0 File Offset: 0x004508C0
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

		// Token: 0x0600B94C RID: 47436 RVA: 0x00452724 File Offset: 0x00450924
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

		// Token: 0x0600B94D RID: 47437 RVA: 0x0045277C File Offset: 0x0045097C
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

		// Token: 0x0600B94E RID: 47438 RVA: 0x004527D4 File Offset: 0x004509D4
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source)
		{
			return source.DefaultIfEmpty(default(TSource));
		}

		// Token: 0x0600B94F RID: 47439 RVA: 0x004527F0 File Offset: 0x004509F0
		public static IEnumerable<TSource> DefaultIfEmpty<TSource>(this IEnumerable<TSource> source, TSource defaultValue)
		{
			Check.Source(source);
			return Enumerable.CreateDefaultIfEmptyIterator<TSource>(source, defaultValue);
		}

		// Token: 0x0600B950 RID: 47440 RVA: 0x004527FF File Offset: 0x004509FF
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

		// Token: 0x0600B951 RID: 47441 RVA: 0x00452816 File Offset: 0x00450A16
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
		{
			return source.Distinct<TSource>(null);
		}

		// Token: 0x0600B952 RID: 47442 RVA: 0x0045281F File Offset: 0x00450A1F
		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			Check.Source(source);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateDistinctIterator<TSource>(source, comparer);
		}

		// Token: 0x0600B953 RID: 47443 RVA: 0x00452838 File Offset: 0x00450A38
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

		// Token: 0x0600B954 RID: 47444 RVA: 0x00452850 File Offset: 0x00450A50
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

		// Token: 0x0600B955 RID: 47445 RVA: 0x004528B8 File Offset: 0x00450AB8
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

		// Token: 0x0600B956 RID: 47446 RVA: 0x004528F0 File Offset: 0x00450AF0
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

		// Token: 0x0600B957 RID: 47447 RVA: 0x0045293E File Offset: 0x00450B3E
		public static IEnumerable<TResult> Empty<TResult>()
		{
			return new TResult[0];
		}

		// Token: 0x0600B958 RID: 47448 RVA: 0x00452946 File Offset: 0x00450B46
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Except<TSource>(second, null);
		}

		// Token: 0x0600B959 RID: 47449 RVA: 0x00452950 File Offset: 0x00450B50
		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateExceptIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600B95A RID: 47450 RVA: 0x0045296B File Offset: 0x00450B6B
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

		// Token: 0x0600B95B RID: 47451 RVA: 0x0045298C File Offset: 0x00450B8C
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

		// Token: 0x0600B95C RID: 47452 RVA: 0x004529F0 File Offset: 0x00450BF0
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

		// Token: 0x0600B95D RID: 47453 RVA: 0x00452A58 File Offset: 0x00450C58
		public static TSource First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600B95E RID: 47454 RVA: 0x00452A6C File Offset: 0x00450C6C
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

		// Token: 0x0600B95F RID: 47455 RVA: 0x00452AC0 File Offset: 0x00450CC0
		public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.First<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600B960 RID: 47456 RVA: 0x00452AD1 File Offset: 0x00450CD1
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.GroupBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B961 RID: 47457 RVA: 0x00452ADB File Offset: 0x00450CDB
		public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return source.CreateGroupByIterator<TSource, TKey>(keySelector, comparer);
		}

		// Token: 0x0600B962 RID: 47458 RVA: 0x00452AEC File Offset: 0x00450CEC
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

		// Token: 0x0600B963 RID: 47459 RVA: 0x00452B0A File Offset: 0x00450D0A
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.GroupBy<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600B964 RID: 47460 RVA: 0x00452B15 File Offset: 0x00450D15
		public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyElementSelectors(source, keySelector, elementSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement>(keySelector, elementSelector, comparer);
		}

		// Token: 0x0600B965 RID: 47461 RVA: 0x00452B28 File Offset: 0x00450D28
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

		// Token: 0x0600B966 RID: 47462 RVA: 0x00452B4D File Offset: 0x00450D4D
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, null);
		}

		// Token: 0x0600B967 RID: 47463 RVA: 0x00452B59 File Offset: 0x00450D59
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.GroupBySelectors(source, keySelector, elementSelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TElement, TResult>(keySelector, elementSelector, resultSelector, comparer);
		}

		// Token: 0x0600B968 RID: 47464 RVA: 0x00452B6F File Offset: 0x00450D6F
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

		// Token: 0x0600B969 RID: 47465 RVA: 0x00452B9C File Offset: 0x00450D9C
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
		{
			return source.GroupBy<TSource, TKey, TResult>(keySelector, resultSelector, null);
		}

		// Token: 0x0600B96A RID: 47466 RVA: 0x00452BA7 File Offset: 0x00450DA7
		public static IEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.SourceAndKeyResultSelectors(source, keySelector, resultSelector);
			return source.CreateGroupByIterator<TSource, TKey, TResult>(keySelector, resultSelector, comparer);
		}

		// Token: 0x0600B96B RID: 47467 RVA: 0x00452BBA File Offset: 0x00450DBA
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

		// Token: 0x0600B96C RID: 47468 RVA: 0x00452BDF File Offset: 0x00450DDF
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
		{
			return outer.GroupJoin<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600B96D RID: 47469 RVA: 0x00452BED File Offset: 0x00450DED
		public static IEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateGroupJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600B96E RID: 47470 RVA: 0x00452C12 File Offset: 0x00450E12
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

		// Token: 0x0600B96F RID: 47471 RVA: 0x00452C47 File Offset: 0x00450E47
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.Intersect<TSource>(second, null);
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x00452C51 File Offset: 0x00450E51
		public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateIntersectIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x00452C6C File Offset: 0x00450E6C
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

		// Token: 0x0600B972 RID: 47474 RVA: 0x00452C8A File Offset: 0x00450E8A
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			Check.JoinSelectors(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
			if (comparer == null)
			{
				comparer = EqualityComparer<TKey>.Default;
			}
			return outer.CreateJoinIterator<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
		}

		// Token: 0x0600B973 RID: 47475 RVA: 0x00452CAF File Offset: 0x00450EAF
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

		// Token: 0x0600B974 RID: 47476 RVA: 0x00452CE4 File Offset: 0x00450EE4
		public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
		{
			return outer.Join<TOuter, TInner, TKey, TResult>(inner, outerKeySelector, innerKeySelector, resultSelector, null);
		}

		// Token: 0x0600B975 RID: 47477 RVA: 0x00452CF4 File Offset: 0x00450EF4
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

		// Token: 0x0600B976 RID: 47478 RVA: 0x00452D5C File Offset: 0x00450F5C
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

		// Token: 0x0600B977 RID: 47479 RVA: 0x00452DF0 File Offset: 0x00450FF0
		public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600B978 RID: 47480 RVA: 0x00452E04 File Offset: 0x00451004
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

		// Token: 0x0600B979 RID: 47481 RVA: 0x00452E8C File Offset: 0x0045108C
		public static TSource LastOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Last<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600B97A RID: 47482 RVA: 0x00452EA0 File Offset: 0x004510A0
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

		// Token: 0x0600B97B RID: 47483 RVA: 0x00452EF8 File Offset: 0x004510F8
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

		// Token: 0x0600B97C RID: 47484 RVA: 0x00452F54 File Offset: 0x00451154
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

		// Token: 0x0600B97D RID: 47485 RVA: 0x00452FB8 File Offset: 0x004511B8
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

		// Token: 0x0600B97E RID: 47486 RVA: 0x00453020 File Offset: 0x00451220
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

		// Token: 0x0600B97F RID: 47487 RVA: 0x00453088 File Offset: 0x00451288
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

		// Token: 0x0600B980 RID: 47488 RVA: 0x004530EC File Offset: 0x004512EC
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

		// Token: 0x0600B981 RID: 47489 RVA: 0x00453154 File Offset: 0x00451354
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

		// Token: 0x0600B982 RID: 47490 RVA: 0x004531D0 File Offset: 0x004513D0
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

		// Token: 0x0600B983 RID: 47491 RVA: 0x00453250 File Offset: 0x00451450
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

		// Token: 0x0600B984 RID: 47492 RVA: 0x004532D0 File Offset: 0x004514D0
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

		// Token: 0x0600B985 RID: 47493 RVA: 0x0045334C File Offset: 0x0045154C
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

		// Token: 0x0600B986 RID: 47494 RVA: 0x004533D0 File Offset: 0x004515D0
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

		// Token: 0x0600B987 RID: 47495 RVA: 0x004534B0 File Offset: 0x004516B0
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

		// Token: 0x0600B988 RID: 47496 RVA: 0x0045351C File Offset: 0x0045171C
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

		// Token: 0x0600B989 RID: 47497 RVA: 0x0045358C File Offset: 0x0045178C
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

		// Token: 0x0600B98A RID: 47498 RVA: 0x004535FC File Offset: 0x004517FC
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

		// Token: 0x0600B98B RID: 47499 RVA: 0x00453668 File Offset: 0x00451868
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

		// Token: 0x0600B98C RID: 47500 RVA: 0x004536D8 File Offset: 0x004518D8
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

		// Token: 0x0600B98D RID: 47501 RVA: 0x00453734 File Offset: 0x00451934
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

		// Token: 0x0600B98E RID: 47502 RVA: 0x004537DC File Offset: 0x004519DC
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

		// Token: 0x0600B98F RID: 47503 RVA: 0x00453884 File Offset: 0x00451A84
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

		// Token: 0x0600B990 RID: 47504 RVA: 0x0045392C File Offset: 0x00451B2C
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

		// Token: 0x0600B991 RID: 47505 RVA: 0x004539D4 File Offset: 0x00451BD4
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

		// Token: 0x0600B992 RID: 47506 RVA: 0x00453A80 File Offset: 0x00451C80
		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Max<TResult>();
		}

		// Token: 0x0600B993 RID: 47507 RVA: 0x00453A98 File Offset: 0x00451C98
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

		// Token: 0x0600B994 RID: 47508 RVA: 0x00453AFC File Offset: 0x00451CFC
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

		// Token: 0x0600B995 RID: 47509 RVA: 0x00453B64 File Offset: 0x00451D64
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

		// Token: 0x0600B996 RID: 47510 RVA: 0x00453BCC File Offset: 0x00451DCC
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

		// Token: 0x0600B997 RID: 47511 RVA: 0x00453C30 File Offset: 0x00451E30
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

		// Token: 0x0600B998 RID: 47512 RVA: 0x00453C98 File Offset: 0x00451E98
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

		// Token: 0x0600B999 RID: 47513 RVA: 0x00453D14 File Offset: 0x00451F14
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

		// Token: 0x0600B99A RID: 47514 RVA: 0x00453D94 File Offset: 0x00451F94
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

		// Token: 0x0600B99B RID: 47515 RVA: 0x00453E14 File Offset: 0x00452014
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

		// Token: 0x0600B99C RID: 47516 RVA: 0x00453E90 File Offset: 0x00452090
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

		// Token: 0x0600B99D RID: 47517 RVA: 0x00453F14 File Offset: 0x00452114
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

		// Token: 0x0600B99E RID: 47518 RVA: 0x00453FF4 File Offset: 0x004521F4
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

		// Token: 0x0600B99F RID: 47519 RVA: 0x00454060 File Offset: 0x00452260
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

		// Token: 0x0600B9A0 RID: 47520 RVA: 0x004540D0 File Offset: 0x004522D0
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

		// Token: 0x0600B9A1 RID: 47521 RVA: 0x00454140 File Offset: 0x00452340
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

		// Token: 0x0600B9A2 RID: 47522 RVA: 0x004541AC File Offset: 0x004523AC
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

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x0045421C File Offset: 0x0045241C
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

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x004542C4 File Offset: 0x004524C4
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

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x0045436C File Offset: 0x0045256C
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

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x00454414 File Offset: 0x00452614
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

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x004544BC File Offset: 0x004526BC
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

		// Token: 0x0600B9A8 RID: 47528 RVA: 0x00454568 File Offset: 0x00452768
		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return source.Select<TSource, TResult>(selector).Min<TResult>();
		}

		// Token: 0x0600B9A9 RID: 47529 RVA: 0x0045457D File Offset: 0x0045277D
		public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
		{
			Check.Source(source);
			return Enumerable.CreateOfTypeIterator<TResult>(source);
		}

		// Token: 0x0600B9AA RID: 47530 RVA: 0x0045458B File Offset: 0x0045278B
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

		// Token: 0x0600B9AB RID: 47531 RVA: 0x0045459B File Offset: 0x0045279B
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9AC RID: 47532 RVA: 0x004545A5 File Offset: 0x004527A5
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Ascending);
		}

		// Token: 0x0600B9AD RID: 47533 RVA: 0x004545B7 File Offset: 0x004527B7
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.OrderByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9AE RID: 47534 RVA: 0x004545C1 File Offset: 0x004527C1
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return new OrderedSequence<TSource, TKey>(source, keySelector, comparer, SortDirection.Descending);
		}

		// Token: 0x0600B9AF RID: 47535 RVA: 0x004545D3 File Offset: 0x004527D3
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

		// Token: 0x0600B9B0 RID: 47536 RVA: 0x00454601 File Offset: 0x00452801
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

		// Token: 0x0600B9B1 RID: 47537 RVA: 0x00454618 File Offset: 0x00452818
		public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Enumerable.CreateRepeatIterator<TResult>(element, count);
		}

		// Token: 0x0600B9B2 RID: 47538 RVA: 0x0045462B File Offset: 0x0045282B
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

		// Token: 0x0600B9B3 RID: 47539 RVA: 0x00454642 File Offset: 0x00452842
		public static IEnumerable<TSource> Reverse<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return Enumerable.CreateReverseIterator<TSource>(source);
		}

		// Token: 0x0600B9B4 RID: 47540 RVA: 0x00454650 File Offset: 0x00452850
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

		// Token: 0x0600B9B5 RID: 47541 RVA: 0x00454660 File Offset: 0x00452860
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600B9B6 RID: 47542 RVA: 0x00454670 File Offset: 0x00452870
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

		// Token: 0x0600B9B7 RID: 47543 RVA: 0x00454687 File Offset: 0x00452887
		public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600B9B8 RID: 47544 RVA: 0x00454697 File Offset: 0x00452897
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

		// Token: 0x0600B9B9 RID: 47545 RVA: 0x004546AE File Offset: 0x004528AE
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600B9BA RID: 47546 RVA: 0x004546BE File Offset: 0x004528BE
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

		// Token: 0x0600B9BB RID: 47547 RVA: 0x004546D5 File Offset: 0x004528D5
		public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
		{
			Check.SourceAndSelector(source, selector);
			return Enumerable.CreateSelectManyIterator<TSource, TResult>(source, selector);
		}

		// Token: 0x0600B9BC RID: 47548 RVA: 0x004546E5 File Offset: 0x004528E5
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

		// Token: 0x0600B9BD RID: 47549 RVA: 0x004546FC File Offset: 0x004528FC
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600B9BE RID: 47550 RVA: 0x0045470E File Offset: 0x0045290E
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

		// Token: 0x0600B9BF RID: 47551 RVA: 0x0045472C File Offset: 0x0045292C
		public static IEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
		{
			Check.SourceAndCollectionSelectors(source, collectionSelector, resultSelector);
			return Enumerable.CreateSelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
		}

		// Token: 0x0600B9C0 RID: 47552 RVA: 0x0045473E File Offset: 0x0045293E
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

		// Token: 0x0600B9C1 RID: 47553 RVA: 0x0045475C File Offset: 0x0045295C
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

		// Token: 0x0600B9C2 RID: 47554 RVA: 0x004547CC File Offset: 0x004529CC
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

		// Token: 0x0600B9C3 RID: 47555 RVA: 0x00454834 File Offset: 0x00452A34
		public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Throw);
		}

		// Token: 0x0600B9C4 RID: 47556 RVA: 0x00454848 File Offset: 0x00452A48
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

		// Token: 0x0600B9C5 RID: 47557 RVA: 0x004548A8 File Offset: 0x00452AA8
		public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return source.Single<TSource>(predicate, Enumerable.Fallback.Default);
		}

		// Token: 0x0600B9C6 RID: 47558 RVA: 0x004548B9 File Offset: 0x00452AB9
		public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateSkipIterator<TSource>(source, count);
		}

		// Token: 0x0600B9C7 RID: 47559 RVA: 0x004548C8 File Offset: 0x00452AC8
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

		// Token: 0x0600B9C8 RID: 47560 RVA: 0x004548DF File Offset: 0x00452ADF
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600B9C9 RID: 47561 RVA: 0x004548EF File Offset: 0x00452AEF
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

		// Token: 0x0600B9CA RID: 47562 RVA: 0x00454906 File Offset: 0x00452B06
		public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateSkipWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600B9CB RID: 47563 RVA: 0x00454916 File Offset: 0x00452B16
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

		// Token: 0x0600B9CC RID: 47564 RVA: 0x00454930 File Offset: 0x00452B30
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

		// Token: 0x0600B9CD RID: 47565 RVA: 0x00454980 File Offset: 0x00452B80
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

		// Token: 0x0600B9CE RID: 47566 RVA: 0x004549E4 File Offset: 0x00452BE4
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

		// Token: 0x0600B9CF RID: 47567 RVA: 0x00454A3C File Offset: 0x00452C3C
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

		// Token: 0x0600B9D0 RID: 47568 RVA: 0x00454AA8 File Offset: 0x00452CA8
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

		// Token: 0x0600B9D1 RID: 47569 RVA: 0x00454AF8 File Offset: 0x00452CF8
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

		// Token: 0x0600B9D2 RID: 47570 RVA: 0x00454B5C File Offset: 0x00452D5C
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

		// Token: 0x0600B9D3 RID: 47571 RVA: 0x00454BB4 File Offset: 0x00452DB4
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

		// Token: 0x0600B9D4 RID: 47572 RVA: 0x00454C20 File Offset: 0x00452E20
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

		// Token: 0x0600B9D5 RID: 47573 RVA: 0x00454C78 File Offset: 0x00452E78
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

		// Token: 0x0600B9D6 RID: 47574 RVA: 0x00454CE4 File Offset: 0x00452EE4
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

		// Token: 0x0600B9D7 RID: 47575 RVA: 0x00454D44 File Offset: 0x00452F44
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

		// Token: 0x0600B9D8 RID: 47576 RVA: 0x00454DB8 File Offset: 0x00452FB8
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

		// Token: 0x0600B9D9 RID: 47577 RVA: 0x00454E0C File Offset: 0x0045300C
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

		// Token: 0x0600B9DA RID: 47578 RVA: 0x00454E74 File Offset: 0x00453074
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

		// Token: 0x0600B9DB RID: 47579 RVA: 0x00454ED0 File Offset: 0x004530D0
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

		// Token: 0x0600B9DC RID: 47580 RVA: 0x00454F40 File Offset: 0x00453140
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

		// Token: 0x0600B9DD RID: 47581 RVA: 0x00454F98 File Offset: 0x00453198
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

		// Token: 0x0600B9DE RID: 47582 RVA: 0x00455004 File Offset: 0x00453204
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

		// Token: 0x0600B9DF RID: 47583 RVA: 0x00455064 File Offset: 0x00453264
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

		// Token: 0x0600B9E0 RID: 47584 RVA: 0x004550DC File Offset: 0x004532DC
		public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
		{
			Check.Source(source);
			return Enumerable.CreateTakeIterator<TSource>(source, count);
		}

		// Token: 0x0600B9E1 RID: 47585 RVA: 0x004550EB File Offset: 0x004532EB
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

		// Token: 0x0600B9E2 RID: 47586 RVA: 0x00455102 File Offset: 0x00453302
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600B9E3 RID: 47587 RVA: 0x00455112 File Offset: 0x00453312
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

		// Token: 0x0600B9E4 RID: 47588 RVA: 0x00455129 File Offset: 0x00453329
		public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
		{
			Check.SourceAndPredicate(source, predicate);
			return Enumerable.CreateTakeWhileIterator<TSource>(source, predicate);
		}

		// Token: 0x0600B9E5 RID: 47589 RVA: 0x00455139 File Offset: 0x00453339
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

		// Token: 0x0600B9E6 RID: 47590 RVA: 0x00455150 File Offset: 0x00453350
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenBy<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9E7 RID: 47591 RVA: 0x0045515A File Offset: 0x0045335A
		public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, false);
		}

		// Token: 0x0600B9E8 RID: 47592 RVA: 0x00455171 File Offset: 0x00453371
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ThenByDescending<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9E9 RID: 47593 RVA: 0x0045517B File Offset: 0x0045337B
		public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
		{
			Check.SourceAndKeySelector(source, keySelector);
			return (source as OrderedEnumerable<TSource>).CreateOrderedEnumerable<TKey>(keySelector, comparer, true);
		}

		// Token: 0x0600B9EA RID: 47594 RVA: 0x00455194 File Offset: 0x00453394
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

		// Token: 0x0600B9EB RID: 47595 RVA: 0x0045524C File Offset: 0x0045344C
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToDictionary<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600B9EC RID: 47596 RVA: 0x00455258 File Offset: 0x00453458
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

		// Token: 0x0600B9ED RID: 47597 RVA: 0x004552C8 File Offset: 0x004534C8
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToDictionary<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9EE RID: 47598 RVA: 0x004552D4 File Offset: 0x004534D4
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

		// Token: 0x0600B9EF RID: 47599 RVA: 0x0045533C File Offset: 0x0045353C
		public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
		{
			Check.Source(source);
			return new List<TSource>(source);
		}

		// Token: 0x0600B9F0 RID: 47600 RVA: 0x0045534A File Offset: 0x0045354A
		public static ILookup<TKey, TSource> ToLookup<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			return source.ToLookup<TSource, TKey>(keySelector, null);
		}

		// Token: 0x0600B9F1 RID: 47601 RVA: 0x00455354 File Offset: 0x00453554
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

		// Token: 0x0600B9F2 RID: 47602 RVA: 0x004553FC File Offset: 0x004535FC
		public static ILookup<TKey, TElement> ToLookup<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
		{
			return source.ToLookup<TSource, TKey, TElement>(keySelector, elementSelector, null);
		}

		// Token: 0x0600B9F3 RID: 47603 RVA: 0x00455408 File Offset: 0x00453608
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

		// Token: 0x0600B9F4 RID: 47604 RVA: 0x004554B8 File Offset: 0x004536B8
		public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			return first.SequenceEqual<TSource>(second, null);
		}

		// Token: 0x0600B9F5 RID: 47605 RVA: 0x004554C4 File Offset: 0x004536C4
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

		// Token: 0x0600B9F6 RID: 47606 RVA: 0x0045555C File Offset: 0x0045375C
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			Check.FirstAndSecond(first, second);
			return first.Union<TSource>(second, null);
		}

		// Token: 0x0600B9F7 RID: 47607 RVA: 0x0045556D File Offset: 0x0045376D
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Check.FirstAndSecond(first, second);
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			return Enumerable.CreateUnionIterator<TSource>(first, second, comparer);
		}

		// Token: 0x0600B9F8 RID: 47608 RVA: 0x00455588 File Offset: 0x00453788
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

		// Token: 0x0600B9F9 RID: 47609 RVA: 0x004555A8 File Offset: 0x004537A8
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

		// Token: 0x0600B9FA RID: 47610 RVA: 0x004555D5 File Offset: 0x004537D5
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

		// Token: 0x0600B9FB RID: 47611 RVA: 0x004555EC File Offset: 0x004537EC
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

		// Token: 0x0600B9FC RID: 47612 RVA: 0x00455604 File Offset: 0x00453804
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

		// Token: 0x0600B9FD RID: 47613 RVA: 0x00455631 File Offset: 0x00453831
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

		// Token: 0x0600B9FE RID: 47614 RVA: 0x00455648 File Offset: 0x00453848
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

		// Token: 0x0600B9FF RID: 47615 RVA: 0x0045565F File Offset: 0x0045385F
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception EmptySequence()
		{
			return new InvalidOperationException("Sequence contains no elements");
		}

		// Token: 0x0600BA00 RID: 47616 RVA: 0x0045566B File Offset: 0x0045386B
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception NoMatchingElement()
		{
			return new InvalidOperationException("Sequence contains no matching element");
		}

		// Token: 0x0600BA01 RID: 47617 RVA: 0x00455677 File Offset: 0x00453877
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneElement()
		{
			return new InvalidOperationException("Sequence contains more than one element");
		}

		// Token: 0x0600BA02 RID: 47618 RVA: 0x00455683 File Offset: 0x00453883
		[PublicizedFrom(EAccessModifier.Private)]
		public static Exception MoreThanOneMatchingElement()
		{
			return new InvalidOperationException("Sequence contains more than one matching element");
		}

		// Token: 0x02001768 RID: 5992
		[PublicizedFrom(EAccessModifier.Private)]
		public enum Fallback
		{
			// Token: 0x04008AC7 RID: 35527
			Default,
			// Token: 0x04008AC8 RID: 35528
			Throw
		}
	}
}
