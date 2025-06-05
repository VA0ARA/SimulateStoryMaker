using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StoryMaker.DataStructure;
using StoryMaker.Helpers;
using StoryMaker.Models.Actions;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Interfaces;

namespace StoryMaker.Models.Components
{
    public abstract class InterActivity : Component, IAssetModelCollector , IDisposable
    {
        private ObservableCollection<IAction> _actions = new ObservableCollection<IAction>();

        public ObservableCollection<IAction> Actions
        {
            get { return _actions; }

            set
            {
                _actions = value;
                RaisePropertyChanged(nameof(Actions));
            }
        }

        public void AddAction(IAction action)
        {
            Actions.Add(action);
        }

        public void RemoveAction(IAction action)
        {
            if (action != null)
            {
                Actions.Remove(action);
            }
        }

        #region ITimeline

        public void Start()
        {
            Play();
        }

        public void Stop()
        {
            Pause();
        }

        protected virtual void Play()
        {
        }

        protected virtual void Pause()
        {
            foreach (var a in Actions)
                if (a is IStop stop)
                    stop.Stop();
        }

        protected virtual void Execute()
        {
            foreach (var a in Actions)
                a.Execute();
        }

        #endregion

        public abstract TriggerType GetTriggerType();
        // public abstract object GetParameter();

        public virtual IEnumerable<AssetModel> GetAssets()
        {
            IEnumerable<AssetModel> assets = new List<AssetModel>();
            foreach(var a in Actions.OfType<IAssetModelCollector>())
            {
                var newAssets = a.GetAssets();
                if (newAssets != null)
                    assets = assets.Concat(newAssets);
            }

            return assets;
        }

        public virtual void Dispose()
        {
            foreach (var a in Actions)
                if (a is IDisposable disposable)
                    disposable.Dispose();
        }
    }
}