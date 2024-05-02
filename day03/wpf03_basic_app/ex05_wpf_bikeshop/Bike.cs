﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ex05_wpf_bikeshop
{
    // Notifire를 상속받음녀 AutoProperty {get; set;}은 사용할 수 없음
    internal class Bike : Notifier
    {
        private double speed;
        private Color color;

        public double Speed {   // 속성
            get { return speed; }
            set { speed = value;
                  // 속성 값이 변경되는 것을 알려주려면 이 작업이 반드시 필요함
                  OnPropertyChanged(nameof(Speed));  // "Speed"
            }
        }

        public Color Color {
            get { return color; }
            set { color = value; 
                OnPropertyChanged(nameof (Color));   
            }
        }
    }
}
