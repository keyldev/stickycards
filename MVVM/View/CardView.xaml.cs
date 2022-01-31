using System.Windows;
using System.Windows.Input;

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

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            bCardHeader.Height = 20;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            bCardHeader.Height = 5;
        }

        private void CloseCardButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
