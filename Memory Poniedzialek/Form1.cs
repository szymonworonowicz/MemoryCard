using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_Poniedzialek
{
    public partial class Form1 : Form
    {
        private GameSettings settings;
        public Form1()
        {
            InitializeComponent();
            settings = new GameSettings();
            UstawKontrolki();
            GenerujKarty();
        }

        private void UstawKontrolki()
        {
            panelKart.Width = settings.Kolumny * settings.Bok;
            panelKart.Height = settings.Wiersze * settings.Bok;

            Width = panelKart.Width + 40;
            Height = panelKart.Height + 130;

            lblStartInfo.Text = $"Gra rozpocznie sie za {settings.CzasPodgladu}";
            lblPunktyWartosc.Text = settings.AktualnePunkty.ToString();
            lblCzasWartosc.Text = settings.CzasGry.ToString();

            lblStartInfo.Visible = true;
        }

        private void GenerujKarty()
        {
            string[] memories = Directory.GetFiles(settings.FolderObrazki);

            settings.MaxPunkty = memories.Length;

            //torzymy liste do przetrzymywania elementow
            List<MemoryCard> buttons = new List<MemoryCard>();

            //iterujemy po obrazkach
            foreach (string path in memories)
            {
                //tworzymy unikalny identyfikator
                Guid id = Guid.NewGuid();
                //stworzenie pierwszj karty
                MemoryCard card1 = new MemoryCard(id, settings.PlikLogo, path);

                //dodajemy karte do listy
                buttons.Add(card1);

                //tworzymy 2 karte
                MemoryCard card2 = new MemoryCard(id, settings.PlikLogo, path);
                //dodajemy karte do listy
                buttons.Add(card2);
            }

            //tworzymmy generator liczb losowych
            Random random = new Random();

            //czyscimy panel z pozostalych elementow
            panelKart.Controls.Clear();

            //iterowanie po kolumnach
            for (int x = 0; x < settings.Kolumny; x++)
            {
                //iterowanie po wierszach
                for (int y = 0; y < settings.Wiersze; y++)
                {
                    //losujemy index karty
                    int index = random.Next(0, buttons.Count);

                    //wybieramy wylosowana karte
                    MemoryCard b = buttons[index];

                    //dajejmy margines
                    int margines = 2;

                    //obliczamy pozycje naszego kafelka na panelu kart
                    b.Location = new Point((x * settings.Bok), (y * settings.Bok));

                    //dodajemy margines 
                    b.Margin = new Padding(margines);

                    //ustawiamy pozycje
                    b.Width = settings.Bok;
                    b.Height = settings.Bok;

                    //standardowo jest odkryta
                    b.Odkryj();

                    //dodajemy kontrolke do panulu gry
                    panelKart.Controls.Add(b);

                    //juz wylosowana wiec wyrzucamy
                    buttons.Remove(b);

                }
            }
        }
    }
}
