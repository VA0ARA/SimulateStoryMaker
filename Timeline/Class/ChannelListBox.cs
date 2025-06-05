using System;
using System.Windows;
using System.Windows.Controls;
using Timeline.Model;

namespace Timeline.Class
{
    class ChannelGrid : Grid
    {
        public static readonly DependencyProperty ChannelProperty = DependencyProperty.Register(
            nameof(Channel), typeof(Channel), typeof(ChannelGrid), new PropertyMetadata(default(Channel)));

        public Channel Channel
        {
            get { return (Channel) GetValue(ChannelProperty); }
            set { SetValue(ChannelProperty, value); }
        }
    }
}
