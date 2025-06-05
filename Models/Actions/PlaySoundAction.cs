using System.Collections.Generic;
using System.ComponentModel;
using GameEngine;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Interfaces;
using System;
using System.Threading.Tasks;
using StoryMaker.Models.Components;

namespace StoryMaker.Models.Actions
{
    public class PlaySoundAction : IAction, INotifyPropertyChanged, IAssetModelCollector, IStop, IDisposable
    {
        private SoundPlayerModel _player;
        public event PropertyChangedEventHandler PropertyChanged;

        private SoundAssetModel _soundFile;

        public SoundAssetModel SoundFile
        {
            get { return _soundFile; }

            set
            {
                _soundFile = value;
                if (_player == null)
                    _player = new SoundPlayerModel();
                _player.Source = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SoundFile)));
            }
        }

        private float _startTime;

        public float StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value) return;

                _startTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartTime)));
            }
        }

        private float _duration = 0;

        public float Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value) return;
                _duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
            }
        }

        private float _timer = 0;
        public string ID { get; }

        public PlaySoundAction()
        {
            ID = Guid.NewGuid().ToString();
        }
        public PlaySoundAction(string id)
        {
            this.ID = id;
        }

        private readonly PortableLoopEngine _loopEngine = new PortableLoopEngine();

        public async void Execute()
        {
            await Task.Delay(TimeSpan.FromSeconds(StartTime));
            _loopEngine.StartEngine(new List<object> {this});
            _timer = 0;
            _player.Seek(0);
            _player.Play();
        }

        public IEnumerable<AssetModel> GetAssets()
        {
            yield return SoundFile;
        }

        public void Update()
        {
            if (!_player.Playing) return;
            
            _timer += _loopEngine.DeltaTime;

            if (Duration < _timer)
            {
                Stop();
            }
        }


        public void Dispose()
        {
            _player.Dispose();
        }

        public void Stop()
        {
            _player.Seek(0);
            _player.Stop();
            _loopEngine.StopEngine();
        }
    }
}