using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GamePicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] games, gamesAfter;
        string tmp1, tmp2;
        string path, chosen, game;
        int miliseconds = 50;
        int number;

        Random random = new Random();
        List<int> randomNumbers = new List<int>();
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            string directory = Directory.GetCurrentDirectory();
            this.path = directory + @"\Gamelist.txt";
            this.games = File.ReadAllLines(path);

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(miliseconds);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btn.Content is "Fill list")
            {
                Game1.Text = games[RandomNumber()];
                Game2.Text = games[RandomNumber()];
                Game3.Text = games[RandomNumber()];
                Game4.Text = games[RandomNumber()];
                Game5.Text = games[RandomNumber()];
                randomNumbers.Clear();

                btn.Content = "Shuffle";
            }
            else if (btn.Content is "Shuffle")
            {
                dispatcherTimer.Start();

                btn.Content = "Stop";
            }
            else
            {
                dispatcherTimer.Stop();
                setAsPlayed();
                checkIfPlayed();
                this.games = File.ReadAllLines(path);

                btn.Content = "Fill list";
            }
        }

        private int RandomNumber()
        {
            number = random.Next(games.Length);
 
            if (randomNumbers.Contains(number))
            {
                RandomNumber();
            }
            else
            {
                randomNumbers.Add(number);
            }

            return number;
        }


        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            tmp1 = Game2.Text;
            Game2.Text = Game1.Text;
            tmp2 = Game3.Text;
            Game3.Text = tmp1;
            tmp1 = Game4.Text;
            Game4.Text = tmp2;
            tmp2 = Game5.Text;
            Game5.Text = tmp1;
            Game1.Text = tmp2;
        }

        private void setAsPlayed()
        {
            chosen = Game3.Text;

            for (int i=0; i<games.Length; i++)
            {
                if (games[i] == chosen)
                {
                    games[i] = "<Played> "+chosen;
                }
            }

            File.WriteAllLines(path, games);
        }

        private void checkIfPlayed()
        {
            gamesAfter=new string[(games.Length)-1];
            int j = 0;

            for (int i=0; i<games.Length; i++)
            {
                if (games[i].StartsWith("<Played>")==false)
                {
                    gamesAfter[j] = games[i];
                    j++;
                }
            }

            File.WriteAllLines(path, gamesAfter);
        }
    }
}
