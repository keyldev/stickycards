using helloworld.Core;
using helloworld.MVVM.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace helloworld.MVVM.ViewModel
{
    internal class CardViewModel : ObservableObject
    {
        DispatcherTimer timer;
        // Controls collection (TextBoxes, Images etc)
        private ObservableCollection<object> _myControlItems;
        public ObservableCollection<object> MyControlItems
        {
            get { return _myControlItems; }
            set { _myControlItems = value; NotifyPropertyChanged(); }
        }

        #region TECHPROPS
        // card status (over all windows)
        private bool _isTopmost = false;
        public bool isTopmost
        {
            get { return _isTopmost; }
            set { _isTopmost = value; NotifyPropertyChanged(); }
        }

        // popup status (open/not open)
        private bool _isOptionOpen = false;
        public bool IsOptionOpen
        {
            get { return _isOptionOpen; }
            set { _isOptionOpen = value; NotifyPropertyChanged(); }
        }

        // header status (pinned/unpinned)
        private string _noteHeader = "Notes(unpinned)";
        public string NoteHeader
        {
            get { return _noteHeader; }
            set { _noteHeader = value; NotifyPropertyChanged(); }
        }
        //header color
        private string _backgroundColor = "#ffc2c1";
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; NotifyPropertyChanged(); }
        }
        private bool _openTimerSettings = false;

        public bool OpenTimerSettings
        {
            get { return _openTimerSettings; }
            set { _openTimerSettings = value; NotifyPropertyChanged(); }
        }

        private string _timerMinutes = "5";

        public string TimerMinutes
        {
            get { return _timerMinutes; }
            set { _timerMinutes = value; NotifyPropertyChanged(); }
        }

        #endregion

        #region COLORS
        string[] colors = {
            "#ff25cc",
            "#2211AA",
            "#3eb489",
            "#cc00ff",
            "#ff2b2b",
            "#ffc2c1",
            "#66A3A3",
            "#00bfff",
            "#22ccaa"
        };
        private ObservableCollection<object> _colorsList;
        public ObservableCollection<object> ColorsList
        {
            get { return _colorsList; }
            set { _colorsList = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region ButtonCMDS
        public RelayCommand AddTextBoxCommand { get; set; }
        public RelayCommand SetItalicText { get; set; }
        public RelayCommand SetUnderlineText { get; set; }
        public RelayCommand AddImage { get; set; }
        public RelayCommand SetFontWeight { get; set; }
        public RelayCommand AddNoteCommand { get; set; }
        public RelayCommand PinCard { get; set; }
        public RelayCommand AboutAuthor { get; set; } // #to fix
        public RelayCommand ShowOptionsDialog { get; set; }
        public RelayCommand CheckAppUpdates { get; set; }
        public RelayCommand ShowTimerSettings { get; private set; }
        public RelayCommand StartTimer { get; set; }

        #endregion

        // вынести функционал из конструктора
        // добавить методы инициализации команд и т.п.
        public CardViewModel()
        {
            try
            {
                MyControlItems = new ObservableCollection<object>(); // коллекция объектов типа object (если кто увидит - пипяу) #change
                ShowOptionsDialog = new RelayCommand(o => IsOptionOpen = (IsOptionOpen) ? false : true);
                initializeColors();

                PinCard = new RelayCommand((o) =>
                {
                    if (isTopmost)
                    {
                        isTopmost = false;
                        NoteHeader = "Note(unpinned)"; // #to fix
                    }
                    else
                    {
                        isTopmost = true;
                        NoteHeader = "Note(pinned)";// #to fix
                    }
                });
                AddNoteCommand = new RelayCommand(o =>
                {
                    CardView card = new CardView();
                    card.Show();
                });                                               // Добавить текстовое поле
                AddTextBoxCommand = new RelayCommand(o =>
                {
                    MyControlItems.Add(new RichTextBox()
                    {
                        BorderThickness = new Thickness(0),
                        Background = null,
                        Foreground = Brushes.White,
                        ToolTip = "Введите текст",
                        CaretBrush = Brushes.White,
                        FontSize = 14,
                        Document = new FlowDocument(new Paragraph(new Run("Давным-давно.."))),
                    });

                });
                // курсив
                SetItalicText = new RelayCommand((o) =>
                {
                    RichTextBox textSelection = null;

                    foreach (var item in MyControlItems)
                    {
                        if (item is RichTextBox)
                            textSelection = (RichTextBox)item;
                    }
                    TextSelection text = textSelection.Selection;
                    if (!text.IsEmpty)
                    {
                        try
                        {
                            text.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                });
                // подчеркнутый текст
                SetUnderlineText = new RelayCommand((o) =>
                {
                    RichTextBox textSelection = null;
                    foreach (var item in MyControlItems)
                    {
                        if (item is RichTextBox)
                            textSelection = (RichTextBox)item;
                    }
                    TextSelection text = (TextSelection)textSelection.Selection;
                    if (!text.IsEmpty)
                    {
                        text.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                    }
                });
                // картинка
                AddImage = new RelayCommand((o) =>
                {
                    string pathToImage = null;
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.ShowDialog();
                    pathToImage = openFileDialog.FileName;

                    MyControlItems.Add(new Image()
                    {
                        Source = new BitmapImage(new Uri(pathToImage)),
                    });
                });
                // жирность
                SetFontWeight = new RelayCommand(o =>
                {
                    RichTextBox textSelection = null;
                    foreach (var item in MyControlItems)
                    {
                        if (item is RichTextBox)
                            textSelection = (RichTextBox)item;
                    }
                    TextSelection text = textSelection.Selection;
                    if (!text.IsEmpty)
                    {
                        try
                        {
                            text.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                });

                AboutAuthor = new RelayCommand((o) =>
                {
                    MessageBox.Show("Автор: keyldev;\nGit:keyldev;\nGood luck!",
                        "About",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information); // #fix
                });
                CheckAppUpdates = new RelayCommand((o) =>
                {
                    //WebClient web = new WebClient(); // #create app update

                });
                ShowTimerSettings = new RelayCommand(o =>
                {
                    OpenTimerSettings = OpenTimerSettings ? false : true;
                });
                StartTimer = new RelayCommand((o) =>
                {
                    timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, int.Parse(TimerMinutes), 0);
                    timer.Tick += Timer_Tick;
                    timer.Start();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //#update
        private void Timer_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Время таймера закончилось.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            timer.Stop();
        }

        private void initializeColors()
        {
            ColorsList = new ObservableCollection<object>();
            for (int i = 0; i < colors.Length; i++)
            {
                ColorsList.Add(new Button()
                {
                    Width = 30,
                    Height = 30,
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(colors[i]))
                });
                foreach (Button item in ColorsList)
                    item.Click += (o, e) =>
                    {
                        BackgroundColor = item.Background.ToString();
                    };
            }
        }
    }
}
