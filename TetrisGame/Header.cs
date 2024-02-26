using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    class Header
    {
        public const int Max_X = 24;
        public const int Max_Y = 30;
        public const int FrameRate = 200;
        public enum e_BoardState
        {
            blank,
            blocks
        }
        public enum e_BlockType
        {
            None,
            straight,
            square,
            skew_1,
            skew_2,
            L_1,
            L_2,
            T,
            Max
        }

        public class Block
        {
            int x;
            int y;
            e_BlockType type;
            float rotate;
            PlayGame games;
            List<Tuple<int, int>> childBlocks;
            public Block(e_BlockType type, int x, int y, PlayGame games)
            {
                this.type = type;
                this.x = x;
                this.y = y;
                rotate = 0;
                this.games = games;
                childBlocks = new List<Tuple<int, int>>();
                BlockSet();
            }

            public List<Tuple<int, int>> ChildBlocks
            {
                get { return childBlocks; }
                set { childBlocks = value; }
            }

            public bool IsChildBlock(int y, int x)
            {
                foreach (var childBlock in ChildBlocks)
                {
                    if (childBlock.Item1 == y && childBlock.Item2 == x)
                    {
                        return true; // 주어진 좌표가 자식 블록 목록에 포함되어 있으면 true 반환
                    }
                }
                return false; // 주어진 좌표가 자식 블록 목록에 포함되어 있지 않으면 false 반환
            }

            void BlockSet()
            {
                switch (type)
                {
                    case e_BlockType.straight:
                        // 일직선 블록 생성
                        for (int i = 0; i < 4; i++)
                        {
                            if (games.InitGames._GameBoard[y + i, x] == e_BoardState.blocks)
                                games.IsEndGame = true; // 이미 블록이 있는 경우 게임 종료
                        }
                        // 블록이 이미 없는 경우 블록 생성
                        for (int i = 0; i < 4; i++)
                        {
                            games.InitGames._GameBoard[y + i, x] = e_BoardState.blocks;
                            childBlocks.Add(Tuple.Create(y + i, x));
                        }
                        break;
                    case e_BlockType.square:
                        // 정사각형 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true; // 이미 블록이 있는 경우 게임 종료
                        }
                        // 블록이 이미 없는 경우 블록 생성
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        break;
                    case e_BlockType.skew_1:
                        // 왼쪽으로 기울어진 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 2] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 2] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x + 2));
                        break;
                    case e_BlockType.skew_2:
                        // 오른쪽으로 기울어진 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x - 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x - 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x - 1));
                        break;
                    case e_BlockType.L_1:
                        // L자 모양 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2, x + 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x + 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 2, x));
                        childBlocks.Add(Tuple.Create(y + 2, x + 1));
                        break;
                    case e_BlockType.L_2:
                        // 거꾸로 L자 모양 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2 , x - 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x - 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 2, x));
                        childBlocks.Add(Tuple.Create(y + 2, x -1));
                        break;
                    case e_BlockType.T:
                        // T자 모양 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x - 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x - 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y, x - 1));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        break;
                    default:
                        break;
                }
            }

            /*          straight,
                        square,
                        skew_1,
                        skew_2,
                        L_1,
                        L_2,
                        T,
             * **/

            public void StraightRotate()
            {
                rotate += 90;

                if (rotate >= 360)
                    rotate = 0;

                switch (rotate)
                {
                    case 0:
                    case 180:
                        {
                            int flagY = childBlocks[0].Item1;
                            int flagX = childBlocks[0].Item2;
                            int startIdx = 0;

                            // 회전 중간에 블록이 있는지 확인하고, 그 개수를 startIdx에 저장
                            for (int i = 0; i < childBlocks.Count; i++)
                            {
                                if (games.InitGames._GameBoard[flagY - i, flagX] == e_BoardState.blocks && !IsChildBlock(flagY-i,flagX))
                                    startIdx++;
                                else
                                    break;
                            }

                            // 만약 블록이 있을 경우, 회전이 가능한지 확인
                            if (startIdx > 0)
                            {
                                bool isPossible = true;
                                for (int i = 0; i < childBlocks.Count; i++)
                                {
                                    if (games.InitGames._GameBoard[flagY + startIdx - i, flagX] == e_BoardState.blocks && !IsChildBlock(flagY + startIdx - i, flagX))
                                    {
                                        isPossible = false;
                                        break;
                                    }
                                }

                                // 회전이 불가능할 경우, 함수 종료
                                if (!isPossible)
                                    return;
                            }

                            if (flagY >= 5) // 범위 체크
                            {
                                // 회전 가능한 경우, 블록을 회전시킴
                                for (int i = 1; i < childBlocks.Count; i++)
                                {
                                    int nowY = childBlocks[i].Item1;
                                    int nowX = childBlocks[i].Item2;
                                    // 각 위치의 대응을 수정하여 블록을 회전시킴
                                    int newY = flagY - i;

                                    // 회전 중간에 블록이 있는 경우 보정
                                    if (i <= startIdx)
                                    {
                                        newY += startIdx;
                                    }

                                    // 수정된 좌표로 업데이트
                                    childBlocks[i] = Tuple.Create(newY, flagX);
                                    games.InitGames._GameBoard[nowY, nowX] = e_BoardState.blank;
                                    games.InitGames._GameBoard[newY, flagX] = e_BoardState.blocks;
                                }
                            }
                            else
                            {
                                // y축 값을 1, 2, 3, 4로 초기화
                                for (int i = 1; i < childBlocks.Count; i++)
                                {
                                    int nowY = childBlocks[i].Item1;
                                    int nowX = childBlocks[i].Item2;
                                    int newY = 5 - i;
                                    if (i <= startIdx)
                                    {
                                        newY += startIdx;
                                    }
                                    childBlocks[i] = Tuple.Create(newY, flagX);
                                    games.InitGames._GameBoard[nowY, nowX] = e_BoardState.blank;
                                    games.InitGames._GameBoard[newY, flagX] = e_BoardState.blocks;
                                }
                            }
                        }
                        break;
                    case 90:
                    case 270:
                        {
                            int flagY = childBlocks[0].Item1;
                            int flagX = childBlocks[0].Item2;
                            int startIdx = 0;

                            for (int i = 0; i < childBlocks.Count; i++)
                            {
                                if (games.InitGames._GameBoard[flagY, flagX - i] == e_BoardState.blocks && !IsChildBlock(flagY, flagX-i)) // 블록이 있는지 확인
                                    startIdx++;
                                else
                                    break;
                            }

                            if (startIdx > 0)
                            {
                                bool isPossible = true;
                                for (int i = 0; i < childBlocks.Count; i++)
                                {
                                    if (games.InitGames._GameBoard[flagY, flagX + startIdx - i] == e_BoardState.blocks && !IsChildBlock(flagY, flagX + startIdx - i)) // 블록이 있는지 확인
                                    {
                                        isPossible = false;
                                        break;
                                    }
                                }

                                if (!isPossible)
                                    return;
                            }

                            if (flagX >= 5) // 범위 체크
                            {
                                for (int i = 1; i < childBlocks.Count; i++)
                                {
                                    int nowY = childBlocks[i].Item1;
                                    int nowX = childBlocks[i].Item2;
                                    // 각 위치의 대응을 수정하여 블록을 회전시킴
                                    int newX = flagX - i;
                                    if (i <= startIdx)
                                    {
                                        newX += startIdx;
                                    }
                                    // 수정된 좌표로 업데이트
                                    childBlocks[i] = Tuple.Create(flagY, newX);
                                    games.InitGames._GameBoard[nowY, nowX] = e_BoardState.blank;
                                    games.InitGames._GameBoard[flagY, newX] = e_BoardState.blocks;
                                }
                            }
                            else
                            {
                                // x축 값을 1, 2, 3, 4로 초기화
                                for (int i = 1; i < childBlocks.Count; i++)
                                {
                                    int nowY = childBlocks[i].Item1;
                                    int nowX = childBlocks[i].Item2;
                                    // 각 위치의 대응을 수정하여 블록을 회전시킴
                                    int newX = 5 - i;
                                    if (i <= startIdx)
                                    {
                                        newX += startIdx;
                                    }
                                    // 수정된 좌표로 업데이트
                                    childBlocks[i] = Tuple.Create(flagY, newX);
                                    games.InitGames._GameBoard[nowY, nowX] = e_BoardState.blank;
                                    games.InitGames._GameBoard[flagY, newX] = e_BoardState.blocks;
                                
                                }
                            }
                        }
                        break;
                }
            }
            public void squareRotate()
            {
                for (int i = 0; i < childBlocks.Count; i++)
                {

                }
            }
            public void skewRotate()
            {
                for (int i = 0; i < childBlocks.Count; i++)
                {

                }
            }
            public void L_Rotate()
            {
                for (int i = 0; i < childBlocks.Count; i++)
                {

                }
            }
            public void T_Rotate()
            {
                for (int i = 0; i < childBlocks.Count; i++)
                {

                }
            }
            public e_BlockType Type
            {
                get { return type; }
            }

            public int X
            {
                get { return x; }
                set { x = value; }
            }
            public int Y
            {
                get { return y; }
                set { y = value; }
            }
        }
    }
}
