using StoryMaker.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Timeline.Model
{
    /// <summary>
    /// This class stores data related to a hierarchy 
    /// </summary>
    public class Clip : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public ObservableCollection<Channel> Channels { get; }

        public Clip(string name)
        {
            Name = name;
            Channels = new ObservableCollection<Channel>();
        }

        public int GetLastFrame()
        {
            return Channels.Count == 0 ? 0 : Channels.Max(c => c.LastKey);
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Return the length of clip by frame
        /// </summary>
        public int length
        {
            get
            {
                var channelLengths = Channels.Select(c => c.LastKey);
                return channelLengths.Max();
            }
        }

        #region Static Methods

        public void AddKeyValue(IEnumerable<KeyValuePair<ChannelAddress, KeyValuePair<int, object>>> data)
        {

            foreach (var keyValuePair in data)
            {
                var channel = GetChannel(keyValuePair.Key);
                if (channel == null)
                {
                    channel = new Channel(keyValuePair.Key.ObjectAddress, keyValuePair.Key.MemberName,
                        keyValuePair.Key.Type);
                    Channels.Add(channel);
                }

                channel.Add(new KeyValue(keyValuePair.Value.Key, keyValuePair.Value.Value));
            }
        }

        public void AddKeyValue(ChannelAddress channel, int frame, object value)
        {
            var d = new List<KeyValuePair<ChannelAddress, KeyValuePair<int, object>>>
            {
                new KeyValuePair<ChannelAddress, KeyValuePair<int, object>>(channel,
                    new KeyValuePair<int, object>(frame, value))
            };
            AddKeyValue(d);
        }

        private void DeleteKey(IEnumerable<KeyValuePair<Channel, KeyValue>> keyValues)
        {
            foreach (var pair in keyValues)
            {
                pair.Key.Remove(pair.Value);

                if (pair.Key.SortedDictionary.Count == 0)
                    Channels.Remove(pair.Key);
            }
        }

        public void DeleteKey(IEnumerable<KeyValue> keyValues)
        {
            DeleteKey(keyValues.Select(i => new KeyValuePair<Channel, KeyValue>(GetChannel(i), i)));
        }

        public void DeleteKey(KeyValue keyValue)
        {
            var data = new List<KeyValuePair<Channel, KeyValue>>()
            {
                new KeyValuePair<Channel , KeyValue>(GetChannel(keyValue) , keyValue)
            };

            DeleteKey(data);
        }

        public void DeleteKey(ChannelAddress address, int frame)
        {
            var channel = GetChannel(address);
            var data = new List<KeyValuePair<Channel, KeyValue>>()
            {
                new KeyValuePair<Channel , KeyValue>(channel, channel.GetKeyValue(frame))
            };

            DeleteKey(data);
        }

        public void Move(IEnumerable<Tuple<KeyValue, int>> data)
        {
            foreach (var tuple in data)
            {
                var channel = GetChannel(tuple.Item1);
                channel.Remove(tuple.Item1);

                tuple.Item1.Frame = tuple.Item2;
                channel.Add(tuple.Item1);
            }
        }

        public void Move(KeyValue keyValue, int newFrame)
        {
            var data = new List<Tuple<KeyValue, int>>()
            {
                new Tuple<KeyValue, int>(keyValue, newFrame)
            };

            Move(data);
        }

        public bool ContainsFrame(ChannelAddress address, int frame)
        {
            var channel = GetChannel(address);
            if (channel == null) return false;
            return channel.ContainsKeyAtFrame(frame);
        }

        private Channel GetChannel(ChannelAddress address)
        {
            return Channels.FirstOrDefault(r =>
                r.ObjectAddress == address.ObjectAddress && r.MemberName == address.MemberName &&
                r.Type == address.Type);
        }

        private Channel GetChannel(KeyValue keyValue)
        {
            return Channels.First(c =>
                c.SortedDictionary.SingleOrDefault(i => i.Item2 == keyValue) != null);
        }

        public ChannelAddress GetChannelAddress(KeyValue keyValue)
        {
            var channel = GetChannel(keyValue);

            return new ChannelAddress(channel.ObjectAddress, channel.MemberName, channel.Type);
        }

        public KeyValue GetKeyValue(ChannelAddress address, int frame)
        {
            var channel = GetChannel(address);

            return channel.SortedDictionary.SingleOrDefault(i => i.Item1 == frame)?.Item2;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}