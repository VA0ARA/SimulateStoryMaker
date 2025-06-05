using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using StoryMaker.Annotations;
using StoryMaker.Editor.Utility;
using StoryMaker.Helpers;
using StoryMaker.Models;
using StoryMaker.Models.Components;
using StoryMaker.Models.Editor;
using StoryMaker.Views.InteractivityDesigner;
using Timeline.Class;
using Timeline.ViewModel;
using Timeline.Model;

namespace Timeline.User_Controllers
{
    public static class Command
    {
        public static readonly RoutedUICommand NextKey = new RoutedUICommand("Next Key", "Next Key", typeof(TimeLine));

        public static readonly RoutedUICommand PreviousKey =
            new RoutedUICommand("Previous Key", "Previous Key", typeof(TimeLine));

        public static readonly RoutedUICommand CaptureKey =
            new RoutedUICommand("Capture Key", "Capture Key", typeof(TimeLine));
    }

    /// <summary>
    /// Interaction logic for TimeLine.xaml
    /// </summary>
    public partial class TimeLine : INotifyPropertyChanged
    {
        #region Parameters

        private readonly TimelineViewModel _viewModel;

        private Point _scrollMousePoint;

        private double _horizontalOffset = 1;

        private double _columnWidth = 20;

        private float _zoom = 1;

        private int _downIndex = 0;

        private DateTime _downTime;

        private bool _mouseMoved;

        public ObservableCollection<KeyValue> SelectedKeyValues { get; }

        private readonly Dictionary<KeyValue, int> _firstKeyValuesIndex = new Dictionary<KeyValue, int>();

        private List<KeyValuePair<ChannelAddress, KeyValuePair<int, object>>> _copiedKeyValues =
            new List<KeyValuePair<ChannelAddress, KeyValuePair<int, object>>>();

        private readonly List<KeyValue> _movedKeys = new List<KeyValue>();

        public static readonly DependencyProperty PlayModeProperty = DependencyProperty.Register(
            nameof(PlayMode), typeof(PlayMode), typeof(TimeLine), new PropertyMetadata(default(PlayMode), PlayModeChangedCallback));

        private static void PlayModeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(!(d is TimeLine t)) return;
            var playMode = (PlayMode) e.NewValue;
            t.Disable = playMode != PlayMode.Stop;
            if(t.TimelineModel != null)
                t.TimelineModel.Preview = false;
        }

        public PlayMode PlayMode
        {
            get { return (PlayMode) GetValue(PlayModeProperty); }
            set { SetValue(PlayModeProperty, value); }
        }

        private bool _disable;

        public bool Disable
        {
            get => _disable;
            set
            {
                if (_disable == value) return;
                _disable = value;

                RaisePropertyChanged(nameof(Disable));
            }
        }

        public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register(
            nameof(RowHeight), typeof(float), typeof(TimeLine), new PropertyMetadata(40f));

        public float RowHeight
        {
            get { return (float) GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        public static readonly DependencyProperty StoryObjectProperty = DependencyProperty.Register(
            nameof(StoryObject), typeof(StoryObject), typeof(TimeLine),
            new PropertyMetadata(default(StoryObject), StoryObjectChanged));

        private static void StoryObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TimeLine timeLine)) return;

            if (e.OldValue is StoryObject oldStoryObject)
            {
                oldStoryObject.PropertyChanged -= timeLine.StoryObjectOnPropertyChanged;
            }

