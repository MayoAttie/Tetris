using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace TetrisGame
{

    class PlayGame
    {
        bool gameEnd;
        bool isDroping;
        InitGame initGameCls;
        Header.Block newBlock;
        public PlayGame(InitGame gameStart)
        {
            gameEnd = false;
            isDroping = false;
            newBlock = null;
            initGameCls = gameStart;
        }

        public void GameProcess()
        {
            while(true)
            {
                DateTime frameStartTime = DateTime.Now; // 현재 프레임의 시작 시간

                // 게임 상태 업데이트
                UpdateGame();

                // 사용자 입력 처리
                ProcessInput();

                // 화면 업데이트
                Render();

                // 남은 시간만큼 슬립
                DateTime frameEndTime = DateTime.Now; // 현재 프레임의 종료 시간
                int elapsedTime = (int)(frameEndTime - frameStartTime).TotalMilliseconds; // 현재 프레임의 소요 시간
                int remainingTime = Header.FrameRate - elapsedTime; // 남은 시간 계산

                if (remainingTime > 0)
                    Thread.Sleep(remainingTime); // 남은 시간만큼 슬립

                if (gameEnd)
                    break;
            }
        }

        #region 게임 진행 관련 함수
        // 게임 상태 업데이트 메서드
        private void UpdateGame()
        {
            gameEnd = initGameCls.GameEndCheck();
            ControlBlocks();
            if(!isDroping)
                initGameCls.BlockCrush();
        }

        // 사용자 입력 처리 메서드
        private void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        // 왼쪽으로 이동
                        MoveBlockLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        // 오른쪽으로 이동
                        MoveBlockRight();
                        break;
                    case ConsoleKey.DownArrow:
                        // 아래쪽으로 이동
                        MoveBlockDown();
                        break;
                        // 다른 입력에 대한 처리 추가 가능
                }
            }
        }

        // 화면 업데이트 메서드
        private void Render()
        {
            initGameCls.DrawGame();
        }
        #endregion

        #region UpdateGame
        void ControlBlocks()
        {
            if (!isDroping)
            {
                // 블록 생성
                int x = Header.Max_X / 2;
                Header.e_BlockType type = RandBlockReturn();
                newBlock = new Header.Block(Header.e_BlockType.square, x, 2, this);
                isDroping = true;
            }
            else
            {
                // 블록 이동
                List<Tuple<int, int>> prev = new List<Tuple<int, int>>();
                bool blockLanded = false; // 블록이 땅에 닿았는지 여부를 나타내는 변수

                for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                {
                    int prevY = newBlock.ChildBlocks[i].Item1;
                    int prevX = newBlock.ChildBlocks[i].Item2;
                    initGameCls._GameBoard[prevY, prevX] = Header.e_BoardState.blank;
                }

                for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                {
                    int y = newBlock.ChildBlocks[i].Item1;
                    int x = newBlock.ChildBlocks[i].Item2;
                    if (y + 1 < Header.Max_Y)
                    {
                        if (initGameCls._GameBoard[y + 1, x] == Header.e_BoardState.blocks)
                        {
                            // 블록이 다른 블록에 닿았을 때
                            blockLanded = true;
                            prev.Add(Tuple.Create(y, x));
                        }
                        else
                        {
                            prev.Add(Tuple.Create(y, x));
                            newBlock.ChildBlocks[i] = Tuple.Create(y + 1, x);
                        }
                    }
                    else
                    {
                        // 블록이 바닥에 닿았을 때
                        blockLanded = true;
                        break;
                    }
                }

                if (blockLanded)
                {
                    // 블록이 바닥에 닿았으므로 이동 중지
                    isDroping = false;
                    for (int i = 0; i < prev.Count; i++)
                    {
                        int nowY = prev[i].Item1;
                        int nowX = prev[i].Item2;
                        initGameCls._GameBoard[nowY, nowX] = Header.e_BoardState.blocks;
                        newBlock.ChildBlocks[i] = Tuple.Create(nowY, nowX);
                    }
                }
                else
                {
                    for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                    {
                        int nowY = newBlock.ChildBlocks[i].Item1;
                        int nowX = newBlock.ChildBlocks[i].Item2;
                        initGameCls._GameBoard[nowY, nowX] = Header.e_BoardState.blocks;
                    }
                }
            }
        }
        Header.e_BlockType RandBlockReturn()
        {
            Random rand = new Random();
            int index = rand.Next(1, (int)Header.e_BlockType.Max);
            return (Header.e_BlockType)index;
        }
        #endregion


        #region ProcessInput
        void MoveBlockLeft()
        {
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;
                if (initGameCls._GameBoard[nowY, nowX] == Header.e_BoardState.blank)
                    return;
            }

            bool isPossible = true;
            // 왼쪽으로 이동 가능한지 확인
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;

                if (nowX - 1 <= 0)
                {
                    isPossible = false;
                    break;
                }
                if (!newBlock.IsChildBlock(nowY, nowX - 1) &&
                    initGameCls._GameBoard[nowY, nowX - 1] == Header.e_BoardState.blocks)
                {
                    isPossible = false;
                    break;
                }
            }

            // 왼쪽으로 이동 가능하면 블록 이동
            if (isPossible)
            {
                for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                {
                    int nowY = newBlock.ChildBlocks[i].Item1;
                    int nowX = newBlock.ChildBlocks[i].Item2;

                    initGameCls._GameBoard[nowY, nowX] = Header.e_BoardState.blank;
                    initGameCls._GameBoard[nowY, nowX - 1] = Header.e_BoardState.blocks;
                    newBlock.ChildBlocks[i] = Tuple.Create(nowY, nowX - 1);
                }
            }
        }

        void MoveBlockRight()
        {
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;
                if (initGameCls._GameBoard[nowY, nowX] == Header.e_BoardState.blank)
                    return;
            }

            bool isPossible = true;
            // 오른쪽으로 이동 가능한지 확인
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;

                if (nowX + 1 >= Header.Max_X-1)
                {
                    isPossible = false;
                    break;
                }
                if (!newBlock.IsChildBlock(nowY, nowX + 1) &&
                    initGameCls._GameBoard[nowY, nowX + 1] == Header.e_BoardState.blocks)
                {
                    isPossible = false;
                    break;
                }
            }

            // 오른쪽으로 이동 가능하면 블록 이동
            if (isPossible)
            {
                for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                {
                    int nowY = newBlock.ChildBlocks[i].Item1;
                    int nowX = newBlock.ChildBlocks[i].Item2;

                    initGameCls._GameBoard[nowY, nowX] = Header.e_BoardState.blank;
                    initGameCls._GameBoard[nowY, nowX + 1] = Header.e_BoardState.blocks;
                    newBlock.ChildBlocks[i] = Tuple.Create(nowY, nowX + 1);
                }
            }
        }

        void MoveBlockDown()
        {
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;
                if (initGameCls._GameBoard[nowY, nowX] == Header.e_BoardState.blank)
                    return;
            }

            bool isPossible = true;
            // 아래로 이동 가능한지 확인
            for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
            {
                int nowY = newBlock.ChildBlocks[i].Item1;
                int nowX = newBlock.ChildBlocks[i].Item2;

                if (nowY + 1 >= Header.Max_Y)
                {
                    isPossible = false;
                    break;
                }
                if (!newBlock.IsChildBlock(nowY +1, nowX) &&
                    initGameCls._GameBoard[nowY +1, nowX] == Header.e_BoardState.blocks)
                {
                    isPossible = false;
                    break;
                }
            }

            // 아래로 이동 가능하면 블록 이동
            if (isPossible)
            {
                for (int i = 0; i < newBlock.ChildBlocks.Count; i++)
                {
                    int nowY = newBlock.ChildBlocks[i].Item1;
                    int nowX = newBlock.ChildBlocks[i].Item2;

                    initGameCls._GameBoard[nowY, nowX] = Header.e_BoardState.blank;
                    initGameCls._GameBoard[nowY + 1, nowX] = Header.e_BoardState.blocks;
                    newBlock.ChildBlocks[i] = Tuple.Create(nowY + 1, nowX);
                }
            }
        }
        #endregion

        #region 게터세터
        public InitGame InitGames
        {
            get { return initGameCls; }
            set { initGameCls = value; }
        }
        public bool IsEndGame
        {
            get { return gameEnd; }
            set { gameEnd = value; }
        }
        #endregion

    }
}
