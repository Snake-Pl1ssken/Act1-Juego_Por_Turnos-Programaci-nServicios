using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace RockScissorsPaper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int youScore;
        int rivalScore;
        int youPose;
        int rivalPose;

        bool windowInitialized;

        public MainWindow()
        {
            InitializeComponent();

            // Set everything to initial state

            YouLabel.Visibility = Visibility.Hidden;
            RivalLabel.Visibility = Visibility.Hidden;

            YouScoreLabel.Visibility = Visibility.Hidden;
            RivalScoreLabel.Visibility = Visibility.Hidden;

            YouRockImage.Visibility = Visibility.Hidden;
            YouPaperImage.Visibility = Visibility.Hidden;
            YouScissorsImage.Visibility = Visibility.Hidden;

            RivalRockImage.Visibility = Visibility.Hidden;
            RivalPaperImage.Visibility = Visibility.Hidden;
            RivalScissorsImage.Visibility = Visibility.Hidden;
            RivalThinkingImage.Visibility = Visibility.Hidden;

            PoseCombo.IsEnabled = false;
            ReadyButton.IsEnabled = false;

            Background =  Brushes.Gray;
            StartButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Hidden;

            windowInitialized = true;

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void PoseCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!windowInitialized) { return; }

            YouRockImage.Visibility = Visibility.Hidden;
            YouPaperImage.Visibility = Visibility.Hidden;
            YouScissorsImage.Visibility = Visibility.Hidden;

            if(PoseCombo.SelectedIndex == 0)
            {
                YouRockImage.Visibility = Visibility.Visible;
            }
            else if(PoseCombo.SelectedIndex == 1)
            {
                YouPaperImage.Visibility = Visibility.Visible;
            }
            else // PoseCombo.SelectedIndex == 2
            {
                YouScissorsImage.Visibility = Visibility.Visible;
            }
        }

        void ForceRepaint()
        {
            // This is a hack to repaint the window before a blocking call

            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                return null;
            }), null);

            Dispatcher.PushFrame(frame);

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
        }

    }
}