using helloworld.Core;
using helloworld.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helloworld.MVVM.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<object> _notesList;

        public ObservableCollection<object> NotesList
        {
            get { return _notesList; }
            set { _notesList = value; NotifyPropertyChanged(); }
        }
        public RelayCommand AddNoteCommand { get; set; }
        public MainWindowViewModel()
        {
            // доработать, если дойдут руки, не добавляется в список(view) добавленных ноутов (сохранения нэбудэ)
            NotesList = new ObservableCollection<object>();

            AddNoteCommand = new RelayCommand(o =>
            {
                CardView card = new CardView();
                card.Show();
                // переделать (решить проблему)
                NotesList.Add("Added card #" + NotesList.Count);
            });
        }
    }
}
