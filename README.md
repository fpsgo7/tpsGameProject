# tpsGameProject 뒤끝연동 tps 게임 
> tps 게임프로젝트와 회원기능이 추가된 프로젝트입니다.
> 회원 관련 서버는 뒤끝 서버를 사용하였습니다.

## 게임 기능
1. tps 게임으로써 다양한 조작이 가능하다.
2. 회원 , 비회원 으로 게임이 가능하다.
3. 인벤토리와, 키설정이 지원된다.

## 게임 영상 링크
> [영상링크](https://www.youtube.com/watch?v=4NNA7Mjzvtg)
## 게임 다운로드 링크
> [다운로드 링크](https://drive.google.com/file/d/1N6v0PjgJQwgszeTKQbZ1IgcFNaxTCPts/view?usp=sharing)
## 게임 조작
- WASD : 기본 이동 조작키
- 이동중에 shift : 이동중 뛰기 동작
- 조준중에 shift : 좌우 조준 변환
- R : 장전
- G : 수류탄
- 좌클릭 : 총발싸
- 우클릭 : 조준
- 조준중에 Tab : 저격총일경우 스코프로 조준전환
- M : 인벤토리 열고 닫기
- 인벤토리에서 좌클릭 : 삭제할 아이템 선택
- 인벤토리에서 우클릭 : 아이템 장착
- ESC : 설정 메뉴 열기
- F : 상호작용 키

## 오류와 주의점
뒤끝에서  clientappID 같은 값을 변경할경우 fail to load backend.dat 이란 오류가 발생한다 (이전에 사용하던 backend.dat 파일의 값은 그대로기 때문)
그래서 새롭게 해당 파일을 생성하기위해 유니티 PlayerSetting 에서 회사명을 변경후 실행해주면 새로 파일을만들어준다.
