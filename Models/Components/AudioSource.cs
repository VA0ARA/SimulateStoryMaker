using System;
using System.Collections.Generic;
using GameEngine;
using StoryMaker.Helpers;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Interfaces;
using Timeline.Attribute;

namespace StoryMaker.Models.Components
{
    public class AudioSource : Component, IAssetModelCollector, IDisposable
    {
        private bool _enable;

        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (_enable == value) return;
                _enable = value;

                if (_enable)
                    OnEnable();
                else
                    OnDisable();

                RaisePropertyChanged(nameof(Enable));
            }
        }

        private bool _playOnAwake = true;

        [NonRecordable]
        public bool PlayOnAwake
        {
            get { return _playOnAwake; }
            set
            {
                if (_playOnAwake == value) return;
                _playOnAwake = value;
                RaisePropertyChanged(nameof(PlayOnAwake));
            }
        }

        private bool _loop;

        [NonRecordable]
        public bool Loop
        {
            get { return _loop; }
            set
            {
                if (_loop == value) return;
                _loop = value;
                RaisePropertyChanged(nameof(Loop));
            }
        }

        private float _delay;

        [NonRecordable]
        public float Delay
        {
            get => _delay;
            set
            {
                if (_delay == value) return;
                _delay = value;

                RaisePropertyChanged(nameof(Delay));
            }
        }

        private SoundAssetModel _soundAsset;

        [NonRecordable]
        public SoundAssetModel SoundAsset
        {
            get { return _soundAsset; }
            set
            {
                if (Equals(_soundAsset, value)) return;
                _soundAsset = value;

                RaisePropertyChanged(nameof(SoundAsset));
            }
        }

        private SoundPlayerModel _soundPlayer;

        private float _timer;

        public AudioSource()
        {
            ID = Guid.NewGuid().ToString();
        }

        public AudioSource(string id)
        {
            ID = id;
        }
        private void Awake()
        {
            _soundPlayer = new SoundPlayerModel();
            OnEnable();
        }

        private void Update()
        {
            if (Loop && _soundPlayer != null && _soundPlayer.Playing && _soundPlayer.CurrentTime >= _soundPlayer.Length)
                Play();

            if (!PlayOnAwake || _timer > Delay) return;
            _timer += Time.DeltaTime;

            if (_timer >= Delay)
            {
                Play();
            }
        }

        private void Play()
        {
            if (SoundAsset != null)
            {
                _soundPlayer.Stop();
                _soundPlayer.Source = SoundAsset;
                _soundPlayer.Play();
            }
        }

        public void OnEnable()
        {
            if (!PlayOnAwake) return;
        }

        public void OnDisable()
        {
        }

        #region Methods

        public void Stop()
        {
            _soundPlayer?.Stop();
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponent<AudioSource>() == null;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }

        #endregion

        #region Component

        #region IAssetCollector

        public IEnumerable<AssetModel> GetAssets()
        {
            return new List<AssetModel>() {SoundAsset};
        }

        //public IEnumerable<IAssetModelCollector> GetCollectors()
        //{
        //    return new List<IAssetModelCollector>();
        //}

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _soundPlayer?.Dispose();
        }

        #endregion

        #endregion
    }
}