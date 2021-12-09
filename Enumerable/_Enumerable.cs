using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable
{
    public static class _Enumerable
    {
         public static IEnumerable<TResult> _Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("predicate");
            return _SelectIterator<TSource, TResult>(source, selector);
        }

        static IEnumerable<TResult> _SelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            int index = -1;
            foreach (TSource element in source)
            {
                checked { index++; }
                yield return selector(element, index);
            }
        }
        public static IEnumerable<TResult> _SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentException("source");
            if (collectionSelector == null) throw new ArgumentException("collectionSelector");
            if (resultSelector == null) throw new ArgumentException("resultSelector");
            return _SelectManyIterator<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }
       
        static IEnumerable<TResult> _SelectManyIterator<TSource, TCollection, TResult>(IEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            int index = -1;
            foreach (TSource element in source)
            {
                checked { index++; }
                foreach (TCollection subElement in collectionSelector(element, index))
                {
                    yield return resultSelector(element, subElement);
                }
            }
        }
        public static IEnumerable<TSource> _Take<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentException("source");
            return _TakeIterator<TSource>(source, count);
        }

        static IEnumerable<TSource> _TakeIterator<TSource>(IEnumerable<TSource> source, int count)
        {
            if (count > 0)
            {
                foreach (TSource element in source)
                {
                    yield return element;
                    if (--count == 0) break;
                }
            }
        }
        public static IEnumerable<TSource> _TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentException("source");
            if (predicate == null) throw new ArgumentException("predicate");
            return _TakeWhileIterator<TSource>(source, predicate);
        }

        static IEnumerable<TSource> _TakeWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
            {
                if (!predicate(element)) break;
                yield return element;
            }
        }
        public static IEnumerable<TSource> _Skip<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentException("source");
            return _SkipIterator<TSource>(source, count);
        }

        static IEnumerable<TSource> _SkipIterator<TSource>(IEnumerable<TSource> source, int count)
        {
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (count > 0 && e.MoveNext()) count--;
                if (count <= 0)
                {
                    while (e.MoveNext()) yield return e.Current;
                }
            }
        }
        public static IEnumerable<TSource> _SkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentException("source");
            if (predicate == null) throw new ArgumentException("predicate");
            return _SkipWhileIterator<TSource>(source, predicate);
        }

        static IEnumerable<TSource> _SkipWhileIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool yielding = false;
            foreach (TSource element in source)
            {
                if (!yielding && !predicate(element)) yielding = true;
                if (yielding) yield return element;
            }
        }
        public static TSource _First<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (TSource element in source)
            {
                if (predicate(element)) return element;
            }
            throw new Exception();
        }

        public static TSource _FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            IList<TSource> list = source as IList<TSource>;
            if (list != null)
            {
                if (list.Count > 0) return list[0];
            }
            else
            {
                using (IEnumerator<TSource> e = source.GetEnumerator())
                {
                    if (e.MoveNext()) return e.Current;
                }
            }
            return default(TSource);
        }

        public static TSource _FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (TSource element in source)
            {
                if (predicate(element)) return element;
            }
            return default(TSource);
        }
        public static IEnumerable<TSource> _Distinct<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentException("source");
            return _DistinctIterator<TSource>(source, null);
        }

        public static IEnumerable<TSource> _Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentException("source");
            return _DistinctIterator<TSource>(source, comparer);
        }

        static IEnumerable<TSource> _DistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            Set<TSource> set = new Set<TSource>(comparer);
            foreach (TSource element in source)
                if (set.Add(element)) yield return element;
        }
        public static IEnumerable<TSource> _Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null) throw new ArgumentException("first");
            if (second == null) throw new ArgumentException("second");
            return _UnionIterator<TSource>(first, second, null);
        }

        public static IEnumerable<TSource> _Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) throw new ArgumentException("first");
            if (second == null) throw new ArgumentException("second");
            return _UnionIterator<TSource>(first, second, comparer);
        }

        static IEnumerable<TSource> _UnionIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            Set<TSource> set = new Set<TSource>(comparer);
            foreach (TSource element in first)
                if (set.Add(element)) yield return element;
            foreach (TSource element in second)
                if (set.Add(element)) yield return element;
        }
        public static IEnumerable<TSource> _Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null) throw new ArgumentException("first");
            if (second == null) throw new ArgumentException("second");
            return _IntersectIterator<TSource>(first, second, null);
        }

        public static IEnumerable<TSource> _Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null) throw new ArgumentException("first");
            if (second == null) throw new ArgumentException("second");
            return _IntersectIterator<TSource>(first, second, comparer);
        }

        static IEnumerable<TSource> _IntersectIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            Set<TSource> set = new Set<TSource>(comparer);
            foreach (TSource element in second) set.Add(element);
            foreach (TSource element in first)
                if (set.Remove(element)) yield return element;
        }
        internal class Set<TElement>
        {
            int[] buckets;
            Slot[] slots;
            int count;
            int freeList;
            IEqualityComparer<TElement> comparer;

            public Set() : this(null) { }

            public Set(IEqualityComparer<TElement> comparer)
            {
                if (comparer == null) comparer = EqualityComparer<TElement>.Default;
                this.comparer = comparer;
                buckets = new int[7];
                slots = new Slot[7];
                freeList = -1;
            }

            public bool Add(TElement value)
            {
                return !Find(value, true);
            }

            public bool Contains(TElement value)
            {
                return Find(value, false);
            }

            public bool Remove(TElement value)
            {
                int hashCode = InternalGetHashCode(value);
                int bucket = hashCode % buckets.Length;
                int last = -1;
                for (int i = buckets[bucket] - 1; i >= 0; last = i, i = slots[i].next)
                {
                    if (slots[i].hashCode == hashCode && comparer.Equals(slots[i].value, value))
                    {
                        if (last < 0)
                        {
                            buckets[bucket] = slots[i].next + 1;
                        }
                        else
                        {
                            slots[last].next = slots[i].next;
                        }
                        slots[i].hashCode = -1;
                        slots[i].value = default(TElement);
                        slots[i].next = freeList;
                        freeList = i;
                        return true;
                    }
                }
                return false;
            }

            bool Find(TElement value, bool add)
            {
                int hashCode = InternalGetHashCode(value);
                for (int i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next)
                {
                    if (slots[i].hashCode == hashCode && comparer.Equals(slots[i].value, value)) return true;
                }
                if (add)
                {
                    int index;
                    if (freeList >= 0)
                    {
                        index = freeList;
                        freeList = slots[index].next;
                    }
                    else
                    {
                        if (count == slots.Length) Resize();
                        index = count;
                        count++;
                    }
                    int bucket = hashCode % buckets.Length;
                    slots[index].hashCode = hashCode;
                    slots[index].value = value;
                    slots[index].next = buckets[bucket] - 1;
                    buckets[bucket] = index + 1;
                }
                return false;
            }

            void Resize()
            {
                int newSize = checked(count * 2 + 1);
                int[] newBuckets = new int[newSize];
                Slot[] newSlots = new Slot[newSize];
                Array.Copy(slots, 0, newSlots, 0, count);
                for (int i = 0; i < count; i++)
                {
                    int bucket = newSlots[i].hashCode % newSize;
                    newSlots[i].next = newBuckets[bucket] - 1;
                    newBuckets[bucket] = i + 1;
                }
                buckets = newBuckets;
                slots = newSlots;
            }

            internal int InternalGetHashCode(TElement value)
            {
                return (value == null) ? 0 : comparer.GetHashCode(value) & 0x7FFFFFFF;
            }

            internal struct Slot
            {
                internal int hashCode;
                internal TElement value;
                internal int next;
            }
        }

    }
}
