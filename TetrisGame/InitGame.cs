using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TetrisGame
{
    class InitGame
    {
        Header.e_BoardState[,] GameBoard;
        PlayGame games;
        public InitGame()
        {
            GameBoard = new Header.e_BoardState[Header.Max_Y, Header.Max_X];

            // 배열을 기본 값으로 채웁니다.
            for (int i = 0; i < Header.Max_Y; i++)
            {
                for (int j = 0; j < Header.Max_X; j++)
                {
                    GameBoard[i, j] = Header.e_BoardState.blank;
                }
            }
            // 콘솔 창의 가로 및 세로 크기를 설정.
            Console.SetWindowSize(70, 46);

            // 콘솔 창의 왼쪽 상단 모퉁이의 위치를 변경.
            Console.SetWindowPosition(0, 0);
            Console.CursorVisible = false;
            games = new PlayGame(this);
            games.GameProcess();
        }

        #region 드로우
        public void DrawGame()
        {
            // 이전 프레임의 화면을 지우기 위해 콘솔을 비웁니다.
            Console.Clear();

            for (int i = 0; i < Header.Max_Y; i++)
            {
                for (int j = 0; j < Header.Max_X; j++)
                {
                    // 가장자리에 해당하는 셀에 외곽선을 그립니다.
                    if (i == 0 || i == Header.Max_Y-1 || j == 0 || j == Header.Max_X-1)
                    {
                        GameBoard[i, j] = Header.e_BoardState.blocks;
                    }
                }
            }

            // 게임 보드를 콘솔에 출력
            for (int i = 0; i < Header.Max_Y; i++)
            {
                for (int j = 0; j < Header.Max_X; j++)
                {
                    // 셀의 상태에 따라 출력할 문자와 색상을 선택
                    switch (GameBoard[i, j])
                    {
                        case Header.e_BoardState.blank:
                            Console.Write(" ");
                            break;
                        case Header.e_BoardState.blocks:
                            Console.Write("*");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
        #endregion

        #region 게임 진행 함수
        public bool GameEndCheck()
        {
            bool isEnd = false;
            for(int i=1; i<Header.Max_X-1; i++)
            {
                if(GameBoard[1,i] == Header.e_BoardState.blocks)
                {
                    isEnd = true;
                    break;
                }    
            }
            return isEnd;
        }
        public void BlockCrush()
        {
            for (int i = 1; i < Header.Max_Y-1; i++)
            {
                bool isLine = true;
                for (int j = 1; j < Header.Max_X-1; j++)
                {
                    if(GameBoard[i,j] == Header.e_BoardState.blank)
                    {
                        isLine = false;
                        break;
                    }
                }

                if(isLine)
                {
                    for(int k=i; k>0; k--)
                    {
                        for (int j = 1; j < Header.Max_X - 1; j++)
                            GameBoard[k, j] = GameBoard[k - 1, j];
                    }
                }

                for (int j = 1; j < Header.Max_X - 1; j++)
                    GameBoard[0, j] = Header.e_BoardState.blank;
            }
        }
        #endregion


        #region 게터세터
        public Header.e_BoardState[,] _GameBoard
        {
            get { return GameBoard; }
            set { GameBoard = value; }
        }
        #endregion
    }
}
