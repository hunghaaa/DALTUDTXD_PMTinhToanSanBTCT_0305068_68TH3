using System;

namespace Example_MVVM_TinhToanDam.Model
{
    public class BeamModel
    {
        public string Id { get; set; }

        // Thông số hình học và tải trọng
        public double Length { get; set; } // Nhịp L (m)
        public double Load { get; set; }   // Tải trọng q (kN/m)
        public double Width { get; set; }  // Bề rộng b (m)
        public double Height { get; set; } // Chiều cao h (m)
        public double Cover { get; set; } = 0.025; // Lớp bảo vệ a (m), mặc định 2.5cm

        // Thông số vật liệu (Mặc định B20, CB300-V)
        public double Rb { get; set; } // Cường độ bê tông (kN/m2)
        public double Rs { get; set; } // Cường độ cốt thép (kN/m2)

        // Hệ số tính momen (k trong công thức M = ql^2/k)
        // 8: Dầm đơn giản, 11: Biên, 16: Giữa
        public double MomentCoeff { get; set; }
        public string PositionName { get; set; } // Tên vị trí (VD: Nhịp biên)

        // --- TÍNH TOÁN LOGIC ---

        // 1. Momen tính toán: M = (q * l^2) / k
        public double CalculatedMoment => (Load * Math.Pow(Length, 2)) / MomentCoeff;

        // 2. Chiều cao làm việc: h0 = h - a
        public double H0 => Height - Cover;

        // 3. Hệ số Alpha_m = M / (Rb * b * h0^2)
        public double AlphaM
        {
            get
            {
                if (Rb <= 0 || Width <= 0 || H0 <= 0) return 0;
                return CalculatedMoment / (Rb * Width * Math.Pow(H0, 2));
            }
        }

        // 4. Kiểm tra điều kiện phá hoại giòn (Alpha_m <= Alpha_R)
        // Alpha_R thường khoảng 0.433 (CB300) đến 0.39 (CB400)
        public bool IsSafe => AlphaM <= 0.433;

        // 5. Tính diện tích cốt thép As (cm2)
        // As = M / (Rs * Zeta * h0)
        public double RequiredAs
        {
            get
            {
                if (!IsSafe || AlphaM == 0) return 0;

                // Tính Zeta (gần đúng Zeta = 1 - 0.5 * Ksi)
                // Ksi = 1 - sqrt(1 - 2*AlphaM)
                double ksi = 1 - Math.Sqrt(1 - 2 * AlphaM);
                double zeta = 1 - 0.5 * ksi;

                // Tính As (m2)
                double asM2 = CalculatedMoment / (Rs * zeta * H0);

                // Đổi ra cm2
                return asM2 * 10000;
            }
        }
    }
}