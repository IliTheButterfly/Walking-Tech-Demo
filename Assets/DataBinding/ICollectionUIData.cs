﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataBinding
{
    /// <summary>
    /// The UI representation of a collection
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public interface ICollectionUIData<T> : IBindable, ICollection<T>
    {
        T this[int index] { get; set; }
        CollectionAddEvent<T> Added { get; set; }
        CollectionRemoveEvent<T> Removed { get; set; }
        CollectionIndexValueChangeEvent<T> IndexValueChanged { get; set; }

        void OnAdded(IEnumerable<T> items);
        void OnRemoved(IEnumerable<T> items);
        void OnIndexValueChanged(int index, T item);
        ICollectionData<T> GetCollectionData();
    }
}
