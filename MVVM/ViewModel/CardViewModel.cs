using helloworld.Core;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace helloworld.MVVM.ViewModel
{
    internal class CardViewModel : ObservableObject
    {
        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<object> _myControlItems;
        public ObservableCollection<object> MyControlItems
        {
            get { return _myControlItems; }
            set { _myControlItems = value; NotifyPropertyChanged(); }
        }
        string[] colors = {
            "#ff25cc",
            "#2211AA",
            "#000000",
            "#cc00ff",
            "#ff0000",
            "#ffc2c1",
            "#22ccaa"
        };
        Random rnd;

        #region CMDS
        public RelayCommand AddTextBoxCommand { get; set; }
        public RelayCommand SetItalicText { get; set; }
        public RelayCommand SetUnderlineText { get; set; }
        public RelayCommand AddImage { get; set; }
        public RelayCommand SetFontWeight { get; set; }
        #endregion

        // вынести функционал из конструктора
        // добавить методы инициализации команд и т.п.
        public CardViewModel()
        {
            MyControlItems = new ObservableCollection<object>(); // коллекция объектов типа object (если кто увидит - пипяу)

            rnd = new Random(); 
            int value = rnd.Next(0, 6);
            BackgroundColor = colors[value]; // колхозный рандом цветов из стрингового массива (#change)
            // Добавить текстовое поле
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
                    Source = new BitmapImage(new Uri(@"C:\Users\vasya\Pictures\beauti_gif.gif")),
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
        }
    }
}
