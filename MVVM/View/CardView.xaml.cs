using System.Windows;

namespace helloworld.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для CardView.xaml
    /// </summary>
    public partial class CardView : Window
    {

        public CardView()
        {
            InitializeComponent();


            bCardHeader.MouseLeftButtonDown += (o, e) =>
            {
                this.DragMove();
            };

        }

        private void CloseCardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

    }
}
