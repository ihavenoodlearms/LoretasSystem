using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace InventoryOrderSystem.Utils
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;
        private PropertyDescriptor _sortProperty;

        protected override bool SupportsSortingCore => true;

        protected override bool IsSortedCore => _isSorted;
        
        protected override PropertyDescriptor SortPropertyCore => _sortProperty;

        protected override ListSortDirection SortDirectionCore => _sortDirection;

        public SortableBindingList(IList<T> list) : base(list) { }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            _sortProperty = prop;
            _sortDirection = direction;

            List<T> list = Items as List<T>;
            if (list == null) return;

            list.Sort(Compare);

            _isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        private int Compare(T lhs, T rhs)
        {
            var result = OnComparison(lhs, rhs);
            return _sortDirection == ListSortDirection.Ascending ? result : -result;
        }

        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = lhs == null ? null : _sortProperty.GetValue(lhs);
            object rhsValue = rhs == null ? null : _sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1;
            }
            if (rhsValue == null)
            {
                return 1;
            }
            if (lhsValue is IComparable)
            {
                return ((IComparable)lhsValue).CompareTo(rhsValue);
            }
            if (lhsValue.Equals(rhsValue))
            {
                return 0;
            }
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }

        protected override void RemoveSortCore()
        {
            _sortDirection = ListSortDirection.Ascending;
            _sortProperty = null;
            _isSorted = false;
        }
    }
}