            if (e.NewValue is StoryObject newStoryObject)
            {
                var anim = GetAnimator(newStoryObject);
                if (anim != null && !timeLine.TimelineModel.Animators.Contains(anim))
                    timeLine.TimelineModel.Animators.Add(anim);

                Animator parentAnim = null;

                if (anim != null && anim.StoryObject == newStoryObject && anim.ApplyRootMotion)
                {
                    parentAnim = GetAnimator(anim.StoryObject.transform.Parent?.StoryObject);
                    if (parentAnim != null && !timeLine.TimelineModel.Animators.Contains(parentAnim))
                        timeLine.TimelineModel.Animators.Add(parentAnim);
                }

                // this listener is waiting for animator component to be add object
                newStoryObject.PropertyChanged += timeLine.StoryObjectOnPropertyChanged;

                timeLine.ChangeAnimator(anim);

                var deletedAnims = timeLine.TimelineModel.Animators.Where(a => a != anim && a != parentAnim).ToList();
                foreach (var deletedAnim in deletedAnims)
                {
                    timeLine.TimelineModel.Animators.Remove(deletedAnim);
                }
            }
            else
            {
                timeLine.ChangeAnimator(null);
                timeLine.TimelineModel.Animators.Clear();
            }
        }

        private void StoryObjectOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StoryObject.Components))
            {
                var anim = GetAnimator(sender as StoryObject);
                if (anim != null && !TimelineModel.Animators.Contains(anim))
                    TimelineModel.Animators.Add(anim);
                ChangeAnimator(anim);
            }
        }

        public StoryObject StoryObject
        {
            get { return (StoryObject) GetValue(StoryObjectProperty); }
            set { SetValue(StoryObjectProperty, value); }
        }

        public static readonly DependencyProperty TimelineModelProperty = DependencyProperty.Register(
            nameof(TimelineModel), typeof(TimelineModel), typeof(TimeLine),
            new PropertyMetadata(default(TimelineModel)));

        public TimelineModel TimelineModel
        {
            get { return (TimelineModel) GetValue(TimelineModelProperty); }
            set { SetValue(TimelineModelProperty, value); }
        }

        #endregion

        #region Constructors

        public TimeLine()
        {
            SelectedKeyValues = new ObservableCollection<KeyValue>();
            InitializeComponent();

            _viewModel = (TimelineViewModel) FindResource("ViewModel");
        }

        #endregion

        private void ChangeAnimator(Animator animator)
        {
            BtnPause.Visibility = Visibility.Collapsed;
            BtnPlay.Visibility = Visibility.Visible;

            TimelineModel.ChangeAnimator(animator);

            SelectedKeyValues.Clear();
        }

        #region Timeline Key Controllers

        private void Tgl_Record_Checked(object sender, RoutedEventArgs e)
        {
            if (!TglRecord.IsFocused) return;

            TimelineModel.RecordStatus =
                ((ToggleButton) sender).IsChecked == true ? RecordStatus.Record : RecordStatus.Pause;
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            if (TimelineModel.Animator == null) return;
            switch (TimelineModel.RecordStatus)
            {
                case RecordStatus.Play:
                    TglRecord.IsChecked = false;
                    if (TimelineModel.Animator != null)
                        TimelineModel.RecordStatus = RecordStatus.Pause;
                    BtnPause.Visibility = Visibility.Collapsed;
                    BtnPlay.Visibility = Visibility.Visible;
                    break;
                case RecordStatus.Record:
                case RecordStatus.Stop:
                case RecordStatus.Pause:
                    TglRecord.IsChecked = false;
                    if (TimelineModel.Animator != null)
                        TimelineModel.RecordStatus = RecordStatus.Play;
                    BtnPlay.Visibility = Visibility.Collapsed;
                    BtnPause.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region ScrollView Controllers

        #endregion

        #region Key Panel Controllers

        private void Sv_Ruler_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _columnWidth = HgRuler.ColumnWidth;
            _zoom = HgRuler.Zoom;
            SvRuler.CaptureMouse();
            if (TglRecord.IsChecked == false)
            {
                TglRecord.IsChecked = true;
                if (TimelineModel.Animator != null)
                    TimelineModel.RecordStatus = RecordStatus.Record;
                BtnPause.Visibility = Visibility.Collapsed;
                BtnPlay.Visibility = Visibility.Visible;
            }
        }

        private void Sv_Ruler_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (SvRuler.IsMouseCaptured)
            {
                var content = (Grid) SvRuler.Content;
                var lineOffset =
                    (int) e.GetPosition(content).X;
                lineOffset = lineOffset < 0 ? 0 : lineOffset;
                var nearestFrame = (int) Math.Round(lineOffset / (_columnWidth * _zoom));

                TimelineModel.RecordStatus = RecordStatus.Record;
                TimelineModel.CurrentFrame = nearestFrame;

                TbFrame.Text = nearestFrame.ToString();
            }
        }

        private void Sv_Ruler_OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (SvRuler.IsMouseCaptured)
            {
                var content = (Grid) SvRuler.Content;
                var lineOffset =
                    (int) e.GetPosition(content).X;
                lineOffset = lineOffset < 0 ? 0 : lineOffset;
                var nearestFrame = (int) Math.Round(lineOffset / (_columnWidth * _zoom));

                TimelineModel.RecordStatus = RecordStatus.Record;
                TimelineModel.CurrentFrame = nearestFrame;

                TbFrame.Text = nearestFrame.ToString();
                SvRuler.ReleaseMouseCapture();
            }
        }

        private void sv_KeyContainer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // resolve keyContainer drag conflict with keyContainer scrollbar
            var sP = e.GetPosition(SvKeyContainer);
            if (sP.Y < SvKeyContainer.RenderSize.Height - 20 &&
                sP.X < SvKeyContainer.RenderSize.Width - 20)
            {
                e.Handled = false;

                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    e.Handled = true;
                    _scrollMousePoint = e.GetPosition(SvKeyContainer);
                    _horizontalOffset = SvKeyContainer.HorizontalOffset;

                    SvKeyContainer.CaptureMouse();
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    e.Handled = true;
                    _columnWidth = HgRuler.ColumnWidth;
                    _zoom = HgRuler.Zoom;
                    _scrollMousePoint = e.GetPosition(MainPanel);
                    _horizontalOffset = SvKeyContainer.HorizontalOffset;

                    SvKeyContainer.CaptureMouse();
                }
            }
            else
            {
                e.Handled = false;
            }
        }

        private void sv_KeyContainer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (SvKeyContainer.IsMouseCaptured)
            {
                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    SvKeyContainer.ScrollToHorizontalOffset(
                        _horizontalOffset + (_scrollMousePoint.X - e.GetPosition(SvKeyContainer).X));
                }
                else if (e.RightButton == MouseButtonState.Pressed)
                {
                    if (Keyboard.IsKeyDown(Key.LeftAlt))
                    {
                        var xDelta = (int) (e.GetPosition(MainPanel).X - _scrollMousePoint.X);


                        var newZoom = _zoom + (_zoom * ((float) xDelta / 100));

                        HgRuler.Zoom = newZoom;
                    }
                }
            }
        }

        private void sv_KeyContainer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SvKeyContainer.ReleaseMouseCapture();
        }

        private void sv_KeyContainer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            SvKeyContainer.ScrollToVerticalOffset(SvChannelsContainer.VerticalOffset);
            SvKeyContainer.ScrollToHorizontalOffset(SvKeyContainer.HorizontalOffset + e.Delta);
        }

        #endregion

        #region Key Value Controllers

        private void Btn_TimelineKey_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var btn = (TimelineKey) sender;
            _downTime = DateTime.Now;
            _mouseMoved = false;
            _downIndex = GetNearestKey(e);
            _columnWidth = HgRuler.ColumnWidth;
            _zoom = HgRuler.Zoom;
            btn.CaptureMouse();
            e.Handled = true;
        }

        private void Btn_TimelineKey_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var selectedKey = (TimelineKey) sender;
            if (selectedKey.IsMouseCaptured)
            {
                var differenceTime = (DateTime.Now - _downTime).TotalSeconds;

                if (differenceTime < 0.135) return;

                if (!_mouseMoved)
                {
                    if (!SelectedKeyValues.Contains(selectedKey.KeyValue))
                    {
                        SelectedKeyValues.Clear();
                        SelectedKeyValues.Add(selectedKey.KeyValue);
                    }

                    _firstKeyValuesIndex.Clear();
                    foreach (var keyValue in SelectedKeyValues)
                    {
                        _firstKeyValuesIndex.Add(keyValue, keyValue.Frame);
                    }

                    _mouseMoved = true;
                }

                if (TglRecord.IsChecked == false)
                    TglRecord.IsChecked = true;

                if (_viewModel == null) return;

                _movedKeys.Clear();
                foreach (var selectedKeyValue in SelectedKeyValues)
                {
                    var nearestKey = GetNearestKey(e);

                    // Determinate of how many key frame changed 
                    var keyDelta = nearestKey - _downIndex;

                    if (keyDelta == 0)
                        continue;

                    var newKey = _firstKeyValuesIndex[selectedKeyValue] + keyDelta;
                    selectedKeyValue.Frame = newKey < 0 ? 0 : newKey;

                    _movedKeys.Add(selectedKeyValue);
                }

                e.Handled = true;
            }
        }

        private void Btn_TimelineKey_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedKey = (TimelineKey) sender;

            if (!selectedKey.IsMouseCaptured) return;


            if (e.ChangedButton == MouseButton.Right)
            {
                if (selectedKey.ContextMenu != null)
                    selectedKey.ContextMenu.IsOpen = true;
            }

            if (_viewModel == null) return;

            if (!_mouseMoved)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    if (SelectedKeyValues.Contains(selectedKey.KeyValue))
                        SelectedKeyValues.Remove(selectedKey.KeyValue);
                    else
                        SelectedKeyValues.Add(selectedKey.KeyValue);
                }
                else
                {
                    SelectedKeyValues.Clear();
                    SelectedKeyValues.Add(selectedKey.KeyValue);
                }
            }
            else
            {
                var lispTuple = new List<Tuple<KeyValue, int>>();
                foreach (var movedKey in _movedKeys)
                {
                    lispTuple.Add(new Tuple<KeyValue, int>(movedKey, movedKey.Frame));
                    movedKey.Frame = _firstKeyValuesIndex[movedKey];
                }

                TimelineModel.MoveKey(lispTuple);
            }

            selectedKey.ReleaseMouseCapture();
        }

        private int GetNearestKey(MouseEventArgs e)
        {
            var content = (Grid) SvKeyContainer.Content;
            var lineOffset = (int) e.GetPosition(content).X;
            lineOffset = lineOffset < 0 ? 0 : lineOffset;
            return (int) Math.Round(lineOffset / (_columnWidth * _zoom));
        }

        #endregion

        private void NumberValidationTextBox(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox)) return;
            var text = textBox.Text;
            var regex = new Regex("[0-9]+[fsm]?$");
            var flag = regex.IsMatch(text);
            e.Handled = !flag;
            if (!flag) return;

            // s mean second
            if (text.Contains("s"))
            {
                int.TryParse(text.Replace("s", ""), out var sec);
                textBox.Text = (sec * TimelineModel.FramePerSecond).ToString();
            }
            // m mean minuet
            else if (text.Contains("m"))
            {
                int.TryParse(text.Replace("m", ""), out var min);
                textBox.Text = (min * 60 * TimelineModel.FramePerSecond).ToString();
            }
            // f mean frame
            else if (text.Contains("f"))
                textBox.Text = text.Replace("f", "");
        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Equals(sender, SvKeyContainer))
            {
                SvRuler?.ScrollToHorizontalOffset(e.HorizontalOffset);
            }
            else if (Equals(sender, SvRuler))
            {
                SvKeyContainer?.ScrollToHorizontalOffset(e.HorizontalOffset);
            }

            SvChannelsContainer.ScrollToVerticalOffset(e.VerticalOffset);
            SvChannelsContainer.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void Sv_ChannelsContainer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            SvChannelsContainer.ScrollToVerticalOffset(SvChannelsContainer.VerticalOffset);
            SvChannelsContainer.ScrollToHorizontalOffset(SvChannelsContainer.HorizontalOffset);
        }

        private void TimelineKeyDelete_OnClick(object sender, RoutedEventArgs e)
        {
            TimelineModel.DeleteKey(SelectedKeyValues);
        }

        private void Btn_TimelineKey_OnSelected(object sender, bool e)
        {
            if (sender is TimelineKey timelineKey && e)
            {
                timelineKey.BringIntoView();
            }
        }

        private void FindNextKey(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is Channel channel)) return;

            var keyValues = channel;

            var index = SelectedKeyValues.Count > 0
                ? keyValues.NextKey(SelectedKeyValues[0].Frame, false)
                : keyValues.SortedDictionary.ElementAt(0).Item1;

            if (index < 0) return;

            SelectedKeyValues.Clear();
            SelectedKeyValues.Add(keyValues.GetKeyValue(index));
        }

        private void CaptureCurrentFrame(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is Channel channel)) return;

            TimelineModel.CaptureProperty(new ChannelAddress(channel.ObjectAddress, channel.MemberName, channel.Type));
        }

        private void FindPreviousKey(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(e.Parameter is Channel channel)) return;

            var keyValues = channel;

            var index = SelectedKeyValues.Count > 0
                ? keyValues.PreviousKey(SelectedKeyValues[0].Frame, false)
                : keyValues.SortedDictionary.ElementAt(keyValues.SortedDictionary.Count - 1).Item1;

            if (index < 0) return;

            SelectedKeyValues.Clear();
            SelectedKeyValues.Add(keyValues.GetKeyValue(index));
        }

        private void NewClip_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var inputDialog = new InputDialog("نام انیمیشن را وارد کنید.");
            if (inputDialog.ShowDialog() != true) return;
            var answer = inputDialog.Answer;
            TimelineModel.AddNewClip(answer);
        }

        private void CopyCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (_viewModel == null || SelectedKeyValues == null || SelectedKeyValues.Count == 0) return;

            _copiedKeyValues = SelectedKeyValues.Select(k =>
                    new KeyValuePair<ChannelAddress, KeyValuePair<int, object>>(
                        TimelineModel.RecordingClip.GetChannelAddress(k),
                        new KeyValuePair<int, object>(k.Frame, k.Val)))
                .ToList();
        }

        private void PasteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (_viewModel == null || _copiedKeyValues == null || _copiedKeyValues.Count == 0) return;

            var smallestKey = _copiedKeyValues.Min(c => c.Value.Key);

            var keyDelta = (int) TimelineModel.CurrentFrame - smallestKey;


            TimelineModel.AddKey(_copiedKeyValues.Select(s =>
                new KeyValuePair<ChannelAddress, KeyValuePair<int, object>>(s.Key,
                    new KeyValuePair<int, object>(s.Value.Key + keyDelta, s.Value.Value))));


            void GetNewSelectedKeys()
            {
                SelectedKeyValues.Clear();
                _copiedKeyValues.ForEach(k =>
                {
                    var findKeyValue =
                        TimelineModel.RecordingClip.GetKeyValue(k.Key, k.Value.Key + keyDelta);

                    SelectedKeyValues.Add(findKeyValue);
                });
            }

            ActionHelper.EndOfFrame(GetNewSelectedKeys);
        }

        #region Drag Selection

        private Point _mouseDownPos;


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var sP = e.GetPosition(SvKeyContainer);

            // resolve keyContainer drag conflict with keyContainer scrollbar
            if (sP.Y < SvKeyContainer.RenderSize.Height - 20 &&
                sP.X < SvKeyContainer.RenderSize.Width - 20 &&
                e.LeftButton == MouseButtonState.Pressed)
            {
                // Capture and track the mouse.
                _mouseDownPos = e.GetPosition(KeyGrid);
                KeyGrid.CaptureMouse();

                // Initial placement of the drag selection box.         
                Canvas.SetLeft(SelectionBox, _mouseDownPos.X);
                Canvas.SetTop(SelectionBox, _mouseDownPos.Y);
                SelectionBox.Width = 0;
                SelectionBox.Height = 0;

                // Make the drag selection box visible.
                SelectionBox.Visibility = Visibility.Visible;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (KeyGrid.IsMouseCaptured)
            {
                // When the mouse is held down, reposition the drag selection box.

                Point mousePos = e.GetPosition(KeyGrid);

                if (_mouseDownPos.X < mousePos.X)
                {
                    Canvas.SetLeft(SelectionBox, _mouseDownPos.X);
                    SelectionBox.Width = mousePos.X - _mouseDownPos.X;
                }
                else
                {
                    Canvas.SetLeft(SelectionBox, mousePos.X);
                    SelectionBox.Width = _mouseDownPos.X - mousePos.X;
                }

                if (_mouseDownPos.Y < mousePos.Y)
                {
                    Canvas.SetTop(SelectionBox, _mouseDownPos.Y);
                    SelectionBox.Height = mousePos.Y - _mouseDownPos.Y;
                }
                else
                {
                    Canvas.SetTop(SelectionBox, mousePos.Y);
                    SelectionBox.Height = _mouseDownPos.Y - mousePos.Y;
                }
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture and stop tracking it.
            if (!KeyGrid.IsMouseCaptured) return;

            KeyGrid.ReleaseMouseCapture();

            // Hide the drag selection box.
            SelectionBox.Visibility = Visibility.Collapsed;

            Point mouseUpPos = e.GetPosition(KeyGrid);

            var keys = FindVisualChildren<TimelineKey>(HgContainer);

            var startArea = new Point(Math.Min(_mouseDownPos.X, mouseUpPos.X), Math.Min(_mouseDownPos.Y, mouseUpPos.Y));
            var endArea = new Point(Math.Max(_mouseDownPos.X, mouseUpPos.X), Math.Max(_mouseDownPos.Y, mouseUpPos.Y));


            if(!Keyboard.IsKeyDown(Key.LeftCtrl))
                SelectedKeyValues.Clear();

            foreach (var timelineKey in keys)
            {
                var point = timelineKey.TranslatePoint(new Point(0, 0), KeyGrid);
                point = new Point(point.X + timelineKey.RenderSize.Width / 2,
                    point.Y + timelineKey.RenderSize.Height / 2);

                if (point.X >= startArea.X &&
                    point.Y >= startArea.Y &&
                    point.X <= endArea.X &&
                    point.Y <= endArea.Y && 
                    !SelectedKeyValues.Contains(timelineKey.KeyValue))
                    SelectedKeyValues.Add(timelineKey.KeyValue);
            }
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            var queue = new Queue<DependencyObject>(new[] {parent});

            while (queue.Any())
            {
                var reference = queue.Dequeue();
                var count = VisualTreeHelper.GetChildrenCount(reference);

                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    if (child is T children)
                        yield return children;

                    queue.Enqueue(child);
                }
            }
        }

        #endregion

        private static Animator GetAnimator(StoryObject storyObject)
        {
            if (storyObject == null) return null;
            var anim = storyObject.GetComponent<Animator>();

            if (anim != null) return anim;

            return GetAnimator(storyObject.transform.Parent?.StoryObject);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}