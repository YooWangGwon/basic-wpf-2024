using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex05_wpf_bikeshop
{
    internal class Notifier
    {
    // 우리가 만드는 클래스의 프로퍼티(속성값)이 변경되는 것을 알려주는 이벤트 핸들러
        public event PropertyChangedEventHandler? PropertyChanged;


        // 프로퍼티가 변경되었어요!!
        protected void OnPropertyChanged(string propertyName) // protected : Notifier과 Notifier을 상속받은 클래스만 사용가능
        {
            if(PropertyChanged != null) // 프로퍼티가 변경된 경우
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
