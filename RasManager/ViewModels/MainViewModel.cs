using DotRas;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RasManager.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private RasDialer _dialer;
        private RasPhoneBook _phonebook;

        public ICommand AutoConnectCommand { get; set; }
        public ICommand StopAutoConnectCommand { get; set; }

        public ReactiveList<RasEntry> Entries { get; set; }

        private RasEntry _selecteEntry;
        public RasEntry SelectedEntry
        {
            get { return _selecteEntry; }
            set { this.RaiseAndSetIfChanged(ref _selecteEntry, value); }
        }

        private bool _isStarted;
        public bool IsStarted
        {
            get { return _isStarted; }
            set
            {
                this.RaiseAndSetIfChanged(ref _isStarted, value);               
            }
        }

        public MainViewModel()
        {
            InitializeRas();

            Entries = new ReactiveList<RasEntry>(_phonebook.Entries.ToList());
            AutoConnectCommand = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(x => x.SelectedEntry, e => e.Value != null)
                , async _ =>
                {
                    await Task.Delay(1);
                    IsStarted = true;
                });

            StopAutoConnectCommand = ReactiveCommand.CreateAsyncTask(
                async _ =>
                {
                    await Task.Delay(1);
                    IsStarted = false;
                });
        }

        private void InitializeRas()
        {
            _phonebook = new RasPhoneBook();
            _dialer = new RasDialer();
            _phonebook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
        }
    }
}
