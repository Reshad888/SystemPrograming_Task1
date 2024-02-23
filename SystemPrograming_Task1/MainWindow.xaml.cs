using System.Diagnostics;
using System.Text;
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

namespace SystemPrograming_Task1
{
    public partial class MainWindow : Window
    {
        public DispatcherTimer Timer;

        public List<string> prcs = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);

            timer.Tick += Timer_Tick;

            timer.Start();


            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(3);

            Timer.Tick += (object sender, EventArgs e) =>
            {
                var allProcesses = Process.GetProcesses().ToList();

                foreach (var item in prcs)
                {
                    foreach (var process in allProcesses)
                    {
                        if (item == process.ProcessName)
                        {
                            process.Kill();
                        }
                    }
                }
            };

            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            List_View.Items.Clear();


            var allProcesses = Process.GetProcesses().ToList();


            List<Process_> processes = new List<Process_>();

            foreach (var process in allProcesses)
            {
                Process_ process_ = new Process_
                {
                    Id = process.Id,
                    ProcessName = process.ProcessName,
                    MachineName = process.MachineName,
                    HandleCount = process.HandleCount,
                    ThreadCount = process.Threads.Count
                };

                processes.Add(process_);
            }

            foreach (var process in processes)
            {
                List_View.Items.Add(process);
            }


            GridView gridView = new GridView();

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id",
                DisplayMemberBinding = new Binding("Id")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Process Name",
                DisplayMemberBinding = new Binding("ProcessName")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Machine Name",
                DisplayMemberBinding = new Binding("MachineName")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Handle Count",
                DisplayMemberBinding = new Binding("HandleCount")
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Thread Count",
                DisplayMemberBinding = new Binding("ThreadCount")
            });


            List_View.View = gridView;
        }

        private void ButtonKillProcess_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = List_View.SelectedItems.Cast<Process_>().ToList();

            foreach (var selectedItem in selectedItems)
            {
                Process.GetProcessById(selectedItem.Id).Kill();
            }
        }

        private void ButtonCreateProcess_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(TextBoxProcess.Text);
            TextBoxProcess.Text = null;
        }


        private void ButtonBlackListProcess_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = List_View.SelectedItems.Cast<Process_>().ToList();

            foreach (var selectedItem in selectedItems)
            {
                prcs.Add(selectedItem.ProcessName);

                Process.GetProcessById(selectedItem.Id).Kill();
            }
        }
    }
}