# basic-wpf-2024
부경대학교 2024 IoT 개발자 과정 WPF 리포지토리

## 1일차(24.04.29)
- WPF(Window Presentation Foundation) 기본학습
    - 기존의 Winforms의 단점을 보완하여 확장된 WPF
        - 이전의 Winforms는 이미지 비트맵 방식(2D) -> 확대를 하면 할수록 이미지 깨짐 현상 발생
        - WPF 이미지 벡터방식
        - XAML 화면 디자인 - 안드로이드 개발시 Java XML로 화면디자인과 PyQt로 디자인과 동일

    - XAML(엑스에이엠엘, 재믈)
        - 여는 태크<Window>, 닫는 태그</Window>
        - 합치면 <Window/> -> Window 태그 안데 다른 객체가 없음
        - 여는 태그와 닫는 태그 사이에 다른 태그(객체)를 넣어서 디자인

    - WPF 기본 사용법
        - Winforms 와는 다르게 코딩으로 디자인을 함
        - 주석 형태 : <!----> -> XML, HTML과 동일 주석
    
    - 레이아웃
        1. Grid - WPF에서 가장 많이 사용하는 대표적인 레이아웃
        2. StackPanel - 스택으로 컨트롤을 쌓는 레이아웃
        3. Canvas - 미술에서 캔버스와 유사
        4. DockPanel - 컨트롤을 방향에 따라서 도킹시키는 레이아웃
        5. Margin - 여백 기능과 앵커링(Anchoring) 기능을 같이함(중요!)

    - 디자인 코딩 방법
    - 디자인, 백그라운드 코딩, C#코드 완전 분리 개발 -> MVVM 디자인 패턴

