# basic-wpf-2024
부경대학교 2024 IoT 개발자 과정 WPF 리포지토리

## 1일차(23.04.29)
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

## 2일차(23.04.30)
- WPF 기본학습
    - HorizontalAlignment : 수평 위치 조절 -> Left, Right, Center, Stretch
    - VerticalAlignment : 수직 위치 조절 -> Top, Center, Bottom
    - 데이터바인딩 - 데이터 소스(DB, 엑셀, txt, 클라우드에 보관된 데이터 원본)에 데이터를 쉽게 가져다쓰기 위해 데이터 핸들링 방법
        - xaml : {Binding Path=속성, ElementName=객체, Mode=(OneWay|TwoWay), StringFormat={}{0:#,#}}
        - dataContext : 데이터를 담아서 전달하는 중간 단계의 개체
        - 변환기 : 데이터 바인딩 시 유형을 객체 유형으로 변환
        - 전통적인 윈폼 코드비하인드에서 데이터를 처리하는 것을 지양 - 디자인, 개발 부분 분리

## 3일차
- WPF 기본학습
    - 데이터 바인딩 마무리
    - 디자인 리소스
- WPF MVVM