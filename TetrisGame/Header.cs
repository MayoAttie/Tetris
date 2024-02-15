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
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2, x + 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x + 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        childBlocks.Add(Tuple.Create(y + 2, x + 1));
                        break;
                    case e_BlockType.skew_2:
                        // 오른쪽으로 기울어진 블록 생성
                        if (games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 2, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 2, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 2, x));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        break;
                    case e_BlockType.L_1:
                        // L자 모양 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 2] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 2] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 2] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 2] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y, x + 1));
                        childBlocks.Add(Tuple.Create(y, x + 2));
                        childBlocks.Add(Tuple.Create(y + 1, x + 2));
                        break;
                    case e_BlockType.L_2:
                        // 거꾸로 L자 모양 블록 생성
                        if (games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 2] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 2] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 2] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 2] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        childBlocks.Add(Tuple.Create(y + 1, x + 2));
                        childBlocks.Add(Tuple.Create(y, x + 2));
                        break;
                    case e_BlockType.T:
                        // T자 모양 블록 생성
                        if (games.InitGames._GameBoard[y, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y + 1, x + 1] == e_BoardState.blocks ||
                            games.InitGames._GameBoard[y, x + 2] == e_BoardState.blocks)
                        {
                            games.IsEndGame = true;
                        }
                        games.InitGames._GameBoard[y, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y + 1, x + 1] = e_BoardState.blocks;
                        games.InitGames._GameBoard[y, x + 2] = e_BoardState.blocks;
                        childBlocks.Add(Tuple.Create(y, x));
                        childBlocks.Add(Tuple.Create(y + 1, x));
                        childBlocks.Add(Tuple.Create(y + 1, x + 1));
                        childBlocks.Add(Tuple.Create(y, x + 2));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
