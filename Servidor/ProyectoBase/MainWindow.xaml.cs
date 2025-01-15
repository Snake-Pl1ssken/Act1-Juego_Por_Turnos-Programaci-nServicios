using System.IO;
using System.Net;
using System.Net.Sockets;
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
        bool isClient;
        bool windowInitialized;

        Socket clientSocket;
        NetworkStream clientStream;
        StreamWriter clientWriter;
        StreamReader clientReader;

        Socket serverSocket;
        Socket serviceSocket;
        NetworkStream serviceStream;
        StreamWriter serviceWriter;
        StreamReader serviceReader;
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
            try
            {
                IPAddress address = IPAddress.Parse(AddressText.Text);
                int port = Int32.Parse(PortText.Text);
                IPEndPoint endPoint = new IPEndPoint(address, port);
                serverSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(endPoint);
                serverSocket.Listen();
                isClient = false;
                Console.WriteLine("ServConnected");

                serviceSocket = serverSocket.Accept();
                Console.WriteLine("ClienteAceptado");

                PoseCombo.IsEnabled = true;
                

                StartButton.Visibility = Visibility.Hidden;
                StopButton.Visibility = Visibility.Visible;
                Background = Brushes.Blue;
                ForceRepaint();

                // Espero su ready

                byte[] data = new byte[1];
                serviceSocket.Receive(data);
                Console.WriteLine("Recibiste:" + data[0]);
                if (data[0] == 7)
                { 
                    ReadyButton.IsEnabled = true;
                }
            }
            catch(Exception escepcion) { Console.WriteLine(escepcion.ToString()); }
            

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            serverSocket.Close();
            Background = Brushes.Gray;
            PoseCombo.IsEnabled = false;
            ReadyButton.IsEnabled = false;
            StartButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Hidden;
            Console.WriteLine("sERVIDORcERRADO");
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            // Envio mi ready

            byte[] data = new byte[1];
            data[0] = 7;
            serviceSocket.Send(data);

            // Espero su movimiento



            // Envio mi movimiento
            if (PoseCombo.SelectedIndex == 0)
            {
                youPose = 0;
            }
            else if (PoseCombo.SelectedIndex == 1) 
            {
                youPose = 1;
            }
            else 
            {
                youPose = 2;
            }
            
            byte[] PoseB = new byte[1];
            PoseB[0] = Convert.ToByte(youPose);
            serviceSocket.Send(PoseB);
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