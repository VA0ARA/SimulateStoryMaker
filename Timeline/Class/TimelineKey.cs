using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Timeline.Model;

namespace Timeline.Class
{
    public class TimelineKey : Border
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            nameof(IsSelected), typeof(bool), typeof(TimelineKey),
            new PropertyMetadata(default(bool)));

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedKeyValuesProperty = DependencyProperty.Register(
            nameof(SelectedKeyValues), typeof(ObservableCollection<KeyValue>), typeof(TimelineKey),
            new PropertyMetadata(default(ObservableCollection<KeyValue>), SelectedKeysChanged));

        private static void SelectedKeysChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimelineKey key)) return;
            
            if(e.NewValue is ObservableCollection<KeyValue> newKeys)
                newKeys.CollectionChanged += key.SelectedKeysCollectionChanged;

            if (e.OldValue is ObservableCollection<KeyValue> oldKeys)
                oldKeys.CollectionChanged -= key.SelectedKeysCollectionChanged;
        }

        private void SelectedKeysCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsSelected = SelectedKeyValues.Any(k => k != null && KeyValue == k);
        }

        public ObservableCollection<KeyValue> SelectedKeyValues
        {
            get { return (ObservableCollection<KeyValue>)GetValue(SelectedKeyValuesProperty); }
            set { SetValue(SelectedKeyValuesProperty, value); }
        }


        public static readonly DependencyProperty KeyValueProperty = DependencyProperty.Register(
            nameof(KeyValue), typeof(KeyValue), typeof(TimelineKey), new PropertyMetadata(default(KeyValue)));

        public KeyValue KeyValue
        {
            get { return (KeyValue) GetValue(KeyValueProperty); }
            set { SetValue(KeyValueProperty, value); }
        }

        private event EventHandler<bool> _selected;

        public event EventHandler<bool> Selected
        {
            add { _selected += value; }
            remove { _selected -= value; }
        }
    }
}