## 2일차(24.04.30)
- WPF 기본학습
    - HorizontalAlignment : 수평 위치 조절 -> Left, Right, Center, Stretch
    - VerticalAlignment : 수직 위치 조절 -> Top, Center, Bottom
    - 데이터바인딩 - 데이터 소스(DB, 엑셀, txt, 클라우드에 보관된 데이터 원본)에 데이터를 쉽게 가져다쓰기 위해 데이터 핸들링 방법
        - xaml : {Binding Path=속성, ElementName=객체, Mode=(OneWay|TwoWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 중간 단계의 개체
        - 변환기 : 데이터 바인딩 시 유형을 객체 유형으로 변환
        - 전통적인 윈폼 코드비하인드에서 데이터를 처리하는 것을 지양 - 디자인, 개발 부분 분리

## 3일차(24.05.02)
- WPF에서 중요한 개념(Winform과 차이점) ★★★
    1. 데이터 바인딩 : 바인딩 키워드로 코드와 디자인을 분리
    2. 옵저버 패턴 : 값이 변경된 사실을 사용자에게 공지 (OnPropertyChanged 이벤트)
    3. 디자인 리소스 : 각 컨트롤마다 디자인(X), 리소스를 통해 디자인을 공유 -> 리소스만 변경해도 디자인을 일괄적으로 변경

- WPF 기본학습
    - 데이터 바인딩 마무리

    - INofity(OnPropertyChanged)
    ```csharp
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
    ```

    - 디자인 리소스
        - 리소스를 통해 디자인을 공유
        - 각화면당 리소스가 따로 존재 -> 자기 화면에만 적용되는 디자인
        - App.xmal Resources -> 프로젝트 내 모든 화면에 적용되는 디자인
        - 리소스 사전 - 공유할 디자인 내용이 많을 때 독립적인 파일로 따로 지정하여 사용
        - 정적(Static), 동적(Dynamic)
        - Style을 통해 특정 속성값 뿐만이 아닌 디자인 자체를 리소스에 담을 수 있음
        ```csharp
        <Style x:Key="ButtonAccentVisual">
            <Setter Property="ItemsControl.Template">
                <Setter.Value>
                    <ControlTemplate>
                        <Grid>
                            <Rectangle Fill="#25A2FA" RadiusX="10" RadiusY="10" Stroke="Black" StrokeThickness="2"/>
                            <Label Content="Click"
                                HorizontalAlignment="Center"
                                VerticalAlignment="Center"
                                Foreground="White" FontWeight="Bold" FontSize="20"/>
                        </Grid>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
        ```


- WPF MVVM
    - MVC(Model, Viewm, Controller)
        - 웹개발(Spring, ASP.NET MVC, dJango, etc...)에서 현재도 사용되고 있음
        - Model : Data 입출력 처리를 담당, View에 제공할 데이터
        - View : 디스플레이 화면 담당
        - Controller : View를 제어, Model 처리 중앙에 관장

    - MVVM(Model View ViewModel)
        - Model : Data 입출력(DB Side), View에 제공할 데이터
        - View : 화면, 순수 xaml로만 구성
        - ViewModel : 뷰에 대한 메서드, 액션, INotifyPropertyChanged를 구현

        ![MVVM패턴](https://raw.githubusercontent.com/YooWangGwon/basic-wpf-2024/main/images/wpf001.png)

    - 권장 구현방법
        - ViewModel 생성, 알림 속성 구현
        - View에 ViewModel을 데이터바인딩
        - Model DB작업 독립적으로 구현

    - MVVM 구현을 도와주는 프레임워크
        0. ~~MVVMlight.ToolKit~~ 3rd Party 개발. 2009년부터 시작 2014년도 이후부터는 더이상 개발이나 지원이 없음
        1. **Caliburn.Micro** : 3rd Party 개발. MVVM이 아주 간단, 중소형 프로젝트에 적합. 강력 but 디버깅이 조금 어려움 
        3. AvaloniaUI : 3rd Party 개발. 크로스플랫폼. 디자인은 가장 좋음
        2. Prism : Microsoft 개발. 매우 어려움. 대규모 프로젝트 활용

- Caliburn.Micro
    1. 프로젝트 생성 후 MainWindow.xaml 삭제
    2. Models, Views, ViewModels 폴더(네임스페이스) 생성
    3. 종속성 NuGet 패키지 Caliburn.Micro 설치
    4. 루트 폴더에 Bootstrapper.cs 클래스 생성, 작성
    5. App.xaml에서 StartupUri를 삭제
    6. App.xaml에 Bootstrapper 클래스를 리소스 사전에 등록
    7. App.xaml에 App() 생성자 추가
    8. ViewModels 폴더에 MainViewModel.cs 클래스 생성
    9. Bootstrapper.cs에 OnStartup()에 내용을 변경
    10. Views에 MainView.xaml를 생성

    - 작업(3명) 분리
        - DB 개발자 : DBMS 테이블 생성, Models에 클래스 작업, 
            - Model 폴더 안에 테이블명과 동일하게 클래스 생성
            - 테이블의 컬럼에 대한 프로퍼티 생성
        - 화면 디자이너
            - Views 폴더에 있는 xaml 파일을 디자인 작업
        - 총괄 개발자

## 4일차(24.05.03)
- Caliburn.Micro
    - Xaml디자이너 - xaml 파일만 디자인
    - ViewModel 개발자 - Model에 있는 DB관련 정보와 View와 연계 전체구현 작업

- Caliburn.Micro
    - Xaml 디자인시 {Binding ...} 잘 사용하지 않음
    - 대신 x:Name을 사용

- MVVM
    - 예외발생 시, 예외메시지가 표시없이 프로그램이 종료됨
    - ViewModel에서 F5로 디버깅을 하여 예외가 발생하는 부분 탐색
    - View.xaml 바인딩, 버튼 클릭 이름(ViewModel 속성, 메서드) 지정에 주의해야함
    - Model은 DB 테이블 컬럼명과 일치, CRUD 쿼리문 오타 주의
    - ViewModel부분
        - 변수, 속성으로 분리
        - 속성이 Model내의 속성과 이름이 일치해야함
        - List 사용 불가 -> BindableCollection으로 변경
        - 메서드와 이름이 동일한 Can~으로 시작되는 프로퍼티 -> 버튼 활성/비활성화
        - 모든 속성에 NotifyofPropertyChange() 매서드 존재!!(값 변경 알림)

    ![실행화면](https://raw.githubusercontent.com/YooWangGwon/basic-wpf-2024/main/images/wpf002.png)

## 5일차(24.05.07)
- Caliburn.Micro + MahApps.Metro
    - Metro(Modern UI)디자인 접목
    - 접목 가능한 디자인
        - MahApps.Metro : https://mahapps.com/
            - 적용 방법 : https://mahapps.com/docs/guides/quick-start
        - Icon.Pack.Browser : https://github.com/MahApps/IconPacks.Browser/releases/tag/1.0.0
            - MahApps 에서 지원하는 무료 아이콘
        - Material Design In XAML : http://materialdesigninxaml.net/

    - 디자인을 적용한 메인화면
    ![메인화면](https://raw.githubusercontent.com/YooWangGwon/basic-wpf-2024/main/images/wpf003.png)

    - 메트로 다이얼로그
    ![다이얼로그](https://raw.githubusercontent.com/YooWangGwon/basic-wpf-2024/main/images/wpf004.png)

- Movie API 연동앱, MovieFinder 2024
    - DB(SQL Server) 연동
    - MahApps.Metro
    - OpenAPI 두가지 사용
    - MVVM은 시간부족으로 미적용
    - 좋아하는 영화 즐겨찾기 앱
    - [TMDB](https://www.themoviedb.org/) OpenAPI활용
        - 회원가입 후 API 신청
    - [Youtube API](https://console.cloud.google.com/) 활용
        - 새 프로젝트 생성
        - API 및 서비스, 라이브러리 선택
        - YouTube 검색 -> YouTube Data API v3 선택 -> 사용버튼 클릭 
        - 사용자 인증 정보 만들기 버튼 클릭 
            1. 사용자 데이터 라디오 버튼 클릭 -> 다음
            2. OAuth 동의화면, 기본내용 입력 후 다음
            3. 범위는 저장 후 계속
            4. OAuth 클라이언트 ID에서 애플리케이션 유형을 '데스크톱 앱', 이름 입력 후 만들기 클릭

    - Newtonsoft.Json