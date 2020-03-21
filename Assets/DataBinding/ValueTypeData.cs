﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataBinding
{
    public class ValueTypeData<T> : IData<T> where T : struct
    {
        public DataChangedEvent<T> DataChanged { get; set; }

        private T _data;
        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnDataChanged();
            }
        }

        public bool IsBinded => DataChanged != null;

        public void OnDataChanged()
        {
            DataChanged?.Invoke();
        }

        public IUIData<T> GetUIData()
        {
            return DataChanged?.Binder.UIData;
        }

        public void SetValueWithoutNotify(T value)
        {
            _data = value;
        }

        public ValueTypeData()
        {
        }
    }
}
