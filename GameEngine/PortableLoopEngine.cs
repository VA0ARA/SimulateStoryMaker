using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace GameEngine
{
    public class PortableLoopEngine : IDisposable
    {
        #region Variable

        private EngineState _engineState = EngineState.Off;

        public EngineState EngineState
        {
            get { return _engineState; }
            private set
            {
                if (_engineState.Equals(value)) return;
                _engineState = value;
            }
        }

        private float _deltaTime = 0;

        public float DeltaTime
        {
            get { return _deltaTime; }
            set
            {
                _deltaTime = value;
                DeltaTimeChanged?.Invoke(value);
            }
        }

        private bool _pause;
        public bool Pause
        {
            get => _pause;
        }

        public Action<float> DeltaTimeChanged;

        private readonly List<Action> _awakes = new List<Action>();

        private readonly List<Action> _starts = new List<Action>();

        private readonly List<Action> _updates = new List<Action>();

        private readonly List<Action> _lateUpdates = new List<Action>();

        private readonly List<Action> _stops = new List<Action>();

        private event Action EndFrame;

        private readonly Dictionary<object, object> _objects = new Dictionary<object, object>();

        private bool _realTime;

        #endregion

        public PortableLoopEngine(Action endFrame = null)
        {
            EndFrame += endFrame;
        }

        public void StartEngine(List<object> objects , bool realTimeMode = true)
        {
            _realTime = realTimeMode;

            foreach (var o in objects)
            {
                if(_objects.ContainsKey(o)) continue;

                if (o is ICloneable cloneable)
                {
                    _objects.Add(o, cloneable.Clone());
                }
                else _objects.Add(o, null);
            }

            EngineState = EngineState.On;
            GameLoopAsync();
        }

        public void PauseEngine()
        {
            _pause = true;
        }

        public void ResumeEngine()
        {
            _pause = false;
        }

        public void StopEngine()
        {
            _pause = false;
            EngineState = EngineState.Off;
        }

        public void CollectMethods()
        {
            foreach (var obj in _objects)
            {
                var awake = obj.Key.GetType().GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (awake != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), obj.Key, awake);
                    _awakes.Add(action);
                }
                
                var start = obj.Key.GetType().GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (start != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), obj.Key, start);
                    _starts.Add(action);
                }
                
                var update = obj.Key.GetType().GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (update != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), obj.Key, update);
                    _updates.Add(action);
                }
                
                var lateUpdate = obj.Key.GetType().GetMethod("LateUpdate", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (lateUpdate != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), obj.Key, lateUpdate);
                    _lateUpdates.Add(action);
                }
                
                var stop = obj.Key.GetType().GetMethod("Stop", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (stop != null)
                {
                    var action = (Action)Delegate.CreateDelegate(typeof(Action), obj.Key, stop);
                    _stops.Add(action);
                }
            }
        }

        private static void UpdateForType(object source, object destination)
        {
            var type = source.GetType();

            var myObjectProperty = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.GetSetMethod() != null);

            foreach (var propertyInfo in myObjectProperty)
            {
                propertyInfo.SetValue(destination, propertyInfo.GetValue(source));
            }

            var myObjectFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var fi in myObjectFields)
            {
                fi.SetValue(destination, fi.GetValue(source));
            }
        }

        private async void GameLoopAsync()
        {
            CollectMethods();
            _awakes.ForEach(a => a?.Invoke());

            _starts.ForEach(s => s?.Invoke());
            while (EngineState != EngineState.Off)
            {
                if (Pause)
                {
                    await Task.Delay(1); 
                    continue;
                }

                var stopWatch = Stopwatch.StartNew();

                _updates.ForEach(u => u?.Invoke());

                _lateUpdates.ForEach(lu => lu?.Invoke());

                await Task.Delay(1);

                if (!_realTime)
                    DeltaTime = 1f/30;
                else
                    DeltaTime = (float)stopWatch.Elapsed.TotalMilliseconds / 1000;

                EndFrame?.Invoke();
            }

            _stops.ForEach(s => s?.Invoke());

            void ClearListeners()
            {
                _awakes.Clear();
                _starts.Clear();
                _updates.Clear();
                _lateUpdates.Clear();
            }

            ClearListeners();

            foreach (var keyValuePair in _objects)
            {
                if (keyValuePair.Value == null) continue;
                UpdateForType(keyValuePair.Value, keyValuePair.Key);
            }
            _objects.Clear();
        }

        public void Dispose()
        {
            EngineState = EngineState.Off;
        }

    }
}