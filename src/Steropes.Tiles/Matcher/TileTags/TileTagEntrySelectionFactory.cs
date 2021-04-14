using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Matcher.TileTags
{
    public static class TileTagEntrySelectionFactory
    {
        /// <summary>
        ///   Creates a default set of tag entry selections from the given set of unique tags.
        ///   This method will use the first character of the tags as selector key.
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ITileTagEntrySelectionFactory<string> FromTagsAsSingleCharKey(params string[] tags)
        {
            try
            {
                var x = new List<KeyValuePair<string, string>>();
                foreach (var tag in tags)
                {
                    if (string.IsNullOrWhiteSpace(tag))
                    {
                        continue;
                    }

                    x.Add(new KeyValuePair<string, string>(tag[0].ToString(), tag));
                }
                return new TileTagEntrySelectionFactory<string>(x.ToArray());
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot pass an empty string as tag", e);
            }
        }

        public static ITileTagEntrySelectionFactory<string> FromTagsAsTextKey(params string[] tags)
        {
            try
            {
                var x = new List<KeyValuePair<string, string>>();
                foreach (var tag in tags)
                {
                    if (string.IsNullOrWhiteSpace(tag))
                    {
                        continue;
                    }

                    x.Add(new KeyValuePair<string, string>(tag, tag));
                }
                return new TileTagEntrySelectionFactory<string>(x.ToArray());
            }
            catch (IndexOutOfRangeException e)
            {
                throw new ArgumentException("Cannot pass an empty string as tag", e);
            }
        }
    }

    public class TileTagEntrySelectionFactory<TSelector> : ITileTagEntrySelectionFactory<TSelector>
    {
        readonly List<ITileTagEntrySelection<TSelector>> selectionsByIndex;

        public TileTagEntrySelectionFactory(params KeyValuePair<TSelector, string>[] tags)
        {
            selectionsByIndex = new List<ITileTagEntrySelection<TSelector>>();

            foreach (var tag in tags)
            {
                Register(tag.Key, tag.Value);
            }
        }

        ITileTagEntrySelection ITileTagEntrySelectionFactory.this[int idx] => this[idx];

        public ITileTagEntrySelection<TSelector> this[int idx]
        {
            get { return selectionsByIndex[idx]; }
        }

        ITileTagEntrySelection ITileTagEntrySelectionFactory.this[string tag] => this[tag];

        public ITileTagEntrySelection<TSelector> this[string idx]
        {
            get { return selectionsByIndex.Find(e => e.Tag == idx); }
        }

        public ITileTagEntrySelection<TSelector> Lookup(TSelector idx)
        {
            return selectionsByIndex.Find(e => EqualityComparer<TSelector>.Default.Equals(e.Selector, idx));
        }

        public int Count
        {
            get { return selectionsByIndex.Count; }
        }

        public ITileTagEntrySelection<TSelector> Register(TSelector selector, string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException();
            }

            if (selectionsByIndex.Exists(e => e.Tag == tag))
            {
                throw new ArgumentException($"tag {tag} is already registered.");
            }

            var firstChar = selector;
            if (selectionsByIndex.Exists(e => EqualityComparer<TSelector>.Default.Equals(e.Selector, firstChar)))
            {
                throw new ArgumentException($"duplicate character for tag {tag}");
            }

            var idx = Count;
            var v = new TileTagEntrySelection(this, firstChar, (ushort)idx, tag);
            selectionsByIndex.Add(v);
            return v;
        }

        class TileTagEntrySelection : IEquatable<TileTagEntrySelection>, ITileTagEntrySelection<TSelector>
        {
            protected internal TileTagEntrySelection(ITileTagEntrySelectionFactory<TSelector> factory,
                                                     TSelector selector,
                                                     ushort index,
                                                     string tag)
            {
                Owner = factory ?? throw new ArgumentNullException();
                Selector = selector;
                SelectorText = selector?.ToString() ?? "";
                Index = index;
                Tag = tag;
            }

            public ITileTagEntrySelectionFactory<TSelector> Owner { get; }

            public TSelector Selector { get; }
            public ushort Index { get; }
            public string Tag { get; }
            public string SelectorText { get; }

            public int Cardinality
            {
                get { return Owner.Count; }
            }

            public bool Equals(TileTagEntrySelection other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return Owner.Equals(other.Owner) && EqualityComparer<TSelector>.Default.Equals(Selector, other.Selector) && Index == other.Index;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != GetType())
                {
                    return false;
                }

                return Equals((TileTagEntrySelection)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var x = EqualityComparer<TSelector>.Default;
                    var hashCode = Owner.GetHashCode();
                    hashCode = (hashCode * 397) ^ x.GetHashCode(Selector);
                    hashCode = (hashCode * 397) ^ Index.GetHashCode();
                    return hashCode;
                }
            }

            public static bool operator ==(TileTagEntrySelection left, TileTagEntrySelection right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(TileTagEntrySelection left, TileTagEntrySelection right)
            {
                return !Equals(left, right);
            }
        }
    }
}
