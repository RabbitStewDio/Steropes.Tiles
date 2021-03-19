using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  public class StructureTreeModel : INotifyPropertyChanged
  {
    TextureFile content;
    TreeNode root;

    public StructureTreeModel()
    {
    }

    public TreeNode Root
    {
      get { return root; }
      set
      {
        if (Equals(value, root)) return;
        root = value;
        OnPropertyChanged();
      }
    }

    public TextureFile Content
    {
      get { return content; }
      set
      {
        if (Equals(value, content)) return;
        var oldValue = content;
        content = value;
        OnRootChanged(oldValue, content);
        OnPropertyChanged();
      }
    }

    void OnRootChanged(TextureFile oldValue, TextureFile newValue)
    {
      if (newValue == null)
      {
        Root = null;
      }
      else
      {
        Root = new RootNode(newValue);
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public static class TreeNodeCollectionExtensions
  {
    public static TypedTreeNodeCollectionWrapper AsList(this TreeNodeCollection c)
    {
      return new TypedTreeNodeCollectionWrapper(c);
    }

    public static void Resync<T>(this TreeNode self, IEnumerable<T> data, Func<T, TreeNode> producer)
    {
      self.TreeView?.SuspendLayout();
      self.TreeView?.BeginUpdate();
      var nodes = new Dictionary<object, TreeNode>();
      foreach (var node in self.Nodes.AsList())
      {
        if (node.Tag != null)
        {
          nodes[node.Tag] = node;
        }
      }

      self.Nodes.Clear();
      foreach (var collection in data)
      {
        if (nodes.TryGetValue(collection, out var n))
        {
          self.Nodes.Add(n);
        }
        else
        {
          var treeNode = producer(collection);
          if (treeNode != null)
          {
            self.Nodes.Add(treeNode);
          }
        }
      }

      self.TreeView?.EndUpdate();
      self.TreeView?.ResumeLayout(true);

    }
  }

  public class TypedTreeNodeCollectionWrapper : IList<TreeNode>
  {
    readonly TreeNodeCollection parent;

    public TypedTreeNodeCollectionWrapper(TreeNodeCollection parent)
    {
      this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
    }

    public object SyncRoot
    {
      get { return ((ICollection) parent).SyncRoot; }
    }

    public bool IsSynchronized
    {
      get { return ((ICollection) parent).IsSynchronized; }
    }

    public void CopyTo(TreeNode[] array, int arrayIndex)
    {
      parent.CopyTo(array, arrayIndex);
    }

    public bool IsFixedSize
    {
      get { return ((IList) parent).IsFixedSize; }
    }

    public void AddRange(TreeNode[] nodes)
    {
      parent.AddRange(nodes);
    }

    public TreeNode[] Find(string key, bool searchAllChildren)
    {
      return parent.Find(key, searchAllChildren);
    }

    void ICollection<TreeNode>.Add(TreeNode item)
    {
      Add(item);
    }

    public int Add(TreeNode node)
    {
      return parent.Add(node);
    }

    public bool Contains(TreeNode node)
    {
      return parent.Contains(node);
    }

    public bool ContainsKey(string key)
    {
      return parent.ContainsKey(key);
    }

    public int IndexOf(TreeNode node)
    {
      return parent.IndexOf(node);
    }

    public int IndexOfKey(string key)
    {
      return parent.IndexOfKey(key);
    }

    public void Insert(int index, TreeNode node)
    {
      parent.Insert(index, node);
    }

    public TreeNode Insert(int index, string text)
    {
      return parent.Insert(index, text);
    }

    public TreeNode Insert(int index, string key, string text)
    {
      return parent.Insert(index, key, text);
    }

    public TreeNode Insert(int index, string key, string text, int imageIndex)
    {
      return parent.Insert(index, key, text, imageIndex);
    }

    public TreeNode Insert(int index, string key, string text, string imageKey)
    {
      return parent.Insert(index, key, text, imageKey);
    }

    public TreeNode Insert(int index, string key, string text, int imageIndex, int selectedImageIndex)
    {
      return parent.Insert(index, key, text, imageIndex, selectedImageIndex);
    }

    public TreeNode Insert(int index, string key, string text, string imageKey, string selectedImageKey)
    {
      return parent.Insert(index, key, text, imageKey, selectedImageKey);
    }

    public void Clear()
    {
      parent.Clear();
    }

    public void CopyTo(Array dest, int index)
    {
      parent.CopyTo(dest, index);
    }

    bool ICollection<TreeNode>.Remove(TreeNode item)
    {
      Remove(item);
      return true;
    }

    public void Remove(TreeNode node)
    {
      IList p = parent;
      p.Remove(node);
    }

    public void RemoveAt(int index)
    {
      parent.RemoveAt(index);
    }

    public void RemoveByKey(string key)
    {
      parent.RemoveByKey(key);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<TreeNode> GetEnumerator()
    {
      return new TypedEnumerator(parent.GetEnumerator());
    }

    class TypedEnumerator : IEnumerator<TreeNode>
    {
      readonly IEnumerator parent;

      public TypedEnumerator(IEnumerator parent)
      {
        this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        return this.parent.MoveNext();
      }

      public void Reset()
      {
        this.parent.Reset();
      }

      object IEnumerator.Current
      {
        get { return Current; }
      }

      public TreeNode Current => (TreeNode) parent.Current;
    }

    public TreeNode this[int index]
    {
      get { return parent[index]; }
      set { parent[index] = value; }
    }

    public TreeNode this[string key]
    {
      get { return parent[key]; }
    }

    public int Count
    {
      get { return parent.Count; }
    }

    public bool IsReadOnly
    {
      get { return parent.IsReadOnly; }
    }
  }
}