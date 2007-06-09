using System.Windows.Automation.Peers;
using System.Windows.Media;
using System.Windows.Shapes;
#if Implementation
using System.Windows;
using System.Windows.Controls;
using Mono.System.Windows.Controls.Primitives;
namespace Mono.System.Windows.Controls {
#else
using System.Windows.Controls.Primitives;
namespace System.Windows.Controls {
#endif
	[TemplatePart(Name = "PART_Track", Type = typeof(FrameworkElement))]
	[TemplatePart(Name = "PART_Indicator", Type = typeof(FrameworkElement))]
	public class ProgressBar : RangeBase {
		#region Public Fields
		#region Dependency Properties
		public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate", typeof(bool), typeof(ProgressBar), new FrameworkPropertyMetadata());
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ProgressBar), new FrameworkPropertyMetadata());

		#endregion
		#endregion

		#region Static Constructor
		static ProgressBar() {
			Theme.Load();
		}
		#endregion

		#region Public Constructors
		public ProgressBar() {
			//FIXME: I should not do this.
			Style = (Style)FindResource(typeof(ProgressBar));

			Maximum = 100;
		}
		#endregion

		#region Dependency Properties
		public bool IsIndeterminate {
			get { return (bool)GetValue(IsIndeterminateProperty); }
			set { SetValue(IsIndeterminateProperty, value); }
		}

		public Orientation Orientation {
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}
		#endregion

		#region Public Methods
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			SetIndicatorPosition();
		}
		#endregion

		#region Protected Methods
		protected override AutomationPeer OnCreateAutomationPeer() {
#if Implementation
			return null;
#else
			return new ProgressBarAutomationPeer(this);
#endif
		}

		protected override void OnValueChanged(double oldValue, double newValue) {
			base.OnValueChanged(oldValue, newValue);
			SetIndicatorPosition();
		}
		#endregion

		#region Private Methods
		void SetIndicatorPosition() {
			FrameworkElement track = (FrameworkElement)GetTemplateChild("PART_Track");
			if (track == null)
				return;
			((FrameworkElement)GetTemplateChild("PART_Indicator")).Width = track.ActualWidth * Value / Maximum;
		}
		#endregion
	}
}