using GenmCloud.Core.UserControls.Common.ViewModels;
using GenmCloud.Shared.Common;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GenmCloud.Core.UserControls.Common.Views
{
    public class GridLengthAnimation : AnimationTimeline
    {
        public static readonly DependencyProperty FromProperty;
        public static readonly DependencyProperty ToProperty;
        public static readonly DependencyProperty EasingFunctionProperty;

        static GridLengthAnimation()
        {
            FromProperty = DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));
            ToProperty = DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));
            EasingFunctionProperty = DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation));
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override Type TargetPropertyType
        {
            get { return typeof(GridLength); }
        }

        public IEasingFunction EasingFunction
        {
            get
            {
                return (IEasingFunction)GetValue(GridLengthAnimation.EasingFunctionProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.EasingFunctionProperty, value);
            }

        }

        public GridLength From
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.FromProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.FromProperty, value);
            }
        }

        public GridLength To
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.ToProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.ToProperty, value);
            }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = ((GridLength)GetValue(GridLengthAnimation.FromProperty)).Value;
            double toValue = ((GridLength)GetValue(GridLengthAnimation.ToProperty)).Value;

            IEasingFunction easingFunction = this.EasingFunction;

            double progress = (easingFunction != null) ? easingFunction.Ease(animationClock.CurrentProgress.Value) : animationClock.CurrentProgress.Value;

            if (fromValue > toValue)
            {
                return new GridLength((1 - progress) * (fromValue - toValue) + toValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength((progress) * (toValue - fromValue) + fromValue, this.To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
    }

    /// <summary>
    /// UploadInfoPopup.xaml 的交互逻辑
    /// </summary>
    public partial class UploadInfoPopup : UserControl
    {
        private bool isFold { get; set; }

        private readonly IEventAggregator eventAggregator;

        public UploadInfoPopup()
        {
            InitializeComponent();
            eventAggregator = NetCoreProvider.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<UploadInfoPopupViewModel.UploadHeightEvent>().Subscribe(() => 
            {
                double gridHeight = 0;
                if (DataContext is UploadInfoPopupViewModel vm)
                {
                    if (!vm.IsOpenPopup) return;
                    gridHeight = Math.Min(248, vm.UploadFileItemList.Count * 52 + 40);
                }
                GridLengthAnimation d = new GridLengthAnimation
                {
                    From = new GridLength(grid.RowDefinitions[1].ActualHeight, GridUnitType.Pixel),
                    To = new GridLength(gridHeight, GridUnitType.Pixel),
                    Duration = TimeSpan.FromSeconds(0.15)
                };
                grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);
            });
            listsControl.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
        }

        private void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            scrollViewer?.ScrollToBottom();
        }

        private void OpenPopup(object sender, System.Windows.RoutedEventArgs e)
        {
            double gridHeight = 0;
            if (DataContext is UploadInfoPopupViewModel vm)
            {
                gridHeight = Math.Min(248, vm.UploadFileItemList.Count * 52 + 40);
            }
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(0, GridUnitType.Pixel),
                To = new GridLength(gridHeight, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(0.15)
            };
            grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);

        }

        private void ShrinkPopop(object sender, RoutedEventArgs e)
        {
            GridLengthAnimation d = new GridLengthAnimation
            {
                From = new GridLength(grid.RowDefinitions[1].ActualHeight, GridUnitType.Pixel),
                To = new GridLength(0, GridUnitType.Pixel),
                Duration = TimeSpan.FromSeconds(0.15),
            };
            grid.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, d);
        }
    }
}
