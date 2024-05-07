using Caliburn.Micro;
using ex07_EmployeeMngApp.Helpers;
using ex07_EmployeeMngApp.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace ex07_EmployeeMngApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        private IDialogCoordinator _dialogCoordinator; // 메트로 방식의 다이얼로그 디자인을 적용 객체
        // 멤버 변수
        private int id;
        private string empName;
        private decimal salary;
        private string addr;
        private string deptName;

        // DataGrid에 뿌릴 Employees 테이블 데이터
        // MVVM처럼 List사용못함
        private BindableCollection<Employees> listEmployees;

        private Employees selectedEmployee;
        public Employees SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                selectedEmployee = value;
                // 데이터를 TextBox들에 전달

                if(selectedEmployee != null)
                {
                    Id = value.Id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Addr = value.Addr;

                    NotifyOfPropertyChange(() => SelectedEmployee); // View에 데이터가 표시되려면 필수!!!
                    NotifyOfPropertyChange(()=>Id);
                    NotifyOfPropertyChange(()=>EmpName);
                    NotifyOfPropertyChange(()=>Salary);
                    NotifyOfPropertyChange(()=>DeptName);
                    NotifyOfPropertyChange(()=>Addr);
                }
            }
        }

        // 속성
        public int Id
        {
            get => id;
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
                NotifyOfPropertyChange(() => CanDelEmployee);// 변경함을 공지
            }
        }
        public string EmpName
        {
            get => empName; 
            set
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public decimal Salary
        {
            get => salary; 
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public string DeptName
        {
            get => deptName; 
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public string Addr
        {
            get => addr; 
            set
            {
                addr = value;
                NotifyOfPropertyChange(() => Addr);
            }
        }


        // List는 일반적인 정적인 컬렉션, Bindalble
        public  BindableCollection<Employees> ListEmployees
        {
            get => listEmployees;
            set
            {
                listEmployees = value;
                // 값이 변경된 것을 시스템에 알려주는 온프로퍼티체인지
                NotifyOfPropertyChange(() => ListEmployees); // 필수!
            } 
        }

        // 지정버튼 활성화 여부 속성
        public bool CanSaveEmployee
        {
            get
            {
                if (string.IsNullOrEmpty(EmpName) || Salary == 0 || string.IsNullOrEmpty(DeptName))
                    return false;
                else
                    return true;
            }
        }

        public MainViewModel()
        {
            DisplayName = "직원관리 시스템";

            // 조회실행
            GetEmployees();
        }
        /// <summary>
        /// Caliburn.Micro가 Xaml의 버튼 x:Name과 동일한 이름의 메서드로 매핑
        /// </summary>
        public void NewEmployee()
        {
            Id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = string.Empty;
            Addr = string.Empty;
        }

        // 비동기로 동작시키기 위해 async를 사용
        public async void SaveEmployee() // INSERT,UPDATE(삽입,갱신)
        {
            if (Common.DialogCoordinator != null)
            {
                this._dialogCoordinator=Common.DialogCoordinator;
            }

            // MessageBox.Show("저장버튼 동작");
            using(SqlConnection conn = new SqlConnection(Helpers.Common.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if(Id ==0)  cmd.CommandText = Models.Employees.INSERT_QUERY;    // INSERT
                else cmd.CommandText= Models.Employees.UPDATE_QUERY;            // UPDATE

                SqlParameter prmEmpName = new SqlParameter("@EmpName", EmpName);
                cmd.Parameters.Add(prmEmpName);
                SqlParameter prmSalary = new SqlParameter("@Salary", Salary);
                cmd.Parameters.Add(prmSalary);
                SqlParameter prmDeptName = new SqlParameter("@DeptName", DeptName);
                cmd.Parameters.Add(prmDeptName);
                SqlParameter prmAddr = new SqlParameter("@Addr", Addr ?? (object)DBNull.Value); // 주가 빈값일 경우 DB 컬럼에 NULL 값을 입력
                cmd.Parameters.Add(prmAddr);
                        
                if(Id != 0) // UPDATE일 경우
                {
                    SqlParameter prmId = new SqlParameter("@Id", Id);
                    cmd.Parameters.Add (prmId);
                }

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    //MessageBox.Show("저장 성공!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장성공!", "저장");
                }
                else
                {
                    //MessageBox.Show("저장 실패!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "저장실패!", "실패");

                }
            }
            GetEmployees(); 
            NewEmployee(); // 모든 입력 컨트롤 초기화
        }

        public void GetEmployees() // SELECT(조회)
        {
            using(SqlConnection conn = new SqlConnection(Helpers.Common.ConnectionString))
            {
                conn.Open ();
                SqlCommand cmd = new SqlCommand(Models.Employees.SELECT_QUERY, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ListEmployees = new BindableCollection<Employees> ();

                while (reader.Read())
                {
                    ListEmployees.Add(new Employees()
                    {
                        Id = (int)reader["Id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Addr = reader["Addr"].ToString()
                    });
                }
            }
        }   

        public bool CanDelEmployee // 삭제 버튼을 누를 수 있는지 여부를 확인하는 속성
        {
            get { return Id != 0; } // TextBox Id 속성이 0이면 false, 0이 아니면 true
        }
        public async void DelEmployee() // DELETE(삭제)
        {
            if (Common.DialogCoordinator != null)
            {
                this._dialogCoordinator = Common.DialogCoordinator;
            }


            if (Id == 0)
            {
                // var res = MessageBox.Show("삭제하고 싶은 내용을 선택해주세요","삭제불가");
                await this._dialogCoordinator.ShowMessageAsync(this, "삭제불가!", "삭제");

                return;
            }

            var r = await this._dialogCoordinator.ShowMessageAsync(this, "삭제하시겠습니까?", "삭제여부", MessageDialogStyle.AffirmativeAndNegative);
            if (r == MessageDialogResult.Negative)  // AffirmativeAndNegative : MessageBoxButtons.YesOrNo와 유사한 스타일
            {
                return;
            }

            //if( MessageBox.Show("삭제하시겠습니까?", "삭제여부", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            //{
            //    return;
            //}

            using(SqlConnection conn = new SqlConnection (Helpers.Common.ConnectionString))
            {
                conn.Open ();
                SqlCommand cmd = new SqlCommand(Models.Employees.DELETE_QUERY, conn);
                SqlParameter prmId = new SqlParameter("@Id", Id);
                cmd.Parameters.Add(prmId);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    //MessageBox.Show("삭제 성공!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제성공!", "삭제");

                }
                else
                {
                    //MessageBox.Show("삭제 실패!");
                    await this._dialogCoordinator.ShowMessageAsync(this, "삭제실패!", "삭제");
                }
            }
            GetEmployees();
            NewEmployee(); // 입력값 초기화
        }

        
    }
}
