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
        Socket clientSocket;
        bool windowInitialized;
        bool isClient;

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
                // Initialize client

                IPAddress address = IPAddress.Parse(AddressText.Text);
                int port = Int32.Parse(PortText.Text);
                IPEndPoint endPoint = new IPEndPoint(address, port);
                clientSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(endPoint);

                //clientStream = new NetworkStream(clientSocket);
                //clientWriter = new StreamWriter(clientStream, Encoding.UTF8);
                //clientReader = new StreamReader(clientStream, Encoding.UTF8);

                //ChatText.Text += "[Client] Connected to server " + clientSocket.RemoteEndPoint.ToString() + "\n";
                Console.WriteLine("Cliente conectado");
                //Background = Brushes.Orchid;
                isClient = true;

                PoseCombo.IsEnabled = true;
                ReadyButton.IsEnabled = true;
                
                StartButton.Visibility = Visibility.Hidden;
                StopButton.Visibility = Visibility.Visible;
                
                YouLabel.Visibility = Visibility.Visible;
                RivalLabel.Visibility = Visibility.Visible;

                YouScoreLabel.Visibility = Visibility.Visible;
                RivalScoreLabel.Visibility = Visibility.Visible;

                Background = Brushes.Green;



                //ShowDisconnectButtonsForClient();
                //ResetCells();

                // Wait for server greeting

                //EnableInputCommands(false);
                //ForceRepaint();
                //string line = clientReader.ReadLine();

                // Process server greeting

                //ProcessCommand(line);

                // It's our turn to send commands
                // enable UI so player decide which command to send

                //EnableInputCommands(true);

            }
            catch (Exception exception)
            {
                //ChatText.Text += "[Client] Error: " + exception.ToString() + "\n";
                Console.WriteLine(exception.ToString());
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clientSocket.Close();

                PoseCombo.IsEnabled = false;
                ReadyButton.IsEnabled = false;

                StartButton.Visibility = Visibility.Visible;
                StopButton.Visibility = Visibility.Hidden;
                Background = Brushes.Gray;
            }
            catch 
            { 
                Console.WriteLine("Error");
            }
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            // Envio mi ready
            byte[] data = new byte[1];
            data[0] = 7;
            clientSocket.Send(data);

            // Espero su ready

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

            data[0] = (byte)youPose;
            clientSocket.Send(data);

            // Recibo su movimiento

            clientSocket.Receive(data);

            if (clientSocket.Receive(data) == 0)
            {
                rivalPose = 0;
            }
            else if (clientSocket.Receive(data) == 1)
            {
                rivalPose = 1;
            }
            else
            {
                rivalPose = 2;
            }
            Console.WriteLine("Recibiste: " + data[0]);
        }


        private void PoseCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!windowInitialized) { return; }

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