using Example_MVVM_TinhToanDam.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Example_MVVM_TinhToanDam.ViewModel
{
    public class BeamViewModel : INotifyPropertyChanged
    {
        // Các biến lưu giá trị nhập từ màn hình
        private double _length = 4.0;
        private double _load = 15.0;
        private double _width = 0.22;
        private double _height = 0.40;
        private double _rb = 11500;   // B20 (11.5 MPa -> 11500 kN/m2)
        private double _rs = 280000;  // CB300 (280 MPa -> 280000 kN/m2)

        public double Length { get => _length; set { _length = value; OnPropertyChanged(); } }
        public double Load { get => _load; set { _load = value; OnPropertyChanged(); } }
        public double Width { get => _width; set { _width = value; OnPropertyChanged(); } }
        public double Height { get => _height; set { _height = value; OnPropertyChanged(); } }
        public double Rb { get => _rb; set { _rb = value; OnPropertyChanged(); } }
        public double Rs { get => _rs; set { _rs = value; OnPropertyChanged(); } }

        // Danh sách chọn hệ số mô-men (Theo trang 94 sách Sàn BTCT)
        // Key: Tên hiển thị, Value: Hệ số k (M = ql^2/k)
        public Dictionary<string, double> MomentOptions { get; } = new Dictionary<string, double>
        {
            { "Dầm đơn giản (k=8)", 8.0 },
            { "Biên: Nhịp/Gối (k=11)", 11.0 },
            { "Giữa: Nhịp/Gối (k=16)", 16.0 }
        };

        private KeyValuePair<string, double> _selectedOption;
        public KeyValuePair<string, double> SelectedOption
        {
            get => _selectedOption;
            set { _selectedOption = value; OnPropertyChanged(); }
        }

        // Danh sách dầm hiển thị trên bảng
        public ObservableCollection<BeamModel> Beams { get; set; }

        // Các lệnh (Command)
        public ICommand AddBeamCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }

        // Constructor
        public BeamViewModel()
        {
            Beams = new ObservableCollection<BeamModel>();

            // Chọn mặc định là Dầm đơn giản
            SelectedOption = MomentOptions.First();

            AddBeamCommand = new RelayCommand(AddBeam);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
        }

        private void AddBeam()
        {
            var newBeam = new BeamModel
            {
                Id = (Beams.Count + 1).ToString(),
                Length = this.Length,
                Load = this.Load,
                Width = this.Width,
                Height = this.Height,
                Rb = this.Rb,
                Rs = this.Rs,
                MomentCoeff = SelectedOption.Value,
                PositionName = SelectedOption.Key
            };

            Beams.Add(newBeam);
        }

        private void ExportToExcel()
        {
            // Logic xuất Excel sẽ được thêm vào sau
            MessageBox.Show("Đã nhận lệnh xuất Excel!", "Thông báo");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}